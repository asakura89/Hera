using Hera.Core;
using Hera.Modules.Employee.Data;
using Hera.Modules.Employee.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder webAppBuilder = WebApplication.CreateBuilder(args);
webAppBuilder.Services.AddRazorPages(options => {
    options.Conventions.AuthorizeFolder("/Attendance");
    options.Conventions.AuthorizeFolder("/Employee");
    options.Conventions.AuthorizeFolder("/Feedback");
    options.Conventions.AuthorizeFolder("/Overtime");
    options.Conventions.AuthorizeFolder("/Reimbursement");
    options.Conventions.AuthorizeFolder("/Shift");
});

webAppBuilder.Services.AddServerSideBlazor();

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

webAppBuilder.Services.AddDbContext<EmployeeDbContext>(options =>
    options.UseSqlite(webAppBuilder.Configuration.GetConnectionString("EmployeeDbContext"))
);
//webAppBuilder.Services.AddDatabaseDeveloperPageExceptionFilter();

webAppBuilder.Services.AddScoped<FamilyMemberService>();

webAppBuilder.Services.AddSession();
//webAppBuilder.Services.AddScoped<ElapsedTimeMiddleware>();
//webAppBuilder.Services.AddScoped<RouteDebuggerMiddleware>();
webAppBuilder.Services.AddScoped<RequestDebuggerMiddleware>();

WebApplication webApp = webAppBuilder.Build();
if (!webApp.Environment.IsDevelopment()) {
    webApp.UseExceptionHandler("/Error");
    webApp.UseHsts();
}
else {
    webApp.UseDeveloperExceptionPage();
    //webApp.UseMigrationsEndPoint();
}

using (IServiceScope scope = webApp.Services.CreateScope()) {
    IServiceProvider services = scope.ServiceProvider;

    EmployeeDbContext dbCtx = services.GetRequiredService<EmployeeDbContext>();
    dbCtx.Database.EnsureCreated();
    DbInitializer.Initialize(dbCtx);
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
webApp.MapBlazorHub();
webApp.MapFallbackToPage("/_Host");

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