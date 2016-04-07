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
            var vn = EnumUtil.GetValueName<Int32Enum>();
            var nv = EnumUtil.GetNameValue<Int32Enum>();
            var vna = EnumUtil.GetValueNameAttributes<Int32Enum>();
            var vnd = EnumUtil.GetValueNameDescription<Int32Enum>();

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
            var vn = EnumUtil.GetValueName<FlagsEnum>();
            var nv = EnumUtil.GetNameValue<FlagsEnum>();
            var vna = EnumUtil.GetValueNameAttributes<FlagsEnum>();
            var vnd = EnumUtil.GetValueNameDescription<FlagsEnum>();

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
            Assert.IsTrue(EnumUtil.HasFlagsAttribute<FlagsEnum>());
            Assert.IsTrue(EnumUtil.HasAttribute<FlagsAttribute, FlagsEnum>());
            Assert.IsNotNull(EnumUtil.GetAttribute<FlagsAttribute, FlagsEnum>());

            foreach (FlagsEnum fe in EnumUtil.GetValues<FlagsEnum>())
            {
                System.ComponentModel.DescriptionAttribute da = EnumUtil.GetAttribute<System.ComponentModel.DescriptionAttribute, FlagsEnum>(fe);
                Assert.IsNotNull(da);
                Assert.IsFalse(string.IsNullOrEmpty(da.Description));
            }
            
        }

        [TestMethod]
        public void Int32Test() => RunTests<Enum, Int32Enum>();

        [TestMethod]
        public void UInt32Test() => RunTests<Enum, UInt32Enum>();

        [TestMethod]
        public void Int64Test() => RunTests<Enum, Int64Enum>();

        [TestMethod]
        public void UInt64Test() => RunTests<Enum, UInt64Enum>();

        [TestMethod]
        public void ByteTest() => RunTests<Enum, ByteEnum>();

        [TestMethod]
        public void Int16Test() => RunTests<Enum, Int16Enum>();

        [TestMethod]
        public void UInt16Test() => RunTests<Enum, UInt16Enum>();

        [TestMethod]
        public void SByteTest() => RunTests<Enum, SByteEnum>();

        private static void RunTests<E, T>()
            where E : class, IComparable, IFormattable, IConvertible
            where T : struct, E
        {
            string[] names = EnumUtilBase<E>.GetNames<T>();
            T[] values = EnumUtilBase<E>.GetValues<T>();

            IReadOnlyDictionary<T, string> nameValues = 
                EnumUtilBase<E>.GetValueName<T>();
            Assert.IsNotNull(nameValues);

            foreach (var nm in nameValues)
            {
                Assert.IsNotNull(nm.Value);
            }

            Assert.IsTrue(nameValues.Count == values.Length);
            Assert.IsTrue(Enum.GetValues(typeof(T)).Length == values.Length);
            Assert.IsTrue(names.Length == values.Length);

            T aggregate = values.Aggregate((x, y) => EnumUtilBase<E>.BitwiseOr<T>(x, y));
            Assert.IsTrue(Convert.ToInt32(aggregate) == 15);
            T aggregate2 = values.Aggregate((x, y) => EnumUtilBase<E>.BitwiseAnd<T>(x, y));
            Assert.IsTrue(Convert.ToInt32(aggregate2) == 0);

            for (int i = 0; i < values.Length; i++)
            {
                T value = values[i];
                string name = names[i];

                byte byteVal = EnumUtilBase<E>.ToByte(value);
                Assert.AreEqual(EnumUtilBase<E>.FromByte<T>(byteVal), value);

                sbyte sbyteVal = EnumUtilBase<E>.ToSByte(value);
                Assert.AreEqual(EnumUtilBase<E>.FromSByte<T>(sbyteVal), value);

                short int16Val = EnumUtilBase<E>.ToInt16(value);
                Assert.AreEqual(EnumUtilBase<E>.FromInt16<T>(int16Val), value);

                ushort uint16Val = EnumUtilBase<E>.ToUInt16(value);
                Assert.AreEqual(EnumUtilBase<E>.FromUInt16<T>(uint16Val), value);

                int intVal = EnumUtilBase<E>.ToInt32(value);
                Assert.AreEqual(EnumUtilBase<E>.FromInt32<T>(intVal), value);

                uint uintVal = EnumUtilBase<E>.ToUInt32(value);
                Assert.AreEqual(EnumUtilBase<E>.FromUInt32<T>(uintVal), value);

                long longVal = EnumUtilBase<E>.ToInt64(value);
                Assert.AreEqual(EnumUtilBase<E>.FromInt64<T>(longVal), value);

                ulong val = EnumUtilBase<E>.ToUInt64(value);
                Assert.AreEqual(EnumUtilBase<E>.FromUInt64<T>(val), value);

                float floatVal = EnumUtilBase<E>.ToSingle(value);
                Assert.AreEqual(EnumUtilBase<E>.FromSingle<T>(floatVal), value);

                double doubleVal = EnumUtilBase<E>.ToDouble(value);
                Assert.AreEqual(EnumUtilBase<E>.FromDouble<T>(doubleVal), value);

                Assert.IsTrue(EnumUtilBase<E>.BitwiseOr(value, value).Equals(value));
                Assert.IsTrue(EnumUtilBase<E>.BitwiseAnd(value, value).Equals(value));
                Assert.IsTrue(EnumUtilBase<E>.BitwiseExclusiveOr(value, value).Equals(default(T)));

                Assert.IsTrue(EnumUtilBase<E>.HasFlag(aggregate, value));
                Assert.IsFalse(EnumUtilBase<E>.HasFlag(value, aggregate));
                Assert.IsTrue(EnumUtilBase<E>.HasFlag(aggregate, default(T)));

                Assert.IsFalse(EnumUtilBase<E>.HasFlagsAttribute<T>());
                Assert.IsFalse(EnumUtilBase<E>.HasAttribute<FlagsAttribute, T>());

                Assert.IsTrue(EnumUtilBase<E>.IsDefined(value));
                Assert.IsFalse(EnumUtilBase<E>.IsDefined(default(T)));

                Assert.AreEqual(name, EnumUtilBase<E>.GetName(value));
                Assert.AreEqual(value, EnumUtilBase<E>.Parse<T>(name));

                Type t = EnumUtilBase<E>.GetUnderlyingType<T>();
                Assert.AreEqual(t, Enum.GetUnderlyingType(typeof(T)));

                Assert.IsTrue(EnumUtilBase<E>.TryParse(name, out value));
                Assert.IsTrue(EnumUtilBase<E>.TryParse(name, false, out value));
                Assert.IsFalse(EnumUtilBase<E>.TryParse(name.ToLower(), false, out value));
                Assert.IsFalse(EnumUtilBase<E>.TryParse(name.ToUpper(), false, out value));

                Assert.IsTrue(EnumUtilBase<E>.TryParse(name.ToLower(), true, out value));
                Assert.IsTrue(EnumUtilBase<E>.TryParse(name.ToUpper(), true, out value));
                Assert.IsTrue(EnumUtilBase<E>.TryParse(name, true, out value));
            }
        }

        [TestMethod]
        public void SetFlag()
        {
            var value = default(FlagsEnum);
            Assert.AreEqual(FlagsEnum.One, EnumUtil.SetFlag(value, FlagsEnum.One));
            Assert.AreEqual(FlagsEnum.Two, EnumUtil.SetFlag(value, FlagsEnum.Two));

            value = FlagsEnum.One;
            Assert.AreEqual(FlagsEnum.One, EnumUtil.SetFlag(value, FlagsEnum.One));
            Assert.AreEqual(FlagsEnum.One | FlagsEnum.Two, EnumUtil.SetFlag(value, FlagsEnum.Two));
        }

        [TestMethod]
        public void IsDefinedString()
        {
            Assert.IsTrue(EnumUtil.IsDefined<FlagsEnum>(EnumUtil.GetName(FlagsEnum.One)));
            Assert.IsTrue(EnumUtil.IsDefined<FlagsEnum>(EnumUtil.GetName(FlagsEnum.Two)));
            Assert.IsTrue(EnumUtil.IsDefined<FlagsEnum>(EnumUtil.GetName(FlagsEnum.Four)));
            Assert.IsTrue(EnumUtil.IsDefined<FlagsEnum>(EnumUtil.GetName(FlagsEnum.Eight)));
            Assert.IsFalse(EnumUtil.IsDefined<FlagsEnum>(string.Empty));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IsDefinedStringNull()
        {
            Assert.IsFalse(EnumUtil.IsDefined<FlagsEnum>(null));
        }

        [TestMethod]
        public void IsDefinedUnderlyingType() {
            
            IsDefined<Enum, SByteEnum>();
            IsDefined<Enum, ByteEnum>();
            IsDefined<Enum, UInt16Enum>();
            IsDefined<Enum, Int16Enum>();
            IsDefined<Enum, UInt32Enum>();
            IsDefined<Enum, Int32Enum>();
            IsDefined<Enum, Int64Enum>();
            IsDefined<Enum, UInt64Enum>();
        }

        private static void IsDefined<E, T>()
            where E : class, IComparable, IFormattable, IConvertible
            where T : struct, E
        {
            Assert.IsFalse(EnumUtilBase<E>.IsDefined<T>(default(sbyte)));
            Assert.IsFalse(EnumUtilBase<E>.IsDefined<T>(default(byte)));
            Assert.IsFalse(EnumUtilBase<E>.IsDefined<T>(default(ushort)));
            Assert.IsFalse(EnumUtilBase<E>.IsDefined<T>(default(short)));
            Assert.IsFalse(EnumUtilBase<E>.IsDefined<T>(default(uint)));
            Assert.IsFalse(EnumUtilBase<E>.IsDefined<T>(default(int)));
            Assert.IsFalse(EnumUtilBase<E>.IsDefined<T>(default(long)));
            Assert.IsFalse(EnumUtilBase<E>.IsDefined<T>(default(ulong)));

            Assert.IsTrue(EnumUtilBase<E>.IsDefined<T>((sbyte)2));
            Assert.IsTrue(EnumUtilBase<E>.IsDefined<T>((byte)2));
            Assert.IsTrue(EnumUtilBase<E>.IsDefined<T>((ushort)2));
            Assert.IsTrue(EnumUtilBase<E>.IsDefined<T>((short)2));
            Assert.IsTrue(EnumUtilBase<E>.IsDefined<T>((uint)2));
            Assert.IsTrue(EnumUtilBase<E>.IsDefined<T>((int)2));
            Assert.IsTrue(EnumUtilBase<E>.IsDefined<T>((long)2));
            Assert.IsTrue(EnumUtilBase<E>.IsDefined<T>((ulong)2));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IsDefinedNullString()
        {
            Assert.IsFalse(EnumUtil.IsDefined<FlagsEnum>(null));
        }

        [TestMethod]
        public void UnsetFlag()
        {
            // Removing flags from empty value is no-op.
            var value = default(FlagsEnum);
            Assert.AreEqual(value, EnumUtil.UnsetFlag(value, FlagsEnum.One));
            Assert.AreEqual(value, EnumUtil.UnsetFlag(value, FlagsEnum.Two));

            // Starting with one flag.
            value = FlagsEnum.One;
            Assert.AreEqual(default(FlagsEnum), EnumUtil.UnsetFlag(value, FlagsEnum.One));
            Assert.AreEqual(FlagsEnum.One, EnumUtil.UnsetFlag(value, FlagsEnum.Two));

            // Starting with two flags.
            value = FlagsEnum.One | FlagsEnum.Two;
            Assert.AreEqual(FlagsEnum.Two, EnumUtil.UnsetFlag(value, FlagsEnum.One));
            Assert.AreEqual(FlagsEnum.One, EnumUtil.UnsetFlag(value, FlagsEnum.Two));
        }

        [TestMethod]
        public void ToggleFlag()
        {
            var value = default(FlagsEnum);
            // Toggling will effectively set when unset.
            Assert.AreEqual(FlagsEnum.One, EnumUtil.ToggleFlag(value, FlagsEnum.One));
            Assert.AreEqual(FlagsEnum.Two, EnumUtil.ToggleFlag(value, FlagsEnum.Two));

            Assert.AreEqual(FlagsEnum.One, EnumUtil.ToggleFlag(value, FlagsEnum.One, true));
            Assert.AreEqual(FlagsEnum.Two, EnumUtil.ToggleFlag(value, FlagsEnum.Two, true));

            Assert.AreEqual(default(FlagsEnum), EnumUtil.ToggleFlag(value, FlagsEnum.One, false));
            Assert.AreEqual(default(FlagsEnum), EnumUtil.ToggleFlag(value, FlagsEnum.Two, false));

            value = FlagsEnum.One;
            // Toggling will of course unset if already set.
            Assert.AreEqual(default(FlagsEnum), EnumUtil.ToggleFlag(value, FlagsEnum.One));
            Assert.AreEqual(FlagsEnum.One | FlagsEnum.Two, EnumUtil.ToggleFlag(value, FlagsEnum.Two));

            Assert.AreEqual(FlagsEnum.One, EnumUtil.ToggleFlag(value, FlagsEnum.One, true));
            Assert.AreEqual(FlagsEnum.One | FlagsEnum.Two, EnumUtil.ToggleFlag(value, FlagsEnum.Two, true));

            Assert.AreEqual(default(FlagsEnum), EnumUtil.ToggleFlag(value, FlagsEnum.One, false));
            Assert.AreEqual(FlagsEnum.One, EnumUtil.ToggleFlag(value, FlagsEnum.Two, false));

            // Starting with two flags.
            value = FlagsEnum.One | FlagsEnum.Two;
            Assert.AreEqual(FlagsEnum.Two, EnumUtil.ToggleFlag(value, FlagsEnum.One));
            Assert.AreEqual(FlagsEnum.One, EnumUtil.ToggleFlag(value, FlagsEnum.Two));

            Assert.AreEqual(FlagsEnum.One | FlagsEnum.Two, EnumUtil.ToggleFlag(value, FlagsEnum.One, true));
            Assert.AreEqual(FlagsEnum.One | FlagsEnum.Two, EnumUtil.ToggleFlag(value, FlagsEnum.Two, true));

            Assert.AreEqual(FlagsEnum.Two, EnumUtil.ToggleFlag(value, FlagsEnum.One, false));
            Assert.AreEqual(FlagsEnum.One, EnumUtil.ToggleFlag(value, FlagsEnum.Two, false));
        }

        [TestMethod]
        public void HasFlag()
        {
            foreach(FlagsEnum flag in EnumUtil.GetValues<FlagsEnum>())
            {
                Assert.IsTrue(EnumUtil.HasFlag(flag, flag));
                Assert.IsFalse(EnumUtil.HasFlag(default(FlagsEnum), flag));
            }
        }

        [TestMethod]
        public void HasFlagDefault()
        {
            bool resultEnumUtil = EnumUtil.HasFlag(default(FlagsEnum), default(FlagsEnum));
            bool resultOfficial = default(FlagsEnum).HasFlag(default(FlagsEnum));
            Assert.AreEqual(resultEnumUtil, resultOfficial);
        }

        [TestMethod]
        public void TestEmptyEnum()
        {
            var values = EnumUtil.GetValues<EmptyEnum>();
            Assert.IsTrue(values.Length == 0);

            var valueDesc = EnumUtil.GetValueDescription<EmptyEnum>();
            Assert.IsTrue(valueDesc.Count == 0);

            var valueNameDesc = EnumUtil.GetValueNameDescription<EmptyEnum>();
            Assert.IsTrue(valueNameDesc.Count == 0);

            var nameValue = EnumUtil.GetNameValue<EmptyEnum>();
            Assert.IsTrue(nameValue.Count == 0);

            var names = EnumUtil.GetNames<EmptyEnum>();
            Assert.IsTrue(names.Length == 0);

            var fields = EnumUtil.GetEnumFields<EmptyEnum>();
            Assert.IsTrue(fields.Length == 0);

            var attributes = EnumUtil.GetAttributes<DescriptionAttribute, EmptyEnum>();
            Assert.IsTrue(attributes.Count() == 0);

            var valueAttribute = EnumUtil.GetValueAttribute<EmptyEnum, DescriptionAttribute>();
            Assert.IsTrue(valueAttribute.Count == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Type provided must be an Enum.")]
        public void UnsafeImproperGetValues()
        {
            NotEnum[] meg = EnumUtilUnsafe<NotEnum>.GetValues<NotEnum>();
        }

        [TestMethod]
        [ExpectedException(typeof(TypeInitializationException), "Type provided must be an Enum.")]
        public void UnsafeImproperOr()
        {
            NotEnum or = EnumUtilUnsafe<NotEnum>.BitwiseOr(new NotEnum(), new NotEnum());
        }

        [TestMethod]
        public void GetValuesEquivalentBehavior()
        {
            TestEquivalentValues<Enum, ByteEnum>();
            TestEquivalentValues<Enum, SByteEnum>();
            TestEquivalentValues<Enum, FlagsEnum>();
            TestEquivalentValues<Enum, UInt16Enum>();
            TestEquivalentValues<Enum, Int16Enum>();
            TestEquivalentValues<Enum, UInt32Enum>();
            TestEquivalentValues<Enum, Int32Enum>();
            TestEquivalentValues<Enum, UInt64Enum>();
            TestEquivalentValues<Enum, Int64Enum>();
        }

        private void TestEquivalentValues<E, T>()
            where E : class, IComparable, IFormattable, IConvertible
            where T : struct, E
        {
            var values = (T[])typeof(T).GetEnumValues();
            var values2 = EnumUtilBase<E>.GetValues<T>();

            Assert.IsTrue(values.Length == values2.Length);

            for (int i = 0; i < values.Length; i++)
            {
                Assert.AreEqual(values[i], values2[i]);
            }
        }
    }
}
