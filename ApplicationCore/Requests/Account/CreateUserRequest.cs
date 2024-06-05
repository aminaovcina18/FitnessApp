using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Requests.Account
{
    public class CreateUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string Password { get; set; }
        public string? Username { get; set; }

        public List<string> Roles { get; set; }

        public Guid CreatedBy { get; set; }
        public int? ActivityCount { get; set; }
        public int? ActivityDuration { get; set; }
    }
}
