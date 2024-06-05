using FitnessApp_Domain.Common;
using FitnessApp_Domain.Entities.Fitness;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FitnessApp_Domain.Entities.Identity
{
    public class Users : IdentityUser<Guid>, ISoftDelete
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Active { get; set; }
        public bool ApprovedByAdmin { get; set; }

        public Guid? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        public bool IsDeleted { get; set; } = false;

        //user config 
        #region Goal Configuration
        public int? ActivityCount { get; set; }
        public int? ActivityDuration { get; set; }
        #endregion

        public virtual ICollection<UserClaims>? UserClaims { get; set; }
        public virtual ICollection<UserLogins>? UserLogins { get; set; }
        public virtual ICollection<UserTokens>? UserTokens { get; set; }
        public virtual ICollection<UserRoles>? UserRoles { get; set; }

        public virtual IEnumerable<Activity>? Activities { get; set; }
    }
}
