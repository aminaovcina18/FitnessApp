using FitnessApp_Domain.Entities.Fitness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Requests.Activity
{
    public class UpdateActivityRequest
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? Date { get; set; }
        public int? Duration { get; set; }
        public ActivityType? ActivityType { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
