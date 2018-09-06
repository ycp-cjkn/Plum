using System;

namespace ToBeRenamed.Extensions
{
    public static class StringExtensions
    {
        public static bool EqualsIgnoreCase(this string string1, string string2)
        {
            return string.Equals(string1, string2, StringComparison.OrdinalIgnoreCase);
        }
    }
}
