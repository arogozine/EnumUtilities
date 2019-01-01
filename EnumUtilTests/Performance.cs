using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EnumUtilTests.Enums;
using System.Diagnostics;
using EnumUtilities;

namespace EnumUtilTests
{
    [TestClass]
    public class Performance
    {
        // For methods rewritten, the performance is expected to be higher
        // than the default implementation

        public static long IsDefinedStandard(out int i)
        {
            var watch = Stopwatch.StartNew();

            bool isDefined = true;

            i = 0;
            foreach (int val in EnumUtil<Int32Enum>.GetValues())
            {
                for (i = 0; i < 200000; i++)
                {
                    isDefined &= Enum.IsDefined(typeof(Int32Enum), val);
                }
            }

            watch.Stop();
            Assert.IsTrue(isDefined);
            return watch.ElapsedMilliseconds;
        }

        public static long IsDefinedGeneric(out int i)
        {
            var watch = Stopwatch.StartNew();

            bool isDefined = true;

            var isDefinedFunc =

            i = 0;
            foreach (byte val in EnumUtil<Int32Enum>.GetValues())
            {
                for (i = 0; i < 200000; i++)
                {
                    isDefined &= EnumUtil<Int32Enum>.IsDefined(val);
                }
            }

            watch.Stop();
            Assert.IsTrue(isDefined);
            return watch.ElapsedMilliseconds;
        }

        [TestMethod]
        public void IsDefinedNumericPerformance()
        {
            int i;
            // warmup
            long gen = IsDefinedGeneric(out i);
            long std = IsDefinedStandard(out i);

            // test
            gen = IsDefinedGeneric(out i);
            std = IsDefinedStandard(out i);

            Assert.IsTrue(std > gen);
        }

        [TestMethod]
        public void TestIsDefinedEnumPerformance()
        {
            int i;
            // warmup
            long gen = IsDefinedGenericEnum(out i);
            long std = IsDefinedStandardEnum(out i);

            // test
            gen = IsDefinedGenericEnum(out i);
            std = IsDefinedStandardEnum(out i);

            Assert.IsTrue(std > gen);
        }

        public static long IsDefinedStandardEnum(out int i)
        {
            var watch = Stopwatch.StartNew();

            bool isDefined = true;

            i = 0;
            foreach (var val in EnumUtil<Int32Enum>.GetValues())
            {
                for (i = 0; i < 200000; i++)
                {
                    isDefined &= Enum.IsDefined(typeof(Int32Enum), Int32Enum.Two);
                }
            }

            watch.Stop();
            Assert.IsTrue(isDefined);
            return watch.ElapsedMilliseconds;
        }

        public static long IsDefinedGenericEnum(out int i)
        {
            var watch = Stopwatch.StartNew();

            bool isDefined = true;
            i = 0;
            foreach (var val in EnumUtil<Int32Enum>.GetValues())
            {
                for (i = 0; i < 200000; i++)
                {
                    isDefined &= EnumUtil<Int32Enum>.IsDefined(val);
                }
            }

            watch.Stop();
            Assert.IsTrue(isDefined);
            return watch.ElapsedMilliseconds;
        }


        public static long TestGeneric(out int i)
        {
            var watch = Stopwatch.StartNew();

            FlagsEnum natural = FlagsEnum.Two | FlagsEnum.Four;
            FlagsEnum flag = FlagsEnum.Four;

            bool discardMe = false;
            i = 0;
            for (; i < 2000000; i++)
            {
                discardMe = EnumUtil<FlagsEnum>.HasFlag(natural, flag);
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
            // is faster than the default implementation
            // of .HasFlag(...)

            int i;
            // warmup
            long gn = TestGeneric(out i);
            long nat = TestSpeedNatural(out i);

            // test
            gn = TestGeneric(out i);
            nat = TestSpeedNatural(out i);

            Assert.IsTrue(nat > gn);
        }
    }
}
