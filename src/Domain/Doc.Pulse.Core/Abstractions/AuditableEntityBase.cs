using Doc.Pulse.Core.Entities._Kernel;

namespace Doc.Pulse.Core.Abstractions;

public abstract class AuditableEntityBase<IdType> : EntityBase<IdType>, IAuditableEntityBase
{
    public DateTimeOffset Created { get; set; }
    public int? CreatedUserId { get; set; }
    public UserStub? CreatedUser { get; set; }
    public DateTimeOffset Modified { get; set; }
    public int? ModifiedUserId { get; set; }
    public UserStub? ModifiedUser { get; set; }
}