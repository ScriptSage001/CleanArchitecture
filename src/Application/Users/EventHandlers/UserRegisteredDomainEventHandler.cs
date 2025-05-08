using Application.Abstractions;
using Application.Contracts.Events;
using Domain.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedKernel.Messaging.Abstractions;

namespace Application.Users.EventHandlers;

public class UserRegisteredDomainEventHandler(
    ILogger<UserRegisteredDomainEventHandler> logger,
    IPublishEndpoint publisher,
    IEmailService emailService)
    : IDomainEventHandler<UserRegisteredDomainEvent>
{
    private readonly ILogger<UserRegisteredDomainEventHandler> _logger = logger;
    private readonly IPublishEndpoint _publisher = publisher;
    private readonly IEmailService _emailService = emailService;
    
    public async Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
    {
        // Handle the UserRegisteredDomainEvent here
        // For example, send a welcome email or log the event
        
        _logger.LogInformation("UserRegisteredDomainEvent received for UserId: {UserId} " +
                               "with CorrelationId: {CorrelationId}.", 
                                notification.Id, notification.CorrelationId);
        
        await _emailService.SendWelcomeEmailAsync(notification.Email, notification.FullName);
        
        // Perform any additional actions needed for the event
        // For example, you might want to publish an integration event or send a notification
        
        var userRegisteredIntegrationEvent = new UserRegisteredIntegrationEvent
        {
            UserId = notification.Id,
            CorrelationId = notification.CorrelationId
        };
        
        await _publisher
                .Publish(
                    userRegisteredIntegrationEvent,
                    ctx => ctx.CorrelationId = notification.CorrelationId,
                    cancellationToken)
                .ConfigureAwait(false);
        
        await Task.CompletedTask;
    }
}