using API.Contracts.Users;
using API.Extensions;
using API.Http;
using Application.Models;
using Application.Users.Commands.Login;
using Application.Users.Commands.Register;
using MediatR;

namespace API.Endpoints.Users;

/// <summary>
/// Defines the Minimal API Post endpoints for the User entity.
/// </summary>
public class Post : IEndpoint
{
    #region Public Endpoints

    /// <summary>
    /// Maps the HTTP Post endpoints for the User entity.
    /// </summary>
    /// <param name="app">The endpoint route builder used to define routes.</param>
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/register", RegisterUser)
                        .WithTags(Tags.Users)
                        .WithSummary("Register a new user.")
                        .Produces<UserModel>(StatusCodes.Status200OK)
                        .ProducesProblem(StatusCodes.Status404NotFound)
                        .ProducesProblem(StatusCodes.Status500InternalServerError);

        app.MapPost("users/login", LoginUser)
                            .WithTags(Tags.Users)
                            .WithSummary("Login an existing user.")
                            .Produces<string>(StatusCodes.Status200OK)
                            .ProducesProblem(StatusCodes.Status404NotFound)
                            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Handles the HTTP Post request to register a new user.
    /// </summary>
    /// <param name="request">The registration request containing user details.</param>
    /// <param name="mediator">The mediator used to send the registration command.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="IResult"/> representing the outcome of the registration process.</returns>
    private static async Task<IResult> RegisterUser(RegisterRequest request, IMediator mediator, CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand
        {
            UserName = request.UserName,
            Email = request.Email,
            FullName = request.FullName,
            Password = request.Password,
            ConfirmPassword = request.ConfirmPassword
        };

        var result = await mediator.Send(command, cancellationToken);
        return result.Match(Results.Ok, CustomResults.Problem);
    }

    /// <summary>
    /// Handles the HTTP Post request to log in an existing user.
    /// </summary>
    /// <param name="request">The login request containing user credentials.</param>
    /// <param name="mediator">The mediator used to send the login command.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="IResult"/> representing the outcome of the login process.</returns>
    private static async Task<IResult> LoginUser(LoginRequest request, IMediator mediator, CancellationToken cancellationToken)
    {
        var command = new LoginUserCommand
        {
            Email = request.Email,
            Password = request.Password
        };

        var result = await mediator.Send(command, cancellationToken);
        return result.Match(Results.Ok, CustomResults.Problem);
    }

    #endregion
}