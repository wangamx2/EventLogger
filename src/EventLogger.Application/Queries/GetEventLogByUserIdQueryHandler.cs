using EventLogger.Domain.Interfaces;
using MediatR;

namespace EventLogger.Application.Queries;

public class GetEventLogByUserIdQueryHandler : IRequestHandler<GetEventLogByUserIdQuery, List<EventLogDto>?>
{
    private readonly IEventLogRepository _eventLogRepository;
    private readonly IMongoService _mongoService;

    public GetEventLogByUserIdQueryHandler(IEventLogRepository eventLogRepository, IMongoService mongoService)
    {
        _eventLogRepository = eventLogRepository;
        _mongoService = mongoService;
    }

    public async Task<List<EventLogDto>> Handle(GetEventLogByUserIdQuery request, CancellationToken cancellationToken)
    {
        var events = await _eventLogRepository.GetUserEventsAsync(request.UserId);
        if (events == null) return null;

        var results = new List<EventLogDto>();
        foreach (var evt in events)
        {
            var detailsJson = await _mongoService.GetEventDetailsAsync(evt.Id);

            results.Add(new EventLogDto
            {
                Id = evt.Id,
                UserId = evt.UserId,
                EventType = evt.EventType,
                Timestamp = evt.Timestamp,
                DetailsJson = detailsJson
            });
        }

        return results;
    }
}
