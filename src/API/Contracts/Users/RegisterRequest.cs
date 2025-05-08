namespace API.Contracts.Users;

public sealed record RegisterRequest(
    string UserName, 
    string Email, 
    string FullName, 
    string Password, 
    string ConfirmPassword);