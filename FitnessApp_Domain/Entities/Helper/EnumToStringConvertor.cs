using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp_Domain.Entities.Helper
{
    public class EnumToStringConverter<T> : ValueConverter<T, string> where T : Enum
    {
        public EnumToStringConverter() : base(v => v.ToString(), v => (T)Enum.Parse(typeof(T), v))
        { }
    }
}
