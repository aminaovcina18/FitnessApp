using FitnessApp_Domain.Entities.Fitness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp_Domain.Models.Activity.RequestModels
{
    public class UpdateActivity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? Date { get; set; }
        public int? Duration { get; set; }
        public ActivityType? ActivityType { get; set; }
    }
}
