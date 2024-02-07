using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxholeSimpleAPI.WarData
{
    /// <summary>
    /// Data for current war.
    /// </summary>
    public class War
    {
        public string WarId { get; set; }
        public int WarNumber { get; set; }
        public string Winner { get; set; }
        public long? ConquestStartTime { get; set; }
        public long? ConquestEndTime { get; set; }
        public long? ResistanceStartTime { get; set; }
        public int RequiredVictoryTowns { get; set; }
    }
}