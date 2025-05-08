using SharedKernel.Abstractions.Tracing;

namespace API.Services;

/// <summary>
/// Provides an implementation of the <see cref="ICorrelationIdAccessor"/> interface
/// to manage and retrieve correlation IDs for tracing and logging purposes.
/// </summary>
public class CorrelationIdAccessor : ICorrelationIdAccessor
{
    // Stores the current correlation ID, initialized with a new GUID.
    private Guid _correlationId = Guid.NewGuid();

    /// <summary>
    /// Retrieves the current correlation ID.
    /// </summary>
    /// <returns>The current correlation ID as a <see cref="Guid"/>.</returns>
    public Guid Get() => _correlationId;

    /// <summary>
    /// Sets the correlation ID to the specified value if it is a valid GUID.
    /// </summary>
    /// <param name="correlationId">The correlation ID to set, represented as a string.</param>
    public void Set(string correlationId)
    {
        if (!string.IsNullOrWhiteSpace(correlationId) &&
            Guid.TryParse(correlationId, out var id))
        {
            _correlationId = id;
        }
    }
}