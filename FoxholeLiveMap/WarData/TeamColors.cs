using FoxholeSimpleAPI.WarData.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxholeSimpleAPI.WarData
{
    public static class TeamColors
    {
        /// <summary>
        /// Returns <see cref="Color"/> of input Team.
        /// </summary>
        /// <param name="teamType">Type of team.</param>
        /// <returns>Color of that team.</returns>
        public static System.Drawing.Color GetColorOf(TeamType teamType)
        {
            Color color = new Color();
            if (teamType == TeamType.WARDENS)
            {
                color = Color.FromArgb(255,4, 23, 57);
                return color;
            }
            else
            {
                color = Color.FromArgb(255, 21, 38, 18);
                return color;
            }
        }
    }
}
