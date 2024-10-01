using AppDmDoc.SharedKernel.Core.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doc.Pulse.Core.Abstractions;

public abstract class EntityBase<IdType> : EntityBase
{
    public IdType Id { get; set; } = default!;
}

public abstract class EntityBase
{
    private readonly List<DomainEventBase> _domainEvents = [];

    [NotMapped]
    public IEnumerable<DomainEventBase> DomainEvents => _domainEvents.AsReadOnly();

    protected void RegisterDomainEvent(DomainEventBase domainEvent) => _domainEvents.Add(domainEvent);
    public void ClearEvents() => _domainEvents.Clear();

    public bool HasDomainEvents => _domainEvents.Count != 0;
}