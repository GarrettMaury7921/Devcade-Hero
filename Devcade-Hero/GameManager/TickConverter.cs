using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevcadeHero.GameManager
{
    internal class TickConverter
    {

        public static double DistanceToTime(uint tickStartPos, uint tickEndPos, float resolution, float bpm)
        {
            return (tickEndPos - tickStartPos) / resolution * 60.0 / bpm;
        }

        public static double DistanceToBPM(uint tickStartPos, uint tickEndPos, double resolution, double deltaTime)
        {
            return (tickEndPos - tickStartPos) / resolution * 60.0f / deltaTime;
        }

    }
}
