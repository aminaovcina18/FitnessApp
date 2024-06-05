using FitnessApp_Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp_Domain.Models.Activity.RequestModels
{
    public class GetAllActivity : BaseQueryFilter
    {
        public Guid UserId { get; set; }
        public string? ActivityType { get; set; }
        public DateTime? Date { get; set; }
    }
}
