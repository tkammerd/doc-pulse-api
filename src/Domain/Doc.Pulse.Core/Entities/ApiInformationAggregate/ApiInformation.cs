using Doc.Pulse.Core.Abstractions;

namespace Doc.Pulse.Core.Entities.ApiInformationAggregate;

public partial class ApiInformation : EntityBase<int>, IAggregateRoot, ISqlGenTimestampBase
{
}
