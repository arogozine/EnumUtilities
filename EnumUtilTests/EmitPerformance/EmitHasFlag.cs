using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Text;

namespace EnumUtilTests.EmitPerformance
{

    [TestClass]
    public class EmitHasFlag : EmitPerfBase<bool>
    {
        public override Func<T, T, bool> GenerateEmit<T>()
        {
            var dm = new DynamicMethod("HasFlag",
                typeof(bool),
                new[] { typeof(T), typeof(T) });
            ILGenerator generator = dm.GetILGenerator();

            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.And);
            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Ceq);
            generator.Emit(OpCodes.Ret);


            return (Func<T, T, bool>)
                dm.CreateDelegate(typeof(Func<T, T, bool>));
        }

        public override Func<T, T, bool> GenerateExpression<T>()
        {
            var value = Expression.Parameter(typeof(T));
            var flag = Expression.Parameter(typeof(T));

            // Convert from Enum to underlying type (byte, int, long, ...)
            // to allow bitwise functions to work
            UnaryExpression valueConverted = Expression.Convert(value, Enum.GetUnderlyingType(typeof(T)));
            UnaryExpression flagConverted = Expression.Convert(flag, Enum.GetUnderlyingType(typeof(T)));

            // (Value & Flag)
            BinaryExpression bitwiseAnd =
                Expression.MakeBinary(
                    ExpressionType.And,
                    valueConverted,
                    flagConverted);

            // (Value & Flag) == Flag
            BinaryExpression hasFlagExpression =
                Expression.MakeBinary(ExpressionType.Equal, bitwiseAnd, flagConverted);

            return Expression.Lambda<Func<T, T, bool>>(hasFlagExpression, value, flag)
                .Compile();
        }
    }
}
