namespace Doc.Pulse.Contracts.Communications.V1.ObjectCodes.Dtos;

public class ObjectCodeListDto
{
    public int? Id { get; set; } = default!;

    public int CodeNumber { get; set; }
    public string CodeName { get; set; } = null!;
    public int CodeCategoryId { get; set; }
    public bool Inactive { get; set; } = false;
}