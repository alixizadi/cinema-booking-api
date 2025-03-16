using CinemaApp.Application.Commands.Admin.Movies;
using CinemaApp.Application.Commands.Admin.MovieSchedules;
using CinemaApp.Application.Commands.AdminCommands;
using CinemaApp.Application.Commands.AdminQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.Api.Controllers;
[Route("api/admin")]
[ApiController]
[Authorize(Policy = "AdminPolicy")]
public class AdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator)
    {
        _mediator = mediator;
    }

    #region Room CRUD Endpoints

    // Create Room
    [HttpPost("rooms/create")]
    public async Task<IActionResult> CreateRoom([FromBody] CreateRoomCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    // Update Room Name
    [HttpPut("rooms/{roomId}/update")]
    public async Task<IActionResult> UpdateRoom(Guid roomId, [FromBody] UpdateRoomCommand command)
    {
        command = command with { RoomId = roomId }; // Ensure correct RoomId is passed in command
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    // Delete Room
    [HttpDelete("rooms/{roomId}/delete")]
    public async Task<IActionResult> DeleteRoom(Guid roomId)
    {
        var result = await _mediator.Send(new DeleteRoomCommand(roomId));
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    // Get All Rooms
    [HttpGet("rooms/all")]
    public async Task<IActionResult> GetAllRooms()
    {
        var result = await _mediator.Send(new GetAllRoomsQuery());
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    #endregion

    #region Movie CRUD Endpoints
    [HttpPost("movies/create")]
    public async Task<IActionResult> CreateMovie([FromForm] CreateMovieCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    // // Create Movie
    // [HttpPost("movies/create2")]
    // public async Task<IActionResult> CreateMovie2([FromBody] CreateMovieCommand command)
    // {
    //     var result = await _mediator.Send(command);
    //     return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    // }

    // Update Movie
    [HttpPut("movies/{movieId}/update")]
    public async Task<IActionResult> UpdateMovie(Guid movieId, [FromBody] UpdateMovieCommand command)
    {
        command = command with { MovieId = movieId }; // Ensure correct MovieId is passed in command
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    // Delete Movie
    [HttpDelete("movies/{movieId}/delete")]
    public async Task<IActionResult> DeleteMovie(Guid movieId)
    {
        var result = await _mediator.Send(new DeleteMovieCommand(movieId));
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    // Get All Movies
    [HttpGet("movies/all")]
    public async Task<IActionResult> GetAllMovies()
    {
        var result = await _mediator.Send(new GetAllMoviesQuery());
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    #endregion
    
    #region Movie Schedule CRUD Endpoints

    // Create Movie Schedule
    [HttpPost("schedule/create")]
    public async Task<IActionResult> CreateMovieSchedule([FromBody] CreateMovieScheduleCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    // Update Movie Schedule
    [HttpPut("schedule/{scheduleId}/update")]
    public async Task<IActionResult> UpdateMovieSchedule(Guid scheduleId, [FromBody] UpdateMovieScheduleCommand command)
    {
        command = command with { MovieScheduleId = scheduleId };
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    // Delete Movie Schedule
    [HttpDelete("schedule/{scheduleId}/delete")]
    public async Task<IActionResult> DeleteMovieSchedule(Guid scheduleId)
    {
        var result = await _mediator.Send(new DeleteMovieScheduleCommand(scheduleId));
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    // Get All Movie Schedules
    [HttpGet("schedule/{roomId}/all")]
    public async Task<IActionResult> GetAllMovieSchedulesByRoom(Guid roomId)
    {
        var result = await _mediator.Send(new GetAllMovieSchedulesQuery(roomId));
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    #endregion
    
}
