using MediatR;

namespace EventLogger.Application.Commands;

public class CreateEventLogCommand : IRequest<Guid>
{
    public string UserId { get; set; } = default!;
    public string EventType { get; set; } = default!;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string DetailsJson { get; set; } = default!;
}
