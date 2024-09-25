using CompanyEmployees.Extensions;
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

var app = builder.Build(); // Create the app variable of the type WebApplication

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
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
