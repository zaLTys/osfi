using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StatistikosFormos
{
    public static class ConversionExtensions
    {
        public static decimal AsDecimal(this object value)
        {
            return Convert.ToDecimal(value);
        }

        public static decimal? AsDecimalX(this object value)
        {
            if (value == DBNull.Value) return null;
            return Convert.ToDecimal(value);
        }

        public static int AsInt(this object value)
        {
            return Convert.ToInt32(value);
        }
    }
}
