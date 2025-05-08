namespace Application.Abstractions;

public interface IEmailService
{
    Task SendWelcomeEmailAsync(string email, string name);
}