using EventLogger.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventLogger.Infrastructure.Persistence.Sql
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<EventLog> EventLogs => Set<EventLog>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.EventType).IsRequired();
                entity.Property(e => e.Timestamp).IsRequired();
            });
        }
    }
}
