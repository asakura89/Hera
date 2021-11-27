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
            var data = provider.ActionDescriptors.Items
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

            logger.LogInformation(JsonConvert.SerializeObject(data, Formatting.Indented));
            await next(context);
        }
    }
}

/*

Hera.Core.RouteDebuggerMiddleware: Information: [
  {
    "Area": "",
    "Path": "/Auth/Index",
    "File": {
      "Key": "Identifier",
      "Value": "/Pages/Auth/Index.cshtml",
      "TypeId": "Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemMetadataAttribute, Microsoft.AspNetCore.Razor.Runtime, Version=6.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60"
    },
    "PathOfRouteAttribute": "/Auth/Index",
    "Handlers": [
      {
        "HttpMethod": "Get",
        "Name": null
      }
    ]
  },
  {
    "Area": "",
    "Path": "/Auth/Index",
    "File": {
      "Key": "Identifier",
      "Value": "/Pages/Auth/Index.cshtml",
      "TypeId": "Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemMetadataAttribute, Microsoft.AspNetCore.Razor.Runtime, Version=6.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60"
    },
    "PathOfRouteAttribute": "/Auth",
    "Handlers": [
      {
        "HttpMethod": "Get",
        "Name": null
      }
    ]
  },
  {
    "Area": "",
    "Path": "/Auth/SignIn",
    "File": {
      "Key": "Identifier",
      "Value": "/Pages/Auth/SignIn.cshtml",
      "TypeId": "Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemMetadataAttribute, Microsoft.AspNetCore.Razor.Runtime, Version=6.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60"
    },
    "PathOfRouteAttribute": "/Auth/SignIn",
    "Handlers": [
      {
        "HttpMethod": "Get",
        "Name": null
      },
      {
        "HttpMethod": "Post",
        "Name": "TrySignIn"
      },
      {
        "HttpMethod": "Get",
        "Name": "SignOut"
      }
    ]
  },
  {
    "Area": "",
    "Path": "/Employee/Index",
    "File": {
      "Key": "Identifier",
      "Value": "/Pages/Employee/Index.cshtml",
      "TypeId": "Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemMetadataAttribute, Microsoft.AspNetCore.Razor.Runtime, Version=6.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60"
    },
    "PathOfRouteAttribute": "/Employee/Index",
    "Handlers": []
  },
  {
    "Area": "",
    "Path": "/Employee/Index",
    "File": {
      "Key": "Identifier",
      "Value": "/Pages/Employee/Index.cshtml",
      "TypeId": "Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemMetadataAttribute, Microsoft.AspNetCore.Razor.Runtime, Version=6.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60"
    },
    "PathOfRouteAttribute": "/Employee",
    "Handlers": []
  },
  {
    "Area": "",
    "Path": "/Error",
    "File": {
      "Key": "Identifier",
      "Value": "/Pages/Error.cshtml",
      "TypeId": "Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemMetadataAttribute, Microsoft.AspNetCore.Razor.Runtime, Version=6.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60"
    },
    "PathOfRouteAttribute": "/Error",
    "Handlers": [
      {
        "HttpMethod": "Get",
        "Name": null
      }
    ]
  },
  {
    "Area": "",
    "Path": "/Index",
    "File": {
      "Key": "Identifier",
      "Value": "/Pages/Index.cshtml",
      "TypeId": "Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemMetadataAttribute, Microsoft.AspNetCore.Razor.Runtime, Version=6.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60"
    },
    "PathOfRouteAttribute": "/Index",
    "Handlers": [
      {
        "HttpMethod": "Get",
        "Name": null
      }
    ]
  },
  {
    "Area": "",
    "Path": "/Index",
    "File": {
      "Key": "Identifier",
      "Value": "/Pages/Index.cshtml",
      "TypeId": "Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemMetadataAttribute, Microsoft.AspNetCore.Razor.Runtime, Version=6.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60"
    },
    "PathOfRouteAttribute": "/",
    "Handlers": [
      {
        "HttpMethod": "Get",
        "Name": null
      }
    ]
  },
  {
    "Area": "",
    "Path": "/Overtime/Index",
    "File": {
      "Key": "RouteTemplate",
      "Value": "{handler?}",
      "TypeId": "Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemMetadataAttribute, Microsoft.AspNetCore.Razor.Runtime, Version=6.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60"
    },
    "PathOfRouteAttribute": "/Overtime/Index/{handler?}",
    "Handlers": [
      {
        "HttpMethod": "Get",
        "Name": null
      },
      {
        "HttpMethod": "Post",
        "Name": "Modal"
      }
    ]
  },
  {
    "Area": "",
    "Path": "/Overtime/Index",
    "File": {
      "Key": "RouteTemplate",
      "Value": "{handler?}",
      "TypeId": "Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemMetadataAttribute, Microsoft.AspNetCore.Razor.Runtime, Version=6.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60"
    },
    "PathOfRouteAttribute": "/Overtime/{handler?}",
    "Handlers": [
      {
        "HttpMethod": "Get",
        "Name": null
      },
      {
        "HttpMethod": "Post",
        "Name": "Modal"
      }
    ]
  },
  {
    "Area": "",
    "Path": "/Privacy",
    "File": {
      "Key": "Identifier",
      "Value": "/Pages/Privacy.cshtml",
      "TypeId": "Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemMetadataAttribute, Microsoft.AspNetCore.Razor.Runtime, Version=6.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60"
    },
    "PathOfRouteAttribute": "/Privacy",
    "Handlers": [
      {
        "HttpMethod": "Get",
        "Name": null
      }
    ]
  }
]

*/