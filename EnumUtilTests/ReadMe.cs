using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EnumUtilities;
using System.Reflection;
using System.Collections.Generic;

namespace EnumUtilTests
{
    [TestClass]
    public class ReadMe
    {
        // This code is part of the readme document
        // and should not produce errors

        enum YourEnum
        {
            Foo,
            Bar
        }

        [TestMethod]
        public void ReadMeTypeSafeWrappers()
        {
            // Enum.GetUnderlyingType(typeof(T))
            Type underlyingType = EnumUtil<YourEnum>.GetUnderlyingType();

            // Enum.GetName(typeof(T), value)
            string nameFoo = EnumUtil<YourEnum>.GetName(YourEnum.Foo);

            // (T)Enum.Parse(typeof(T), value)
            YourEnum valueFoo = EnumUtil<YourEnum>.Parse("Foo");

            // (T)Enum.Parse(typeof(T), value, ignoreCase)
            YourEnum valueFoo2 = EnumUtil<YourEnum>.Parse("foo", ignoreCase: true);

            // (T[])typeof(T).GetEnumValues()
            YourEnum[] values = EnumUtil<YourEnum>.GetValues();

            // typeof(T).GetEnumNames()
            string[] names = EnumUtil<YourEnum>.GetNames();

            // Enum.TryParse(value, out result)
            YourEnum possibleValue;
            bool success = EnumUtil<YourEnum>.TryParse("foob", out possibleValue);

            // Enum.TryParse(value, ignoreCase, out result)
            YourEnum possibleValue2;
            bool sucess2 = EnumUtil<YourEnum>.TryParse("foo", true, out possibleValue2);

            // Enum.IsDefined(typeof(T), name)
            bool isDef2 = EnumUtil<YourEnum>.IsDefined("Foo");
        }

        [TestMethod]
        public void ReadMeBitwiseOperations()
        {
            // a | b
            YourEnum fooOrBar = EnumUtil<YourEnum>.BitwiseOr(YourEnum.Foo, YourEnum.Bar);

            // a & b
            YourEnum fooAndBar = EnumUtil<YourEnum>.BitwiseAnd(YourEnum.Foo, YourEnum.Bar);

            // a ^ b
            YourEnum fooXorBar = EnumUtil<YourEnum>.BitwiseExclusiveOr(YourEnum.Foo, YourEnum.Bar);

            // ~ a
            YourEnum notBar = EnumUtil<YourEnum>.BitwiseNot(YourEnum.Bar);
        }

        [TestMethod]
        public void ReadMeFlagAttributeOperations()
        {
            // Has Flag?
            bool hasFlag = EnumUtil<YourEnum>.HasFlag(YourEnum.Foo | YourEnum.Bar, YourEnum.Bar);

            // Set Flag
            YourEnum barSet = EnumUtil<YourEnum>.SetFlag(default(YourEnum), YourEnum.Bar);

            // Unset (Remove) Flag
            YourEnum barUnset = EnumUtil<YourEnum>.UnsetFlag(barSet, YourEnum.Bar);

            // Toggle Flag On / Off
            YourEnum fooSet = EnumUtil<YourEnum>.ToggleFlag(barUnset, YourEnum.Foo);

            // Toggle Flag based on a passed in boolean
            YourEnum fooUnset = EnumUtil<YourEnum>.ToggleFlag(fooSet, YourEnum.Foo, false);

            // Checks whether the FlagsAttribute it defined on the Enum
            // Note: Toggle, Set, Unset, and HasFlag functions do not ensure that
            // FlagsAttribute is defined
            bool hasFlagsShortcut = EnumUtil<YourEnum>.HasFlagsAttribute();
        }

        [TestMethod]
        public void ReadMeIsDefinedOperations()
        {
            bool enumValDefined = EnumUtil<YourEnum>.IsDefined((YourEnum)2);
            bool enumNameDefined = EnumUtil<YourEnum>.IsDefined("Foo");

            // Passed in number types get converted automatically
            // to the correct underlying type
            // unlike the vanilla Enum.IsDefined which throws an exception 
            bool byteValDefined = EnumUtil<YourEnum>.IsDefined((byte)2);
            bool sbyteValDefined = EnumUtil<YourEnum>.IsDefined((sbyte)2);
            bool shortValDefined = EnumUtil<YourEnum>.IsDefined((short)2);
            bool ushortValDefined = EnumUtil<YourEnum>.IsDefined((ushort)2);
            bool intValDefined = EnumUtil<YourEnum>.IsDefined((int)2);
            bool uintValDefined = EnumUtil<YourEnum>.IsDefined((uint)2);
            bool longValDefined = EnumUtil<YourEnum>.IsDefined((long)2);
            bool ulongValDefined = EnumUtil<YourEnum>.IsDefined((ulong)2);
            bool floatValDefined = EnumUtil<YourEnum>.IsDefined((float)2);
            bool doubleValDefined = EnumUtil<YourEnum>.IsDefined((double)2);
        }

        [TestMethod]
        public void ReadMeConvFromANumberType()
        {
            // Conversion from a numeric type to an enum type
            YourEnum val0 = EnumUtil<YourEnum>.FromByte(2);
            YourEnum val1 = EnumUtil<YourEnum>.FromSByte(2);
            YourEnum val2 = EnumUtil<YourEnum>.FromInt16(2);
            YourEnum val3 = EnumUtil<YourEnum>.FromUInt16(2);
            YourEnum val4 = EnumUtil<YourEnum>.FromInt32(2);
            YourEnum val5 = EnumUtil<YourEnum>.FromUInt32(2);
            YourEnum val6 = EnumUtil<YourEnum>.FromInt64(2L);
            YourEnum val7 = EnumUtil<YourEnum>.FromUInt64(2UL);
            YourEnum val8 = EnumUtil<YourEnum>.FromSingle(2f);
            YourEnum val9 = EnumUtil<YourEnum>.FromDouble(2.0);
        }

        [TestMethod]
        public void ReadMeConvToANumberType()
        {
            // Conversion from an enum type to a numeric type
            byte byteVal = EnumUtil<YourEnum>.ToByte(YourEnum.Foo);
            sbyte sbyteVal = EnumUtil<YourEnum>.ToSByte(YourEnum.Foo);
            short shortVal = EnumUtil<YourEnum>.ToInt16(YourEnum.Foo);
            ushort ushortVal = EnumUtil<YourEnum>.ToUInt16(YourEnum.Foo);
            int intVal = EnumUtil<YourEnum>.ToInt32(YourEnum.Foo);
            uint uintVal = EnumUtil<YourEnum>.ToUInt32(YourEnum.Bar);
            long longVal = EnumUtil<YourEnum>.ToInt64(YourEnum.Foo);
            ulong ulongVal = EnumUtil<YourEnum>.ToUInt64(YourEnum.Foo);
            float floatVal = EnumUtil<YourEnum>.ToSingle(YourEnum.Bar);
            double doubleVal = EnumUtil<YourEnum>.ToDouble(YourEnum.Bar);
        }

        [TestMethod]
        public void ReadMeReflectedInformation()
        {
            // Shortcut for typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static)
            FieldInfo[] fields = EnumUtil<YourEnum>.GetEnumFields();

            // On the Enumeration itself
            FlagsAttribute attr = EnumUtil<YourEnum>.GetAttribute<FlagsAttribute>();
            IEnumerable<DescriptionAttribute> attrs = EnumUtil<YourEnum>.GetAttributes<DescriptionAttribute>();
            bool hasFlagsAttr = EnumUtil<YourEnum>.HasAttribute<FlagsAttribute>();
            bool hasFlagsShortcut = EnumUtil<YourEnum>.HasFlagsAttribute();

            // On a field in the enumeration
            DescriptionAttribute attr2 = EnumUtil<YourEnum>.GetAttribute<DescriptionAttribute>(YourEnum.Bar);
            IEnumerable<DescriptionAttribute> attrs3 = EnumUtil<YourEnum>.GetAttributes<DescriptionAttribute>(YourEnum.Bar);

            // Various Read Only Dictionaries
            // with data about the members of an enumeration
            var valueDescription = EnumUtil<YourEnum>.GetValueDescription();
            var valueNameDescription = EnumUtil<YourEnum>.GetValueNameDescription();
            var valueNameAttributes = EnumUtil<YourEnum>.GetValueNameAttributes();
            var nameValueAttribute = EnumUtil<YourEnum>.GetNameValueAttribute<DescriptionAttribute>();
            var valueNameAttribute = EnumUtil<YourEnum>.GetValueNameAttribute<DescriptionAttribute>();
            var valueAttribute = EnumUtil<YourEnum>.GetValueAttribute<DescriptionAttribute>();
            var nameValue = EnumUtil<YourEnum>.GetNameValue();
            var valueName = EnumUtil<YourEnum>.GetValueName();
        }
        
        // C# 7.3
        private static void YourFunction<TEnum>()
            where TEnum : struct, Enum, IComparable, IFormattable, IConvertible
        {
            // Call EnumUtilBase within the function
            TEnum[] values = EnumUtil<TEnum>.GetValues();
            // Your Code Here
        }
    }
}
