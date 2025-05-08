using SharedKernel.Messaging.Abstractions;

namespace Application.Contracts.Events;

public record UserRegisteredIntegrationEvent : IEvent
{
    public Guid UserId { get; init; }
    public Guid CorrelationId { get; init; }
    public DateTime TimeStamp { get; init; }
}