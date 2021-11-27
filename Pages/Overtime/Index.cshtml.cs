using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hera.Pages.Overtime;

public class IndexModel : PageModel {
    readonly ILogger<IndexModel> logger;

    public IndexModel(ILogger<IndexModel> logger) {
        this.logger = logger;
    }

    public async Task<IActionResult> OnGetAsync() => Page();

    public async Task<PartialViewResult> OnPostRequestAsync() => Partial("_Request");
    public async Task<PartialViewResult> OnGetRequestAsync() => Partial("_Request");
}
