using MediatR;

namespace EventLogger.Application.Queries;

public class GetEventLogByUserIdQuery : IRequest<List<EventLogDto>>
{
    public string UserId { get; set; }
}
