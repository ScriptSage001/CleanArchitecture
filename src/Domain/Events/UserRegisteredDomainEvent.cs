using SharedKernel.Messaging.Abstractions;

namespace Domain.Events;

public class UserRegisteredDomainEvent : IDomainEvent
{
    public Guid Id { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public Guid CorrelationId { get; init; }
    public DateTime TimeStamp { get; init; } = DateTime.UtcNow;
}