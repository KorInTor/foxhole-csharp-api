using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxholeSimpleAPI.WarData.Enums
{
    [Flags]
    public enum MapFlags
    {
        None = 0,
        IsVictoryBase = 0x01,
        IsHomeBase = 0x02, // Removed in Update 29
        IsBuildSite = 0x04,
        IsScorched = 0x10, // Update 22
        IsTownClaimed = 0x20 // Update 26
    }
}
