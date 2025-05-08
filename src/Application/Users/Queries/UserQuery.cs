using Application.Models;
using SharedKernel.Messaging.Abstractions;

namespace Application.Users.Queries;

public record UserQuery : IQuery<List<UserModel>>
{
    public Guid? UserId { get; init; }
    
    public string? UserName { get; init; }
    
    public string? Email { get; init; }
}