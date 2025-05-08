using Application.Models;
using Domain.Entities;
using Domain.ValueObjects;
using SharedKernel.Abstractions;
using SharedKernel.FunctionalTypes;
using SharedKernel.Messaging.Abstractions;

namespace Application.Users.Queries;

/// <summary>
/// Handles user queries and retrieves user data based on the provided query parameters.
/// </summary>
/// <param name="repository">The repository for accessing user data.</param>
public class UserQueryHandler(IRepository<User> repository)
    : IQueryHandler<UserQuery, List<UserModel>>
{
    private readonly IRepository<User> _repository = repository;

    /// <summary>
    /// Handles the user query and retrieves a list of user models based on the query parameters.
    /// </summary>
    /// <param name="request">The user query containing filter criteria.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A result containing a list of user models.</returns>
    public async Task<Result<List<UserModel>>> Handle(UserQuery request, CancellationToken cancellationToken)
    {
        IQueryable<User> data;

        // Filter by UserId if provided
        if (request.UserId.HasValue)
        {
            data = await _repository.Query(u => u.Id == request.UserId, cancellationToken);
        }
        // Filter by UserName if provided
        else if (!string.IsNullOrWhiteSpace(request.UserName))
        {
            var userName = UserName.Create(request.UserName);
            if (userName.IsFailure)
            {
                return Result.Failure<List<UserModel>>(userName.Error);
            }
            
            data = await _repository.Query(u => u.UserName == userName.Value, cancellationToken);
        }
        // Filter by Email if provided
        else if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var email = Email.Create(request.Email);
            if (email.IsFailure)
            {
                return Result.Failure<List<UserModel>>(email.Error);
            }
            
            data = await _repository.Query(u => u.Email == email.Value, cancellationToken);
        }
        // Retrieve all users if no filter is provided
        else
        {
            data = await _repository.Query(u => true, cancellationToken);
        }

        // Convert the retrieved user entities to user models
        return data.AsEnumerable().Select(CreateUserModel).ToList();
    }

    /// <summary>
    /// Creates a UserModel from a User entity.
    /// </summary>
    /// <param name="user">The user entity to convert.</param>
    /// <returns>A UserModel representing the user entity.</returns>
    private static UserModel CreateUserModel(User user)
    {
        return new UserModel
        {
            Id = user.Id,
            UserName = user.UserName.Value,
            Email = user.Email.Value,
            FullName = user.FullName,
            CreatedBy = user.CreatedBy,
            CreatedOn = user.CreatedOn.UtcDateTime,
            UpdatedBy = user.UpdatedBy,
            UpdatedOn = user.UpdatedOn.UtcDateTime,
            IsDeleted = user.IsDeleted
        };
    }
}