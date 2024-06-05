using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness_WebCore.Helpers
{
    public class TokenIntrospect
    {
        public string Iss { get; set; }
        public int Nbf { get; set; }
        public int Exp { get; set; }
        public string[] Aud { get; set; }
        public string? Client_Id { get; set; }
        public string? Sub { get; set; }
        public int? Auth_Time { get; set; }
        public string? Idp { get; set; }
        public string? Amr { get; set; }
        public string? Role { get; set; }
        public string? Jti { get; set; }
        public int? Iat { get; set; }
        public bool? Active { get; set; }
        public string? Scope { get; set; }
        public string? Name { get; set; }
    }
}
