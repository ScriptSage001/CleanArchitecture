namespace SharedKernel.Abstractions.Tracing;

/// <summary>
/// Provides methods to access and manage the correlation ID used for tracing and logging purposes.
/// </summary>
public interface ICorrelationIdAccessor
{
    /// <summary>
    /// Retrieves the current correlation ID.
    /// </summary>
    /// <returns>The current correlation ID as a <see cref="Guid"/>.</returns>
    Guid Get();

    /// <summary>
    /// Sets the correlation ID to the specified value.
    /// </summary>
    /// <param name="correlationId">The correlation ID to set, represented as a string.</param>
    void Set(string correlationId);
}