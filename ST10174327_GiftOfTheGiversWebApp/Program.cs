using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ST10174327_GiftOfTheGiversWebApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// 1. Register ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GiftOfTheGiversContext")));

// 2. Add Identity services
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // Change to true if you want email confirmation
})
.AddEntityFrameworkStores<ApplicationDbContext>();

// 3. Add MVC controllers with views
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 4. Add authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// 5. Map default routes and Razor pages for Identity UI
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // Required for Identity pages like Register/Login

app.Run();
