using EventLogger.Domain.Entities;

namespace EventLogger.Domain.Interfaces
{
    public interface IEventLogRepository
    {
        Task AddUserEventAsync(EventLog log);
        Task<List<EventLog>> GetUserEventsAsync(string userId);
    }
}
