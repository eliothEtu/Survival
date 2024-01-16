using Survival.GameEngine.entities.ai.pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survival.GameEngine.entities.ai
{
    internal class FollowPlayerBehavior : Behavior
    {
        public Player Player { get; set; }

        private List<Tile> cachedPath = new List<Tile>();
        private DateTime lastCalculation = DateTime.Now;

        public override void Update(Mob entity)
        {
            return;
            double distance = entity.GetDistanceFrom(this.Player.Position);
            if (distance < 2 || distance > entity.FocusDistance)
            {
                this.cachedPath.Clear();
                return;
            }

            if ((DateTime.Now - this.lastCalculation > TimeSpan.FromSeconds(5)) || (DateTime.Now - this.lastCalculation > TimeSpan.FromSeconds(5)) && ((this.cachedPath.Count < 2) || (this.GetCurrentTile(entity) != this.cachedPath[0])))
            {
                Console.WriteLine("a");
                this.cachedPath = this.CalculatePath(entity);
                this.lastCalculation = DateTime.Now;
            }

            if (this.cachedPath.Count < 2)
            {
               // Console.WriteLine("No path found!");
                return;
            }


            // follow path

            Tile targetTile = this.cachedPath[1];

            this.cachedPath.RemoveAt(0);
            this.cachedPath.RemoveAt(0);

            float deltaX = entity.Position.X - targetTile.X;
            float deltaY = entity.Position.Y - targetTile.Y;

            entity.Velocity = new System.Numerics.Vector2(-deltaX, -deltaY);
            Console.WriteLine($"Velocity: {entity.Velocity}");
        }

        private List<Tile> CalculatePath(Mob entity)
        {
            Tile start = this.GetCurrentTile(entity);
            Tile finish = this.GetCurrentTile(this.Player);

            start.SetDistance(finish.X, finish.Y);

            List<Tile> activeTiles = new List<Tile>();
            activeTiles.Add(start);

            List<Tile> visitedTiles = new List<Tile>();

            // loop until there is nowhere we haven't walked
            while (activeTiles.Any())
            {
                Tile checkTile = activeTiles.OrderByDescending(x => x.CostDistance).Last();
                if (checkTile.X == finish.X && checkTile.Y == finish.Y)
                {
                    // we arrived at the destination
                    return activeTiles;
                }

                visitedTiles.Add(checkTile);
                activeTiles.Remove(checkTile);

                List<Tile> walkableTiles = this.GetWalkableTiles(checkTile, finish, entity.FocusDistance);
                foreach (Tile walkableTile in walkableTiles)
                {
                    // we already visited this tile
                    if(visitedTiles.Any(tile => tile.X == walkableTile.X && tile.Y == walkableTile.Y)) continue;

                    if (activeTiles.Any(tile => tile.X == walkableTile.X && tile.Y == walkableTile.Y))
                    {
                        Tile existingTile = activeTiles.First(tile => tile.X == walkableTile.X && tile.Y == walkableTile.Y);
                        if (existingTile.CostDistance > checkTile.CostDistance)
                        {
                            activeTiles.Remove(existingTile);
                            activeTiles.Add(walkableTile);
                        }
                    }
                    else
                    {
                        // we've never been to this tile before
                        activeTiles.Add(walkableTile);
                    }
                }
            }

            // no path found!
            return new List<Tile>();
        }

        private List<Tile> GetWalkableTiles(Tile currentTile, Tile targetTile, int distance) {
            List<Tile> tiles = new List<Tile>();

            for (int x = 0; x < Engine.Instance.MapGenerator.SizeMap.X; x++)
            {
                for (int y = 0; y < Engine.Instance.MapGenerator.SizeMap.Y; y++)
                {
                    Tile tile = new Tile();
                    tile.X = x;
                    tile.Y = y;
                    tile.Parent = currentTile;
                    tile.Cost = currentTile.Cost + 1;
                    tile.SetDistance(targetTile.X, targetTile.Y);
                    tiles.Add(tile);
                }
            }

            // Only return tiles where there are no walls and no borders
            return tiles
                .Where(tile => Engine.Instance.MapGenerator.Map[tile.X][tile.Y] == 0)
                .Where(tile => Math.Sqrt(Math.Abs((Math.Pow(tile.X, 2) + Math.Pow(tile.Y, 2)))) <= distance)
                .ToList();
        }

        private Tile GetCurrentTile(Entity entity)
        {
            Tile tile = new Tile();
            tile.X = (int)entity.Position.X;
            tile.Y = (int)entity.Position.Y;

            return tile;
        }
    }
}
