using MediatR;

namespace SharedKernel.Messaging.Abstractions;

/// <summary>
/// Marker Interface for an EventPublisher
/// Publish a event through the mediator pipeline to be handled by multiple handlers
/// </summary>
public interface IEventPublisher : IPublisher;