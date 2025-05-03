using webdev_SIS.DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using webdev_SIS.Models;
using webdev_SIS.Services;

var builder = WebApplication.CreateBuilder(args);

// Services Configuration
builder.Services.AddControllersWithViews();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<UserRepository>();

// Add EmailService
builder.Services.AddScoped<system_SIS.Services.EmailService>();

// Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Authentication & Authorization
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Use Always in production
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("FacultyOnly", policy => policy.RequireRole("Faculty"));
    options.AddPolicy("AdmissionOnly", policy => policy.RequireRole("Admission"));
});

var app = builder.Build();

//await SeedAdminUserAsync(app);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();





//// ---------------------------------------------------------
//// Admin Seeder Method
//// ---------------------------------------------------------
//static async Task SeedAdminUserAsync(WebApplication app)
//{
//    using var scope = app.Services.CreateScope();
//    var userRepo = scope.ServiceProvider.GetRequiredService<UserRepository>();

//    var existingAdmins = await userRepo.GetUsersByRole("Admin");

//    if (!existingAdmins.Any())
//    {
//        var defaultAdmin = new UserEntity
//        {
//            FirstName = "Althea",
//            LastName = "Amor",
//            Email = "altheaamor12@gmail.com",
//            PasswordHash = "adminAlthea?21", // will be hashed
//            Role = "Admin",
//            Status = "Active",
//            CreatedAt = DateTime.Now
//        };

//        await userRepo.AddUser(defaultAdmin);
//        Console.ForegroundColor = ConsoleColor.Green;
//        Console.WriteLine("✅ Default Admin account seeded successfully.");
//        Console.ResetColor();
//    }
//    else
//    {
//        Console.ForegroundColor = ConsoleColor.Yellow;
//        Console.WriteLine("ℹ️ Admin account already exists. No seeding needed.");
//        Console.ResetColor();
//    }
//}
