using System.Security.Claims;
using Hera.Core.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hera.Pages.Auth;

public class SignInModel : PageModel {
    readonly ILogger<SignInModel> logger;

    [BindProperty]
    public SignInRequest RequestPayload { get; set; }

    public SignInModel(ILogger<SignInModel> logger) {
        this.logger = logger;
    }

    public async Task<IActionResult> OnGetAsync() => Page();

    public async Task<IActionResult> OnPostTrySignInAsync(SignInRequest payload) {
        if (!ModelState.IsValid)
            return Page();

        var claims = new List<Claim> {
            new Claim(ClaimTypes.NameIdentifier, payload.Username),
            new Claim(ClaimTypes.Name, "Admin"),
            new Claim(ClaimTypes.Email, "admin@xample.com"),
            new Claim(ClaimTypes.Role, "")
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        return RedirectToPage("/Index");
    }

    public async Task<IActionResult> OnGetSignOutAsync() {
        await HttpContext.SignOutAsync();
        return RedirectToPage("/Auth/Index");
    }
}