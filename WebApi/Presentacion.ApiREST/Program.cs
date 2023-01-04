using Aplicacion.Core.Interfaces;
using Applicacion.Core.IOC;
using Infraestructura.Data;
using Infraestructura.Data.Repositorios;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Presentacion.ApiREST.Middleware;
using Presentacion.ApiREST.Rutas;
using Serilog;
using Serilog.Sinks.File;
using Serilog.Extensions.Logging;
using System;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
  .MinimumLevel.Information()
  .WriteTo.File("logs/log-system.txt", rollingInterval: RollingInterval.Hour)
  .CreateLogger();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepositorioGenerico<>), (typeof(RepositorioGenerico<>)));
builder.Services.AddDbContext<ContextoBD>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.TryAddSingleton<ISystemClock, SystemClock>();
builder.Services.AgregarValidadores();


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsRule", rule =>
    {
        rule.AllowAnyHeader().AllowAnyMethod().WithOrigins("*");
    });
});

//builder.Logging.AddEventLog();
//builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    //var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
    try
    {
        var dataContext = scope.ServiceProvider.GetRequiredService<ContextoBD>();
        await dataContext.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        //var logger = loggerFactory.CreateLogger<Program>();
        //logger.LogError(ex, "Errores en el proceso de migracion");
        Log.Error($"Errores en el proceso de migracion: {ex.Message} - {(ex.InnerException != null ? ex.InnerException.Message : "")}");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseCors("CorsRule");
app.AgregarRutasProductos();
app.AgregarRutasCategorias();

app.Run();
