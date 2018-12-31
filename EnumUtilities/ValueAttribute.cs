using System;
using System.Runtime.CompilerServices;

namespace EnumUtilities
{
    /// <summary>
    /// Hold the Value and the Attribute for a specific enumeration.
    /// </summary>
    /// <typeparam name="T">Enumeration type</typeparam>
    /// <typeparam name="Y">Attribute type</typeparam>
    public readonly struct ValueAttribute<T, Y>
        where Y : Attribute
        where T : struct, IComparable, IFormattable, IConvertible
    {
        /// <summary>
        /// Enumeration Value
        /// </summary>
        public readonly T Value;
        
        /// <summary>
        /// Enumeration Attribute
        /// </summary>
        public readonly Y Attribute;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ValueAttribute(T value, Y attribute)
        {
            this.Value = value;
            this.Attribute = attribute;
        }
    }
}
