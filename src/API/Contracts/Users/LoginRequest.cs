namespace API.Contracts.Users;

public record LoginRequest(
    string Email,
    string Password);