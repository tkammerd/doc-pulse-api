using System.ComponentModel.DataAnnotations;

namespace Doc.Pulse.Contracts.Bases;

public class GetByIdQueryBase<IdType>
{
    [Required]
    //[StringLength(256)]
    public IdType Id { get; set; } = default!;
}
