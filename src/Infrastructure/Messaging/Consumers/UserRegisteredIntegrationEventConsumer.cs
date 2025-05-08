using Application.Abstractions.Authentication;
using Application.Contracts.Events;
using Infrastructure.Messaging.Common;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Messaging.Consumers;

public class UserRegisteredIntegrationEventConsumer(
    ILogger<UserRegisteredIntegrationEventConsumer> logger,
    IUserContext userContext)
    : BaseEventConsumer<UserRegisteredIntegrationEventConsumer, UserRegisteredIntegrationEvent>(logger, userContext)
{
    private readonly ILogger<UserRegisteredIntegrationEventConsumer> _logger = logger;

    protected override async Task ConsumeEventAsync(ConsumeContext<UserRegisteredIntegrationEvent> context)
    {
        // Handle the UserRegisteredIntegrationEvent here
        
        _logger.LogInformation("UserRegisteredIntegrationEvent received for UserId: {UserId} with CorrelationId: {CorrelationId}", 
            context.Message.UserId, context.Message.CorrelationId);
        
        await Task.CompletedTask;
    }
}