using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using ChatApplicationYT.Hub;
using ChatApplicationYT;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configuración de Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ChatApplicationYT API", Version = "v1" });
});

builder.Services.AddSignalR();
builder.Services.AddSingleton<IDictionary<string, UserRoomConnection>>(opt =>
    new Dictionary<string, UserRoomConnection>());

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ChatApplicationYT API V1");
    });
}

app.UseRouting();

// Configuración CORS
app.UseCors();

app.UseEndpoints(endpoint =>
{
    endpoint.MapHub<ChatHub>("/chat");
    endpoint.MapControllers();
});

app.Run();
