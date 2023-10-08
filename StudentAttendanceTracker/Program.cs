//C# and Razor code written by Zaid Abuisba
using Microsoft.EntityFrameworkCore;
using StudentAttendanceTracker.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Azure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();
builder.Services.AddSession();
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

builder.Services.AddDbContext<AttendanceTrackerContext>(options => options.UseSqlServer(
builder.Configuration.GetConnectionString("AttendanceTrackerContext") ?? ""));

//Sets up password rules for users
builder.Services.AddIdentity<User, IdentityRole>(options => {
    options.Password.RequiredLength = 6; 
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false; 
    options.Password.RequireLowercase = false;
    
}).AddEntityFrameworkStores<AttendanceTrackerContext>().AddDefaultTokenProviders();


//Redirects user to index page for access denied error
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/Home/Index";
    options.AccessDeniedPath = "/Home/Index";
    options.SlidingExpiration = true;
});

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.AppendTrailingSlash = true;
});
var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Index");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();

var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    await ConfigureIdentity.CreateAdminUserAsync(scope.ServiceProvider);
    await ConfigureIdentity.CreateStudentUserAsync(scope.ServiceProvider);
    await ConfigureIdentity.CreateProfessorUsersAsync(scope.ServiceProvider);
    await ConfigureIdentity.CreateQStaffUserAsync(scope.ServiceProvider);
}


app.MapRazorPages();

app.UseAuthentication();
app.UseAuthorization();

app.MapAreaControllerRoute(
    name: "admin",
    areaName: "Admin",
    pattern: "Admin/{controller=Home}/{action=Index}/");

app.MapAreaControllerRoute(
    name: "student",
    areaName: "Student",
    pattern: "Student/{controller=Home}/{action=Index}/");

app.MapAreaControllerRoute(
    name: "faculty",
    areaName: "Faculty",
    pattern: "Faculty/{controller=Home}/{action=Index}/");


app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
