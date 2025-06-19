using EventLogger.Api.Endpoints;
using EventLogger.Api.Middlewares;
using EventLogger.Application.Commands;
using EventLogger.Infrastructure.DI;
using EventLogger.Infrastructure.Persistence.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services

        builder.Services.AddMediatR(cfg =>
          cfg.RegisterServicesFromAssemblyContaining<CreateEventLogCommand>());

        // Repositories
        builder.Services.AddInfrastructure(builder.Configuration);

        builder.Services.AddEndpointsApiExplorer(); 
        builder.Services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "EventLogger API",
                Version = "v1",
                Description = "API for managing and retrieving user event logs with SQL + Mongo"
            });
        });

        var app = builder.Build();

        // Middleware
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Database.Migrate(); 
        }

        // Swagger
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.MapEventLogEndpoints();
        app.Run();
    }
}