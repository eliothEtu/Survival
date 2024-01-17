using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

// -1: Border | 0: Invisible | 1: Mur

namespace Survival.GameEngine.world
{
    public class MapGenerator
    {
        public static readonly int BLOCK_SIZE = 75;
        Random random = new Random();
        private List<List<int>> map = new List<List<int>>();
        public List<List<int>> Map { get => map; set => map = value; }

        private Vector2 sizeMap;
        public Vector2 SizeMap { get => sizeMap; set => sizeMap = value; }


        public void CreateMap()
        {
            SizeMap = new Vector2(40, 100);//new Vector2((float)Math.Round(MainWindow.canvas.Width / BLOCK_SIZE), (float)Math.Round(MainWindow.canvas.Height / BLOCK_SIZE));

            Map.Clear();

            for (int x = 0; x < SizeMap.X; x++)
            {
                List<int> line = new List<int>();
                for (int y = 0; y < SizeMap.Y; y++)
                {
                    if (x == 0 || x == SizeMap.X - 1 || y == 0 || y == SizeMap.Y - 1)
                    {
                        line.Add(-1);
                    }
                    else if (random.Next(101) < 50)
                    {
                        line.Add(1);
                    }
                    else
                    {
                        line.Add(0);
                    }
                }
                Map.Add(line);
            }
        }
        public bool IsInMap(int x, int y)
        {
            return 0 <= x && x < Map.Count && 0 <= y && y < Map[0].Count;
        }

        private int GetNeighborTiles(int i, int j)
        {
            int count = 0;
            for (int x = i - 1; x <= i + 1; x++)
            {
                for (int y = j - 1; y <= j + 1; y++)
                {
                    if (IsInMap(x, y))
                    {
                        if (x != i || y != j)
                        {
                            if (Map[x][y] != 0)
                            {
                                count++;
                            }
                        }
                    }
                    else
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public void SmoothMap(int times)
        {
            for (int i = 0; i < times; i++)
            {
                for (int x = 0; x < Map.Count; x++)
                {
                    for (int y = 0; y < Map[0].Count; y++)
                    {
                        int neighWallTiles = GetNeighborTiles(x, y);
                        if (neighWallTiles > 5 && Map[x][y] == 0)
                        {
                            Map[x][y] = 1;
                        }
                        else if (neighWallTiles < 4)
                        {
                            Map[x][y] = 0;
                        }
                    }
                }
            }
        }

        public Vector2 GetPlayerSpawnPos()
        {
            Vector2 randomPos = new Vector2(random.Next(0, (int)sizeMap.X), random.Next(0, (int)sizeMap.Y));
            for (int x = (int)randomPos.X; x < sizeMap.X; x++)
            {
                for (int y = (int)randomPos.Y ; y < sizeMap.Y; y++)
                {
                    if (IsInMap(x, y))
                    {
                        if (this.Map[x][y] == 0)
                        {
                            return new Vector2(x, y);
                        }
                    }
                }
            }
            return Vector2.Zero;
        }

        public Vector2 GetMobSpawnPos(ushort minLength, ushort maxLength)
        {
            if (maxLength < minLength)
            {
                throw new ArgumentException("maxLength cannot be lower than minLength");
            }

            Vector2 playerPos = Engine.Instance.Player.Position;
            List<Vector2> availableSpawns = new List<Vector2>();

            for (int x = (int)playerPos.X - maxLength; x < (int)playerPos.X + maxLength; x++)
            {
                if (x >= playerPos.X - minLength && x >= playerPos.X + minLength)
                {
                    continue;
                }
                for (int y = (int)playerPos.Y - maxLength; y < (int)playerPos.Y + maxLength; y++)
                {
                    if (y >= playerPos.Y - minLength && y >= playerPos.Y + minLength)
                    {
                        continue;
                    }

                    if (IsInMap(x, y))
                    {
                        if (this.Map[x][y] == 0)
                        {
                            availableSpawns.Add(new Vector2(x, y));
                        }
                    }
                }
            }

            return availableSpawns[random.Next(1, availableSpawns.Count)-1];
        }
    }
}