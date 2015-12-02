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
        public static long TestGeneric(out int i) {
            var watch = Stopwatch.StartNew();

            FlagsEnum natural = FlagsEnum.Two | FlagsEnum.Four;
            FlagsEnum flag = FlagsEnum.Four;

            bool discardMe = false;
            i = 0;
            for (; i < 2000000; i++)
            {
                discardMe = EnumUtil.HasFlag(natural, flag);
            }

            watch.Stop();
            return watch.ElapsedMilliseconds;
        }

        public static long TestSpeedNatural(out int i)
        {
            var watch = Stopwatch.StartNew();

            FlagsEnum natural = FlagsEnum.Two | FlagsEnum.Four;
            FlagsEnum flag = FlagsEnum.Four;

            bool discardMe = false;
            i = 0;
            for (; i < 2000000; i++)
            {
                discardMe = natural.HasFlag(flag);
            }

            watch.Stop();
            return watch.ElapsedMilliseconds;
        }

        [TestMethod]
        public void TestHasFlagGenericSpeed()
        {
            // Test that the new generic implementation
            // is fater than the default implementation
            // of .HasFlag(...)

            int i;
            // warmup
            long gn = TestGeneric( out i);
            long nat = TestSpeedNatural(out i);

            // test
            gn = TestGeneric(out i);
            nat = TestSpeedNatural(out i);
            
            Assert.IsTrue(nat > gn);
        }

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

        private static void RunTests<T>()
            where T : struct, IComparable, IFormattable, IConvertible
        {
            string[] names = EnumUtilBase<T>.GetNames<T>();
            T[] values = EnumUtilBase<T>.GetValues<T>();

            IReadOnlyDictionary<T, string> nameValues = 
                EnumUtilBase<T>.GetValueName<T>();
            Assert.IsNotNull(nameValues);

            foreach (var nm in nameValues)
            {
                Assert.IsNotNull(nm.Value);
            }

            Assert.IsTrue(nameValues.Count == values.Length);
            Assert.IsTrue(Enum.GetValues(typeof(T)).Length == values.Length);
            Assert.IsTrue(names.Length == values.Length);

            T aggregate = values.Aggregate((x, y) => EnumUtilBase<T>.BitwiseOr<T>(x, y));
            Assert.IsTrue(Convert.ToInt32(aggregate) == 15);
            T aggregate2 = values.Aggregate((x, y) => EnumUtilBase<T>.BitwiseAnd<T>(x, y));
            Assert.IsTrue(Convert.ToInt32(aggregate2) == 0);

            for (int i = 0; i < values.Length; i++)
            {
                T value = values[i];
                string name = names[i];

                byte byteVal = EnumUtilBase<T>.ToByte(value);
                sbyte sbyteVal = EnumUtilBase<T>.ToSByte(value);
                short int16Val = EnumUtilBase<T>.ToInt16(value);
                ushort uint16Val = EnumUtilBase<T>.ToUInt16(value);
                int intVal = EnumUtilBase<T>.ToInt32(value);
                uint uintVal = EnumUtilBase<T>.ToUInt32(value);
                long longVal = EnumUtilBase<T>.ToInt64(value);
                ulong val = EnumUtilBase<T>.ToUInt64(value);

                Assert.IsTrue(EnumUtilBase<T>.BitwiseOr(value, value).Equals(value));
                Assert.IsTrue(EnumUtilBase<T>.BitwiseAnd(value, value).Equals(value));
                Assert.IsTrue(EnumUtilBase<T>.BitwiseExclusiveOr(value, value).Equals(default(T)));

                Assert.IsTrue(EnumUtilBase<T>.HasFlag(aggregate, value));
                Assert.IsFalse(EnumUtilBase<T>.HasFlag(value, aggregate));
                Assert.IsTrue(EnumUtilBase<T>.HasFlag(aggregate, default(T)));

                Assert.IsFalse(EnumUtilBase<T>.HasFlagsAttribute<T>());
                Assert.IsFalse(EnumUtilBase<T>.HasAttribute<FlagsAttribute, T>());

                Assert.AreEqual(name, EnumUtilBase<T>.GetName(value));
                Assert.AreEqual(value, EnumUtilBase<T>.Parse<T>(name));

                Type t = EnumUtilBase<T>.GetUnderlyingType<T>();
                Assert.AreEqual(t, Enum.GetUnderlyingType(typeof(T)));

                Assert.IsTrue(EnumUtilBase<T>.TryParse(name, out value));
                Assert.IsTrue(EnumUtilBase<T>.TryParse(name, false, out value));
                Assert.IsFalse(EnumUtilBase<T>.TryParse(name.ToLower(), false, out value));
                Assert.IsFalse(EnumUtilBase<T>.TryParse(name.ToUpper(), false, out value));

                Assert.IsTrue(EnumUtilBase<T>.TryParse(name.ToLower(), true, out value));
                Assert.IsTrue(EnumUtilBase<T>.TryParse(name.ToUpper(), true, out value));
                Assert.IsTrue(EnumUtilBase<T>.TryParse(name, true, out value));
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
    }
}
