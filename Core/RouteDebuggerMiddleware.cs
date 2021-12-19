using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Razor.Hosting;
using Newtonsoft.Json;

namespace Hera.Core {
    public class RouteDebuggerMiddleware : IMiddleware {
        private readonly ILogger logger;
        public RouteDebuggerMiddleware(ILogger<RouteDebuggerMiddleware> logger) {
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
            var provider = context.RequestServices.GetService<IActionDescriptorCollectionProvider>();
            var data = provider
                .ActionDescriptors
                .Items
                .Select(desc => new {
                    Area = desc.RouteValues.ContainsKey("area") ? desc.RouteValues["area"] : "",
                    Path = desc.DisplayName,
                    File = desc.EndpointMetadata.FirstOrDefault(meta => meta is RazorCompiledItemMetadataAttribute),
                    PathOfRouteAttribute = $"/{desc.AttributeRouteInfo.Template}",
                    Handlers = (desc is CompiledPageActionDescriptor actDesc) ? actDesc
                        .HandlerMethods
                        .Select(handler => new {
                            handler.HttpMethod,
                            handler.Name,
                            Params = handler
                                .Parameters
                                .Select(hParam => new {
                                    hParam.BindingInfo,
                                    hParam.Name,
                                    hParam.ParameterType.FullName
                                })
                        }) : null
                });

            logger.LogInformation(JsonConvert.SerializeObject(data, Formatting.Indented));
            await next(context);
        }
    }
}
