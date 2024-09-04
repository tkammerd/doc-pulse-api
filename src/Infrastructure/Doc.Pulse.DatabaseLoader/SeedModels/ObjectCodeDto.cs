using Doc.Pulse.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc.Pulse.DatabaseLoader.SeedModels;

internal class ObjectCodeDto
{
    public int CodeNumber { get; set; }
    public string CodeName { get; set; } = null!;
    public int CodeCategoryId { get; set; }
    public bool Inactive { get; set; } = false;

    public T ToEntity<T>() where T : ObjectCode, new()
    {
        return new T()
        {
            //Id = , 
            CodeNumber = CodeNumber,
            CodeName = ParsingHelpers.TrimPreventNull(CodeName, "CategoryShortName"),
            CodeCategoryId = CodeCategoryId,
            Inactive = Inactive
        };
    }
}