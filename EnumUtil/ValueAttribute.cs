using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnumUtilities
{
    public struct ValueAttribute<T, Y>
        where Y : Attribute
        where T : struct, IComparable, IFormattable, IConvertible
    {
        public readonly T Value;
        public readonly Y Attribute;

        internal ValueAttribute(T value, Y attribute)
        {
            this.Value = value;
            this.Attribute = attribute;
        }
    }
}
