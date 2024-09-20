using Doc.Pulse.Core.Entities;

namespace Doc.Pulse.DatabaseLoader.SeedModels;

internal class AgencyDto
{
    public int AgencyId { get; set; }
    public string AgencyName { get; set; } = null!;
    public bool Inactive { get; set; } = false;

    public T ToEntity<T>() where T : Agency, new()
    {
        return new T()
        {
            Id = AgencyId,
            AgencyName = ParsingHelpers.TrimPreventNull(AgencyName,"AgencyName"),
            Inactive = Inactive
        };
    }
}