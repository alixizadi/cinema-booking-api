using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using CinemaApp.Application.Queries.GetRoomScheduleSeats;

namespace CinemaApp.Api.Controllers;

[Route("api/rooms/{roomId:guid}/schedules/{scheduleId:guid}/seats")]
[ApiController]
public class SeatsController : ControllerBase
{
    private readonly ISender _mediator;

    public SeatsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetRoomScheduleSeats(Guid roomId, Guid scheduleId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetRoomScheduleSeatsQuery(roomId, scheduleId), cancellationToken);

        if (result.IsFailure)
            return NotFound(result.Error);

        return Ok(result.Value);
    }


}