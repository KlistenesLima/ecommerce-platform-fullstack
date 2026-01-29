using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using ProjetoEcommerce.Api.Extensions;
using ProjetoEcommerce.Application;
using ProjetoEcommerce.Infra.IoC;
using ProjetoEcommerce.Infra.MessageQueue.RabbitMQ; // Importante
using ProjetoEcommerce.Api.Workers; // Importante
using System.Net;
using System.Net.Security;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

// Infrastructure Layer (Aqui dentro deve estar injetando os repositórios)
builder.Services.AddInfrastructure(configuration);

// Application Layer
builder.Services.AddApplication(configuration);

// === MENSAGERIA (RABBITMQ) ===
// Registrando as classes existentes da sua Infra
builder.Services.AddSingleton<IRabbitMQPublisher, RabbitMQPublisher>();
builder.Services.AddSingleton<IRabbitMQConsumer, RabbitMQConsumer>();

// Registrando o Worker que roda em segundo plano
builder.Services.AddHostedService<SmsNotificationWorker>();

var secretKey = configuration["Jwt:SecretKey"];
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
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = configuration["Jwt:Audience"]
    };
});

builder.Services.AddControllers();
builder.Services.AddSwaggerConfiguration();
builder.Services.AddHealthChecks();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Development", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
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

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health");
app.MapControllers();

app.Run();
