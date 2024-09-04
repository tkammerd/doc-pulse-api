using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc.Pulse.Core.Entities;

public partial class CodeCategory
{
    public CodeCategory()
    {
        ObjectCodes = new HashSet<ObjectCode>();
    }
    public int CategoryNumber { get; set; }
    public string CategoryShortName { get; set; } = null!;
    public string? CategoryName { get; set; }
    public bool Inactive { get; set; } = false;

    public virtual ICollection<ObjectCode>? ObjectCodes { get; set; }
}