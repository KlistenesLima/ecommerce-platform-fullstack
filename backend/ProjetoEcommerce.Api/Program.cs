using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjetoEcommerce.Api.Extensions;
using ProjetoEcommerce.Application;
using ProjetoEcommerce.Infra.IoC;

var builder = WebApplication.CreateBuilder(args);

// ============================================
// 1. CONFIGURATION
// ============================================
var configuration = builder.Configuration;

// ============================================
// 2. SERVICES REGISTRATION
// ============================================

// Infrastructure Layer (Database, Repositories, Cache, Cloud, Message Queue)
builder.Services.AddInfrastructure(configuration);

// Application Layer (Services, AutoMapper, Validators)
builder.Services.AddApplication(configuration);

// Authentication & Authorization (JWT)
builder.Services.AddJwtAuthentication(configuration);

// CORS Configuration
builder.Services.AddCorsConfiguration(configuration);

// API Controllers
builder.Services.AddControllers();

// Swagger/OpenAPI Documentation
builder.Services.AddSwaggerConfiguration();

// Health Checks
builder.Services.AddHealthChecks();

// ============================================
// 3. BUILD APPLICATION
// ============================================
var app = builder.Build();

// ============================================
// 4. MIDDLEWARE PIPELINE
// ============================================

// Development-only middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerConfiguration();
    app.UseDeveloperExceptionPage();
}

// Security Headers
app.UseHttpsRedirection();

// CORS - must be before Auth
app.UseCors(app.Environment.IsDevelopment() ? "Development" : "Production");

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// Health Check endpoint
app.MapHealthChecks("/health");

// Map Controllers
app.MapControllers();

// ============================================
// 5. RUN APPLICATION
// ============================================
app.Run();