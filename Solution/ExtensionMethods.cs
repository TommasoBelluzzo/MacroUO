#region Using Directives
using System;
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

        #region Int32
        public static String ToOrdinal(this Int32 value)
        {
            String suffix = String.Empty;

            if (value <= 0)
                return String.Concat(value, suffix);

            Int32 i = value - ((value / 100) * 100);

            if ((i == 11) || (i == 12) || (i == 13))
                suffix = "th";
            else
            {
                Int32 j = value % 10;

                switch (j)
                {
                    case 1:
                        suffix = "st";
                        break;

                    case 2:
                        suffix = "nd";
                        break;

                    case 3:
                        suffix = "rd";
                        break;

                    default:
                        suffix = "th";
                        break;
                }
            }

            return String.Concat(value, suffix);
        }
        #endregion

        #region String
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