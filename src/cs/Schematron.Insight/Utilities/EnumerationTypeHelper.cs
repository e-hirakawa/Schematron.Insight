using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Schematron.Insight.Utilities
{
    public static class EnumerationTypeHelper
    {
        public static bool Contains<Enum>(this Enum target, Enum flags)
        {
            int a = Convert.ToInt32(target);
            int b = Convert.ToInt32(flags);
            return (a & b) == b;
        }
        public static bool NotContains<Enum>(this Enum target, Enum flags)
        {
            int a = Convert.ToInt32(target);
            int b = Convert.ToInt32(flags);
            return (a & (~b)) == 0;
        }

        public static T Append<T>(this T target, T flags) where T : struct
        {
            Type t = typeof(T);
            if (!t.IsEnum)
                throw new ArgumentException();
            int a = (int)Enum.Parse(t, target.ToString());
            int b = (int)Enum.Parse(t, flags.ToString());
            return (T)Enum.ToObject(t, a | b);
        }

        public static T Remove<T>(this T target, T flags) where T: struct
        {
            Type t = typeof(T);
            if (!t.IsEnum)
                throw new ArgumentException();
            int a = (int)Enum.Parse(t, target.ToString());
            int b = (int)Enum.Parse(t, flags.ToString());
            return (T)Enum.ToObject(t, a & (~b));
        }

        public static string DisplayName<T>(this T value)
        {
            FieldInfo info = value.GetType().GetField(value.ToString());
            var attrs = info.GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];

            return (attrs?.Length ?? 0) > 0 ? attrs[0].Name : value.ToString();
        }
        public static T GetValueFromDisplayName<T>(string name)
        {
            Type t = typeof(T);
            foreach (T s in Enum.GetValues(t))
            {
                if (s.DisplayName() == name)
                    return s;
            }
            return default(T);
        }
    }
}
