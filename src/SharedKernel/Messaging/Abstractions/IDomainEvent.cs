namespace SharedKernel.Messaging.Abstractions;

/// <summary>
/// Interface to Represent a Domain Event
/// </summary>
public interface IDomainEvent : IEvent
{
    public Guid Id { get; init; }
    
    public DateTime TimeStamp { get; init; }
    
    public Guid CorrelationId { get; init; }
}