using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjetoEcommerce.Api.Extensions;
using ProjetoEcommerce.Application;
using ProjetoEcommerce.Infra.IoC;
using Microsoft.AspNetCore.Authentication.JwtBearer; // Adicionado
using Microsoft.IdentityModel.Tokens; // Adicionado
using System.Text; // Adicionado

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Infrastructure Layer
builder.Services.AddInfrastructure(configuration);

// Application Layer
builder.Services.AddApplication(configuration);

// === CONFIGURAÇÃO DE JWT EXPLÍCITA (Para corrigir o erro de assinatura) ===
// Removemos a chamada da extensão e fazemos direto aqui para garantir a chave certa
var secretKey = configuration["Jwt:SecretKey"]; // Lê a mesma chave do TokenService
var key = Encoding.ASCII.GetBytes(secretKey);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key), // Usa a chave correta!
        ValidateIssuer = true,
        ValidIssuer = configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = configuration["Jwt:Audience"]
    };
});
// ========================================================================

builder.Services.AddControllers();
builder.Services.AddSwaggerConfiguration();
builder.Services.AddHealthChecks();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Development", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerConfiguration();
    app.UseDeveloperExceptionPage();
}

app.UseCors("Development");

app.UseHttpsRedirection();

// Ordem importante: Auth -> Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health");
app.MapControllers();

app.Run();