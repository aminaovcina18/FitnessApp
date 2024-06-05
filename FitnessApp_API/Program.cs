using ApplicationCore.Helpers.Identity;
using ApplicationCore.IRepositories;
using ApplicationCore.IServices;
using ApplicationCore.Services;
using FitnessApp_Domain.Entities.Identity;
using FitnessApp_Infrastructure;
using FitnessApp_Infrastructure.Repositories;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using Microsoft.AspNetCore.Diagnostics;
using ApplicationCore.Helpers.Error;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            //you can configure your custom policy
            builder.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

#region Database access
var connectionString = configuration.GetConnectionString("FitnessConnection");
var migrationsAssemly = typeof(DBContext).GetTypeInfo().Assembly.GetName().Name;

builder.Services.AddDbContext<DBContext>(options =>
{
    options.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssemly));
});

builder.Services.AddIdentity<Users, Roles>(options =>
{
    options.Password.RequiredLength = 6;
    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.MaxFailedAccessAttempts = 10;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
}).AddEntityFrameworkStores<DBContext>().AddDefaultTokenProviders();
#endregion

#region Authentication
builder.Services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
    .AddIdentityServerAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme,
        jwtOptions =>
        {
            // base-address of your identityserver
            jwtOptions.Authority = configuration.GetSection("IdentityServer:IssuerUri").Value;
            jwtOptions.Audience = configuration.GetSection("IdentityServer:Audience").Value;

            jwtOptions.TokenValidationParameters.ValidateIssuer = false;
            jwtOptions.TokenValidationParameters.ValidateLifetime = false;

            // audience is optional
            jwtOptions.TokenValidationParameters.ValidateAudience = false;

            // it's recommended to check the type header to avoid "JWT confusion" attacks
            jwtOptions.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };

            jwtOptions.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                NameClaimType = "name",
                RoleClaimType = "role"
            };
        },
        referenceOptions =>
        {
            // oauth2 introspection options
            referenceOptions.Authority = configuration.GetSection("IdentityServer:IssuerUri").Value;

            referenceOptions.ClientId = configuration.GetSection("IdentityServer:Client:ClientId").Value;
            referenceOptions.ClientSecret = configuration.GetSection("IdentityServer:Client:ClientSecret").Value;

            referenceOptions.RoleClaimType = "role";
        });

#endregion
# region Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ActivityPolicy", policy =>
    policy.RequireRole("REGULAR", "ADMIN"));
});

#endregion
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddTransient<ILogger>(s => s.GetService<ILogger<Program>>());
builder.Services.AddScoped<DbContext, DBContext>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

builder.Services.AddScoped<IUserClaimsUtil, UserClaimsUtil>();

builder.Services.AddScoped<IActivityService, ActivityService>();
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddSwaggerGen();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Ignore objects when cycles have been detected in deserialization:
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    })
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });


var app = builder.Build();
app.UseExceptionHandler(_ => { });
using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DBContext>();
    if (dbContext.Database.IsRelational())
    {
        dbContext.Database.Migrate();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}
app.UseCors();

app.UseRouting();

app.UseExceptionHandler("/error");

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
