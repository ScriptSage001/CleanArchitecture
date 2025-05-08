using Domain.Events;
using Domain.ValueObjects;
using SharedKernel.Primitives;

namespace Domain.Entities;

/// <summary>
/// Represents a user in the system.
/// </summary>
/// <remarks>
/// This entity maps to the <c>Users</c> table in the database.
/// </remarks>
public sealed class User : AggregateRoot
{
    #region Constructor

    /// <summary>
    /// Parameterless constructor for EF Core.
    /// </summary>
    private User() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="User"/> class.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userName"></param>
    /// <param name="email"></param>
    /// <param name="fullName"></param>
    /// <param name="passwordHash"></param>
    /// <param name="correlationId"></param>
    private User(Guid id, UserName userName, Email email, string fullName, string passwordHash, Guid correlationId)
        : base(id)
    {
        UserName = userName;
        Email = email;
        FullName = fullName;
        PasswordHash = passwordHash;
        CorrelationId = correlationId;
    }
    
    #endregion
    
    #region Properties
    
    /// <summary>
    /// Gets or sets the unique userName for the user.
    /// </summary>
    public UserName UserName { get; private set; }
    
    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    public Email Email { get; private set; }
    
    /// <summary>
    /// Gets or sets the full name of the user.
    /// </summary>
    public string FullName { get; private set; }
    
    /// <summary>
    /// Gets or sets the hashed password of the user.
    /// </summary>
    public string PasswordHash { get; private set; }
    
    #endregion

    #region Public Methods

    /// <summary>
    /// Factory method to crate a new <see cref="User"/> instance.
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="email"></param>
    /// <param name="fullName"></param>
    /// <param name="passwordHash"></param>
    /// <param name="correlationId"></param>
    /// <returns></returns>
    public static User Create(UserName userName, Email email, string fullName, string passwordHash, Guid correlationId)
    {
        var user = new User(Guid.NewGuid(), userName, email, fullName, passwordHash, correlationId);
        var domainEvent = new UserRegisteredDomainEvent
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email.Value,
            CorrelationId = correlationId
        };
        user.RaiseDomainEvent(domainEvent);
        return user;
    }

    #endregion
}