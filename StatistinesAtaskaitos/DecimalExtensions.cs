using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StatistinesAtaskaitos
{
    public static class DecimalExtensions
    {
        public static string ToString(this decimal? value, string format)
        {
            if (value == null) return "";
            return value.Value.ToString(format);
        }
    }
    
}