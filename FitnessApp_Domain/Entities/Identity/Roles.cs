using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp_Domain.Entities.Identity
{
    public class Roles : IdentityRole<Guid>
    {
        public virtual ICollection<UserRoles>? UserRoles { get; set; }
    }
}
