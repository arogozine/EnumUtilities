using EnumUtilTests.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EnumUtilTests.EmitPerformance
{
    public abstract class EmitPerfBase<TOut>
    {
        public abstract Func<T, T, TOut> GenerateEmit<T>();
        public abstract Func<T, T, TOut> GenerateExpression<T>();

        public long TestEmit<T>()
        {
            var watch = Stopwatch.StartNew();

            Func<T, T, TOut> result;

            for (int i = 0; i < 20000; i++)
            {
                result = GenerateEmit<T>();
            }

            watch.Stop();
            return watch.ElapsedMilliseconds;
        }

        public long TestExpression<T>()
        {
            var watch = Stopwatch.StartNew();

            Func<T, T, TOut> result;

            for (int i = 0; i < 20000; i++)
            {
                result = GenerateExpression<T>();
            }

            watch.Stop();
            return watch.ElapsedMilliseconds;
        }

        [TestMethod]
        public void FasterCompileTime()
        {
            // warmup
            long emt = TestEmit<Int32Enum>();
            long exp = TestExpression<Int32Enum>();

            // test
            emt = TestEmit<Int32Enum>();
            exp = TestExpression<Int32Enum>();

            Assert.IsTrue(emt < exp);
        }
    }
}
