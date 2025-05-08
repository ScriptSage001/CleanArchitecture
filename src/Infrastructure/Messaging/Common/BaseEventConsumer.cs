using Application.Abstractions.Authentication;
using MassTransit;
using Microsoft.Extensions.Logging;
using LogContext = Serilog.Context.LogContext;

namespace Infrastructure.Messaging.Common;

/// <summary>
/// Base class for event consumers that consume messages of type <typeparamref name="TEvent"/>.
/// Implements the <see cref="IConsumer{TEvent}"/> interface from MassTransit.
/// </summary>
/// <typeparam name="T">The type of the consumer.</typeparam>
/// <typeparam name="TEvent">The type of the event being consumed.</typeparam>
public abstract class BaseEventConsumer<T, TEvent> : IConsumer<TEvent> where TEvent : class
{
    private readonly ILogger<BaseEventConsumer<T, TEvent>> _logger;
    private readonly IUserContext _userContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseEventConsumer{T,TEvent}"/> class.
    /// Implements the <see cref="IConsumer{TEvent}"/> interface from MassTransit.
    /// </summary>
    /// <typeparam name="T">The type of the consumer.</typeparam>
    /// <typeparam name="TEvent">The type of the event being consumed.</typeparam>
    /// <param name="logger">The logger instance used for logging information and errors.</param>
    /// <param name="userContext">The user context used to manage user-related information.</param>
    protected BaseEventConsumer(ILogger<BaseEventConsumer<T, TEvent>> logger,
        IUserContext userContext)
    {
        _logger = logger;
        _userContext = userContext;
    }

    /// <summary>
    /// Consumes the event and processes it.
    /// Logs the event consuming process and manages exceptions.
    /// </summary>
    /// <param name="context">The consume context containing the event data and metadata.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Consume(ConsumeContext<TEvent> context)
    {
        // Pushes the CorrelationId into the Serilog log context for structured logging.
        using (LogContext.PushProperty("CorrelationId", context.CorrelationId?.ToString()))
        {
            _logger.LogInformation("Consuming event {EventName} with CorrelationId: {CorrelationId}",
                typeof(TEvent).Name, context.CorrelationId);
            try
            {
                // Sets the username in the user context to the event type name.
                _userContext.SetUserName(typeof(TEvent).Name);

                // Calls the abstract method to consume the event-specific logic.
                await ConsumeEventAsync(context).ConfigureAwait(true);

                _logger.LogInformation("Event {EventName} consumed successfully with CorrelationId: {CorrelationId}",
                    typeof(TEvent).Name, context.CorrelationId);
            }
            catch (Exception e)
            {
                // Logs the error and rethrows the exception.
                _logger.LogError(e, "Error consuming event {EventName} with CorrelationId: {CorrelationId}",
                    typeof(TEvent).Name, context.CorrelationId);
                throw;
            }
        }
    }

    /// <summary>
    /// Abstract method to consume the event-specific logic asynchronously.
    /// Must be implemented by derived classes.
    /// </summary>
    /// <param name="context">The consume context containing the event data and metadata.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected abstract Task ConsumeEventAsync(ConsumeContext<TEvent> context);
}