using FitnessApp_Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Helpers.Error
{
    public static class ErrorHandlerHelper
    {
        public static int ReturnStatusCode(Exception e)
        {
            if (e is HttpStatusException httpException)
            {
                return (int)httpException.Status;
            }
            else if (e is KeyNotFoundException)
            {
                return 404;
            }
            else if (e is ArgumentException || e is ArgumentNullException)
            {
                return 400;
            }
            else if (e is UnauthorizedAccessException)
            {
                return 403;
            }

            return 500;
        }
    }
}
