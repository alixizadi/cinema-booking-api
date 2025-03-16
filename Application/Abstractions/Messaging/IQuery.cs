using CinemaApp.Domain.Abstractions;
using MediatR;

namespace CinemaApp.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
    
}