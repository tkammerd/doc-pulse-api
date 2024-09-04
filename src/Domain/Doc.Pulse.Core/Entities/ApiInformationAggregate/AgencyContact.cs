using AppDmDoc.SharedKernel.Core.Abstractions;

namespace Doc.Pulse.Core.Entities.ApiInformationAggregate;

public class AgencyContact(string agency, string section, string name, string attn, string email, string phone) : ValueObject //BaseEntity<string>
{
    public string Agency { get; private set; } = agency;
    public string Section { get; private set; } = section;

    public string Name { get; private set; } = name;
    public string Attn { get; private set; } = attn;
    public string Email { get; private set; } = email;
    public string Phone { get; private set; } = phone;

    private AgencyContact() : this(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty) { }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Agency;
        yield return Section;
        yield return Name;
        yield return Attn;
        yield return Email;
        yield return Phone;
    }
}