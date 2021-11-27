using System.Diagnostics;
using System.Text;

namespace Hera.Core {
    public class ElapsedTimeMiddleware : IMiddleware {
        private readonly ILogger logger;
        public ElapsedTimeMiddleware(ILogger<ElapsedTimeMiddleware> logger) {
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
            HttpRequest req = context.Request;
            String requestInfo = new StringBuilder()
                .Append(req.Protocol).Append(' ')
                .Append(req.Method).Append(' ')
                .Append(req.Scheme).Append(':')
                .Append(req.Host.ToString())
                .Append(req.Path.ToString())
                .Append(req.QueryString.ToString())
                .ToString();

            var sw = new Stopwatch();
            sw.Start();
            await next(context);
            logger.LogInformation($"{requestInfo} executed in {sw.ElapsedMilliseconds}ms");
        }
    }
}