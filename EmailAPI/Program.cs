using EmailAPI.Middleware;
using EmailAPI.Models;
using EmailAPI.Services;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Options;

namespace EmailAPI {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            if (!builder.Environment.IsDevelopment()) {
                string? connectionString = Environment.GetEnvironmentVariable("AZ_APP_CONFIG");

                builder.Configuration.AddAzureAppConfiguration(options => {
                    options.Connect(connectionString)
                        .Select("EmailAPI:*", LabelFilter.Null)
                        .ConfigureRefresh(refreshOptions => {
                            refreshOptions.Register("EmailAPI:Settings:Sentinel", refreshAll: true)
                                .SetCacheExpiration(TimeSpan.FromDays(1));
                        });
                });

                builder.Services.AddAzureAppConfiguration();
            }

            // Add services to the container.
            builder.Services.AddScoped<IEmailService, EmailService>();

            builder.Services.Configure<Settings>(builder.Configuration.GetRequiredSection("EmailAPI:Settings"));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseHttpsRedirection();

            app.UseMiddleware<AuthenticationMiddleware>();

            if (!app.Environment.IsDevelopment()) {
                app.UseAzureAppConfiguration();
            }

            // Map minimal API endpoints.
            app.MapPost("/send-email", async (EmailDTO emailDto, IEmailService emailService) =>
            {
                return await emailService.SendEmailAsync(emailDto);
            });

            app.MapGet("/servers/keys", (IOptionsSnapshot<Settings> options) => {
                var keys = options.Value.Servers.Keys;
                return TypedResults.Ok(keys);
            });

            app.Run();
        }
    }
}
