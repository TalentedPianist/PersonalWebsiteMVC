
using Wangkanai.Detection.Models;
using Wangkanai.Detection.Services;

public class DeviceDetectionMiddleware
{
    private readonly RequestDelegate _next;

    public DeviceDetectionMiddleware(RequestDelegate next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }

    public async Task InvokeAsync(HttpContext context, IDetectionService detection)
    {
        if (detection.Device.Type == Device.Mobile)
            await context.Response.WriteAsync("You are mobile!");

        await _next(context);
    }
}