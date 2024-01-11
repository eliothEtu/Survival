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
        private Player Player { get; set; }

        public override void Update(Entity entity)
        {
            Tile start = new Tile();

            //TODO: int / float
            start.X = (int)entity.Position.X;
            start.Y = (int)entity.Position.Y;

            Tile finish = new Tile();
            finish.X = (int)this.Player.Position.X;
            finish.Y = (int)this.Player.Position.Y;

            start.SetDistance(finish.X, finish.Y);

            List<Tile> activeTiles = new List<Tile>();
            activeTiles.Add(start);

            List<Tile> visitedTiles = new List<Tile>();

            while (activeTiles.Any())
            {
                Tile checkTile = activeTiles.OrderBy(x => x.CostDistance).First();
                if (checkTile.X == finish.X && checkTile.Y == finish.Y)
                {
                    // we arrived at the destination
                    return;
                }

                visitedTiles.Add(checkTile);
                activeTiles.Remove(checkTile);

                List<Tile> walkableTiles = this.GetWalkableTiles(checkTile, finish);
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
        }

        private List<Tile> GetWalkableTiles(Tile currentTile, Tile targetTile) {
            List<Tile> tiles = new List<Tile>();

            for (int x = 0; x < Engine.Instance.MapGenerator.sizeMap.X; x++)
            {
                for (int y = 0; y < Engine.Instance.MapGenerator.sizeMap.Y; y++)
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
            return tiles.Where(tile => Engine.Instance.MapGenerator.Map[tile.X][tile.Y] == 0).ToList();
        }
    }
}
