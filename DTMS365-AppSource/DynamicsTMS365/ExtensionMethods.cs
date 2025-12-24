using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365
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

            return ( maxLength >= value.Length
                   ? value
                   : value.Substring(value.Length - maxLength, maxLength)
                   );
        }
    }
}