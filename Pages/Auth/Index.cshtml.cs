using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hera.Pages.Auth;

public class IndexModel : PageModel {
    readonly ILogger<IndexModel> logger;

    public IndexModel(ILogger<IndexModel> logger) {
        this.logger = logger;
    }

    public async Task<IActionResult> OnGetAsync() {
        if (!HttpContext.User.Identity.IsAuthenticated)
            return RedirectToPage("/Auth/SignIn");

        return RedirectToPage("/Index");
    }
}