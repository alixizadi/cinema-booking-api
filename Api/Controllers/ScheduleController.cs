using System.Security.Claims;
using CinemaApp.Application.Commands.BookSeatCommand;
using CinemaApp.Application.Queries.GetRoomScheduleSeats;
using CinemaApp.Domain.Errors;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.Api.Controllers;

[Route("api/schedules")]
[ApiController]
[Authorize]
public class ScheduleController : ControllerBase
{
    private ISender _mediator;

    public ScheduleController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{roomId}/{movieScheduleId}/seats")]
    public async Task<IActionResult> GetSeats(Guid roomId, Guid movieScheduleId)
    {
        var result = await _mediator.Send(new GetRoomScheduleSeatsQuery(roomId, movieScheduleId));
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
    
    [HttpPost("book-seat")]
    public async Task<IActionResult> BookSeat([FromBody] BookSeatCommand request)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }

        request.UserId = Guid.Parse(userId);
        var result = await _mediator.Send(request);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}