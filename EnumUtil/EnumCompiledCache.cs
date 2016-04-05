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
        public static Func<Y, bool> GenerateIsDefined<Y>()
            where Y : struct, IComparable, IFormattable, IConvertible, IComparable<Y>, IEquatable<Y>
        {
            var value = Expression.Parameter(typeof(Y), "value");
            var convVal = Expression.Convert(value, typeof(T));

            Expression<Func<T, bool>> e =
                x => Array.IndexOf(ReflectionCache<T>.FieldValues, x) >= 0;

            return Expression.Lambda<Func<Y, bool>>(Expression.Invoke(e, convVal), value)
                .Compile();
        }

        private static Func<T, T, bool> GenerateHasFlag()
        {
            var value = Expression.Parameter(typeof(T));
            var flag = Expression.Parameter(typeof(T));

            // Convert from Enum to underlying type (byte, int, long, ...)
            // to allow bitwise functions to work
            UnaryExpression valueConverted = Expression.Convert(value, Enum.GetUnderlyingType(typeof(T)));
            UnaryExpression flagConverted = Expression.Convert(flag, Enum.GetUnderlyingType(typeof(T)));

            // (Value & Flag)
            BinaryExpression bitwiseAnd =
                Expression.MakeBinary(
                    ExpressionType.And,
                    valueConverted,
                    flagConverted);

            // (Value & Flag) == Flag
            BinaryExpression hasFlagExpression =
                Expression.MakeBinary(ExpressionType.Equal, bitwiseAnd, flagConverted);

            return Expression.Lambda<Func<T, T, bool>>(hasFlagExpression, value, flag)
                .Compile();
        }

        private static Func<T, T, T> BitwiseOperator(ExpressionType expressionType)
        {
            ParameterExpression leftVal = Expression.Parameter(typeof(T));
            ParameterExpression rightVal = Expression.Parameter(typeof(T));

            // Convert from Enum to Enum's underlying type (byte, int, long, ...)
            // to allow bitwise functions to work
            UnaryExpression leftValConverted = Expression.Convert(leftVal, Enum.GetUnderlyingType(typeof(T)));
            UnaryExpression rightValConverted = Expression.Convert(rightVal, Enum.GetUnderlyingType(typeof(T)));

            // left [expressionType] right
            BinaryExpression binaryExpression =
                Expression.MakeBinary(
                    expressionType,
                    leftValConverted,
                    rightValConverted);

            // Convert back to Enum
            UnaryExpression backToEnumType = Expression.Convert(binaryExpression, typeof(T));
            return Expression.Lambda<Func<T, T, T>>(backToEnumType, leftVal, rightVal)
                .Compile();
        }

        private static Func<T, T> BitwiseUnaryOperator(ExpressionType expressionType)
        {
            var val = Expression.Parameter(typeof(T));

            // Convert from Enum to Enum’s underlying type (byte, int, long, …)
            // to allow bitwise functions to work
            var valConverted = Expression.Convert(val, Enum.GetUnderlyingType(typeof(T)));

            var unaryExpression =
                Expression.MakeUnary(
                    expressionType,
                    valConverted,
                    null);

            // Convert back to Enum
            var backToEnumType = Expression.Convert(unaryExpression, typeof(T));
            return Expression.Lambda<Func<T, T>>(backToEnumType, val)
                .Compile();
        }

        private static Func<T, T, T> GenerateUnsetFlag()
        {
            var val = Expression.Parameter(typeof(T));
            var flag = Expression.Parameter(typeof(T));

            // Convert from Enum to Enum’s underlying type (byte, int, long, …)
            // to allow bitwise functions to work
            var underlyingType = Enum.GetUnderlyingType(typeof(T));
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
            UnaryExpression backToEnumType = Expression.Convert(andExpression, typeof(T));
            return Expression.Lambda<Func<T, T, T>>(backToEnumType, val, flag)
                .Compile();
        }

        private static Func<T, T, T> GenerateBitwiseOr()
        {
            return BitwiseOperator(ExpressionType.Or);
        }

        private static Func<T, T, T> GenerateBitwiseAnd()
        {
            return BitwiseOperator(ExpressionType.And);
        }

        private static Func<T, T, T> GenerateBitwiseExclusiveOr()
        {
            return BitwiseOperator(ExpressionType.ExclusiveOr);
        }

        private static Func<T, T> GenerateBitwiseNot()
            => BitwiseUnaryOperator(ExpressionType.Not);

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
