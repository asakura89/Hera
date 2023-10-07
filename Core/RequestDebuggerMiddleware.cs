using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using Microsoft.AspNetCore.Razor.Hosting;

namespace Hera.Core {
    public class RequestDebuggerMiddleware : IMiddleware {
        private readonly ILogger logger;
        public RequestDebuggerMiddleware(ILogger<RequestDebuggerMiddleware> logger) {
            this.logger = logger;
        }

        String GetRequestInfoFromHttpContext(HttpContext context) {
            HttpRequest req = context.Request;
            return new StringBuilder()
                .Append(req.Protocol).Append(' ')
                .Append(req.Method).Append(' ')
                .Append(req.Scheme).Append("://")
                .Append(req.Host.ToString())
                .Append(req.Path.ToString())
                .Append(req.QueryString.ToString())
                .ToString();
        }

        String GetAreaFromActionDescriptor(ActionDescriptor descriptor) {
            Boolean routeContainsArea = descriptor
                .RouteValues
                .ContainsKey("area");

            if (routeContainsArea)
                return descriptor.RouteValues["area"];

            return String.Empty;
        }

        String GetFilenameFromActionDescriptor(ActionDescriptor descriptor) {
            var metadata = descriptor
                .EndpointMetadata
                .FirstOrDefault(meta => meta is RazorCompiledItemMetadataAttribute)
                as RazorCompiledItemMetadataAttribute;

            if (metadata != null)
                return metadata.Value;

            return String.Empty;
        }

        String GetParameterFromHandlerParameterDescriptor(HandlerParameterDescriptor descriptor) => $"&{descriptor.Name}={descriptor.ParameterType.FullName}";

        String GetHandlerFromHandlerMethodDescriptor(HandlerMethodDescriptor descriptor, String mainRoute) {
            var handlerInfo = new StringBuilder()
                .Append(descriptor.HttpMethod.ToUpperInvariant()).Append(' ')
                .Append(mainRoute);

            if (descriptor.Name == null)
                return handlerInfo
                    .ToString();

            handlerInfo
                .Append("?handler=")
                .Append(descriptor.Name);

            if (descriptor.Parameters == null || !descriptor.Parameters.Any())
                return handlerInfo
                    .ToString();

            IEnumerable<String> @params = descriptor
                .Parameters
                .Select(GetParameterFromHandlerParameterDescriptor);

            handlerInfo.AppendJoin("", @params);

            return handlerInfo
                .ToString();
        }

        String GetRouteTemplateFromActionDescriptor(ActionDescriptor descriptor) => $"/{descriptor.AttributeRouteInfo.Template}";

        IList<String> GetHandlersFromActionDescriptor(ActionDescriptor descriptor) {
            String mainRoute = GetRouteTemplateFromActionDescriptor(descriptor);
            Boolean isRazorPages = descriptor is CompiledPageActionDescriptor;
            if (!isRazorPages)
                return null;

            var razorPagesDesc = descriptor as CompiledPageActionDescriptor;
            return razorPagesDesc
                .HandlerMethods
                .Select(handlerDesc => GetHandlerFromHandlerMethodDescriptor(handlerDesc, mainRoute))
                .ToList();
        }

        IEnumerable<RequestDebugInfo> GetRegisteredRoutesFromActionDescriptors(IReadOnlyList<ActionDescriptor> descriptors) =>
            descriptors
                .Select(desc => new RequestDebugInfo(
                    Area: GetAreaFromActionDescriptor(desc),
                    Path: desc.DisplayName,
                    PathofRoute: GetRouteTemplateFromActionDescriptor(desc),
                    File: GetFilenameFromActionDescriptor(desc),
                    Handlers: GetHandlersFromActionDescriptor(desc)
                ))
                .DistinctBy(route => $"{route.Path}+{route.File}");

        String GetOrGenerateRegisteredRoutes(HttpContext context) {
            String sessionKey = "Hera.Session.RegisteredRoutes";
            String dataInSession = context.Session.GetString(sessionKey)?.Trim();
            if (String.IsNullOrEmpty(dataInSession)) {
                var provider = context.RequestServices.GetService<IActionDescriptorCollectionProvider>();
                IReadOnlyList<ActionDescriptor> descriptors = provider.ActionDescriptors.Items;
                IEnumerable<RequestDebugInfo> registeredRoutes = GetRegisteredRoutesFromActionDescriptors(descriptors);
                dataInSession = registeredRoutes.AsJson();

                context.Session.SetString(sessionKey, dataInSession);
            }

            return dataInSession;
        }

        RequestDebugInfo FindRequestedRoute(String requested, String registered) {
            var routes = JsonSerializer.Deserialize<IEnumerable<RequestDebugInfo>>(registered);
            RequestDebugInfo found = routes
                .FirstOrDefault(route =>
                    route.Path == requested ||
                    route.PathofRoute == requested);

            if (found != null)
                return found;

            requested = $"{(requested.Length == 1 && requested == "/" ? String.Empty : requested)}/Index";
            return found = routes
                .FirstOrDefault(route =>
                    route.Path == requested ||
                    route.PathofRoute == requested);
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
            String requested = GetRequestInfoFromHttpContext(context);
            String registered = GetOrGenerateRegisteredRoutes(context);
            Object found = FindRequestedRoute(context.Request.Path, registered);

            logger.LogInformation("Request: {0}",
                new {
                    Requested = requested,
                    Registered = found
                }
                .AsIndentedJson());

            await next(context);
        }

        record RequestDebugInfo(String Area, String Path, String PathofRoute, String File, IEnumerable<String> Handlers);
    }
}