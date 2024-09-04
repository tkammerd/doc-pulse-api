using Doc.Pulse.Core.Entities._Kernel;

namespace Doc.Pulse.Core.Abstractions;

public interface IAuditableEntityBase
{
    DateTimeOffset Created { get; set; }

    int? CreatedUserId { get; set; }

    UserStub? CreatedUser { get; set; }

    DateTimeOffset Modified { get; set; }

    int? ModifiedUserId { get; set; }

    UserStub? ModifiedUser { get; set; }
}
