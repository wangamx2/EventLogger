namespace EventLogger.Application.Queries;

public class EventLogDto
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = default!;
    public string EventType { get; set; } = default!;
    public DateTime Timestamp { get; set; }
    public string DetailsJson { get; set; } = default!;
}
