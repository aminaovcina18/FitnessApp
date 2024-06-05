using Fitness_WebCore.Helpers;
using Fitness_WebCore.IServices;
using Fitness_WebCore.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;

builder.Services.Configure<BaseUrl>(Configuration.GetSection("BaseUrls"));
builder.Services.Configure<IdentityConfig>(Configuration.GetSection("IdentityServer:Client"));

builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IActivityService, ActivityService>();

builder.Services.AddScoped<IHttpClientService, HttpClientService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.Name = "fitness";
                options.ExpireTimeSpan = TimeSpan.FromDays(15);
                options.LoginPath = "/Identity/Login";
                options.AccessDeniedPath = "/Identity/AccessDenied";
            });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireRegularRole", policy =>
    {
        policy.RequireAuthenticatedUser()
            .RequireRole("REGULAR")
            .Build();
    });
});

builder.Services.AddMvc()
    .AddRazorPagesOptions(options =>
    {
            options.Conventions.AddAreaPageRoute("Identity", "/Identity/Login", "");
    });
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddRazorPages();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.UseCookiePolicy();

app.UseAuthentication();
app.UseAuthorization();


app.UseRequestLocalization(((IApplicationBuilder)app).ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

app.UseMiddleware<ErrorHandlerMiddleware>();

app.MapControllers();

app.MapRazorPages();

app.Run();
