using CinemaApp.Domain.Abstractions;
using CinemaApp.Domain.Entities;
using CinemaApp.Domain.Errors;
using CinemaApp.Domain.Repositories;

namespace CinemaApp.Application.Commands.Admin.MovieSchedules;

using CinemaApp.Application.Abstractions.Messaging;
using System;

    public sealed record CreateMovieScheduleCommand(
        Guid CinemaRoomId, 
        Guid MovieId, 
        DateTime StartTime, 
        DateTime EndTime) : ICommand<Guid>;

    public sealed class CreateMovieScheduleCommandHandler : ICommandHandler<CreateMovieScheduleCommand, Guid>
    {
        private readonly IMovieScheduleRepository _movieScheduleRepository;
        private readonly IMovieRepository _movieRepository;

        public CreateMovieScheduleCommandHandler(IMovieScheduleRepository movieScheduleRepository, IMovieRepository movieRepository)
        {
            _movieScheduleRepository = movieScheduleRepository;
            _movieRepository = movieRepository;
        }

        public async Task<Result<Guid>> Handle(CreateMovieScheduleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var movie = await _movieRepository.GetByIdAsync(request.MovieId);
                if (movie is null)
                {
                    return Result.Failure<Guid>(MovieErrors.MovieNotFound);
                }

                bool isRoomReserved =
                   await _movieScheduleRepository.IsRoomReserved(request.CinemaRoomId, request.StartTime, request.EndTime);

                if (isRoomReserved)
                {
                    return Result.Failure<Guid>(ScheduleErrors.ScheduleOverlap);
                }

                var movieSchedule = MovieSchedule.Create(
                    request.CinemaRoomId,
                    movie,
                    request.StartTime,
                    request.EndTime
                );

                await _movieScheduleRepository.AddAsync(movieSchedule);
                return Result.Success(movieSchedule.Id);
            }
            catch (Exception e)
            {
                return Result.Failure<Guid>(Error.FromException(e));
            }
        }
    }