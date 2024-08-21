using EmailAPI.Models;
using Microsoft.Extensions.Options;

namespace EmailAPI.Middleware {
    public class AuthenticationMiddleware(RequestDelegate next, IOptionsMonitor<Settings> options) {
        private readonly RequestDelegate _next = next;
        private readonly Settings _settings = options.CurrentValue;

        public async Task InvokeAsync(HttpContext context) {
            if (!context.Request.Headers.TryGetValue("Subscription-Key", out var token)) {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("API Key was not provided.");
                return;
            }

            string authToken = _settings.Token;

            if (!authToken.Equals(token)) {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized client.");
                return;
            }

            await _next(context);
        }
    }
}
