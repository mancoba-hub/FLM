using System;

namespace FLM.LisoMbiza
{
    public static class DataTypeExtension
    {
        /// <summary>
        /// Extension for int
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt32(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0;

            int.TryParse(value, out int result);
            return result;
        }

        /// <summary>
        /// Extension for DateTime
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            DateTime.TryParse(value, out DateTime dateTime);
            return dateTime;
        }

        /// <summary>
        /// Extension for decimal
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0;

            decimal.TryParse(value, out decimal amount);
            return amount;
        }
    }
}
