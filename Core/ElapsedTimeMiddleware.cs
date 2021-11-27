using System.Diagnostics;

namespace Hera.Core {
    public class ElapsedTimeMiddleware : IMiddleware
    {
        private readonly ILogger _logger;
        public ElapsedTimeMiddleware(ILogger<ElapsedTimeMiddleware> logger) => _logger = logger;
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var sw = new Stopwatch();
            sw.Start();
            await next(context);
            var isHtml = context.Response.ContentType?.ToLower().Contains("text/html");
            if (context.Response.StatusCode == 200 && isHtml.GetValueOrDefault())
            {
                _logger.LogInformation($"{context.Request.Path} executed in {sw.ElapsedMilliseconds}ms");
            }
        }
    }
}