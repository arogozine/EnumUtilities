using System;
using System.Linq.Expressions;
using System.Reflection;

namespace EnumUtilities
{
    public static partial class EnumUtil<TEnum>
        where TEnum : struct, Enum, IComparable, IFormattable, IConvertible
    {
        #region Generate Functions

        private static Func<string, TEnum> GenerateParse()
        {
            Type enumType = typeof(TEnum);
            FieldInfo[] fields = ReflectionCache<TEnum>.Fields;
            TEnum[] fieldValues = ReflectionCache<TEnum>.FieldValues;
            Type underLyingType = enumType.GetEnumUnderlyingType();

            var inputValue = Expression.Parameter(typeof(string), "value"); // (String value)

            // Generate Switch Cases
            var switchCases = new SwitchCase[fields.Length];

            for (int i = 0; i < fields.Length; i++)
            {
                TEnum fieldEnum = fieldValues[i];
                string fieldName = fields[i].Name;
                string fieldValue = Convert.ChangeType(fieldEnum, underLyingType)
                    .ToString();
                
                var switchCase = Expression.SwitchCase(
                    Expression.Constant(fieldEnum),
                    Expression.Constant(fieldName),
                    Expression.Constant(fieldValue));

                switchCases[i] = switchCase;
            }

            // Default Switch Case
            var defaultException = Expression.Block(enumType,
              Expression.Throw(
                  Expression.New(typeof(ArgumentException)
                    .GetConstructor(Type.EmptyTypes))
              ),
              Expression.Default(enumType));

            return Expression.Lambda<Func<string, TEnum>>(
                Expression.Block(enumType,
                    Expression.Switch(inputValue, defaultException, switchCases)),
                inputValue)
                .Compile();
        }

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

        private static Func<TEnum, TEnum> GenerateBitwiseNot()
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

        private static readonly Func<sbyte, bool> isDefinedSByte = GenerateIsDefined<sbyte>();

        private static readonly Func<byte, bool> isDefinedByte = GenerateIsDefined<byte>();

        private static readonly Func<ushort, bool> isDefinedUInt16 = GenerateIsDefined<ushort>();

        private static readonly Func<short, bool> isDefinedInt16 = GenerateIsDefined<short>();

        private static readonly Func<uint, bool> isDefinedUInt32 = GenerateIsDefined<uint>();

        private static readonly Func<int, bool> isDefinedInt32 = GenerateIsDefined<int>();

        private static readonly Func<ulong, bool> isDefinedUInt64 = GenerateIsDefined<ulong>();

        private static readonly Func<long, bool> isDefinedInt64 = GenerateIsDefined<long>();

        private static readonly Func<float, bool> isDefinedSingle = GenerateIsDefined<float>();

        private static readonly Func<double, bool> isDefinedDouble = GenerateIsDefined<double>();

        #endregion

        #region Bitwise

        private static readonly Func<TEnum, TEnum, TEnum> unsetFlag = GenerateUnsetFlag();

        private static readonly Func<TEnum, TEnum, TEnum> bitwiseOr = GenerateBitwiseOr();

        private static readonly Func<TEnum, TEnum, TEnum> bitwiseAnd = GenerateBitwiseAnd();

        private static readonly Func<TEnum, TEnum, TEnum> bitwiseExclusiveOr = GenerateBitwiseExclusiveOr();

        private static readonly Func<TEnum, TEnum> bitwiseNot = GenerateBitwiseNot();

        private static readonly Func<TEnum, TEnum, bool> hasFlag = GenerateHasFlag();

        #endregion

        #region To

        private static readonly Func<TEnum, ulong> toUInt64 = GenerateConvertTo<ulong>();

        private static readonly Func<TEnum, long> toInt64 = GenerateConvertTo<long>();

        private static readonly Func<TEnum, uint> toUInt32 = GenerateConvertTo<uint>();

        private static readonly Func<TEnum, int> toInt32 = GenerateConvertTo<int>();

        private static readonly Func<TEnum, ushort> toUInt16 = GenerateConvertTo<ushort>();

        private static readonly Func<TEnum, short> toInt16 = GenerateConvertTo<short>();

        private static readonly Func<TEnum, byte> toByte = GenerateConvertTo<byte>();

        private static readonly Func<TEnum, sbyte> toSByte = GenerateConvertTo<sbyte>();

        private static readonly Func<TEnum, float> toSingle = GenerateConvertTo<float>();

        private static readonly Func<TEnum, double> toDouble = GenerateConvertTo<double>();

        #endregion

        #region From

        private static readonly Func<ulong, TEnum> fromUInt64 = GenerateConvertFrom<ulong>();

        private static readonly Func<long, TEnum> fromInt64 = GenerateConvertFrom<long>();

        private static readonly Func<uint, TEnum> fromUInt32 = GenerateConvertFrom<uint>();

        private static readonly Func<int, TEnum> fromInt32 = GenerateConvertFrom<int>();

        private static readonly Func<ushort, TEnum> fromUInt16 = GenerateConvertFrom<ushort>();

        private static readonly Func<short, TEnum> fromInt16 = GenerateConvertFrom<short>();

        private static readonly Func<byte, TEnum> fromByte = GenerateConvertFrom<byte>();

        private static readonly Func<sbyte, TEnum> fromSByte = GenerateConvertFrom<sbyte>();

        private static readonly Func<float, TEnum> fromSingle = GenerateConvertFrom<float>();

        private static readonly Func<double, TEnum> fromDouble = GenerateConvertFrom<double>();

        #endregion

        private static readonly Func<string, TEnum> quickParse = GenerateParse();

    }
}
