using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp_Domain.Models.Account
{
    public class UsersDto
    {
        public Guid Id { get; set; }

        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }

        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Place { get; set; }
        public List<string>? Roles { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool? Active { get; set; }
        public int? ActivityCount { get; set; }
        public int? ActivityDuration { get; set; }
    }
}
