﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hera.Pages;

public class IndexModel : PageModel {
    readonly ILogger<IndexModel> logger;

    public String Username { get; set; }

    public IndexModel(ILogger<IndexModel> logger) {
        this.logger = logger;
    }

    public async Task<IActionResult> OnGetAsync() {
        if (!HttpContext.User.Identity.IsAuthenticated)
            return RedirectToPage("/Auth/SignIn");

        Username = HttpContext.User.Identity.Name;
        return Page();
    }
}
