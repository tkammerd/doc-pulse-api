using MediatR;

namespace Doc.Pulse.Core.Abstractions;

public abstract class DomainEventBase : INotification
{
    public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;

}
