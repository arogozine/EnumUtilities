using System;
using System.Linq.Expressions;
using System.Reflection;

namespace EnumUtilities
{
    /// <summary>
    /// Uses Linq Expressions to compile Enum functions
    /// 
    /// (c) Alexandre Rogozine 2016
    /// </summary>
    /// <typeparam name="TEnum">Enum Type</typeparam>
    internal static class EnumCompiledCache<TEnum>
        where TEnum : struct, Enum, IComparable, IFormattable, IConvertible
    {
        #region Generate Functions

        private static Func<TEnum, Y> GenerateConvertTo<Y>() 
            where Y : struct, IComparable, IFormattable, IConvertible, IComparable<Y>, IEquatable<Y>
        {
            var value = Expression.Parameter(typeof(TEnum));
            UnaryExpression ue = Expression.Convert(value, typeof(Y));
            return Expression.Lambda<Func<TEnum, Y>>(ue, value)
                .Compile();
        }

        private static Func<Y, TEnum> GenerateConvertFrom<Y>()
            where Y : struct, IComparable, IFormattable, IConvertible, IComparable<Y>, IEquatable<Y>
        {
            var value = Expression.Parameter(typeof(Y));
            UnaryExpression ue = Expression.Convert(value, typeof(TEnum));
            return Expression.Lambda<Func<Y, TEnum>>(ue, value)
                .Compile();
        }

        private static Func<Y, bool> GenerateIsDefined<Y>()
            where Y : struct, IComparable, IFormattable, IConvertible, IComparable<Y>, IEquatable<Y>
        {
            var value = Expression.Parameter(typeof(Y), "value");
            var convVal = Expression.Convert(value, typeof(TEnum));

            Expression<Func<TEnum, bool>> lookup =
                val => Array.IndexOf(ReflectionCache<TEnum>.FieldValues, val) >= 0;

            return Expression.Lambda<Func<Y, bool>>(Expression.Invoke(lookup, convVal), value)
                .Compile();
        }

        private static Func<TEnum, TEnum, bool> GenerateHasFlag()
        {
            var value = Expression.Parameter(typeof(TEnum));
            var flag = Expression.Parameter(typeof(TEnum));

            // Convert from Enum to underlying type (byte, int, long, ...)
            // to allow bitwise functions to work
            UnaryExpression valueConverted = Expression.Convert(value, Enum.GetUnderlyingType(typeof(TEnum)));
            UnaryExpression flagConverted = Expression.Convert(flag, Enum.GetUnderlyingType(typeof(TEnum)));

            // (Value & Flag)
            BinaryExpression bitwiseAnd =
                Expression.MakeBinary(
                    ExpressionType.And,
                    valueConverted,
                    flagConverted);

            // (Value & Flag) == Flag
            BinaryExpression hasFlagExpression =
                Expression.MakeBinary(ExpressionType.Equal, bitwiseAnd, flagConverted);

            return Expression.Lambda<Func<TEnum, TEnum, bool>>(hasFlagExpression, value, flag)
                .Compile();
        }

        private static Func<TEnum, TEnum, TEnum> BitwiseOperator(ExpressionType expressionType)
        {
            ParameterExpression leftVal = Expression.Parameter(typeof(TEnum));
            ParameterExpression rightVal = Expression.Parameter(typeof(TEnum));

            // Convert from Enum to Enum's underlying type (byte, int, long, ...)
            // to allow bitwise functions to work
            UnaryExpression leftValConverted = Expression.Convert(leftVal, Enum.GetUnderlyingType(typeof(TEnum)));
            UnaryExpression rightValConverted = Expression.Convert(rightVal, Enum.GetUnderlyingType(typeof(TEnum)));

            // left [expressionType] right
            BinaryExpression binaryExpression =
                Expression.MakeBinary(
                    expressionType,
                    leftValConverted,
                    rightValConverted);

            // Convert back to Enum
            UnaryExpression backToEnumType = Expression.Convert(binaryExpression, typeof(TEnum));
            return Expression.Lambda<Func<TEnum, TEnum, TEnum>>(backToEnumType, leftVal, rightVal)
                .Compile();
        }

        private static Func<TEnum, TEnum, TEnum> GenerateUnsetFlag()
        {
            var val = Expression.Parameter(typeof(TEnum));
            var flag = Expression.Parameter(typeof(TEnum));

            // Convert from Enum to Enum’s underlying type (byte, int, long, …)
            // to allow bitwise functions to work
            var underlyingType = Enum.GetUnderlyingType(typeof(TEnum));
            var valConverted = Expression.Convert(val, underlyingType);
            var flagConverted = Expression.Convert(flag, underlyingType);

            // ~flag
            var notFlagExpression =
                Expression.MakeUnary(
                    ExpressionType.Not,
                    flagConverted,
                    null);

            // val & (~flag)
            var andExpression = Expression.MakeBinary(
                ExpressionType.And,
                valConverted,
                notFlagExpression);

            // Convert back to Enum
            UnaryExpression backToEnumType = Expression.Convert(andExpression, typeof(TEnum));
            return Expression.Lambda<Func<TEnum, TEnum, TEnum>>(backToEnumType, val, flag)
                .Compile();
        }

        private static Func<TEnum, TEnum, TEnum> GenerateBitwiseOr()
        {
            return BitwiseOperator(ExpressionType.Or);
        }

        private static Func<TEnum, TEnum, TEnum> GenerateBitwiseAnd()
        {
            return BitwiseOperator(ExpressionType.And);
        }

        private static Func<TEnum, TEnum, TEnum> GenerateBitwiseExclusiveOr()
        {
            return BitwiseOperator(ExpressionType.ExclusiveOr);
        }

        private static Func<TEnum, TEnum> ()
        {
            var val = Expression.Parameter(typeof(TEnum));

            // Convert from Enum to Enum’s underlying type (byte, int, long, …)
            // to allow bitwise functions to work
            var valConverted = Expression.Convert(val, Enum.GetUnderlyingType(typeof(TEnum)));

            var unaryExpression =
                Expression.MakeUnary(
                    ExpressionType.Not,
                    valConverted,
                    null);

            // Convert back to Enum
            var backToEnumType = Expression.Convert(unaryExpression, typeof(TEnum));
            return Expression.Lambda<Func<TEnum, TEnum>>(backToEnumType, val)
                .Compile();
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

        internal static readonly Func<TEnum, TEnum, TEnum> UnsetFlag = GenerateUnsetFlag();

        internal static readonly Func<TEnum, TEnum, TEnum> BitwiseOr = GenerateBitwiseOr();

        internal static readonly Func<TEnum, TEnum, TEnum> BitwiseAnd = GenerateBitwiseAnd();

        internal static readonly Func<TEnum, TEnum, TEnum> BitwiseExclusiveOr = GenerateBitwiseExclusiveOr();

        internal static readonly Func<TEnum, TEnum> BitwiseNot = GenerateBitwiseNot();

        internal static readonly Func<TEnum, TEnum, bool> HasFlag = GenerateHasFlag();

        #endregion

        #region To

        internal static readonly Func<TEnum, ulong> ToUInt64 = GenerateConvertTo<ulong>();

        internal static readonly Func<TEnum, long> ToInt64 = GenerateConvertTo<long>();

        internal static readonly Func<TEnum, uint> ToUInt32 = GenerateConvertTo<uint>();

        internal static readonly Func<TEnum, int> ToInt32 = GenerateConvertTo<int>();

        internal static readonly Func<TEnum, ushort> ToUInt16 = GenerateConvertTo<ushort>();

        internal static readonly Func<TEnum, short> ToInt16 = GenerateConvertTo<short>();

        internal static readonly Func<TEnum, byte> ToByte = GenerateConvertTo<byte>();

        internal static readonly Func<TEnum, sbyte> ToSByte = GenerateConvertTo<sbyte>();

        internal static readonly Func<TEnum, float> ToSingle = GenerateConvertTo<float>();

        internal static readonly Func<TEnum, double> ToDouble = GenerateConvertTo<double>();

        #endregion

        #region From

        internal static readonly Func<ulong, TEnum> FromUInt64 = GenerateConvertFrom<ulong>();

        internal static readonly Func<long, TEnum> FromInt64 = GenerateConvertFrom<long>();

        internal static readonly Func<uint, TEnum> FromUInt32 = GenerateConvertFrom<uint>();

        internal static readonly Func<int, TEnum> FromInt32 = GenerateConvertFrom<int>();

        internal static readonly Func<ushort, TEnum> FromUInt16 = GenerateConvertFrom<ushort>();

        internal static readonly Func<short, TEnum> FromInt16 = GenerateConvertFrom<short>();

        internal static readonly Func<byte, TEnum> FromByte = GenerateConvertFrom<byte>();

        internal static readonly Func<sbyte, TEnum> FromSByte = GenerateConvertFrom<sbyte>();

        internal static readonly Func<float, TEnum> FromSingle = GenerateConvertFrom<float>();

        internal static readonly Func<double, TEnum> FromDouble = GenerateConvertFrom<double>();

        #endregion
    }
}
