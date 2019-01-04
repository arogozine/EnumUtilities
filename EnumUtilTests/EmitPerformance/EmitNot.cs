using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Linq.Expressions;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using EnumUtilTests.Enums;

namespace EnumUtilTests.EmitPerformance
{
    [TestClass]
    public class EmitNot
    {
        public Func<T, T> GenerateEmit<T>()
        {
            DynamicMethod dm =
                new DynamicMethod("Not", typeof(T), new[] { typeof(T) });
            ILGenerator lgen = dm.GetILGenerator();
            // IL_0000: ldarg.0
            // IL_0001: not
            // IL_0002: ret
            lgen.Emit(OpCodes.Ldarg_0);
            lgen.Emit(OpCodes.Not);
            lgen.Emit(OpCodes.Ret);
            // Finish the method and create new delegate
            // pointing at it.
            return (Func<T, T>)dm.CreateDelegate(
                typeof(Func<T, T>));
        }

        public Func<T, T> GenerateExpression<T>()
        {
            var val = Expression.Parameter(typeof(T));

            // Convert from Enum to Enum’s underlying type (byte, int, long, …)
            // to allow bitwise functions to work
            var valConverted = Expression.Convert(val, Enum.GetUnderlyingType(typeof(T)));

            var unaryExpression =
                Expression.MakeUnary(
                    ExpressionType.Not,
                    valConverted,
                    null);

            // Convert back to Enum
            var backToEnumType = Expression.Convert(unaryExpression, typeof(T));
            return Expression.Lambda<Func<T, T>>(backToEnumType, val)
                .Compile();
        }

        public long TestEmit<T>()
        {
            var watch = Stopwatch.StartNew();

            Func<T, T> result;

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

            Func<T, T> result;

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
