namespace Application.Models;

public abstract record BaseModel
{
    public Guid Id { get; init; }
    
    public DateTime CreatedOn { get; init; }
    
    public string CreatedBy { get; init; } = string.Empty;
    
    public DateTime UpdatedOn { get; init; }
    
    public string UpdatedBy { get; init; } = string.Empty;
    
    public bool IsDeleted { get; init; }
}