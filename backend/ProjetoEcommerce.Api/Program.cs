using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjetoEcommerce.Api.Extensions;
using ProjetoEcommerce.Application;
using ProjetoEcommerce.Infra.IoC;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Infrastructure Layer (Database, Repositories, Cache, Cloud, Message Queue)
builder.Services.AddInfrastructure(configuration);

// Application Layer (Services, AutoMapper, Validators)
builder.Services.AddApplication(configuration);

// Authentication & Authorization (JWT)
builder.Services.AddJwtAuthentication(configuration);

// API Controllers
builder.Services.AddControllers();

// Swagger/OpenAPI Documentation
builder.Services.AddSwaggerConfiguration();

// Health Checks
builder.Services.AddHealthChecks();

// CORS - Simples para desenvolvimento, configurável para produção
builder.Services.AddCors(options =>
{
    options.AddPolicy("Development", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });

    options.AddPolicy("Production", policy =>
    {
        policy.WithOrigins("https://seusite.com")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerConfiguration();
    app.UseDeveloperExceptionPage();
}

// CORS - antes de Auth
app.UseCors(app.Environment.IsDevelopment() ? "Development" : "Production");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapHealthChecks("/health");
app.MapControllers();

app.Run();