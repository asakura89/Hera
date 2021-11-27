using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Razor.Hosting;
using Newtonsoft.Json;

namespace Hera.Core {
    public class RouteDebuggerMiddleware : IMiddleware
    {
        private readonly ILogger _logger;
        public RouteDebuggerMiddleware(ILogger<RouteDebuggerMiddleware> logger) => _logger = logger;
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var providers = context.RequestServices.GetService<IActionDescriptorCollectionProvider>();

            /*var controllerActionDescriptor = context
                .GetEndpoint()
                .Metadata
                .GetMetadata<IActionDescriptorCollectionProvider>();*/

            //var controllerName = controllerActionDescriptor.ActionDescriptors.Items;
            //var urlHelper = context.RequestServices.GetService<IUrlHelper>();
            //var routeData = 
            var data = providers.ActionDescriptors.Items
                .Select(desc => new {
                    Area = desc.RouteValues.ContainsKey("area") ? desc.RouteValues["area"] : "",
                    Path = desc.DisplayName,
                    File = desc.EndpointMetadata.FirstOrDefault(meta => meta is RazorCompiledItemMetadataAttribute),
                    PathOfRouteAttribute = $"/{desc.AttributeRouteInfo.Template}",
                    Handlers = (desc is CompiledPageActionDescriptor actDesc) ? actDesc
                        .HandlerMethods
                        .Select(handler => new {
                            handler.HttpMethod,
                            handler.Name
                        }) : null
                });

            _logger.LogInformation(JsonConvert.SerializeObject(data));
            await next(context);
        }
    }
}