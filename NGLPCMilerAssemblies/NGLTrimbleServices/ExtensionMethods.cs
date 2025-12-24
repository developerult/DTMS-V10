using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGLTrimbleServices
{

    public static class StringExtensions
    {
        public static string Left(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            maxLength = Math.Abs(maxLength);

            return (value.Length <= maxLength
                   ? value
                   : value.Substring(0, maxLength)
                   );
        }
        public static string Right(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            maxLength = Math.Abs(maxLength);

            return (maxLength >= value.Length
                   ? value
                   : value.Substring(value.Length - maxLength, maxLength)
                   );
        }

        public static string Mid(this string value, int iFrom, int iLength = 0)
        {
            if (string.IsNullOrEmpty(value)) return value;
            iFrom = Math.Abs(iFrom);
            iLength = Math.Abs(iLength);
            if (iFrom >= value.Length) { return value; }
            if (iLength == 0)
            {
                return value.Substring(iFrom);
            } else
            {
                return value.Substring(iFrom,iLength);
            }                     
        }
    }
}
