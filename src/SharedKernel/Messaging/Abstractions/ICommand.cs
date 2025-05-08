using MediatR;
using SharedKernel.FunctionalTypes;

namespace SharedKernel.Messaging.Abstractions;

/// <summary>
/// Marker interface for the commands in the application.
/// </summary>
public interface ICommand : IRequest<Result>;

/// <summary>
/// Marker interface for the commands with a response.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface ICommand<TResponse> : IRequest<Result<TResponse>>;