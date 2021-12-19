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
webAppBuilder.Services.AddAntiforgery(opts => {
    opts.SuppressXFrameOptionsHeader = false;
    opts.HeaderName = "XSRF-TOKEN";
});
webAppBuilder.Services.AddSession();
//webAppBuilder.Services.AddScoped<ElapsedTimeMiddleware>();
//webAppBuilder.Services.AddScoped<RouteDebuggerMiddleware>();
webAppBuilder.Services.AddScoped<RequestDebuggerMiddleware>();

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
webApp.UseAuthentication();
webApp.UseAuthorization();
webApp.UseSession();
//webApp.UseMiddleware<ElapsedTimeMiddleware>();
//webApp.UseMiddleware<RouteDebuggerMiddleware>();
webApp.UseMiddleware<RequestDebuggerMiddleware>();
webApp.MapRazorPages();

webApp.Run();

void OnAppStarted() {
    ILogger logger = webApp.Services.GetService<ILogger<Program>>();
    logger.LogInformation(nameof(OnAppStarted));
}

void OnAppStopping() {
    ILogger logger = webApp.Services.GetService<ILogger<Program>>();
    logger.LogInformation(nameof(OnAppStopping));
}

void OnAppStopped() {
    ILogger logger = webApp.Services.GetService<ILogger<Program>>();
    logger.LogInformation(nameof(OnAppStopped));
}