using System;

namespace src.Helpers
{
    public static class StringExtensions
    {
        public static int AsInt(this string value)
        {
            int.TryParse(value, out var intValue);
            return intValue;
        }
        
        public static decimal AsDecimal(this string value)
        {
            decimal.TryParse(value, out var intValue);
            return intValue;
        }
        
        public static decimal AsDecimal(this string value, int accuracy)
        {
            return decimal.Round(AsDecimal(value), accuracy,MidpointRounding.AwayFromZero);
        }
    }
}