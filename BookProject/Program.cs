using Microsoft.EntityFrameworkCore;
using BookProject.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Register the DbContext with SQL Server
builder.Services.AddDbContext<BookProjectDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookProjectConnection")));

// Enable session services
builder.Services.AddDistributedMemoryCache(); // Required for session state
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true; // Ensures cookies are accessible only via HTTP requests
    options.Cookie.IsEssential = true; // Ensures session is available even if user opts out of tracking
});

var app = builder.Build();

// Configure middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession(); // Enable session handling

// Configure routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
