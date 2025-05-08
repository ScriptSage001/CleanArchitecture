using Application.Models;

namespace Application.Abstractions.Authentication;

public interface ITokenProvider
{
    string Create(UserModel user);
}