using EnumUtilTests.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace EnumUtilTests
{
    [TestClass]
    public class EmitTests
    {
        public static long TestEmit<T>()
        {
            var watch = Stopwatch.StartNew();

            Func<T, T> result;

                for (int i = 0; i < 200000; i++)
                {
                result = GenerateNot<T>();
                }

            watch.Stop();
            return watch.ElapsedMilliseconds;
        }

        public static long TestExpression<T>()
        {
            var watch = Stopwatch.StartNew();

            Func<T, T> result;

            for (int i = 0; i < 200000; i++)
            {
                result = GenerateExpressionNot<T>();
            }

            watch.Stop();
            return watch.ElapsedMilliseconds;
        }

        private static Func<T, T> GenerateExpressionNot<T>()
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

        public static Func<T, T> GenerateNot<T>()
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

        [TestMethod]
        public void EmitVsExpression()
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
