using DeviceDetectorNET;
using DeviceDetectorNET.Cache;
using DeviceDetectorNET.Parser;

namespace PersonalWebsiteMVC.Middleware
{
    public class MobileDetectionMiddleware
    {
        private readonly RequestDelegate _next;
        // https://www.linkedin.com/pulse/aspnet-core-middleware-how-rendering-tables-mobile-devices-mariano
        public MobileDetectionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            DeviceDetector.SetVersionTruncation(VersionTruncation.VERSION_TRUNCATION_NONE);

            var userAgent = context.Request.Headers["User-Agent"];
            var dd = new DeviceDetector(userAgent);

            dd.SetCache(new DictionaryCache());
            dd.SkipBotDetection();
            dd.Parse();

            if (dd.IsMobile() == true)
            {
                context.Items.Remove("isMobile");
                context.Items.Add("isMobile", true);
            }
            else
            {
                context.Items.Remove("isMobile");
                context.Items.Add("isMobile", false);
            }

            await _next.Invoke(context);
        }
    }

    public static class RequestCultureMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestCulture(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MobileDetectionMiddleware>();
        }
    }
}
