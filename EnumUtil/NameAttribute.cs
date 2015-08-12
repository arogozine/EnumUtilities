using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnumUtilities
{
    public struct NameAttribute<Y>
        where Y : Attribute
    {
        public readonly string Name;
        public readonly Y Attribute;

        internal NameAttribute(string name, Y attribute)
        {
            this.Name = name;
            this.Attribute = attribute;
        }
    }
}
