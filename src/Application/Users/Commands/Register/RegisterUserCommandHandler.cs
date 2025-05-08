using Application.Abstractions.Authentication;
using Application.Abstractions.Persistence;
using Application.Models;
using Domain.Entities;
using Domain.Errors;
using Domain.Events;
using Domain.ValueObjects;
using MassTransit;
using SharedKernel.Abstractions;
using SharedKernel.Abstractions.Tracing;
using SharedKernel.FunctionalTypes;
using SharedKernel.Messaging.Abstractions;

namespace Application.Users.Commands.Register;

/// <summary>
/// Handles the registration of a new user by validating the request, creating the user entity, 
/// and saving it to the repository.
/// </summary>
/// <param name="repository">The repository for accessing and managing user data.</param>
/// <param name="unitOfWork">The unit of work for managing transactions.</param>
/// <param name="passwordHasher">The service for hashing user passwords.</param>
public class RegisterUserCommandHandler(
    IRepository<User> repository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    ICorrelationIdAccessor correlationIdAccessor,
    IUserContext userContext)
    : ICommandHandler<RegisterUserCommand, UserModel>
{
    private readonly IRepository<User> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly ICorrelationIdAccessor _correlationIdAccessor = correlationIdAccessor;
    private readonly IUserContext _userContext = userContext;

    /// <summary>
    /// Handles the registration command by validating the request, creating a user, 
    /// and returning the created user model.
    /// </summary>
    /// <param name="request">The command containing user registration details.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A result containing the created user model or an error.</returns>
    public async Task<Result<UserModel>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRequest(request, cancellationToken);
        if (validationResult.IsFailure)
        {
            return Result.Failure<UserModel>(validationResult.Error);
        }

        var user = CreateUser(request);

        _userContext.SetUserName(user.UserName.Value);
        await _repository.Add(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return CreateUserModel(user);
    }

    /// <summary>
    /// Validates the registration request by checking for existing users and validating 
    /// the email and username.
    /// </summary>
    /// <param name="request">The command containing user registration details.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A result indicating success or failure of the validation.</returns>
    private async Task<Result> ValidateRequest(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var email = Email.Create(request.Email);
        if (email.IsFailure)
        {
            return Result.Failure(email.Error);
        }

        var userName = UserName.Create(request.UserName);
        if (userName.IsFailure)
        {
            return Result.Failure(userName.Error);
        }
        
        var existingUser = (await _repository
                                .Query(
                                    u =>
                                        u.Email == email.Value ||
                                        u.UserName == userName.Value,
                                    cancellationToken))
                                .AsEnumerable()
                                .FirstOrDefault();
        if (existingUser is not null)
        {
            return Result.Failure(DomainErrors.User.UserNameOrEmailAlreadyInUse);
        }

        return Result.Success();
    }

    /// <summary>
    /// Creates a new user entity based on the registration request.
    /// </summary>
    /// <param name="request">The command containing user registration details.</param>
    /// <returns>The created user entity.</returns>
    private User CreateUser(RegisterUserCommand request)
    {
        var email = Email.Create(request.Email).Value;
        var userName = UserName.Create(request.UserName).Value;

        var user = User.Create(
            userName: userName,
            email: email,
            fullName: request.FullName,
            passwordHash: _passwordHasher.Hash(request.Password),
            correlationId: _correlationIdAccessor.Get());

        return user;
    }

    /// <summary>
    /// Converts a user entity into a user model.
    /// </summary>
    /// <param name="user">The user entity to convert.</param>
    /// <returns>A user model representing the user entity.</returns>
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