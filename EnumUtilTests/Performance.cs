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

        [TestMethod]
        public void IsDefinedNumericPerformance()
        {
            // warmup
            long gen = IsDefinedGeneric(out int ii);
            long std = IsDefinedStandard(out ii);

            // test
            gen = IsDefinedGeneric(out ii);
            std = IsDefinedStandard(out ii);

            Assert.IsTrue(std > gen);

            long IsDefinedGeneric(out int i)
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

            long IsDefinedStandard(out int i)
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
        }

        [TestMethod]
        public void TestIsDefinedEnumPerformance()
        {
            // warmup
            long gen = IsDefinedGenericEnum(out int ii);
            long std = IsDefinedStandardEnum(out ii);

            // test
            gen = IsDefinedGenericEnum(out ii);
            std = IsDefinedStandardEnum(out ii);

            Assert.IsTrue(std > gen);

            long IsDefinedGenericEnum(out int i)
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

            long IsDefinedStandardEnum(out int i)
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
        }
        
        [TestMethod]
        public void TestHasFlagGenericSpeed()
        {
            // Test that the new generic implementation
            // is faster than the default implementation
            // of .HasFlag(...)
            
            // warmup
            long gn = TestGeneric(out int ii);
            long nat = TestSpeedNatural(out ii);

            // test
            gn = TestGeneric(out ii);
            nat = TestSpeedNatural(out ii);

            Assert.IsTrue(nat > gn);

            long TestGeneric(out int i)
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

            long TestSpeedNatural(out int i)
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
        }

        [TestMethod]
        public void TestQuickParse()
        {
            // Test that the new generic implementation
            // is faster than the default implementation
            // of .Parse(...)
            
            // warmup
            long gn = TestGeneric(out int ii);
            long nat = TestSpeedNatural(out ii);

            // test
            gn = TestGeneric(out ii);
            nat = TestSpeedNatural(out ii);

            Assert.IsTrue(nat > gn);

            long TestSpeedNatural(out int i)
            {
                var watch = Stopwatch.StartNew();


                string flag = FlagsEnum.Four.ToString();
                FlagsEnum discardMe;

                i = 0;
                for (; i < 2000000; i++)
                {
                    discardMe = (FlagsEnum)Enum.Parse(typeof(FlagsEnum), flag);
                }

                watch.Stop();
                return watch.ElapsedMilliseconds;
            }

            long TestGeneric(out int i)
            {
                var watch = Stopwatch.StartNew();

                string flag = FlagsEnum.Four.ToString();
                FlagsEnum discardMe;
                i = 0;
                for (; i < 2000000; i++)
                {
                    discardMe = EnumUtil<FlagsEnum>.QuickParse(flag);
                }

                watch.Stop();
                return watch.ElapsedMilliseconds;
            }
        }
    }
}
