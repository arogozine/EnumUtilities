using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EnumUtilities
{
    // EnumUtilBase must be public
    // because a public class (EnumUtil) extends it
    // but EnumUtilBase should not be used, 
    // thus EnumUtilBase is marked Obsolete
    // we disable this obsolete warning here
    // because it only applies to the end user.
    #pragma warning disable 618

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

    #pragma warning restore 618

}
