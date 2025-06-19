using EventLogger.Application.Commands;
using EventLogger.Application.Queries;
using EventLogger.Domain.Entities;
using Moq;
using EventLogger.Domain.Interfaces;

namespace EventLogger.Tests;

public class EventLogTests
{
    private readonly Mock<IEventLogRepository> _mockSqlRepo = new();
    private readonly Mock<IMongoService> _mockMongo = new();

    [Fact]
    public async Task CreateEventLog_HappyPath_ShouldReturnEventId()
    {
        // Arrange
        var command = new CreateEventLogCommand
        {
            UserId = "user123",
            EventType = "LOGIN",
            DetailsJson = "{ \"ip\": \"127.0.0.1\" }"
        };

        var handler = new CreateEventLogCommandHandler(_mockSqlRepo.Object, _mockMongo.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, result);
        _mockSqlRepo.Verify(r => r.AddUserEventAsync(It.IsAny<EventLog>()), Times.Once);
        _mockMongo.Verify(m => m.SaveEventDetailsAsync(result, command.DetailsJson), Times.Once);
    }

    [Fact]
    public async Task GetEventLog_HappyPath_ShouldReturnCombinedResult()
    {
        var userId = "user123";
        _mockSqlRepo.Setup(r => r.GetUserEventsAsync(userId))
            .ReturnsAsync(new List<EventLog>
            {
                new EventLog
                {
                    Id = Guid.NewGuid(),
                    UserId = "user123",
                    EventType = "VIEW",
                    Timestamp = DateTime.UtcNow
                }
            });

        _mockMongo.Setup(m => m.GetEventDetailsAsync(It.IsAny<Guid>()))
            .ReturnsAsync("{ \"section\": \"dashboard\" }");

        var handler = new GetEventLogByUserIdQueryHandler(_mockSqlRepo.Object, _mockMongo.Object);

        var result = await handler.Handle(new GetEventLogByUserIdQuery { UserId = "user123" }, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Single(result);

        var log = result.First();
        Assert.Equal("user123", log.UserId);
        Assert.Contains("dashboard", log.DetailsJson);
    }

    [Fact]
    public async Task GetEventLog_UnknownId_ShouldReturnEmpty()
    {
        _mockSqlRepo.Setup(r => r.GetUserEventsAsync(It.IsAny<string>())).ReturnsAsync(new List<EventLog>());

        var handler = new GetEventLogByUserIdQueryHandler(_mockSqlRepo.Object, _mockMongo.Object);
        var result = await handler.Handle(new GetEventLogByUserIdQuery { UserId = "user123" }, CancellationToken.None);

        Assert.Empty(result);
    }

    [Fact]
    public async Task CreateEventLog_DatabaseFails_ShouldThrow()
    {
        var command = new CreateEventLogCommand
        {
            UserId = "user123",
            EventType = "DELETE",
            DetailsJson = "{}"
        };

        _mockSqlRepo.Setup(r => r.AddUserEventAsync(It.IsAny<EventLog>())).ThrowsAsync(new Exception("SQL failure"));

        var handler = new CreateEventLogCommandHandler(_mockSqlRepo.Object, _mockMongo.Object);

        await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
    }
}
