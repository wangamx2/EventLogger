using EventLogger.Domain.Entities;
using EventLogger.Domain.Interfaces;
using MediatR;

namespace EventLogger.Application.Commands;

public class CreateEventLogCommandHandler : IRequestHandler<CreateEventLogCommand, Guid>
{
    private readonly IEventLogRepository _eventLogRepository;
    private readonly IMongoService _mongoService;

    public CreateEventLogCommandHandler(IEventLogRepository eventLogRepository, IMongoService mongoService)
    {
        _eventLogRepository = eventLogRepository;
        _mongoService = mongoService;
    }

    public async Task<Guid> Handle(CreateEventLogCommand request, CancellationToken cancellationToken)
    {
        var logId = Guid.NewGuid();

        var eventLog = new EventLog
        {
            Id = logId,
            UserId = request.UserId,
            EventType = request.EventType,
            Timestamp = request.Timestamp
        };

        await _eventLogRepository.AddUserEventAsync(eventLog);
        await _mongoService.SaveEventDetailsAsync(logId, request.DetailsJson);

        return logId;
    }
}
