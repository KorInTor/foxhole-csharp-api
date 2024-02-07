using System;
using FoxholeSimpleAPI.WarData.MapItems;
using FoxholeSimpleAPI.WarData.Enums;

namespace FoxholeSimpleAPI.WarData
{
    /// <summary>
    /// Contains info about world conquest data from WarAPI.
    /// </summary>
    public class WorldConquest
    {

        /// <summary>
        /// Data about current war.
        /// </summary>
        public War War { get; set; }
        
        /// <summary>
        /// List of maps.
        /// </summary>
        public List<Map> Maps { get; set; }


        public List<string> MapNames { get; set; } 

        public WorldConquest()
        {
            Maps = new List<Map>();
            War = new War();
            MapNames = new List<string>();
        }
    }
}

