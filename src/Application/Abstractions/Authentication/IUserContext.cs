namespace Application.Abstractions.Authentication;

/// <summary>
/// Interface for the current user context.
/// </summary>
public interface IUserContext
{
    Guid UserId { get; }
    
    bool IsAuthenticated { get; }
    
    string GetUserName();
    
    void SetUserName(string userName);
}