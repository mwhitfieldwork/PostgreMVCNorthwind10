using Microsoft.EntityFrameworkCore;
using NWCodeFirstMVC.Domain.Contracts;
using NWCodeFirstMVC.Domain;
// using NWCodeFirstMVC.Domain.Models; (removed - SQL Server scaffold)
using NWCodeFirstMVC.Infrastructure.Services;
using NWCodeFirstMVC.Infrastructure;
using NWCodeFirstMVC.Infrastructure.Repositories;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ProductRepository>();
// AutoMapper profiles for mapping between PgModels and PocoModels
// Register AutoMapper manually (avoid needing the AddAutoMapper extension package in this project)
builder.Services.AddSingleton<IMapper>(sp =>
{
    var config = new MapperConfiguration(cfg =>
    {
        cfg.AddProfile(new NWCodeFirstMVC.Infrastructure.Mapping.ProductMappingProfile());
    });

    return config.CreateMapper();
});

// Switched to Postgres context (PgNwContext). The old SQL Server northwindContext registration is commented out
// to allow non-destructive migration. Remove the commented block after refactoring controllers/views.
/*
builder.Services.AddDbContext<northwindContext>(opt => // needed  to bring the northwind db into the project
{
    var configuration = builder.Configuration;
    var connectionString = configuration.GetConnectionString("Default");
    opt.UseSqlServer(connectionString);
});
*/

builder.Services.AddDbContext<PgNwContext>(options =>
{
    var configuration = builder.Configuration;
    // use the same conn string key as API app (DefaultConnection) - update appsettings if needed
    var connectionString = configuration.GetConnectionString("DefaultConnection") ?? configuration.GetConnectionString("Default");
    options.UseNpgsql(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
