using System;
using System.Reflection;

namespace EnumUtilities
{
    internal static class ReflectionCache<T>
        where T : struct, IComparable, IFormattable, IConvertible
    {
        static ReflectionCache()
        {
            var fields = typeof(T)
                .GetFields(BindingFlags.Public | BindingFlags.Static);

            T[] values = new T[fields.Length];
            for (int i = 0; i < fields.Length; i++)
            {
                values[i] = (T)fields[i].GetRawConstantValue();
            }

            Fields = fields;
            FieldValues = values;
        }

        internal static readonly FieldInfo[] Fields;
        internal static readonly T[] FieldValues;
    }
}
