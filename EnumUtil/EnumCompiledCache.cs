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
        static EnumCompiledCache()
        {
            OpCode? toSbyte, toByte, toInt16, toUInt16, toInt32, toUInt32, toInt64, toUInt64, frSbyte, frByte, frInt16, frUInt16, frInt32, frUInt32, frInt64, frUInt64;
            OpCode[] toSingle, toDouble, frSingle, frDouble;
            DetermineOpCodes(out toSbyte, out toByte, out toInt16, out toUInt16, out toInt32, out toUInt32, out toInt64, out toUInt64, out toSingle, out toDouble, out frSbyte, out frByte, out frInt16, out frUInt16, out frInt32, out frUInt32, out frInt64, out frUInt64, out frSingle, out frDouble);

            // To
            ToSByte = GenerateConvert<T, sbyte>(nameof(EnumUtilUnsafe<T>.ToSByte), toSbyte);
            ToByte = GenerateConvert<T, byte>(nameof(EnumUtilUnsafe<T>.ToByte), toByte);
            ToInt16 = GenerateConvert<T, short>(nameof(EnumUtilUnsafe<T>.ToInt16), toInt16);
            ToUInt16 = GenerateConvert<T, ushort>(nameof(EnumUtilUnsafe<T>.ToUInt16), toUInt16);
            ToInt32 = GenerateConvert<T, int>(nameof(EnumUtilUnsafe<T>.ToInt32), toInt32);
            ToUInt32 = GenerateConvert<T, uint>(nameof(EnumUtilUnsafe<T>.ToUInt32), toUInt32);
            ToInt64 = GenerateConvert<T, long>(nameof(EnumUtilUnsafe<T>.ToInt64), toInt64);
            ToUInt64 = GenerateConvert<T, ulong>(nameof(EnumUtilUnsafe<T>.ToUInt64), toUInt64);
            ToSingle = GenerateConvertFl<T, float>(nameof(EnumUtilUnsafe<T>.ToSingle), toSingle);
            ToDouble = GenerateConvertFl<T, double>(nameof(EnumUtilUnsafe<T>.ToDouble), toDouble);

            // TODO,
            IsDefinedSByte = GenerateIsDefined<sbyte>();//(frSbyte);
            IsDefinedByte = GenerateIsDefined<byte>();//(frByte);
            IsDefinedInt16 = GenerateIsDefined<short>();//(frInt16);
            IsDefinedUInt16 = GenerateIsDefined<ushort>();//(frUInt16);
            IsDefinedInt32 = GenerateIsDefined<int>();//(frInt32);
            IsDefinedUInt32 = GenerateIsDefined<uint>();//(frUInt32);
            IsDefinedInt64 = GenerateIsDefined<long>();//(frUInt64);
            IsDefinedUInt64 = GenerateIsDefined<ulong>();//(frUInt64);
            IsDefinedSingle = GenerateIsDefined<float>();//GenerateIsDefinedFl<float, T>(frSingle);
            IsDefinedDouble = GenerateIsDefined<double>();//GenerateIsDefinedFl<double, T>(frDouble);

            // FROM
            FromSByte = GenerateConvert<sbyte, T>(nameof(EnumUtilUnsafe<T>.FromSByte), frSbyte);
            FromByte = GenerateConvert<byte, T>(nameof(EnumUtilUnsafe<T>.FromByte), frByte);
            FromInt16 = GenerateConvert<short, T>(nameof(EnumUtilUnsafe<T>.FromInt16), frInt16);
            FromUInt16 = GenerateConvert<ushort, T>(nameof(EnumUtilUnsafe<T>.FromUInt16), frUInt16);
            FromInt32 = GenerateConvert<int, T>(nameof(EnumUtilUnsafe<T>.FromInt32), frInt32);
            FromUInt32 = GenerateConvert<uint, T>(nameof(EnumUtilUnsafe<T>.FromUInt32), frUInt32);
            FromInt64 = GenerateConvert<long, T>(nameof(EnumUtilUnsafe<T>.FromInt64), frInt64);
            FromUInt64 = GenerateConvert<ulong, T>(nameof(EnumUtilUnsafe<T>.FromUInt64), frUInt64);
            FromSingle = GenerateConvertFl<float, T>(nameof(EnumUtilUnsafe<T>.FromSingle), frSingle);
            FromDouble = GenerateConvertFl<double, T>(nameof(EnumUtilUnsafe<T>.FromDouble), frDouble);

            // IsDefined = GenerateIsDefined<T>(null);

        }

        private static void DetermineOpCodes(out OpCode? toSbyte, out OpCode? toByte, out OpCode? toInt16, out OpCode? toUInt16, out OpCode? toInt32, out OpCode? toUInt32, out OpCode? toInt64, out OpCode? toUInt64, out OpCode[] toSingle, out OpCode[] toDouble, out OpCode? frSbyte, out OpCode? frByte, out OpCode? frInt16, out OpCode? frUInt16, out OpCode? frInt32, out OpCode? frUInt32, out OpCode? frInt64, out OpCode? frUInt64, out OpCode[] frSingle, out OpCode[] frDouble)
        {
            string typeName = typeof(T)
                .GetEnumUnderlyingType().Name;

            switch (typeName)
            {
                case nameof(SByte):
                    // VALUE TYPE -> ENUM
                    frSbyte = null;
                    frByte = OpCodes.Conv_I1;
                    frInt16 = OpCodes.Conv_I1;
                    frUInt16 = OpCodes.Conv_I1;
                    frInt32 = OpCodes.Conv_I1;
                    frUInt32 = OpCodes.Conv_I1;
                    frInt64 = OpCodes.Conv_I1;
                    frUInt64 = OpCodes.Conv_I1;
                    frSingle = new[] { OpCodes.Conv_I1 };
                    frDouble = new[] { OpCodes.Conv_I1 };
                    // ENUM -> VALUE TYPE
                    toSbyte = null;
                    toByte = OpCodes.Conv_U1;
                    toInt16 = null;
                    toUInt16 = OpCodes.Conv_U2;
                    toInt32 = null;
                    toUInt32 = null;
                    toInt64 = OpCodes.Conv_I8;
                    toUInt64 = OpCodes.Conv_I8;
                    toSingle = new[] { OpCodes.Conv_R4 };
                    toDouble = new[] { OpCodes.Conv_R8 };
                    break;

                case nameof(Byte):
                    // VALUE TYPE -> ENUM
                    frSbyte = OpCodes.Conv_U1;
                    frByte = null;
                    frInt16 = OpCodes.Conv_U1;
                    frUInt16 = OpCodes.Conv_U1;
                    frInt32 = OpCodes.Conv_U1;
                    frUInt32 = OpCodes.Conv_U1;
                    frInt64 = OpCodes.Conv_U1;
                    frUInt64 = OpCodes.Conv_U1;
                    frSingle = new[] { OpCodes.Conv_U1 };
                    frDouble = new[] { OpCodes.Conv_U1 };
                    // ENUM -> VALUE TYPE
                    toSbyte = OpCodes.Conv_I1;
                    toByte = null;
                    toInt16 = null;
                    toUInt16 = null;
                    toInt32 = null;
                    toUInt32 = null;
                    toInt64 = OpCodes.Conv_U8;
                    toUInt64 = OpCodes.Conv_U8;
                    toSingle = new[] { OpCodes.Conv_R4 };
                    toDouble = new[] { OpCodes.Conv_R8 };
                    break;

                case nameof(Int16):
                    // VALUE TYPE -> ENUM
                    frSbyte = null;
                    frByte = null;
                    frInt16 = null;
                    frUInt16 = OpCodes.Conv_I2;
                    frInt32 = OpCodes.Conv_I2;
                    frUInt32 = OpCodes.Conv_I2;
                    frInt64 = OpCodes.Conv_I2;
                    frUInt64 = OpCodes.Conv_I2;
                    frSingle = new[] { OpCodes.Conv_I2 };
                    frDouble = new[] { OpCodes.Conv_I2 };
                    // ENUM -> VALUE TYPE
                    toSbyte = OpCodes.Conv_I1;
                    toByte = OpCodes.Conv_U1;
                    toInt16 = null;
                    toUInt16 = OpCodes.Conv_U2;
                    toInt32 = null;
                    toUInt32 = null;
                    toInt64 = OpCodes.Conv_I8;
                    toUInt64 = OpCodes.Conv_I8;
                    toSingle = new[] { OpCodes.Conv_R4 };
                    toDouble = new[] { OpCodes.Conv_R8 };
                    break;

                case nameof(UInt16):
                    // VALUE TYPE -> ENUM
                    frSbyte = OpCodes.Conv_U2;
                    frByte = null;
                    frInt16 = OpCodes.Conv_U2;
                    frUInt16 = null;
                    frInt32 = OpCodes.Conv_U2;
                    frUInt32 = OpCodes.Conv_U2;
                    frInt64 = OpCodes.Conv_U2;
                    frUInt64 = OpCodes.Conv_U2;
                    frSingle = new[] { OpCodes.Conv_U2 };
                    frDouble = new[] { OpCodes.Conv_U2 };
                    // ENUM -> VALUE TYPE
                    toSbyte = OpCodes.Conv_I1;
                    toByte = OpCodes.Conv_U1;
                    toInt16 = OpCodes.Conv_I2;
                    toUInt16 = null;
                    toInt32 = null;
                    toUInt32 = null;
                    toInt64 = OpCodes.Conv_U8;
                    toUInt64 = OpCodes.Conv_U8;
                    toSingle = new[] { OpCodes.Conv_R4 };
                    toDouble = new[] { OpCodes.Conv_R8 };
                    break;

                case nameof(Int32):
                    // VALUE TYPE -> ENUM
                    frSbyte = null;
                    frByte = null;
                    frInt16 = null;
                    frUInt16 = null;
                    frInt32 = null;
                    frUInt32 = null;
                    frInt64 = OpCodes.Conv_I4;
                    frUInt64 = OpCodes.Conv_I4;
                    frSingle = new[] { OpCodes.Conv_I4 };
                    frDouble = new[] { OpCodes.Conv_I4 };
                    // ENUM -> VALUE TYPE
                    toSbyte = OpCodes.Conv_I1;
                    toByte = OpCodes.Conv_U1;
                    toInt16 = OpCodes.Conv_I2;
                    toUInt16 = OpCodes.Conv_U2;
                    toInt32 = null;
                    toUInt32 = null;
                    toInt64 = OpCodes.Conv_I8;
                    toUInt64 = OpCodes.Conv_I8;
                    toSingle = new[] { OpCodes.Conv_R4 };
                    toDouble = new[] { OpCodes.Conv_R8 };
                    break;

                case nameof(UInt32):
                    // VALUE TYPE -> ENUM
                    frSbyte = null;
                    frByte = null;
                    frInt16 = null;
                    frUInt16 = null;
                    frInt32 = null;
                    frUInt32 = null;
                    frInt64 = OpCodes.Conv_U4;
                    frUInt64 = OpCodes.Conv_U4;
                    frSingle = new[] { OpCodes.Conv_U4 };
                    frDouble = new[] { OpCodes.Conv_U4 };
                    // ENUM -> VALUE TYPE
                    toSbyte = OpCodes.Conv_I1;
                    toByte = OpCodes.Conv_U1;
                    toInt16 = OpCodes.Conv_I2;
                    toUInt16 = OpCodes.Conv_U2;
                    toInt32 = null;
                    toUInt32 = null;
                    toInt64 = OpCodes.Conv_U8;
                    toUInt64 = OpCodes.Conv_U8;
                    toSingle = new[] { OpCodes.Conv_R_Un, OpCodes.Conv_R4 };
                    toDouble = new[] { OpCodes.Conv_R_Un, OpCodes.Conv_R8 };
                    break;

                case nameof(Int64):
                    // VALUE TYPE -> ENUM
                    frSbyte = OpCodes.Conv_I8;
                    frByte = OpCodes.Conv_U8;
                    frInt16 = OpCodes.Conv_I8;
                    frUInt16 = OpCodes.Conv_U8;
                    frInt32 = OpCodes.Conv_I8;
                    frUInt32 = OpCodes.Conv_U8;
                    frInt64 = null;
                    frUInt64 = null;
                    frSingle = new[] { OpCodes.Conv_I8 };
                    frDouble = new[] { OpCodes.Conv_I8 };
                    // ENUM -> VALUE TYPE
                    toSbyte = OpCodes.Conv_I1;
                    toByte = OpCodes.Conv_U1;
                    toInt16 = OpCodes.Conv_I2;
                    toUInt16 = OpCodes.Conv_U2;
                    toInt32 = OpCodes.Conv_I4;
                    toUInt32 = OpCodes.Conv_U4;
                    toInt64 = null;
                    toUInt64 = null;
                    toSingle = new[] { OpCodes.Conv_R4 };
                    toDouble = new[] { OpCodes.Conv_R8 };
                    break;

                case nameof(UInt64):
                    // VALUE TYPE -> ENUM
                    frSbyte = OpCodes.Conv_I8;
                    frByte = OpCodes.Conv_U8;
                    frInt16 = OpCodes.Conv_I8;
                    frUInt16 = OpCodes.Conv_U8;
                    frInt32 = OpCodes.Conv_I8;
                    frUInt32 = OpCodes.Conv_U8;
                    frInt64 = null;
                    frUInt64 = null;
                    frSingle = new[] { OpCodes.Conv_U8 };
                    frDouble = new[] { OpCodes.Conv_U8 };
                    // ENUM -> VALUE TYPE
                    toSbyte = OpCodes.Conv_I1;
                    toByte = OpCodes.Conv_U1;
                    toInt16 = OpCodes.Conv_I2;
                    toUInt16 = OpCodes.Conv_U2;
                    toInt32 = OpCodes.Conv_I4;
                    toUInt32 = OpCodes.Conv_U4;
                    toInt64 = null;
                    toUInt64 = null;
                    toSingle = new[] { OpCodes.Conv_R_Un, OpCodes.Conv_R4 };
                    toDouble = new[] { OpCodes.Conv_R_Un, OpCodes.Conv_R8 };
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        private static Func<FROM, TO> GenerateConvert<FROM, TO>(string methodName, OpCode? convCode)
            where FROM : struct
            where TO : struct
        {
            DynamicMethod convertMethod = new DynamicMethod(methodName,
                typeof(TO),
                new[] { typeof(FROM) },
                true);

            ILGenerator generator = convertMethod.GetILGenerator();

            generator.Emit(OpCodes.Ldarg_0);
            if (convCode.HasValue) generator.Emit(convCode.Value);
            generator.Emit(OpCodes.Ret);

            return (Func<FROM, TO>)convertMethod.CreateDelegate(typeof(Func<FROM, TO>));
        }

        private static Func<FROM, TO> GenerateConvertFl<FROM, TO>(string methodName, OpCode[] convCodes)
            where FROM : struct
            where TO : struct
        {
            DynamicMethod floatConvertMethod = new DynamicMethod(methodName,
                typeof(TO),
                new[] { typeof(FROM) },
                true);
            ILGenerator generator = floatConvertMethod.GetILGenerator();

            generator.Emit(OpCodes.Ldarg_0);
            for (int i = 0; i < convCodes.Length; i++)
            {
                generator.Emit(convCodes[i]);
            }
            generator.Emit(OpCodes.Ret);

            return (Func<FROM, TO>)floatConvertMethod.CreateDelegate(typeof(Func<FROM, TO>));
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

        internal static readonly Func<sbyte, bool> IsDefinedSByte;
        internal static readonly Func<byte, bool> IsDefinedByte;
        internal static readonly Func<ushort, bool> IsDefinedUInt16;
        internal static readonly Func<short, bool> IsDefinedInt16;
        internal static readonly Func<uint, bool> IsDefinedUInt32;
        internal static readonly Func<int, bool> IsDefinedInt32;
        internal static readonly Func<ulong, bool> IsDefinedUInt64;
        internal static readonly Func<long, bool> IsDefinedInt64;
        internal static readonly Func<float, bool> IsDefinedSingle;
        internal static readonly Func<double, bool> IsDefinedDouble;

        internal static readonly Func<T, T, T> UnsetFlag = GenerateUnsetFlag();
        internal static readonly Func<T, T, T> BitwiseOr = BitwiseOperator(nameof(OpCodes.Or), OpCodes.Or);
        internal static readonly Func<T, T, T> BitwiseAnd = BitwiseOperator(nameof(OpCodes.And), OpCodes.And);
        internal static readonly Func<T, T, T> BitwiseExclusiveOr = BitwiseOperator(nameof(OpCodes.Xor), OpCodes.Xor);
        internal static readonly Func<T, T> BitwiseNot = GenerateBitwiseNot();
        internal static readonly Func<T, T, bool> HasFlag = GenerateHasFlag();

        internal static readonly Func<T, ulong> ToUInt64;
        internal static readonly Func<T, long> ToInt64;
        internal static readonly Func<T, uint> ToUInt32;
        internal static readonly Func<T, int> ToInt32;
        internal static readonly Func<T, ushort> ToUInt16;
        internal static readonly Func<T, short> ToInt16;
        internal static readonly Func<T, byte> ToByte;
        internal static readonly Func<T, sbyte> ToSByte;
        internal static readonly Func<T, float> ToSingle;
        internal static readonly Func<T, double> ToDouble;

        internal static readonly Func<ulong, T> FromUInt64;
        internal static readonly Func<long, T> FromInt64;
        internal static readonly Func<uint, T> FromUInt32;
        internal static readonly Func<int, T> FromInt32;
        internal static readonly Func<ushort, T> FromUInt16;
        internal static readonly Func<short, T> FromInt16;
        internal static readonly Func<byte, T> FromByte;
        internal static readonly Func<sbyte, T> FromSByte;
        internal static readonly Func<float, T> FromSingle;
        internal static readonly Func<double, T> FromDouble;
    }
}
