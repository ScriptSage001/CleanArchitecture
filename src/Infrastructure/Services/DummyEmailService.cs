using Application.Abstractions;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class DummyEmailService(ILogger<DummyEmailService> logger) : IEmailService
{
    private readonly ILogger<DummyEmailService> _logger = logger;
    
    public Task SendWelcomeEmailAsync(string email, string name)
    {
        // Simulate sending an email
        _logger.LogInformation("Sending welcome email to {Email} for {Name}.", email, name);
        return Task.CompletedTask;
    }
}