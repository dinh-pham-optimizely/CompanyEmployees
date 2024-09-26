using CompanyEmployees;
using CompanyEmployees.Extensions;
using Contracts;
using Microsoft.AspNetCore.HttpOverrides;
using NLog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "nlog.config"));

builder.Services.ConfigureCors();

builder.Services.ConfigureRepositoryManager();

builder.Services.AddControllers();

builder.Services.ConfigureLoggerService();

builder.Services.ConfigureServiceManager();

builder.Services.ConfigureSqlContext(builder.Configuration);

builder.Services.AddControllers()
    .AddApplicationPart(typeof(CompanyEmployees.Presentation.AssemblyReference).Assembly);

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddControllers(config =>
{
    // Tell the server to respect the Accept header.
    config.RespectBrowserAcceptHeader = true;
}).AddXmlDataContractSerializerFormatters();

var app = builder.Build(); // Create the app variable of the type WebApplication

// Extract ILoggerManager service.
// var logger = app.Services.GetRequiredService<ILoggerManager>();
// app.ConfigureExceptionHandler(logger);
app.UseExceptionHandler(opt => { });

// Configure the HTTP request pipeline.

if (app.Environment.IsProduction())
{
    app.UseHsts();
}

app.UseHttpsRedirection(); // To add the middleware for the redirection from
// HTTP to HTTPS.

app.UseStaticFiles();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseCors("CorsPolicy");

app.UseAuthorization(); // To add authorization middleware.

app.MapControllers(); // To add enpoints from controller actions
// to the IEndpointRouteBuilder

app.Run();
