using EventLogger.Application.Commands;
using EventLogger.Application.Queries;
using MediatR;

namespace EventLogger.Api.Endpoints;

public static class EventLogEndpoints
{
    public static IEndpointRouteBuilder MapEventLogEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/events", async (CreateEventLogCommand cmd, ISender sender) =>
        {
            if (string.IsNullOrWhiteSpace(cmd?.UserId) ||
                string.IsNullOrWhiteSpace(cmd?.EventType) ||
                string.IsNullOrWhiteSpace(cmd?.DetailsJson))
            {
                return Results.BadRequest("Missing required fields.");
            }

            var id = await sender.Send(cmd);
            return Results.Created($"/events/{id}", new { id });
        })
        .WithName("CreateEventLog")
        .WithSummary("Create a new event log")
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .WithOpenApi();

        app.MapGet("/events/{userId}", async (string userId, ISender sender) =>
        {
            var result = await sender.Send(new GetEventLogByUserIdQuery { UserId = userId });
            return result is not null ? Results.Ok(result) : Results.NotFound();
        })
        .WithName("GetEventLogsByUser")
        .WithSummary("Get all event logs for a user")
        .Produces<List<EventLogDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithOpenApi();

        return app;
    }
}
