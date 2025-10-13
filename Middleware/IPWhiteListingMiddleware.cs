using BloodDonorProject.Configurations;
using Microsoft.Extensions.Options;

namespace BloodDonorProject.Middleware
{
    public class IPWhiteListingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly List<string> _whitelistedIPs;
        private EmailSetting _mailSetting;
        public IPWhiteListingMiddleware(RequestDelegate next, IConfiguration configuration, IOptions<EmailSetting> options, IOptionsMonitor<EmailSetting> monitor)
        {
            _next = next;
            _whitelistedIPs = configuration.GetSection("AllowedIPs").Get<List<string>>() ?? new();
            _mailSetting = options.Value;
            monitor.OnChange(mailSetting => {
                Console.WriteLine("Mail Changed!");
                _mailSetting = mailSetting;
            });
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var remoteIp = context.Connection.RemoteIpAddress;
            if (remoteIp != null && _whitelistedIPs.Contains(remoteIp.ToString()))
            {
                await _next(context);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Forbidden: Your IP is not allowed to access this resource.");
                return;
            }
        }
    }
}
