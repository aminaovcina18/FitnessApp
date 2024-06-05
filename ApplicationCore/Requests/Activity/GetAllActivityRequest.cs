using FitnessApp_Domain.Entities.Fitness;
using FitnessApp_Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Requests.Activity
{
    public class GetAllActivityRequest
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string OrderBy { get; set; }
        public OrderByDirectionType OrderByDirection { get; set; }
        public string? Search { get; set; }
        public ActivityType? ActivityType { get; set; }
        public DateTime? Date { get; set; }
        public Guid UserId { get; set; }
    }
}
