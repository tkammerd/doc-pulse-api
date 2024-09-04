using Doc.Pulse.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc.Pulse.DatabaseLoader.SeedModels;

internal class CodeCategoryDto
{
    public int CategoryNumber { get; set; }
    public string CategoryShortName { get; set; } = null!;
    public string? CategoryName { get; set; }
    public bool Inactive { get; set; } = false;

    public T ToEntity<T>() where T : CodeCategory, new()
    {
        return new T()
        {
            //Id = , 
            CategoryNumber = CategoryNumber,
            CategoryShortName = ParsingHelpers.TrimPreventNull(CategoryShortName, "CategoryShortName"),
            CategoryName = ParsingHelpers.TrimAllowNull(CategoryName),
            Inactive = Inactive
        };
    }
}