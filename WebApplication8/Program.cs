using Microsoft.EntityFrameworkCore;
using WebApplication8.Data;
using WebApplication8.Implementation;
using WebApplication8.Middleware;
using WebApplication8.Repositories;
using WebApplication8.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<MyDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("AppWvcConnectionString"));
});
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();

builder.Services.AddSession(cfg =>
{
    cfg.Cookie.Name = "User";
    cfg.IdleTimeout = new TimeSpan(0, 30, 0);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();


app.UseRouting();

app.UseAuthorization();
app.UseMiddleware<LoginCheckMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
