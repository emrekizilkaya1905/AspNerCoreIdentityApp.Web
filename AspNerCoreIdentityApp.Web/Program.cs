using AspNerCoreIdentityApp.Web.Models;
using Microsoft.EntityFrameworkCore;
using AspNerCoreIdentityApp.Web.Extensions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using AspNerCoreIdentityApp.Web.OptionsModels;
using AspNerCoreIdentityApp.Web.Services;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Authentication;
using AspNerCoreIdentityApp.Web.ClaimProviders;
using Microsoft.Build.Framework;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"));
});
builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
	options.ValidationInterval = TimeSpan.FromMinutes(30);
});
builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddIdentityWithExtension();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IClaimsTransformation, UserClaimProvider>();
builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("AnkaraPolicy", policy =>
	{
		policy.RequireClaim("city", "ankara");
	});
});
builder.Services.ConfigureApplicationCookie(opt =>
{
	var cookiebuilder = new CookieBuilder();
	cookiebuilder.Name = "EmreCookie";
	opt.LoginPath = new PathString("/Home/Signin");
	opt.Cookie = cookiebuilder;
	opt.LogoutPath = new PathString("/Member/Logout");
	opt.AccessDeniedPath = new PathString("/Member/AccessDenied");
	opt.ExpireTimeSpan = TimeSpan.FromDays(60);
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
