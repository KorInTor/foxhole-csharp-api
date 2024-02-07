using FoxholeSimpleAPI.WarData.MapItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxholeSimpleAPI.WarData
{
    public class Map
    {
        public delegate void MapItemHandler(string mapName, string data);
        public event MapItemHandler ItemChanged;

        private List<MapItem> _mapItems;

        public WarReport Report { get; set; }

        public string Name { get; set; }

        public int regionId { get; set; }

        public int scorchedVictoryTowns { get; set; }

        public List<MapItem> mapItems 
        {
            get
            {
                return _mapItems;
            }
            set 
            {
                _mapItems = value;
                // Реализовать вызов события который будет указывать какие именно MapItem обновились (Удобство для фронтенда?).
            }
        }

        public List<MapTextItem> mapTextItems { get; set; }

    }
}
