using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EnumUtilities;
using EnumUtilTests.Enums;
using System.Linq;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;

namespace EnumUtilTests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void TestNoAttributes()
        {
            var vn = EnumUtil<Int32Enum>.GetValueName();
            var nv = EnumUtil<Int32Enum>.GetNameValue();
            var vna = EnumUtil<Int32Enum>.GetValueNameAttributes();
            var vnd = EnumUtil<Int32Enum>.GetValueNameDescription();

            Assert.IsNotNull(vn);
            Assert.IsNotNull(nv);

            Assert.IsNotNull(vna);
            Assert.IsNotNull(vnd);

            Assert.IsTrue(vn.Count == vn.Count);
            Assert.IsTrue(vn.Count == vna.Count);
            Assert.IsTrue(vna.Count == vnd.Count);

            foreach (var i in vn)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(i.Value));
            }

            foreach (var i in nv)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(i.Key));
            }

            foreach (var i in vna)
            {
                Assert.IsNotNull(i.Value);
                Assert.IsFalse(string.IsNullOrWhiteSpace(i.Value.Item1));
                Assert.IsNotNull(i.Value.Item2);
                Assert.IsTrue(i.Value.Item2.Count() == 0);
            }

            foreach (var i in vnd)
            {
                Assert.IsNotNull(i.Value);
                Assert.IsFalse(string.IsNullOrWhiteSpace(i.Value.Name));
                Assert.IsNull(i.Value.Attribute);
            }
        }

        [TestMethod]
        public void TestAttributes2()
        {
            var vn = EnumUtil<FlagsEnum>.GetValueName();
            var nv = EnumUtil<FlagsEnum>.GetNameValue();
            var vna = EnumUtil<FlagsEnum>.GetValueNameAttributes();
            var vnd = EnumUtil<FlagsEnum>.GetValueNameDescription();

            Assert.IsNotNull(vn);
            Assert.IsNotNull(nv);

            Assert.IsNotNull(vna);
            Assert.IsNotNull(vnd);

            Assert.IsTrue(vn.Count == vn.Count);
            Assert.IsTrue(vn.Count == vna.Count);
            Assert.IsTrue(vna.Count == vnd.Count);

            foreach (var i in vn)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(i.Value));
            }

            foreach (var i in nv)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(i.Key));
            }

            foreach (var i in vna)
            {
                Assert.IsNotNull(i.Value);
                Assert.IsFalse(string.IsNullOrWhiteSpace(i.Value.Item1));
                Assert.IsNotNull(i.Value.Item2);
                Assert.IsTrue(i.Value.Item2.Count() == 1);
            }

            foreach (var i in vnd)
            {
                Assert.IsNotNull(i.Value);
                Assert.IsFalse(string.IsNullOrWhiteSpace(i.Value.Name));
                Assert.IsNotNull(i.Value.Attribute);
            }
        }

        [TestMethod]
        public void TestAttributes()
        {
            Assert.IsTrue(EnumUtil<FlagsEnum>.HasFlagsAttribute());
            Assert.IsTrue(EnumUtil<FlagsEnum>.HasAttribute<FlagsAttribute>());
            Assert.IsNotNull(EnumUtil<FlagsEnum>.GetAttribute<FlagsAttribute>());

            foreach (FlagsEnum fe in EnumUtil<FlagsEnum>.GetValues())
            {
                System.ComponentModel.DescriptionAttribute da = EnumUtil<FlagsEnum>.GetAttribute<System.ComponentModel.DescriptionAttribute>(fe);
                Assert.IsNotNull(da);
                Assert.IsFalse(string.IsNullOrEmpty(da.Description));
            }
            
        }

        [TestMethod]
        public void Int32Test() => RunTests<Int32Enum>();

        [TestMethod]
        public void UInt32Test() => RunTests<UInt32Enum>();

        [TestMethod]
        public void Int64Test() => RunTests<Int64Enum>();

        [TestMethod]
        public void UInt64Test() => RunTests<UInt64Enum>();

        [TestMethod]
        public void ByteTest() => RunTests<ByteEnum>();

        [TestMethod]
        public void Int16Test() => RunTests<Int16Enum>();

        [TestMethod]
        public void UInt16Test() => RunTests<UInt16Enum>();

        [TestMethod]
        public void SByteTest() => RunTests<SByteEnum>();

        private static void RunTests<TEnum>()
            where TEnum : struct, Enum, IComparable, IFormattable, IConvertible
        {
            string[] names = EnumUtil<TEnum>.GetNames();
            TEnum[] values = EnumUtil<TEnum>.GetValues();

            IReadOnlyDictionary<TEnum, string> nameValues = 
                EnumUtil<TEnum>.GetValueName();
            Assert.IsNotNull(nameValues);

            foreach (var nm in nameValues)
            {
                Assert.IsNotNull(nm.Value);
            }

            Assert.IsTrue(nameValues.Count == values.Length);
            Assert.IsTrue(Enum.GetValues(typeof(TEnum)).Length == values.Length);
            Assert.IsTrue(names.Length == values.Length);

            TEnum aggregate = values.Aggregate((x, y) => EnumUtil<TEnum>.BitwiseOr(x, y));
            Assert.IsTrue(Convert.ToInt32(aggregate) == 15);
            TEnum aggregate2 = values.Aggregate((x, y) => EnumUtil<TEnum>.BitwiseAnd(x, y));
            Assert.IsTrue(Convert.ToInt32(aggregate2) == 0);

            for (int i = 0; i < values.Length; i++)
            {
                TEnum value = values[i];
                string name = names[i];

                byte byteVal = EnumUtil<TEnum>.ToByte(value);
                Assert.AreEqual(EnumUtil<TEnum>.FromByte(byteVal), value);

                sbyte sbyteVal = EnumUtil<TEnum>.ToSByte(value);
                Assert.AreEqual(EnumUtil<TEnum>.FromSByte(sbyteVal), value);

                short int16Val = EnumUtil<TEnum>.ToInt16(value);
                Assert.AreEqual(EnumUtil<TEnum>.FromInt16(int16Val), value);

                ushort uint16Val = EnumUtil<TEnum>.ToUInt16(value);
                Assert.AreEqual(EnumUtil<TEnum>.FromUInt16(uint16Val), value);

                int intVal = EnumUtil<TEnum>.ToInt32(value);
                Assert.AreEqual(EnumUtil<TEnum>.FromInt32(intVal), value);

                uint uintVal = EnumUtil<TEnum>.ToUInt32(value);
                Assert.AreEqual(EnumUtil<TEnum>.FromUInt32(uintVal), value);

                long longVal = EnumUtil<TEnum>.ToInt64(value);
                Assert.AreEqual(EnumUtil<TEnum>.FromInt64(longVal), value);

                ulong val = EnumUtil<TEnum>.ToUInt64(value);
                Assert.AreEqual(EnumUtil<TEnum>.FromUInt64(val), value);

                float floatVal = EnumUtil<TEnum>.ToSingle(value);
                Assert.AreEqual(EnumUtil<TEnum>.FromSingle(floatVal), value);

                double doubleVal = EnumUtil<TEnum>.ToDouble(value);
                Assert.AreEqual(EnumUtil<TEnum>.FromDouble(doubleVal), value);

                Assert.IsTrue(EnumUtil<TEnum>.BitwiseOr(value, value).Equals(value));
                Assert.IsTrue(EnumUtil<TEnum>.BitwiseAnd(value, value).Equals(value));
                Assert.IsTrue(EnumUtil<TEnum>.BitwiseExclusiveOr(value, value).Equals(default(TEnum)));

                Assert.IsTrue(EnumUtil<TEnum>.HasFlag(aggregate, value));
                Assert.IsFalse(EnumUtil<TEnum>.HasFlag(value, aggregate));
                Assert.IsTrue(EnumUtil<TEnum>.HasFlag(aggregate, default));

                Assert.IsFalse(EnumUtil<TEnum>.HasFlagsAttribute());
                Assert.IsFalse(EnumUtil<TEnum>.HasAttribute<FlagsAttribute>());

                Assert.IsTrue(EnumUtil<TEnum>.IsDefined(value));
                Assert.IsFalse(EnumUtil<TEnum>.IsDefined(default(TEnum)));

                Assert.AreEqual(name, EnumUtil<TEnum>.GetName(value));
                Assert.AreEqual(value, EnumUtil<TEnum>.Parse(name));
                Assert.AreEqual(value, EnumUtil<TEnum>.QuickParse(name));

                Type t = EnumUtil<TEnum>.GetUnderlyingType();
                Assert.AreEqual(t, Enum.GetUnderlyingType(typeof(TEnum)));

                Assert.IsTrue(EnumUtil<TEnum>.TryParse(name, out value));
                Assert.IsTrue(EnumUtil<TEnum>.TryParse(name, false, out value));
                Assert.IsFalse(EnumUtil<TEnum>.TryParse(name.ToLower(), false, out value));
                Assert.IsFalse(EnumUtil<TEnum>.TryParse(name.ToUpper(), false, out value));

                Assert.IsTrue(EnumUtil<TEnum>.TryParse(name.ToLower(), true, out value));
                Assert.IsTrue(EnumUtil<TEnum>.TryParse(name.ToUpper(), true, out value));
                Assert.IsTrue(EnumUtil<TEnum>.TryParse(name, true, out value));
            }
        }

        [TestMethod]
        public void SetFlag()
        {
            var value = default(FlagsEnum);
            Assert.AreEqual(FlagsEnum.One, EnumUtil<FlagsEnum>.SetFlag(value, FlagsEnum.One));
            Assert.AreEqual(FlagsEnum.Two, EnumUtil<FlagsEnum>.SetFlag(value, FlagsEnum.Two));

            value = FlagsEnum.One;
            Assert.AreEqual(FlagsEnum.One, EnumUtil<FlagsEnum>.SetFlag(value, FlagsEnum.One));
            Assert.AreEqual(FlagsEnum.One | FlagsEnum.Two, EnumUtil<FlagsEnum>.SetFlag(value, FlagsEnum.Two));
        }

        [TestMethod]
        public void IsDefinedString()
        {
            Assert.IsTrue(EnumUtil<FlagsEnum>.IsDefined(EnumUtil<FlagsEnum>.GetName(FlagsEnum.One)));
            Assert.IsTrue(EnumUtil<FlagsEnum>.IsDefined(EnumUtil<FlagsEnum>.GetName(FlagsEnum.Two)));
            Assert.IsTrue(EnumUtil<FlagsEnum>.IsDefined(EnumUtil<FlagsEnum>.GetName(FlagsEnum.Four)));
            Assert.IsTrue(EnumUtil<FlagsEnum>.IsDefined(EnumUtil<FlagsEnum>.GetName(FlagsEnum.Eight)));
            Assert.IsFalse(EnumUtil<FlagsEnum>.IsDefined(string.Empty));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IsDefinedStringNull()
        {
            Assert.IsFalse(EnumUtil<FlagsEnum>.IsDefined(null));
        }

        [TestMethod]
        public void IsDefinedUnderlyingType() {
            
            IsDefined<SByteEnum>();
            IsDefined<ByteEnum>();
            IsDefined<UInt16Enum>();
            IsDefined<Int16Enum>();
            IsDefined<UInt32Enum>();
            IsDefined<Int32Enum>();
            IsDefined<Int64Enum>();
            IsDefined<UInt64Enum>();
        }

        private static void IsDefined<TEnum>()
            where TEnum : struct, Enum, IComparable, IFormattable, IConvertible
        {
            Assert.IsFalse(EnumUtil<TEnum>.IsDefined(default(sbyte)));
            Assert.IsFalse(EnumUtil<TEnum>.IsDefined(default(byte)));
            Assert.IsFalse(EnumUtil<TEnum>.IsDefined(default(ushort)));
            Assert.IsFalse(EnumUtil<TEnum>.IsDefined(default(short)));
            Assert.IsFalse(EnumUtil<TEnum>.IsDefined(default(uint)));
            Assert.IsFalse(EnumUtil<TEnum>.IsDefined(default(int)));
            Assert.IsFalse(EnumUtil<TEnum>.IsDefined(default(long)));
            Assert.IsFalse(EnumUtil<TEnum>.IsDefined(default(ulong)));
            Assert.IsFalse(EnumUtil<TEnum>.IsDefined(default(float)));
            Assert.IsFalse(EnumUtil<TEnum>.IsDefined(default(double)));

            Assert.IsTrue(EnumUtil<TEnum>.IsDefined((sbyte)2));
            Assert.IsTrue(EnumUtil<TEnum>.IsDefined((byte)2));
            Assert.IsTrue(EnumUtil<TEnum>.IsDefined((ushort)2));
            Assert.IsTrue(EnumUtil<TEnum>.IsDefined((short)2));
            Assert.IsTrue(EnumUtil<TEnum>.IsDefined((uint)2));
            Assert.IsTrue(EnumUtil<TEnum>.IsDefined((int)2));
            Assert.IsTrue(EnumUtil<TEnum>.IsDefined((long)2));
            Assert.IsTrue(EnumUtil<TEnum>.IsDefined((ulong)2));
            Assert.IsTrue(EnumUtil<TEnum>.IsDefined((float)2));
            Assert.IsTrue(EnumUtil<TEnum>.IsDefined((double)2));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IsDefinedNullString()
        {
            Assert.IsFalse(EnumUtil<FlagsEnum>.IsDefined(null));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseException()
        {
            ByteEnum result = EnumUtil<ByteEnum>.Parse("Meh");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void QuickParseException()
        {
            ByteEnum result = EnumUtil<ByteEnum>.QuickParse("Meh");
        }


        [TestMethod]
        public void Parse()
        {
            AreEqual<ByteEnum>(ByteEnum.Eight.ToString());
            AreEqual<ByteEnum>(((byte)ByteEnum.Eight).ToString());

            AreEqual<SByteEnum>(SByteEnum.Eight.ToString());
            AreEqual<SByteEnum>(((sbyte)Int64Enum.Eight).ToString());

            AreEqual<Int16Enum>(Int16Enum.Eight.ToString());
            AreEqual<Int16Enum>(((short)Int16Enum.Eight).ToString());

            AreEqual<UInt16Enum>(UInt16Enum.Eight.ToString());
            AreEqual<UInt16Enum>(((ushort)UInt16Enum.Eight).ToString());

            AreEqual<Int32Enum>(Int32Enum.Eight.ToString());
            AreEqual<Int32Enum>(((int)Int32Enum.Eight).ToString());

            AreEqual<UInt32Enum>(UInt32Enum.Eight.ToString());
            AreEqual<UInt32Enum>(((uint)UInt32Enum.Eight).ToString());

            AreEqual<Int64Enum>(Int64Enum.Eight.ToString());
            AreEqual<Int64Enum>(((long)Int64Enum.Eight).ToString());

            AreEqual<UInt64Enum>(UInt64Enum.Eight.ToString());
            AreEqual<UInt64Enum>(((ulong)UInt64Enum.Eight).ToString());

            void AreEqual<TEnum>(string value)
                where TEnum: struct, Enum, IComparable, IFormattable, IConvertible
            {
                TEnum enumParse = (TEnum)Enum.Parse(typeof(TEnum), value);
                TEnum wrapperParse = EnumUtil<TEnum>.Parse(value);
                TEnum quickParse = EnumUtil<TEnum>.QuickParse(value);

                Assert.AreEqual(enumParse, wrapperParse);
                Assert.AreEqual(enumParse, quickParse);
            }
        }

        [TestMethod]
        public void UnsetFlag()
        {
            // Removing flags from empty value is no-op.
            var value = default(FlagsEnum);
            Assert.AreEqual(value, EnumUtil<FlagsEnum>.UnsetFlag(value, FlagsEnum.One));
            Assert.AreEqual(value, EnumUtil<FlagsEnum>.UnsetFlag(value, FlagsEnum.Two));

            // Starting with one flag.
            value = FlagsEnum.One;
            Assert.AreEqual(default(FlagsEnum), EnumUtil<FlagsEnum>.UnsetFlag(value, FlagsEnum.One));
            Assert.AreEqual(FlagsEnum.One, EnumUtil<FlagsEnum>.UnsetFlag(value, FlagsEnum.Two));

            // Starting with two flags.
            value = FlagsEnum.One | FlagsEnum.Two;
            Assert.AreEqual(FlagsEnum.Two, EnumUtil<FlagsEnum>.UnsetFlag(value, FlagsEnum.One));
            Assert.AreEqual(FlagsEnum.One, EnumUtil<FlagsEnum>.UnsetFlag(value, FlagsEnum.Two));
        }

        [TestMethod]
        public void ToggleFlag()
        {
            var value = default(FlagsEnum);
            // Toggling will effectively set when unset.
            Assert.AreEqual(FlagsEnum.One, EnumUtil<FlagsEnum>.ToggleFlag(value, FlagsEnum.One));
            Assert.AreEqual(FlagsEnum.Two, EnumUtil<FlagsEnum>.ToggleFlag(value, FlagsEnum.Two));

            Assert.AreEqual(FlagsEnum.One, EnumUtil<FlagsEnum>.ToggleFlag(value, FlagsEnum.One, true));
            Assert.AreEqual(FlagsEnum.Two, EnumUtil<FlagsEnum>.ToggleFlag(value, FlagsEnum.Two, true));

            Assert.AreEqual(default(FlagsEnum), EnumUtil<FlagsEnum>.ToggleFlag(value, FlagsEnum.One, false));
            Assert.AreEqual(default(FlagsEnum), EnumUtil<FlagsEnum>.ToggleFlag(value, FlagsEnum.Two, false));

            value = FlagsEnum.One;
            // Toggling will of course unset if already set.
            Assert.AreEqual(default(FlagsEnum), EnumUtil<FlagsEnum>.ToggleFlag(value, FlagsEnum.One));
            Assert.AreEqual(FlagsEnum.One | FlagsEnum.Two, EnumUtil<FlagsEnum>.ToggleFlag(value, FlagsEnum.Two));

            Assert.AreEqual(FlagsEnum.One, EnumUtil<FlagsEnum>.ToggleFlag(value, FlagsEnum.One, true));
            Assert.AreEqual(FlagsEnum.One | FlagsEnum.Two, EnumUtil<FlagsEnum>.ToggleFlag(value, FlagsEnum.Two, true));

            Assert.AreEqual(default(FlagsEnum), EnumUtil<FlagsEnum>.ToggleFlag(value, FlagsEnum.One, false));
            Assert.AreEqual(FlagsEnum.One, EnumUtil<FlagsEnum>.ToggleFlag(value, FlagsEnum.Two, false));

            // Starting with two flags.
            value = FlagsEnum.One | FlagsEnum.Two;
            Assert.AreEqual(FlagsEnum.Two, EnumUtil<FlagsEnum>.ToggleFlag(value, FlagsEnum.One));
            Assert.AreEqual(FlagsEnum.One, EnumUtil<FlagsEnum>.ToggleFlag(value, FlagsEnum.Two));

            Assert.AreEqual(FlagsEnum.One | FlagsEnum.Two, EnumUtil<FlagsEnum>.ToggleFlag(value, FlagsEnum.One, true));
            Assert.AreEqual(FlagsEnum.One | FlagsEnum.Two, EnumUtil<FlagsEnum>.ToggleFlag(value, FlagsEnum.Two, true));

            Assert.AreEqual(FlagsEnum.Two, EnumUtil<FlagsEnum>.ToggleFlag(value, FlagsEnum.One, false));
            Assert.AreEqual(FlagsEnum.One, EnumUtil<FlagsEnum>.ToggleFlag(value, FlagsEnum.Two, false));
        }

        [TestMethod]
        public void HasFlag()
        {
            foreach(FlagsEnum flag in EnumUtil<FlagsEnum>.GetValues())
            {
                Assert.IsTrue(EnumUtil<FlagsEnum>.HasFlag(flag, flag));
                Assert.IsFalse(EnumUtil<FlagsEnum>.HasFlag(default(FlagsEnum), flag));
            }
        }

        [TestMethod]
        public void HasFlagDefault()
        {
            bool resultEnumUtil = EnumUtil<FlagsEnum>.HasFlag(default(FlagsEnum), default(FlagsEnum));
            bool resultOfficial = default(FlagsEnum).HasFlag(default(FlagsEnum));
            Assert.AreEqual(resultEnumUtil, resultOfficial);
        }

        [TestMethod]
        public void TestEmptyEnum()
        {
            var values = EnumUtil<EmptyEnum>.GetValues();
            Assert.IsTrue(values.Length == 0);

            var valueDesc = EnumUtil<EmptyEnum>.GetValueDescription();
            Assert.IsTrue(valueDesc.Count == 0);

            var valueNameDesc = EnumUtil<EmptyEnum>.GetValueNameDescription();
            Assert.IsTrue(valueNameDesc.Count == 0);

            var nameValue = EnumUtil<EmptyEnum>.GetNameValue();
            Assert.IsTrue(nameValue.Count == 0);

            var names = EnumUtil<EmptyEnum>.GetNames();
            Assert.IsTrue(names.Length == 0);

            var fields = EnumUtil<EmptyEnum>.GetEnumFields();
            Assert.IsTrue(fields.Length == 0);

            var attributes = EnumUtil<EmptyEnum>.GetAttributes<DescriptionAttribute>();
            Assert.IsTrue(attributes.Count() == 0);

            var valueAttribute = EnumUtil<EmptyEnum>.GetValueAttribute<DescriptionAttribute>();
            Assert.IsTrue(valueAttribute.Count == 0);
        }
        
        [TestMethod]
        public void GetValuesEquivalentBehavior()
        {
            TestEquivalentValues<ByteEnum>();
            TestEquivalentValues<SByteEnum>();
            TestEquivalentValues<FlagsEnum>();
            TestEquivalentValues<UInt16Enum>();
            TestEquivalentValues<Int16Enum>();
            TestEquivalentValues<UInt32Enum>();
            TestEquivalentValues<Int32Enum>();
            TestEquivalentValues<UInt64Enum>();
            TestEquivalentValues<Int64Enum>();
        }

        private void TestEquivalentValues<TEnum>()
            where TEnum : struct, Enum, IComparable, IFormattable, IConvertible
        {
            var values = (TEnum[])typeof(TEnum).GetEnumValues();
            var values2 = EnumUtil<TEnum>.GetValues();

            Assert.IsTrue(values.Length == values2.Length);

            for (int i = 0; i < values.Length; i++)
            {
                Assert.AreEqual(values[i], values2[i]);
            }
        }
    }
}
