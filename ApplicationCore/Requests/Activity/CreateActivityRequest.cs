using FitnessApp_Domain.Entities.Fitness;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Requests.Activity
{
    public class CreateActivityRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public ActivityType ActivityType { get; set; }
        public Guid UserId { get; set; }
    }
}
