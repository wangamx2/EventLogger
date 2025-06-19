namespace EventLogger.Domain.Entities;

public class EventLog
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = default!;
    public string EventType { get; set; } = default!;
    public DateTime Timestamp { get; set; }
}
