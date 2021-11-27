using Hera.Core;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;

WebApplicationBuilder webAppBuilder = WebApplication.CreateBuilder(args);
webAppBuilder.Services.AddRazorPages();
webAppBuilder.Services
    .AddAuthentication(opts => {
        opts.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        opts.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        opts.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);
webAppBuilder.Services.AddScoped<ElapsedTimeMiddleware>();
//webAppBuilder.Services.AddScoped<RouteDebuggerMiddleware>();

WebApplication webApp = webAppBuilder.Build();
if (!webApp.Environment.IsDevelopment()) {
    webApp.UseExceptionHandler("/Error");
    webApp.UseHsts();
}

webApp.UseHttpsRedirection();
webApp.UseStaticFiles();
webApp.UseRouting();
webApp.UseMiddleware<ElapsedTimeMiddleware>();
//webApp.UseMiddleware<RouteDebuggerMiddleware>();
webApp.UseAuthentication();
webApp.UseAuthorization();
webApp.MapRazorPages();

webApp.Run();
