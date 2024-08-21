using EmailAPI.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace EmailAPI.Services {
    public class EmailService(IOptionsSnapshot<Settings> options) : IEmailService {
        private readonly Settings _settings = options.Value;

        public async Task<IResult> SendEmailAsync(EmailDTO request) {

            if (!_settings.Servers.TryGetValue(request.App, out var server) || server == null) {
                return TypedResults.NotFound($"SMTP settings not found for application: {request.App}");
            }

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(request.App, server.Username));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(server.Host, 587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(server.Username, server.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

            return TypedResults.Ok("Email sent successfully.");
        }
    }
}
