namespace PersonalWebsiteMVC
{
    public class MyCustomMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await context.Response.WriteAsync("Custom middleware incoming request");
            await next(context);
            await context.Response.WriteAsync("Custom middleware outgoing response");
        }
    }
}
