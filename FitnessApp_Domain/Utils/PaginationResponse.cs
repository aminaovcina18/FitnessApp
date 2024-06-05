using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp_Domain.Utils
{
    public class PaginationResponse<T> where T : class
    {
        public int Page { get; set; }
        public int Pages { get; set; }
        public IEnumerable<T> Data { get; set; }
        public int Total { get; set; }
    }
}
