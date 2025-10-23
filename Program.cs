using BloodDonorProject.Configurations;
using BloodDonorProject.Data;
using BloodDonorProject.Data.Implementations;
using BloodDonorProject.Data.Interfaces;
using BloodDonorProject.Extensions;
using BloodDonorProject.Mapping;
using BloodDonorProject.Middleware;
using BloodDonorProject.Repositories.Implementations;
using BloodDonorProject.Repositories.Interfaces;
using BloodDonorProject.Services.Implementations;
using BloodDonorProject.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IFileService, FileService>();
builder.Services.AddScoped<IBloodDonorService, BloodDonorService>();
builder.Services.AddScoped<IDonationService, DonationService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IBloodDonorRepository, BloodDonorRepository>();
builder.Services.AddScoped<IDonationRepository, DonationRepository>();
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddDbContext<BloodDonorDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<BloodDonorDbContext>()
.AddDefaultTokenProviders()
.AddDefaultUI();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

builder.Services.AddRazorPages();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Services.AddSerilog();
builder.Services.Configure<EmailSetting>(builder.Configuration.GetSection("EmailSetting"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<IPWhiteListingMiddleware>();

app.UseStaticFiles();
app.UseHttpMethodOverride();
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
app.MapRazorPages();

await app.SeedDatabaseAsync();

app.Run();