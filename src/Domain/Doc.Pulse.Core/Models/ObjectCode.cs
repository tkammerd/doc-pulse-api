using Doc.Pulse.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc.Pulse.Core.Entities;

public partial class ObjectCode
{
    public int CodeNumber { get; set; }
    [PaginateFilterAttribute]
    public string CodeName { get; set; } = null!;
    public int CodeCategoryId { get; set; }
    public bool Inactive { get; set; } = false;

    public virtual CodeCategory? CodeCategory { get; set; }
}