namespace SharedKernel.Primitives;

/// <summary>
/// Base Entity Class for Domain Entities
/// </summary>
public abstract class Entity : IEquatable<Entity>
{
    #region Constructor
    
    /// <summary>
    /// Constructor to initialize 
    /// the base properties of any entity
    /// </summary>
    /// <param name="id"></param>
    protected Entity(Guid id)
    {
        Id = id;
    }
    
    protected Entity() { }
    
    #endregion
    
    #region Properties

    /// <summary>
    /// Identifier Property for the Entities
    /// </summary>
    public Guid Id { get; private init; }
    
    /// <summary>
    /// Flag to check if the entity is deleted
    /// </summary>
    public bool IsDeleted { get; set; }
    
    #endregion

    #region Equatable Functions

    public bool Equals(Entity? other)
    {
        if (other is null ||
            other.GetType() != GetType())
        {
            return false;
        }

        return Id == other.Id;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null ||
            obj.GetType() != GetType() ||
            obj is not Entity entity)
        {
            return false;
        }

        return entity.Id == Id;
    }

    public static bool operator ==(Entity? first, Entity? second)
    {
        return first is not null && second is not null && first.Equals(second);
    }

    public static bool operator !=(Entity? first, Entity? second)
    {
        return !(first == second);
    }

    public override int GetHashCode() => Id.GetHashCode();

    #endregion
}