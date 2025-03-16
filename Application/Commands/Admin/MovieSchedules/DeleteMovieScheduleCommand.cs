using CinemaApp.Application.Abstractions.Messaging;
using CinemaApp.Domain.Abstractions;
using CinemaApp.Domain.Errors;
using CinemaApp.Domain.Repositories;

namespace CinemaApp.Application.Commands.Admin.MovieSchedules;

public sealed record DeleteMovieScheduleCommand(Guid MovieScheduleId) : ICommand;

public sealed class DeleteMovieScheduleCommandHandler : ICommandHandler<DeleteMovieScheduleCommand>
{
    private readonly IMovieScheduleRepository _movieScheduleRepository;

    public DeleteMovieScheduleCommandHandler(IMovieScheduleRepository movieScheduleRepository)
    {
        _movieScheduleRepository = movieScheduleRepository;
    }

    public async Task<Result> Handle(DeleteMovieScheduleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var movieSchedule = await _movieScheduleRepository.GetByIdAsync(request.MovieScheduleId);
            if (movieSchedule == null)
            {
                return Result.Failure(ScheduleErrors.ScheduleNotFound);
            }

            await _movieScheduleRepository.DeleteAsync(movieSchedule.Id);
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(Error.FromException(e));
        }
    }
}