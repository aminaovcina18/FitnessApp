using ApplicationCore.IRepositories;
using FitnessApp_Domain.Entities.Identity;
using FitnessApp_Domain.Models.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Helpers.Identity
{
    public class UserClaimsUtil : IUserClaimsUtil
    {
        public UserClaimsUtil(IHttpContextAccessor httpContextAccessor, IRepository<Users> appUserRepository, IRepository<Roles> rolesRepository)
        { 
            if (httpContextAccessor.HttpContext.User.Claims.Any())
            {
                var Id = Guid.Parse(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sub").Value);

                Expression<Func<Roles, bool>> criteriaRoles = e => e.UserRoles!.Any(u => u.UserId == Id);
                this.Roles = rolesRepository.GetAllWithCriteria(criteriaRoles).Select(s => s.Name).ToList();

                Expression<Func<Users, bool>> criteria = e => e.Id == Id;
                this.User = appUserRepository.Get(Id);
            }
        }

        public Guid UserId => this.User.Id;
        public Users User { get; set; }
        public List<string> Roles { get; set; }
        public bool IsAdmin => this.Roles!.Contains(RoleEnum.ADMIN.ToString());
    }
}
