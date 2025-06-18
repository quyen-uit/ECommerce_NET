using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Specifications;

namespace Core.Specifications.Categories
{
    public class CategorySpecParams : QueryStringParameter
    {
        public CategoryFilter Filter { get; set; } = new CategoryFilter();
    }

    public class CategoryFilter
    {
        public string? Name { get; set; }
        public string? ParentName { get; set; }
        public bool? IsActive { get; set; }
    }
}
