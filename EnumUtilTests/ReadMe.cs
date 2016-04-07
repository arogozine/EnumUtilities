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
            Type underlyingType = EnumUtil.GetUnderlyingType<YourEnum>();

            // Enum.GetName(typeof(T), value)
            string nameFoo = EnumUtil.GetName(YourEnum.Foo);

            // (T)Enum.Parse(typeof(T), value)
            YourEnum valueFoo = EnumUtil.Parse<YourEnum>("Foo");

            // (T)Enum.Parse(typeof(T), value, ignoreCase)
            YourEnum valueFoo2 = EnumUtil.Parse<YourEnum>("foo", ignoreCase: true);

            // (T[])typeof(T).GetEnumValues()
            YourEnum[] values = EnumUtil.GetValues<YourEnum>();

            // typeof(T).GetEnumNames()
            string[] names = EnumUtil.GetNames<YourEnum>();

            // Enum.TryParse(value, out result)
            YourEnum possibleValue;
            bool success = EnumUtil.TryParse("foob", out possibleValue);

            // Enum.TryParse(value, ignoreCase, out result)
            YourEnum possibleValue2;
            bool sucess2 = EnumUtil.TryParse("foo", true, out possibleValue2);

            // Enum.IsDefined(typeof(T), name)
            bool isDef2 = EnumUtil.IsDefined<YourEnum>("Foo");
        }

        [TestMethod]
        public void ReadMeBitwiseOperations()
        {
            // a | b
            YourEnum fooOrBar = EnumUtil.BitwiseOr(YourEnum.Foo, YourEnum.Bar);

            // a & b
            YourEnum fooAndBar = EnumUtil.BitwiseAnd(YourEnum.Foo, YourEnum.Bar);

            // a ^ b
            YourEnum fooXorBar = EnumUtil.BitwiseExclusiveOr(YourEnum.Foo, YourEnum.Bar);

            // ~ a
            YourEnum notBar = EnumUtil.BitwiseNot(YourEnum.Bar);
        }

        [TestMethod]
        public void ReadMeFlagAttributeOperations()
        {
            // Has Flag?
            bool hasFlag = EnumUtil.HasFlag(YourEnum.Foo | YourEnum.Bar, YourEnum.Bar);

            // Set Flag
            YourEnum barSet = EnumUtil.SetFlag(default(YourEnum), YourEnum.Bar);

            // Unset (Remove) Flag
            YourEnum barUnset = EnumUtil.UnsetFlag(barSet, YourEnum.Bar);

            // Toggle Flag On / Off
            YourEnum fooSet = EnumUtil.ToggleFlag(barUnset, YourEnum.Foo);

            // Toggle Flag based on a passed in boolean
            YourEnum fooUnset = EnumUtil.ToggleFlag(fooSet, YourEnum.Foo, false);

            // Checks whether the FlagsAttribute it defined on the Enum
            // Note: Toggle, Set, Unset, and HasFlag functions do not ensure that
            // FlagsAttribute is defined
            bool hasFlagsShortcut = EnumUtil.HasFlagsAttribute<YourEnum>();
        }

        [TestMethod]
        public void ReadMeIsDefinedOperations()
        {
            bool enumValDefined = EnumUtil.IsDefined((YourEnum)2);
            bool enumNameDefined = EnumUtil.IsDefined<YourEnum>("Foo");

            // Passed in number types get converted automatically
            // to the correct underlying type
            // unlike the vanilla Enum.IsDefined which throws an exception 
            bool byteValDefined = EnumUtil.IsDefined<YourEnum>((byte)2);
            bool sbyteValDefined = EnumUtil.IsDefined<YourEnum>((sbyte)2);
            bool shortValDefined = EnumUtil.IsDefined<YourEnum>((short)2);
            bool ushortValDefined = EnumUtil.IsDefined<YourEnum>((ushort)2);
            bool intValDefined = EnumUtil.IsDefined<YourEnum>((int)2);
            bool uintValDefined = EnumUtil.IsDefined<YourEnum>((uint)2);
            bool longValDefined = EnumUtil.IsDefined<YourEnum>((long)2);
            bool ulongValDefined = EnumUtil.IsDefined<YourEnum>((ulong)2);
        }

        [TestMethod]
        public void ReadMeConvFromANumberType()
        {
            // Conversion from a numeric type to an enum type
            YourEnum val0 = EnumUtil.FromByte<YourEnum>(2);
            YourEnum val1 = EnumUtil.FromSByte<YourEnum>(2);
            YourEnum val2 = EnumUtil.FromInt16<YourEnum>(2);
            YourEnum val3 = EnumUtil.FromUInt16<YourEnum>(2);
            YourEnum val4 = EnumUtil.FromInt32<YourEnum>(2);
            YourEnum val5 = EnumUtil.FromUInt32<YourEnum>(2);
            YourEnum val6 = EnumUtil.FromInt64<YourEnum>(2L);
            YourEnum val7 = EnumUtil.FromUInt64<YourEnum>(2UL);
            YourEnum val8 = EnumUtil.FromSingle<YourEnum>(2f);
            YourEnum val9 = EnumUtil.FromDouble<YourEnum>(2.0);
        }

        [TestMethod]
        public void ReadMeConvToANumberType()
        {
            // Conversion from an enum type to a numeric type
            byte byteVal = EnumUtil.ToByte(YourEnum.Foo);
            sbyte sbyteVal = EnumUtil.ToSByte(YourEnum.Foo);
            short shortVal = EnumUtil.ToInt16(YourEnum.Foo);
            ushort ushortVal = EnumUtil.ToUInt16(YourEnum.Foo);
            int intVal = EnumUtil.ToInt32(YourEnum.Foo);
            uint uintVal = EnumUtil.ToUInt32(YourEnum.Bar);
            long longVal = EnumUtil.ToInt64(YourEnum.Foo);
            ulong ulongVal = EnumUtil.ToUInt64(YourEnum.Foo);
            float floatVal = EnumUtil.ToSingle(YourEnum.Bar);
            double doubleVal = EnumUtil.ToDouble(YourEnum.Bar);
        }

        [TestMethod]
        public void ReadMeReflectedInformation()
        {
            // Shortcut for typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static)
            FieldInfo[] fields = EnumUtil.GetEnumFields<YourEnum>();

            // On the Enumeration itself
            FlagsAttribute attr = EnumUtil.GetAttribute<FlagsAttribute, YourEnum>();
            IEnumerable<DescriptionAttribute> attrs = EnumUtil.GetAttributes<DescriptionAttribute, YourEnum>();
            bool hasFlagsAttr = EnumUtil.HasAttribute<FlagsAttribute, YourEnum>();
            bool hasFlagsShortcut = EnumUtil.HasFlagsAttribute<YourEnum>();

            // On a field in the enumeration
            DescriptionAttribute attr2 = EnumUtil.GetAttribute<DescriptionAttribute, YourEnum>(YourEnum.Bar);
            IEnumerable<DescriptionAttribute> attrs3 = EnumUtil.GetAttributes<DescriptionAttribute, YourEnum>(YourEnum.Bar);

            // Various Read Only Dictionaries
            // with data about the members of an enumeration
            var valueDescription = EnumUtil.GetValueDescription<YourEnum>();
            var valueNameDescription = EnumUtil.GetValueNameDescription<YourEnum>();
            var valueNameAttributes = EnumUtil.GetValueNameAttributes<YourEnum>();
            var nameValueAttribute = EnumUtil.GetNameValueAttribute<YourEnum, DescriptionAttribute>();
            var valueNameAttribute = EnumUtil.GetValueNameAttribute<YourEnum, DescriptionAttribute>();
            var valueAttribute = EnumUtil.GetValueAttribute<YourEnum, DescriptionAttribute>();
            var nameValue = EnumUtil.GetNameValue<YourEnum>();
            var valueName = EnumUtil.GetValueName<YourEnum>();
        }

    }
}
