using GRH_SENTECH.Data;
using GRH_SENTECH.Repository;
using GRH_SENTECH.Repository.Implement;
using GRH_SENTECH.Service;
using GRH_SENTECH.Service.Implement;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<GrhDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IEmployeRepository, EmployeRepository>();
builder.Services.AddScoped<IDepartementRepository, DepartementRepository>();
builder.Services.AddScoped<IContratRepository, ContratRepository>();
builder.Services.AddScoped<IPosteRepository, PosteRepository>();
builder.Services.AddScoped<ICongeRepository, CongeRepository>();

builder.Services.AddScoped<IEmployeService, EmployeService>();
builder.Services.AddScoped<IDepartementService, DepartementService>();

var app = builder.Build();

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
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<GrhDbContext>();
        db.Database.Migrate();
    }
    catch { /* */ }
}

app.Run();
