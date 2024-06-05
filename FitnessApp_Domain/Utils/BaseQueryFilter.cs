using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp_Domain.Utils
{
    public class BaseQueryFilter
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string OrderBy { get; set; } = "CreatedOn";
        public string? Search { get; set; }
        public string OrderByDirection { get; set; } = "DESC";
    }
}
