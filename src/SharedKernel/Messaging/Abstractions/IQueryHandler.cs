using MediatR;
using SharedKernel.FunctionalTypes;

namespace SharedKernel.Messaging.Abstractions;

/// <summary>
/// Marker interface for the query handlers.
/// </summary>
/// <typeparam name="TQuery"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>;