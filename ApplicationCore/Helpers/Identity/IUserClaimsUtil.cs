using FitnessApp_Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Helpers.Identity
{
    public interface IUserClaimsUtil
    {
        Guid UserId { get; }
        Users User { get; set; }
        List<string> Roles { get; set; }
        bool IsAdmin { get; }
    }
}
