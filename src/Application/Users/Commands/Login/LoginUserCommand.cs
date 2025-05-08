using SharedKernel.Messaging.Abstractions;

namespace Application.Users.Commands.Login;

/// <summary>
/// Represents a command to log in a user with their email and password.
/// </summary>
public sealed record LoginUserCommand : ICommand<string>
{
    /// <summary>
    /// Gets or initializes the email address of the user attempting to log in.
    /// </summary>
    public string Email { get; init; } = string.Empty;

    /// <summary>
    /// Gets or initializes the password of the user attempting to log in.
    /// </summary>
    public string Password { get; init; } = string.Empty;
}