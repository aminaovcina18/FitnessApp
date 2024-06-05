using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Requests.Activity
{
    public record DeleteActivityRequest(int ActivityId, Guid? UpdatedBy)
    {
    }
}
