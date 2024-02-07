using FoxholeSimpleAPI.WarData.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace FoxholeSimpleAPI.WarData.MapItems
{
    public class MapItem
    {
        [JsonConverter(typeof(StringEnumConverter)), JsonProperty("teamId")]
        public TeamType TeamId { get; set; }

        [JsonProperty("iconType")]
        public MapIcon IconType { get; set; }

        [JsonProperty("x")]
        public double X { get; set; }

        [JsonProperty("y")]
        public double Y { get; set; }

        [JsonProperty("flags")]
        public MapFlags Flags { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            MapItem other = obj as MapItem;

            return other.X == this.X
                && other.Y == this.Y
                && other.IconType == this.IconType;
        }

        public override int GetHashCode()
        {
            return TeamId.GetHashCode() ^ IconType.GetHashCode() ^ X.GetHashCode() ^ Y.GetHashCode() ^ Flags.GetHashCode();
        }

        public override string? ToString()
        {
            return base.ToString(); //TODO: Удобное для end user преобразование string.
        }
    }

}
