using AspNerCoreIdentityApp.Web.Models;
using Microsoft.EntityFrameworkCore;
using AspNerCoreIdentityApp.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"));
});

builder.Services.AddIdentityWithExtension();
builder.Services.ConfigureApplicationCookie(opt =>
{
    var cookiebuilder=new CookieBuilder();
    cookiebuilder.Name = "EmreCookie";
    opt.LoginPath = new PathString("/Home/Signin");
    opt.Cookie=cookiebuilder;
    opt.LogoutPath = new PathString("/Member/Logout");
    opt.ExpireTimeSpan=TimeSpan.FromDays(60);
    opt.SlidingExpiration = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapControllerRoute(
	name: "areas",
	pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");





app.Run();
