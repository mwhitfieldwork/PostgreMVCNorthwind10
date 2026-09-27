using Microsoft.EntityFrameworkCore;
using NWCodeFirstMVC.Domain.Contracts;
using NWCodeFirstMVC.Infrastructure.Services;
using NWCodeFirstMVC.Domain.Contracts;
using NWCodeFirstMVC.Domain;
using NwCodeFirstMVC.Data;
using NWCodeFirstMVC.Domain;
using NLog;
using NWCodeFirstMVC.Api.Configurations;
using AutoMapper;
using NWCodeFirstMVC.Infrastructure;
using NWCodeFirstMVC.Api.Converters;
using NWCodeFirstMVC.Infrastructure.Mapping;
using NWCodeFirstMVC.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// If the deployment platform (e.g. Railway) provides a PORT environment variable,
// bind the web host to that port. Locally the app will continue to use launchSettings.json / Kestrel defaults.
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}

builder.Services.AddDbContext<PgNwContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new NullableDateOnlyJsonConverter());
    });


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", b => b
        .AllowAnyHeader()
        .AllowAnyOrigin()
        .AllowAnyMethod());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IOrderHistory, OrderHistoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IEmployeeTerritoryMap, EmployeeTerritoryMapService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericService<>));
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.Configure<GoogleAuthOptions>(builder.Configuration.GetSection("GoogleAuth"));
builder.Services.AddHttpClient<IGoogleAuthService, GoogleAuthService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<ICsvService, CsvService>();


builder.Services.AddAutoMapper(typeof(MapperConfig));
builder.Services.AddAutoMapper(typeof(ProductMappingProfile));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



NLog.LogManager.Setup().LoadConfiguration(builder =>
{
    builder.ForLogger().FilterMinLevel(NLog.LogLevel.Info).WriteToConsole();
    builder.ForLogger().FilterMinLevel(NLog.LogLevel.Debug).WriteToFile(fileName: "file.txt");
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.UseStaticFiles();


// Minimal hosting model requires MapControllers(), NOT UseEndpoints()
app.MapControllers();

app.Run();
