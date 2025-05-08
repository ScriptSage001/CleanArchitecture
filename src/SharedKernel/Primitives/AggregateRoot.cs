using SharedKernel.Messaging.Abstractions;

namespace SharedKernel.Primitives;

/// <summary>
/// Base Class for Aggregate Roots
/// </summary>
public abstract class AggregateRoot : Entity, IAuditable
{
    #region Fields

    /// <summary>
    /// List of domain events that have occurred on this aggregate root.
    /// </summary>
    private readonly List<IDomainEvent> _domainEvents = [];
    
    #endregion
    
    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateRoot"/> class.
    /// </summary>
    /// <param name="id"></param>
    protected AggregateRoot(Guid id) : base(id) { }
    
    /// <summary>
    /// Parameterless constructor for Autogenerate ID in DB scenarios.
    /// </summary>
    protected AggregateRoot() { }
    
    #endregion
    
    #region Properties
    
    /// <summary>
    /// Gets or sets the Correlation ID for the aggregate root.
    /// </summary>
    public Guid CorrelationId { get; set; }
    
    /// <summary>
    /// Gets or sets the Created On timestamp for the aggregate root.
    /// </summary>
    public DateTimeOffset CreatedOn { get; set; }
    
    /// <summary>
    /// Gets or sets the Created By user for the aggregate root.
    /// </summary>
    public string CreatedBy { get; set; }
    
    /// <summary>
    /// Gets or sets the Updated On timestamp for the aggregate root.
    /// </summary>
    public DateTimeOffset UpdatedOn { get; set; }
    
    /// <summary>
    /// Gets or sets the Updated By user for the aggregate root.
    /// </summary>
    public string UpdatedBy { get; set; }
    
    #endregion
    
    #region Domain Events

    /// <summary>
    /// Raises a domain event and adds it to the list of domain events.
    /// </summary>
    /// <param name="domainEvent"></param>
    public void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    /// <summary>
    /// Gets the list of domain events that have occurred on this aggregate root.   
    /// </summary>
    /// <returns></returns>
    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => [.. _domainEvents];

    /// <summary>
    /// Clears the list of domain events that have occurred on this aggregate root.
    /// </summary>
    public void ClearDomainEvents() => _domainEvents.Clear();

    #endregion
}