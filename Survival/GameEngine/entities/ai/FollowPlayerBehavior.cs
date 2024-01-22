using Survival.GameEngine.entities.ai.pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;
using System.Threading.Tasks;

namespace Survival.GameEngine.entities.ai
{
    internal class FollowPlayerBehavior : Behavior
    {
        private static TaskFactory TASK_FACTORY = new TaskFactory(new MultithreadedTaskScheduler(3));

        public Player Player { get; set; }

        private List<Tile> cachedPath = new List<Tile>();
        private Vector2 target;

        public override void Update(Mob entity)
        {
            double distance = entity.GetDistanceFrom(this.Player.Position);
            if (distance < 2 || distance > entity.FocusDistance)
            {
                this.cachedPath.Clear();
                return;
            }

            if (((this.cachedPath.Count < 2) || (this.GetCurrentTile(entity) != this.cachedPath[0]) || this.target != entity.Position))
            {
                TASK_FACTORY.StartNew(() =>
                {
                    this.cachedPath = this.CalculatePath(entity);
                    this.target = entity.Position;
                });
            }

            if (this.cachedPath.Count < 2)
            {
                return;
            }


            // follow path

            Tile targetTile = this.cachedPath[0];
            this.cachedPath.RemoveAt(0);

            float deltaX = targetTile.X - entity.Position.X;
            float deltaY = targetTile.Y - entity.Position.Y;

            entity.Velocity = Vector2.Normalize(new Vector2(deltaX, deltaY));
        }

        private List<Tile> CalculatePath(Mob entity)
        {
            Tile start = this.GetCurrentTile(entity);
            Tile finish = this.GetCurrentTile(this.Player);

            if (Engine.Instance.MapGenerator.IsInMap(start.X, start.Y) && Engine.Instance.MapGenerator.IsInMap(finish.X, finish.Y))
                if (Engine.Instance.MapGenerator.Map[start.X][start.Y] != 0 || Engine.Instance.MapGenerator.Map[finish.X][finish.Y] != 0)
                    return new List<Tile>();

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
                    List<Tile> tiles = new List<Tile>();
                    Tile tile = checkTile;

                    while (true)
                    {
                        tiles.Add(tile);
                        tile = tile.Parent;
                        
                        if (tile == null)
                            return tiles;
                    }
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

            // no path found!
            return new List<Tile>();
        }

        private List<Tile> GetWalkableTiles(Tile currentTile, Tile targetTile) {
            List<Tile> tiles = new List<Tile>()
            {
                new Tile { X = currentTile.X, Y = currentTile.Y - 1, Parent = currentTile, Cost = currentTile.Cost + 1 },
                new Tile { X = currentTile.X, Y = currentTile.Y + 1, Parent = currentTile, Cost = currentTile.Cost + 1 },
                new Tile { X = currentTile.X - 1, Y = currentTile.Y, Parent = currentTile, Cost = currentTile.Cost + 1 },
                new Tile { X = currentTile.X + 1, Y = currentTile.Y, Parent = currentTile, Cost = currentTile.Cost + 1 },
            };

            tiles.ForEach(tile => tile.SetDistance(targetTile.X, targetTile.Y));

            // Only return tiles where there are no walls and no borders
            return tiles
                .Where(tile => Engine.Instance.MapGenerator.IsInMap(tile.X, tile.Y))
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
