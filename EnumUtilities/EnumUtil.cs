using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EnumUtilities
{
    /// <summary>
    /// Enum Utilities.
    /// 
    /// Provides type safe Enum extension methods
    /// by "bypassing" .NET restrictions.
    /// </summary>
    public class EnumUtil : EnumUtilBase<Enum>
    {
        private EnumUtil() { }
    }
}
