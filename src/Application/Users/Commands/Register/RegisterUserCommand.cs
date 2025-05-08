using Application.Models;
using SharedKernel.Messaging.Abstractions;

namespace Application.Users.Commands.Register;

public sealed record RegisterUserCommand : ICommand<UserModel>
{
    public string UserName {get; init;}
    
    public string Email { get; init; }
    
    public string FullName { get; init; }
    
    public string Password { get; init; }
    
    public string ConfirmPassword { get; init; }
}