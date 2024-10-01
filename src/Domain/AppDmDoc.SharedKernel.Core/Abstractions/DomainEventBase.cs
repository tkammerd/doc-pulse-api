using MediatR;

namespace AppDmDoc.SharedKernel.Core.Abstractions;

public abstract class DomainEventBase : INotification
{
    public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;

}
