using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnumUtilTests.Enums
{
    [Flags]
    [Description("Flags Enum")]
    public enum FlagsEnum
    {
        [Description("0001")]
        One = 1,
        [Description("0010")]
        Two = 2,
        [Description("0100")]
        Four = 4,
        [Description("1000")]
        Eight = 8,
    }
}
