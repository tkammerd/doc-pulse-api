using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Doc.Pulse.Contracts.Bases;

public class DefaultQueryContractsBase
{
    public class PaginatedList : PaginatedRequestBase
    {
        //public const string _route = $"/api/{nameof(EntityType)}/List";
    }

    public class GetById //: GetByIdQueryBase<IdType>
    {
        //public const string _route = $"/api/{nameof(EntityType)}/GetById";

        [Required]
        public int Id { get; set; } = default!;
    }

    public class GetFirstByProperty
    {
        //public const string _route = $"/api/{nameof(EntityType)}/GetFirstByProperty";

        [FromQuery]
        [StringLength(1024)]
        public string? Filter { get; set; }
    }
}
