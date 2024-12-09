using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AspnetCoreMvcFull.Data;
using AspnetCoreMvcFull.Models;

var builder = WebApplication.CreateBuilder(args);

// Add Database Context (SQLite)
builder.Services.AddDbContext<AspnetCoreMvcFullContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("AspnetCoreMvcFullContext")
    ?? throw new InvalidOperationException("Connection string 'AspnetCoreMvcFullContext' not found.")));

// Add IHttpContextAccessor
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Add PWA Services
builder.Services.AddProgressiveWebApp();

// Add MVC Services
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Database Seeding
using (var scope = app.Services.CreateScope())
{
  var services = scope.ServiceProvider;
  SeedData.Initialize(services);
}

// Middleware Pipeline Configuration
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboards}/{action=Index}/{id?}");

app.Run();
