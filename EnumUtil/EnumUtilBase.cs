using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace EnumUtilities
{
    /// <summary>
    /// Provides all the functions of EnumUtil without Enum only type safety.
    /// 
    /// Do not use this class directly if possible.
    /// Go through EnumUtil class instead for true type safety.
    /// </summary>
    /// <typeparam name="E">Enum</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class EnumUtilBase<E>
        where E :/*Class,*/ IComparable, IFormattable, IConvertible
    {
        internal EnumUtilBase() { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T BitwiseOr<T>(T left, T right) where T : struct, E
            => EnumCompiledCache<T>.BitwiseOr(left, right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T BitwiseAnd<T>(T left, T right) where T : struct, E
            => EnumCompiledCache<T>.BitwiseAnd(left, right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T BitwiseExclusiveOr<T>(T left, T right) where T : struct, E
            => EnumCompiledCache<T>.BitwiseExclusiveOr(left, right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T BitwiseNot<T>(T value) where T : struct, E
            => EnumCompiledCache<T>.BitwiseNot(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasFlag<T>(T value, T flag) where T : struct, E
            => EnumCompiledCache<T>.HasFlag(value, flag);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Y GetAttribute<Y, T>()
            where T : struct, E
            where Y : Attribute
        {
            return typeof(T).GetCustomAttribute<Y>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Y> GetAttributes<Y, T>()
            where T : struct, E
            where Y : Attribute
        {
            return typeof(T).GetCustomAttributes<Y>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Y GetAttribute<Y, T>(T value)
            where T : struct, E
            where Y : Attribute
        {
            FieldInfo field = typeof(T)
                .GetField(Enum.GetName(typeof(T), value), BindingFlags.Public | BindingFlags.Static);

            return field.GetCustomAttribute<Y>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Y> GetAttributes<Y, T>(T value)
            where T : struct, E
            where Y : Attribute
        {
            FieldInfo field = typeof(T)
                .GetField(Enum.GetName(typeof(T), value), BindingFlags.Public | BindingFlags.Static);

            return field.GetCustomAttributes<Y>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasAttribute<Y, T>()
            where T : struct, E
            where Y : Attribute
        {
            return typeof(T).IsDefined(typeof(Y), false);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasFlagsAttribute<T>() where T : struct, E
            => typeof(T).IsDefined(typeof(FlagsAttribute), false);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetName<T>(T value) where T : struct, E
            => Enum.GetName(typeof(T), value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Parse<T>(string value) where T : struct, E
            => (T)Enum.Parse(typeof(T), value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetValues<T>() where T : struct, E
            => (T[])Enum.GetValues(typeof(T));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string[] GetNames<T>() where T : struct, E
            => Enum.GetNames(typeof(T));

        public static T SetFlag<T>(T value, T flag, bool on = true) where T : struct, E
            => on ? BitwiseOr(value, flag) : BitwiseAnd(value, BitwiseNot(flag));

        public static T UnsetFlag<T>(T value, T flag) where T : struct, E
            => SetFlag(value, flag, false);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse<T>(string value, out T result) where T : struct, E
            => Enum.TryParse(value, out result);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse<T>(string value, bool ignoreCase, out T result)
            where T : struct, E
            => Enum.TryParse(value, ignoreCase, out result);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type GetUnderlyingType<T>() where T : struct, E
            => Enum.GetUnderlyingType(typeof(T));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ToByte<T>(T val) where T : struct, E
            => EnumCompiledCache<T>.ToByte(val);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte ToSByte<T>(T val) where T : struct, E
            => EnumCompiledCache<T>.ToSByte(val);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short ToInt16<T>(T val) where T : struct, E
            => EnumCompiledCache<T>.ToInt16(val);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ToUInt16<T>(T val) where T : struct, E
            => EnumCompiledCache<T>.ToUInt16(val);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToInt32<T>(T val) where T : struct, E
            => EnumCompiledCache<T>.ToInt32(val);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ToUInt32<T>(T val) where T : struct, E
            => EnumCompiledCache<T>.ToUInt32(val);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ToInt64<T>(T val) where T : struct, E
            => EnumCompiledCache<T>.ToInt64(val);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ToUInt64<T>(T val) where T : struct, E
            => EnumCompiledCache<T>.ToUInt64(val);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FieldInfo[] GetEnumFields<T>() where T : struct, E
            => typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static);

        public static IReadOnlyDictionary<T, DescriptionAttribute> GetValueDescription<T>()
            where T : struct, E
        {
            return GetValueAttribute<T, DescriptionAttribute>();
        }

        public static IReadOnlyDictionary<T, NameAttribute<DescriptionAttribute>> GetValueNameDescription<T>()
            where T : struct, E
        {
            return GetValueNameAttribute<T, DescriptionAttribute>();
        }

        public static IReadOnlyDictionary<T, Tuple<string, IEnumerable<Attribute>>> GetValueNameAttributes<T>()
            where T : struct, E
        {
            FieldInfo[] fields = GetEnumFields<T>();

            var dict = new Dictionary<T, Tuple<string, IEnumerable<Attribute>>>(fields.Length);

            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                dict[(T)field.GetRawConstantValue()] = Tuple.Create(field.Name, field.GetCustomAttributes());
            }

            return dict;
        }

        public static IReadOnlyDictionary<string, ValueAttribute<T, Y>> GetNameValueAttribute<T, Y>()
            where T : struct, E
            where Y : Attribute
        {
            FieldInfo[] fields = GetEnumFields<T>();

            var dict = new Dictionary<string, ValueAttribute<T, Y>>(fields.Length);

            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                dict[field.Name] = new ValueAttribute<T, Y>((T)field.GetRawConstantValue(), field.GetCustomAttribute<Y>());
            }

            return dict;
        }

        public static IReadOnlyDictionary<T, NameAttribute<Y>> GetValueNameAttribute<T, Y>()
            where T : struct, E
            where Y : Attribute
        {
            FieldInfo[] fields = GetEnumFields<T>();

            var dict = new Dictionary<T, NameAttribute<Y>>(fields.Length);

            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                dict[(T)field.GetRawConstantValue()] = new NameAttribute<Y>(field.Name, field.GetCustomAttribute<Y>());
            }

            return dict;
        }

        public static IReadOnlyDictionary<T, Y> GetValueAttribute<T, Y>()
            where T : struct, E
            where Y : Attribute
        {
            FieldInfo[] fields = GetEnumFields<T>();

            var dict = new Dictionary<T, Y>(fields.Length);

            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                dict[(T)field.GetRawConstantValue()] = field.GetCustomAttribute<Y>();
            }

            return dict;

        }

        public static IReadOnlyDictionary<string, T> GetNameValue<T>()
            where T : struct, E
        {
            FieldInfo[] fields = GetEnumFields<T>();

            var dict = new Dictionary<string, T>(fields.Length);

            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                dict[field.Name] = (T)field.GetRawConstantValue();
            }

            return dict;
        }

        public static IReadOnlyDictionary<T, string> GetValueName<T>()
            where T : struct, E
        {
            FieldInfo[] fields = GetEnumFields<T>();

            var dict = new Dictionary<T, string>(fields.Length);

            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                dict[(T)field.GetRawConstantValue()] = field.Name;
            }

            return dict;
        }
    }
}
