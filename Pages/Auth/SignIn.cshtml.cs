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

    public async Task<IActionResult> OnGetAsync() => await Task.Run(Page);

    public async Task<IActionResult> OnPostTrySignInAsync(SignInRequest payload) {
        if (!ModelState.IsValid)
            return Page();

        var claims = new List<Claim> {
            new Claim(ClaimTypes.NameIdentifier, payload.Username),
            new Claim(ClaimTypes.Name, $"{payload.Username[0].ToString().ToUpperInvariant()}{payload.Username.Substring(1)}"),
            new Claim(ClaimTypes.Email, $"{payload.Username.ToLowerInvariant()}@xample.com"),
            new Claim(ClaimTypes.Role, "Admin")
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