using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace EnumUtilities
{
    /// <summary>
    /// Uses Linq Expressions to compile Enum functions
    /// 
    /// (c) Alexandre Rogozine 2016
    /// </summary>
    /// <typeparam name="T">Enum Type</typeparam>
    internal static class EnumCompiledCache<T>
        where T : struct, IComparable, IFormattable, IConvertible
    {
        #region Generate Functions

        private static Func<T, Y> GenerateConvertTo<Y>() 
            where Y : struct, IComparable, IFormattable, IConvertible, IComparable<Y>, IEquatable<Y>
        {
            var value = Expression.Parameter(typeof(T));
            UnaryExpression ue = Expression.Convert(value, typeof(Y));
            return Expression.Lambda<Func<T, Y>>(ue, value)
                .Compile();
        }

        private static Func<Y, T> GenerateConvertFrom<Y>()
            where Y : struct, IComparable, IFormattable, IConvertible, IComparable<Y>, IEquatable<Y>
        {
            var value = Expression.Parameter(typeof(Y));
            UnaryExpression ue = Expression.Convert(value, typeof(T));
            return Expression.Lambda<Func<Y, T>>(ue, value)
                .Compile();
        }

        private static Func<Y, bool> GenerateIsDefined<Y>()
            where Y : struct, IComparable, IFormattable, IConvertible, IComparable<Y>, IEquatable<Y>
        {
            var value = Expression.Parameter(typeof(Y), "value");
            var convVal = Expression.Convert(value, typeof(T));

            Expression<Func<T, bool>> lookup =
                val => Array.IndexOf(ReflectionCache<T>.FieldValues, val) >= 0;

            return Expression.Lambda<Func<Y, bool>>(Expression.Invoke(lookup, convVal), value)
                .Compile();
        }

        private static Func<T, T, bool> GenerateHasFlag()
        {
            var dm = new DynamicMethod("HasFlag",
                typeof(bool),
                new[] { typeof(T), typeof(T) },
                true);

            ILGenerator generator = dm.GetILGenerator();

            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.And);
            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Ceq);
            generator.Emit(OpCodes.Ret);

            return (Func<T, T, bool>)
                dm.CreateDelegate(typeof(Func<T, T, bool>));
        }

        private static Func<T, T, T> BitwiseOperator(string name, OpCode bitwiseCode)
        {
            var dm = new DynamicMethod(name,
                typeof(T),
                new[] { typeof(T), typeof(T) },
                true);

            var generator = dm.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(bitwiseCode);
            generator.Emit(OpCodes.Ret);

            return (Func<T, T, T>)
                dm.CreateDelegate(typeof(Func<T, T, T>));
        }

        private static Func<T, T> GenerateBitwiseNot()
        {
            var dm = new DynamicMethod("Not",
                typeof(T),
                new[] { typeof(T) },
                true);

            var lgen = dm.GetILGenerator();
            lgen.Emit(OpCodes.Ldarg_0);
            lgen.Emit(OpCodes.Not);
            lgen.Emit(OpCodes.Ret);

            return (Func<T, T>)dm.CreateDelegate(
                typeof(Func<T, T>));
        }

        private static Func<T, T, T> GenerateUnsetFlag()
        {
            var dm = new DynamicMethod("UnsetFlag",
                typeof(T),
                new[] { typeof(T), typeof(T) },
                true);

            var lgen = dm.GetILGenerator();
            lgen.Emit(OpCodes.Ldarg_0);
            lgen.Emit(OpCodes.Ldarg_1);
            lgen.Emit(OpCodes.Not);
            lgen.Emit(OpCodes.And);
            lgen.Emit(OpCodes.Ret);

            return (Func<T, T, T>)dm.CreateDelegate(
                typeof(Func<T, T, T>));
        }

        private static Func<T, T, T> GenerateBitwiseOr()
        {
            return BitwiseOperator(nameof(OpCodes.Or), OpCodes.Or);
        }

        private static Func<T, T, T> GenerateBitwiseAnd()
        {
            return BitwiseOperator(nameof(OpCodes.And), OpCodes.And);
        }

        private static Func<T, T, T> GenerateBitwiseExclusiveOr()
        {
            return BitwiseOperator(nameof(OpCodes.Xor), OpCodes.Xor);
        }

        #endregion

        #region Is Defined

        internal static readonly Func<sbyte, bool> IsDefinedSByte = GenerateIsDefined<sbyte>();

        internal static readonly Func<byte, bool> IsDefinedByte = GenerateIsDefined<byte>();

        internal static readonly Func<ushort, bool> IsDefinedUInt16 = GenerateIsDefined<ushort>();

        internal static readonly Func<short, bool> IsDefinedInt16 = GenerateIsDefined<short>();

        internal static readonly Func<uint, bool> IsDefinedUInt32 = GenerateIsDefined<uint>();

        internal static readonly Func<int, bool> IsDefinedInt32 = GenerateIsDefined<int>();

        internal static readonly Func<ulong, bool> IsDefinedUInt64 = GenerateIsDefined<ulong>();

        internal static readonly Func<long, bool> IsDefinedInt64 = GenerateIsDefined<long>();

        internal static readonly Func<float, bool> IsDefinedSingle = GenerateIsDefined<float>();

        internal static readonly Func<double, bool> IsDefinedDouble = GenerateIsDefined<double>();

        #endregion

        #region Bitwise

        internal static readonly Func<T, T, T> UnsetFlag = GenerateUnsetFlag();

        internal static readonly Func<T, T, T> BitwiseOr = GenerateBitwiseOr();

        internal static readonly Func<T, T, T> BitwiseAnd = GenerateBitwiseAnd();

        internal static readonly Func<T, T, T> BitwiseExclusiveOr = GenerateBitwiseExclusiveOr();

        internal static readonly Func<T, T> BitwiseNot = GenerateBitwiseNot();

        internal static readonly Func<T, T, bool> HasFlag = GenerateHasFlag();

        #endregion

        #region To

        internal static readonly Func<T, ulong> ToUInt64 = GenerateConvertTo<ulong>();

        internal static readonly Func<T, long> ToInt64 = GenerateConvertTo<long>();

        internal static readonly Func<T, uint> ToUInt32 = GenerateConvertTo<uint>();

        internal static readonly Func<T, int> ToInt32 = GenerateConvertTo<int>();

        internal static readonly Func<T, ushort> ToUInt16 = GenerateConvertTo<ushort>();

        internal static readonly Func<T, short> ToInt16 = GenerateConvertTo<short>();

        internal static readonly Func<T, byte> ToByte = GenerateConvertTo<byte>();

        internal static readonly Func<T, sbyte> ToSByte = GenerateConvertTo<sbyte>();

        internal static readonly Func<T, float> ToSingle = GenerateConvertTo<float>();

        internal static readonly Func<T, double> ToDouble = GenerateConvertTo<double>();

        #endregion

        #region From

        internal static readonly Func<ulong, T> FromUInt64 = GenerateConvertFrom<ulong>();

        internal static readonly Func<long, T> FromInt64 = GenerateConvertFrom<long>();

        internal static readonly Func<uint, T> FromUInt32 = GenerateConvertFrom<uint>();

        internal static readonly Func<int, T> FromInt32 = GenerateConvertFrom<int>();

        internal static readonly Func<ushort, T> FromUInt16 = GenerateConvertFrom<ushort>();

        internal static readonly Func<short, T> FromInt16 = GenerateConvertFrom<short>();

        internal static readonly Func<byte, T> FromByte = GenerateConvertFrom<byte>();

        internal static readonly Func<sbyte, T> FromSByte = GenerateConvertFrom<sbyte>();

        internal static readonly Func<float, T> FromSingle = GenerateConvertFrom<float>();

        internal static readonly Func<double, T> FromDouble = GenerateConvertFrom<double>();

        #endregion
    }
}
