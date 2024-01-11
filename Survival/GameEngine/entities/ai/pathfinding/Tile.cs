using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survival.GameEngine.entities.ai.pathfinding
{
    internal class Tile
    {
        public int X { get; set; }
        public int Y { get; set; }
        // How many tiles were traversed to get here
        public int Cost { get; set; }
        // Distance to the target tile, currently ignoring walls
        public int Distance { get; set; }
        // How many tiles were traversed so far + how many tiles we think will probably take to reach our goal
        public int CostDistance => Cost + Distance;
        public Tile? Parent { get; set; }

        // TODO: this is currently ignoring walls ect...
        public void SetDistance(int targetX, int targetY)
        {
            this.Distance = Math.Abs(targetX - this.X) + Math.Abs(targetY - this.Y);
        }
    }
}
