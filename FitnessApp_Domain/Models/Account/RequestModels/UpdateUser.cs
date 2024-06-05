using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp_Domain.Models.Account.RequestModels
{
    public class UpdateUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? Active { get; set; }
        public int? ActivityCount { get; set; }
        public int? ActivityDuration { get; set; }
    }
}
