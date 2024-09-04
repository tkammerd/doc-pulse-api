using Doc.Pulse.Core.Abstractions;

namespace Doc.Pulse.Core.Entities._Kernel;

public class UserStub : EntityBase<int>, ISqlGenTimestampBase
{
    public string Identifier { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;
}
