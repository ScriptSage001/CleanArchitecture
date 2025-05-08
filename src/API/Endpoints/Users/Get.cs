using API.Extensions;
using API.Http;
using Application.Models;
using Application.Users.Queries;
using MediatR;

namespace API.Endpoints.Users;

/// <summary>
/// Defines the Minimal API Get endpoints for the User entity.
/// </summary>
public class Get : IEndpoint
{
    #region Public Endpoints
    
    /// <summary>
    /// Maps the HTTP Get endpoints for the User entity.
    /// </summary>
    /// <param name="app"></param>
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("users/{userId:guid}", async (Guid userId, IMediator mediator, CancellationToken token) 
            => await GetUserByIdAsync(userId, mediator, token))
                        .RequireAuthorization()
                        .WithTags(Tags.Users)
                        .WithSummary("Fetch an user by id.")
                        .Produces<UserModel>(StatusCodes.Status200OK)
                        .ProducesProblem(StatusCodes.Status404NotFound)
                        .ProducesProblem(StatusCodes.Status500InternalServerError);
        
        app.MapGet("users/email/{email}", async (string email, IMediator mediator, CancellationToken token) 
            => await GetUserByEmailAsync(email, mediator, token))
                        .RequireAuthorization()
                        .WithTags(Tags.Users)
                        .WithSummary("Fetch an user by email.")
                        .Produces<UserModel>(StatusCodes.Status200OK)
                        .ProducesProblem(StatusCodes.Status404NotFound)
                        .ProducesProblem(StatusCodes.Status500InternalServerError);
        
        app.MapGet("users/username/{userName}", async (string userName, IMediator mediator, CancellationToken token) 
            => await GetUserByUserNameAsync(userName, mediator, token))
                        .RequireAuthorization()
                        .WithTags(Tags.Users)
                        .WithSummary("Fetch an user by username.")
                        .Produces<UserModel>(StatusCodes.Status200OK)
                        .ProducesProblem(StatusCodes.Status404NotFound)
                        .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
    
    #endregion
    
    #region Private Methods
    
    /// <summary>
    /// Handles the HTTP Get request to fetch a user by their ID.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="mediator"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    private static async Task<IResult> GetUserByIdAsync(Guid userId, IMediator mediator, CancellationToken token)
    {
        var query = new UserQuery
        {
            UserId = userId
        };
        
        var result = await mediator.Send(query, token);
        return result.Match(Results.Ok, CustomResults.Problem);
    }

    /// <summary>
    /// Handles the HTTP Get request to fetch a user by their email.
    /// </summary>
    /// <param name="email"></param>
    /// <param name="mediator"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    private static async Task<IResult> GetUserByEmailAsync(string email, IMediator mediator, CancellationToken token)
    {
        var query = new UserQuery
        {
            Email = email
        };
        
        var result = await mediator.Send(query, token);
        return result.Match(Results.Ok, CustomResults.Problem);
    }
    
    /// <summary>
    /// Handles the HTTP Get request to fetch a user by their username.
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="mediator"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    private static async Task<IResult> GetUserByUserNameAsync(string userName, IMediator mediator, CancellationToken token)
    {
        var query = new UserQuery
        {
            UserName = userName
        };
        
        var result = await mediator.Send(query, token);
        return result.Match(Results.Ok, CustomResults.Problem);
    }
    
    #endregion
}