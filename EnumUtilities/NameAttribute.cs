using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EnumUtilities
{
    /// <summary>
    /// Holds a Name and an Attribute for a specific enumeration.
    /// </summary>
    /// <typeparam name="Y">An Attribute Type</typeparam>
    public readonly struct NameAttribute<Y>
        where Y : Attribute
    {
        /// <summary>
        /// Name for the enumeration value.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Attribute associated with the enumeration value.
        /// </summary>
        public readonly Y Attribute;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal NameAttribute(string name, Y attribute)
        {
            this.Name = name;
            this.Attribute = attribute;
        }
    }
}
