using MediatR;
using SharedKernel.FunctionalTypes;

namespace SharedKernel.Messaging.Abstractions;

/// <summary>
/// Marker interface for the queries.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface IQuery<TResponse> : IRequest<Result<TResponse>>;