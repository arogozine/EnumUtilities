using EnumUtilTests.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Text;

namespace EnumUtilTests.EmitPerformance
{
    [TestClass]
    public class EmitUnsetFlag : EmitPerfBase<Int32Enum>
    {
        public override Func<T, T, Int32Enum> GenerateEmit<T>()
        {
            var dm = new DynamicMethod("UnsetFlag",
                typeof(T),
                new[] { typeof(T), typeof(T) });
            var lgen = dm.GetILGenerator();

            lgen.Emit(OpCodes.Ldarg_0);
            lgen.Emit(OpCodes.Ldarg_1);
            lgen.Emit(OpCodes.Not);
            lgen.Emit(OpCodes.And);
            lgen.Emit(OpCodes.Ret);

            return (Func<T, T, Int32Enum>)dm.CreateDelegate(
                typeof(Func<T, T, Int32Enum>));
        }

        public override Func<T, T, Int32Enum> GenerateExpression<T>()
        {
            var val = Expression.Parameter(typeof(T));
            var flag = Expression.Parameter(typeof(T));

            // Convert from Enum to Enum’s underlying type (byte, int, long, …)
            // to allow bitwise functions to work
            var underlyingType = Enum.GetUnderlyingType(typeof(T));
            var valConverted = Expression.Convert(val, underlyingType);
            var flagConverted = Expression.Convert(flag, underlyingType);

            // ~flag
            var notFlagExpression =
                Expression.MakeUnary(
                    ExpressionType.Not,
                    flagConverted,
                    null);

            // val & (~flag)
            var andExpression = Expression.MakeBinary(
                ExpressionType.And,
                valConverted,
                notFlagExpression);

            // Convert back to Enum
            UnaryExpression backToEnumType = Expression.Convert(andExpression, typeof(T));
            return Expression.Lambda<Func<T, T, Int32Enum>>(backToEnumType, val, flag)
                .Compile();
        }
    }
}
