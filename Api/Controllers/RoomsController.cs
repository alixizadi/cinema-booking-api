using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using CinemaApp.Application.Queries.GetAllRooms;
using CinemaApp.Application.Queries.GetRoomSchedules;
using Microsoft.AspNetCore.Authorization;

namespace CinemaApp.Api.Controllers;

[Route("api/rooms")]
[ApiController]
[Authorize]
public class RoomsController : ControllerBase
{
    private readonly ISender _mediator;

    public RoomsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    
    public async Task<IActionResult> GetAllRooms(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllRoomsQuery(), cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpGet("{roomId:guid}/schedules")]
    public async Task<IActionResult> GetRoomSchedules(Guid roomId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetRoomSchedulesQuery(roomId), cancellationToken);

        if (result.IsFailure)
            return NotFound(result.Error);

        return Ok(result.Value);
    }
}