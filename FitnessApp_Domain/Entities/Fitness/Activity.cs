using FitnessApp_Domain.Common;
using FitnessApp_Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp_Domain.Entities.Fitness
{
    public class Activity : BaseEntity<int>, ISoftDelete
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int Duration { get; set; }
        public bool IsDeleted { get; set; } = false;
        public ActivityType ActivityType { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual Users User { get; set; }
    }
}
