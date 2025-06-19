namespace EventLogger.Domain.Interfaces
{
    public interface IMongoService
    {
        Task SaveEventDetailsAsync(Guid eventId, string detailsJson);
        Task<string?> GetEventDetailsAsync(Guid eventId);
    }
}
