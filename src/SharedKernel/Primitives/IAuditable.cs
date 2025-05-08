namespace SharedKernel.Primitives;

/// <summary>
/// Base Interface for Auditable Entities
/// </summary>
public interface IAuditable
{
    /// <summary>
    /// The CorrelationId to track the request
    /// </summary>
    public Guid CorrelationId { get; set; }
    
    /// <summary>
    /// The Created On DateTimeOffset
    /// </summary>
    public DateTimeOffset CreatedOn { get; set; }
    
    /// <summary>
    /// The Created By User
    /// </summary>
    public string CreatedBy { get; set; }

    /// <summary>
    /// The Updated On DateTimeOffset
    /// </summary>
    public DateTimeOffset UpdatedOn { get; set; }

    /// <summary>
    /// The Updated By User
    /// </summary>
    public string UpdatedBy { get; set; }
}