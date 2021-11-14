using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hera.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel : PageModel {
    public String? RequestId { get; set; }

    public Boolean ShowRequestId => !String.IsNullOrEmpty(RequestId);

    readonly ILogger<ErrorModel> logger;

    public ErrorModel(ILogger<ErrorModel> logger) {
        this.logger = logger;
    }

    public void OnGet() => RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
}

