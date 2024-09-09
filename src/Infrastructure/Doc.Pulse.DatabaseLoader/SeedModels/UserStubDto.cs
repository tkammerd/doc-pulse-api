using Doc.Pulse.Core.Entities._Kernel;

namespace Doc.Pulse.DatabaseLoader.SeedModels;

internal class UserStubDto
{
    public int Id { get; set; } = default!;
    public string Identifier { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;

    public T ToEntity<T>() where T : UserStub, new()
    {
        return new T()
        {
            Id = Id,
            Identifier = ParsingHelpers.TrimWithDefault(Identifier, string.Empty),
            DisplayName = ParsingHelpers.TrimWithDefault(DisplayName, string.Empty)
        };
    }
}