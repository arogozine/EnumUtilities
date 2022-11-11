using System;
using System.ComponentModel;

namespace EnumUtilTests.Enums
{
    [Flags]
    [Description("Flags Enum with extra overlapping values")]
    public enum OverlappingValuesFlagsEnum
    {
        [Description("0000")]
        Zero = 0,
        [Description("0001")]
        One = 1,
        [Description("0010")]
        Two = 2,
        [Description("0100")]
        Four = 4,
        [Description("1000")]
        Eight = 8,
        [Description("0101")]
        OneOrFour = 5,
        [Description("1111")]
        All = 15,
    }
}
