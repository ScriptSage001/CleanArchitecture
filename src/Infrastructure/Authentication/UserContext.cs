using System.Security.Claims;
using Application.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Authentication;

public sealed class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    private readonly ClaimsPrincipal _user = httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal();
    private string? _userName;
    
    public bool IsAuthenticated => _user.Identity?.IsAuthenticated ?? false;

    public Guid UserId =>
        IsAuthenticated && 
        Guid.TryParse(_user.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id)
            ? id
            : throw new UnauthorizedAccessException("User ID not found or invalid");
    
    public string GetUserName()
    {
        if (string.IsNullOrWhiteSpace(_userName))
        {
            _userName = IsAuthenticated 
                            ? _user.FindFirst(ClaimTypes.Name)?.Value ?? throw new UnauthorizedAccessException("User name not found") 
                            : throw new UnauthorizedAccessException("User is not authenticated");
        }
        
        return _userName;
    }
    
    public void SetUserName(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
            throw new ArgumentException("User name cannot be null or empty", nameof(userName));
        
        _userName = userName;
    }
}