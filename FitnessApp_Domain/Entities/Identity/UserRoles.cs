using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp_Domain.Entities.Identity
{
    public class UserRoles : IdentityUserRole<Guid>
    {
        public virtual Roles Role { get; set; }
        public virtual Users User { get; set; }
    }
}
