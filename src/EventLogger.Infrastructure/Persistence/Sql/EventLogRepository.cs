using EventLogger.Domain.Entities;
using EventLogger.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventLogger.Infrastructure.Persistence.Sql
{
    public class EventLogRepository : IEventLogRepository
    {
        private readonly ApplicationDbContext _db;
        public EventLogRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddUserEventAsync(EventLog log)
        {
            _db.EventLogs.Add(log);
            await _db.SaveChangesAsync();
        }

        public async Task<List<EventLog>> GetUserEventsAsync(string userId)
        {
            return await _db.EventLogs
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.Timestamp)
                .ToListAsync();
        }
    }
}
