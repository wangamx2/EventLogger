using EventLogger.Domain.Interfaces;
using EventLogger.Infrastructure.Persistence.Mongo;
using EventLogger.Infrastructure.Persistence.Sql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace EventLogger.Infrastructure.DI
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(config.GetConnectionString("SqlServer")));
            services.AddScoped<IEventLogRepository, EventLogRepository>();
            services.AddTransient<IMongoClient>(sp => new MongoClient(config.GetConnectionString("MongoDb")));
            services.AddScoped<IMongoService, MongoService>();
            return services;
        }
    }
}
