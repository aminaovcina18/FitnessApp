using FitnessApp_Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Requests.Activity
{
    public class GetAllActivityGoalRequest
    {
        public Guid UserId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string OrderBy { get; set; }
        public OrderByDirectionType OrderByDirection { get; set; }
    }
}
