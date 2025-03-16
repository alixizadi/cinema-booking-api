using CinemaApp.Domain.Abstractions;
using MediatR;

namespace CinemaApp.Application.Abstractions.Messaging;

public interface IQueryHandler<TQuery, TResponse> 
    : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
    
}