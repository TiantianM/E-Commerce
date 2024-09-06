using MengGrocery.DAL;
using MengGrocery.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MengGrocery.Migrations;
using System;
//using MengGrocery.Data;
using NLog;
using NLog.Extensions.Logging;
using Microsoft.Extensions.Logging;
using NLog.Web;
using MengGrocery.LogHelper;


var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddScoped<IProductQuery, ProductQuery>();
builder.Services.AddScoped<ICartQuery, CartQuery>();
builder.Services.AddScoped<IOrderQuery, OrderQuery>();
builder.Services.AddScoped<ICookieHelper, CookieHelper>();
builder.Services.AddScoped<IAccountQuery, AccountQuery>();

builder.Services.AddSingleton(typeof(ILogHelper<>), typeof(LogHelper<>));
builder.Services.AddSingleton<ILoggerProvider, NLogLoggerProvider>();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, 
ServerVersion.AutoDetect(connectionString)));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddUserManager<CustomUserManager>()  // Use custom UserManager
        .AddSignInManager<CustomSignInManager>();  // Use custom SignInManager

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Add custom middleware
// app.UseMiddleware<CustomMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    /*app.MapAreaControllerRoute(name: "product_route",
                                     areaName: "Product",
                                     pattern: "Product/{action}/{id?}");*/

    // Generic route for all areas
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    // Fallback route without area
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

app.Run();

