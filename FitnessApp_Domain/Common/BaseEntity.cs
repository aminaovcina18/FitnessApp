using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp_Domain.Common
{
    public class BaseEntity<T> : BaseEntity
    {
        [Key]
        public T? Id { get; set; }
    }

    public class BaseEntity
    {
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
