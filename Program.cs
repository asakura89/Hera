using Hera.Core;
using Microsoft.AspNetCore.Authentication.Cookies;

WebApplicationBuilder webAppBuilder = WebApplication.CreateBuilder(args);
webAppBuilder.Services.AddRazorPages(options => {
    options.Conventions.AuthorizeFolder("/Attendance");
    options.Conventions.AuthorizeFolder("/Employee");
    options.Conventions.AuthorizeFolder("/Feedback");
    options.Conventions.AuthorizeFolder("/Overtime");
    options.Conventions.AuthorizeFolder("/Reimbursement");
    options.Conventions.AuthorizeFolder("/Shift");
});
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

webApp.Lifetime.ApplicationStarted.Register(OnAppStarted);
webApp.Lifetime.ApplicationStopping.Register(OnAppStopping);
webApp.Lifetime.ApplicationStopped.Register(OnAppStopped);

webApp.UseHttpsRedirection();
webApp.UseStaticFiles();
webApp.UseRouting();
webApp.UseMiddleware<ElapsedTimeMiddleware>();
//webApp.UseMiddleware<RouteDebuggerMiddleware>();
webApp.UseAuthentication();
webApp.UseAuthorization();
webApp.MapRazorPages();

webApp.Run();


void OnAppStarted() {
    
}

void OnAppStopping() {
    
}

void OnAppStopped() {
    
}