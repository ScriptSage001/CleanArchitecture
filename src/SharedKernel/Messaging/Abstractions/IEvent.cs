using MediatR;

namespace SharedKernel.Messaging.Abstractions;

/// <summary>
/// Marker Interface to Represent an Event
/// </summary>
public interface IEvent : INotification;