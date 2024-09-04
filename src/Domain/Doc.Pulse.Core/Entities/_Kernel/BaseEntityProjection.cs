using System.ComponentModel.DataAnnotations.Schema;

namespace Doc.Pulse.Core.Entities._Kernel;

public class BaseEntityProjection<IdType>
{
    public IdType Id { get; set; } = default!;

    [NotMapped]
    public Type EntityType { get; } = default!;  // Use as Salt Prefix
}
