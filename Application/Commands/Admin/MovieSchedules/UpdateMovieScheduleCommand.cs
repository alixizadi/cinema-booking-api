using CinemaApp.Application.Abstractions.Messaging;
using CinemaApp.Domain.Abstractions;
using CinemaApp.Domain.Errors;
using CinemaApp.Domain.Repositories;

namespace CinemaApp.Application.Commands.Admin.MovieSchedules;

public sealed record UpdateMovieScheduleCommand(
    Guid MovieScheduleId, 
    DateTime StartTime, 
    DateTime EndTime) : ICommand;
    
    
public sealed class UpdateMovieScheduleCommandHandler : ICommandHandler<UpdateMovieScheduleCommand>
{
    private readonly IMovieScheduleRepository _movieScheduleRepository;

    public UpdateMovieScheduleCommandHandler(IMovieScheduleRepository movieScheduleRepository)
    {
        _movieScheduleRepository = movieScheduleRepository;
    }

    public async Task<Result> Handle(UpdateMovieScheduleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var movieSchedule = await _movieScheduleRepository.GetByIdAsync(request.MovieScheduleId);
            if (movieSchedule == null)
            {
                return Result.Failure(ScheduleErrors.ScheduleNotFound);
            }

            var updated = movieSchedule.UpdateTimes(request.StartTime, request.EndTime);
            if (!updated)
            {
                return Result.Failure(ScheduleErrors.TimesNotCorrect);
            }
            await _movieScheduleRepository.UpdateAsync(movieSchedule);
                
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(Error.FromException(e));
        }
    }
}