using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.ComponentModel;

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
    }
}
