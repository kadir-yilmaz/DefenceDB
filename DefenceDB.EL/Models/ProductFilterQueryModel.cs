using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefenceDB.EL.Models
{
    public class ProductFilterQueryModel
    {
        public string? CategorySlug { get; set; }
        public string? Country { get; set; }
        public string? Search { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 30;
        public Dictionary<string, List<string>> DynamicFilters { get; set; } = new();
    }
}
