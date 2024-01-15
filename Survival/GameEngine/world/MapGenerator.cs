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
            SizeMap = new Vector2(40, 40);//new Vector2((float)Math.Round(MainWindow.canvas.Width / BLOCK_SIZE), (float)Math.Round(MainWindow.canvas.Height / BLOCK_SIZE));

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

        /*public void ShowMap(Canvas canv)
        {
            for (int x = 0; x < Map.Count; x++)
            {
                for (int y = 0; y < Map[0].Count; y++)
                {
                    if (Map[x][y] == -1)
                    {
                        Rectangle borderWall = new Rectangle()
                        {
                            Width = BLOCK_SIZE,
                            Height = BLOCK_SIZE,
                            Fill = Brushes.Yellow,
                            Stroke = Brushes.Yellow,
                            StrokeThickness = 2,
                        };
                        canv.Children.Add(borderWall);
                        Canvas.SetLeft(borderWall, x * BLOCK_SIZE);
                        Canvas.SetTop(borderWall, y * BLOCK_SIZE);
                    }
                    else if (Map[x][y] == 1)
                    {
                        Rectangle wall = new Rectangle()
                        {
                            Width = BLOCK_SIZE,
                            Height = BLOCK_SIZE,
                            Fill = Brushes.Red,
                            Stroke = Brushes.Red,
                            StrokeThickness = 2,
                        };
                        canv.Children.Add(wall);
                        Canvas.SetLeft(wall, x * BLOCK_SIZE);
                        Canvas.SetTop(wall, y * BLOCK_SIZE);
                    }
                    else
                    {
                        Rectangle invisible = new Rectangle()
                        {
                            Width = BLOCK_SIZE,
                            Height = BLOCK_SIZE
                        };
                        canv.Children.Add(invisible);
                        Canvas.SetLeft(invisible, x * BLOCK_SIZE);
                        Canvas.SetTop(invisible, y * BLOCK_SIZE);
                    }
                }
            }
        }*/
    }
}
