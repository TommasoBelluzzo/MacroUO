#region Using Directives
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using MacroUO.Properties;
#endregion

namespace MacroUO
{
    internal static class ExtensionMethods
    {
        #region Decimal
        public static String ToStringTime(this Decimal value)
        {
            TimeSpan span = TimeSpan.FromMilliseconds((Double)value);
            Int32 days = (Int32)Math.Floor(span.TotalDays);
            Int32 hours = span.Hours;
            Int32 minutes = span.Minutes;

            StringBuilder builder = new StringBuilder();

            if (days > 0)
                builder.Append(String.Format(CultureInfo.CurrentCulture, "{0:0}d ", days));

            if (hours > 0)
                builder.Append(String.Format(CultureInfo.CurrentCulture, "{0:0}h ", hours));

            if (minutes > 0)
                builder.Append(String.Format(CultureInfo.CurrentCulture, "{0:0}m ", minutes));

            builder.Append(String.Format(CultureInfo.CurrentCulture, "{0:0}s", span.Seconds));

            return builder.ToString();
        }
        #endregion

        #region String
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public static T ToEnum<T>(this String value, Boolean ignoreCase = false) where T : struct, IComparable, IConvertible, IFormattable
        {
            if (!typeof(T).IsEnum)
                throw new InvalidOperationException("The specified type is not an enumerated type.");

            if (value == null)
                throw new ArgumentNullException("value");

            if ((value.Length == 0) || value.All(Char.IsWhiteSpace))
                throw new ArgumentException(Resources.ErrorStringEmpty, "value");

            T result;

            if (!Enum.TryParse(value, ignoreCase, out result))
                throw new ArgumentException(Resources.ErrorStringEnumerationMember, "value");

            return result;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public static String[] SplitAndTrim(this String value, params Char[] separators)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            if ((value.Length == 0) || value.All(Char.IsWhiteSpace))
                throw new ArgumentException(Resources.ErrorStringEmpty, "value");

            if (separators == null)
                throw new ArgumentNullException("separators");

            if (separators.Length == 0)
                throw new ArgumentException(Resources.ErrorStringSeparators, "separators");

            String[] valueChunks = value.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            return Array.ConvertAll(valueChunks, x => x.Trim());
        }

        public static String[] SplitAndTrim(this String value, params String[] separators)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            if ((value.Length == 0) || value.All(Char.IsWhiteSpace))
                throw new ArgumentException(Resources.ErrorStringEmpty, "value");

            if (separators == null)
                throw new ArgumentNullException("separators");

            if (separators.Length == 0)
                throw new ArgumentException(Resources.ErrorStringSeparators, "separators");

            String[] valueChunks = value.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            return Array.ConvertAll(valueChunks, x => x.Trim());
        }
        #endregion
    }
}