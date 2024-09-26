using Doc.Pulse.Core.Abstractions;
using Doc.Pulse.Core.Entities.ValueObjects;

namespace Doc.Pulse.Core.Entities.ApiInformationAggregate;

public partial class ApiInformation
{
    public ApiInformation() { }
    public ApiInformation(string name, string description, string maintainer, string owner, EffectiveDate effective)
    {
        Name = name;
        Description = description;
        Maintainer = maintainer; 
        Owner = owner;
        Effective = effective;
    }

    public string? Name { get; private set; } = default;
    public string? Description { get; private set; } = default;

    public string? Maintainer { get; private set; }
    public string? Owner { get; private set; }

    public EffectiveDate? Effective { get; set; }


    public static ApiInformation New(string name, string description, string maintainer, string owner, EffectiveDate effective) => new(name, description, maintainer, owner, effective);
}
