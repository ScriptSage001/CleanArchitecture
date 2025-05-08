using Application.Abstractions.Authentication;
using Application.Models;
using Application.Users.Queries;
using Domain.Entities;
using Domain.Errors;
using Domain.ValueObjects;
using MediatR;
using SharedKernel.Abstractions;
using SharedKernel.FunctionalTypes;
using SharedKernel.Messaging.Abstractions;

namespace Application.Users.Commands.Login;

internal sealed class LoginUserCommandHandler(
    IRepository<User> repository,
    IPasswordHasher passwordHasher,
    ITokenProvider tokenProvider)
    : ICommandHandler<LoginUserCommand, string>
{
    private readonly IRepository<User> _repository = repository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    
    public async Task<Result<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var email = Email.Create(request.Email);
        if (email.IsFailure)
        {
            return Result.Failure<string>(email.Error);
        }
        
        var user = (await _repository
                                    .Query(u => u.Email == email.Value, cancellationToken))
                                    .AsEnumerable()
                                    .FirstOrDefault();

        if (user is null)
        {
            return Result.Failure<string>(DomainErrors.User.UserNotFound);
        }
        
        var verified = _passwordHasher.Verify(request.Password, user.PasswordHash);

        if (!verified)
        {
            return Result.Failure<string>(DomainErrors.User.UserNotFound);
        }

        var userModel = new UserModel
        {
            Id = user.Id,
            Email = user.Email.Value,
            UserName = user.UserName.Value
        };
        
        var token = _tokenProvider.Create(userModel);

        return token;
    }
}