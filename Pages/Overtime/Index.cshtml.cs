using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hera.Pages.Overtime;

public class IndexModel : PageModel {
    readonly ILogger<IndexModel> logger;

    public IndexModel(ILogger<IndexModel> logger) {
        this.logger = logger;
    }

    public async Task<IActionResult> OnGetAsync() => Page();

    // public async Task<PartialViewResult> OnPostModalAsync() {
    //     return Partial("_Request");
    // }

    public PartialViewResult OnPostModal() {
        return Partial("_Request");
    }
}
