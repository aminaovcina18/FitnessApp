using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness_WebCore.Helpers
{
    public class IdentityConfig
    {
        public string Client_Id { get; set; }
        public string Client_Secret { get; set; }
        public string Grant_Type { get; set; }
        public string Scope { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Token { get; set; }
        public string? Refresh_Token { get; set; }
    }
}
