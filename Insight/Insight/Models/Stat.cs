using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insight.Models
{
    public class Stat
    {
        public object Field { get; set; }
        public float Value { get; set; }

        public Stat(object field, float value)
        {
            Field = field;
            Value = value;
        }

    }
}
