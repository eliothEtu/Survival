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

namespace Survival.GameEngine
{
    public class MapGenerator
    {
        public static readonly int BLOCK_SIZE = 10;
        Random random = new Random();
        private List<List<int>> map = new List<List<int>>();
        public Vector2 sizeMap;

        public void CreateMap()
        {
            sizeMap = new Vector2((float)Math.Round(MainWindow.canvas.Width / BLOCK_SIZE), (float)Math.Round(MainWindow.canvas.Height / BLOCK_SIZE));

            for (int x = 0; x < sizeMap.X; x++)
            {
                List<int> line = new List<int>();
                for (int y = 0; y < sizeMap.Y; y++)
                {
                    if (x == 0 || x == sizeMap.X - 1 || y == 0 || y == sizeMap.Y - 1)
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
                map.Add(line);
            }
        }
        public bool IsInMap(int x, int y)
        {
            return ((0 <= x && x < map.Count) && (0 <= y && y < map[0].Count));
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
                            if (map[x][y] != 0)
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
                for (int x = 0; x < map.Count; x++)
                {
                    for (int y = 0; y < map[0].Count; y++)
                    {
                        int neighWallTiles = GetNeighborTiles(x, y);
                        if (neighWallTiles > 4 && map[x][y] == 0)
                        {
                            map[x][y] = 1;
                        }
                        else if (neighWallTiles < 4)
                        {
                            map[x][y] = 0;
                        }
                    }
                }
            }
        }

        public void ShowMap()
        {
            for (int x = 0; x < map.Count; x++)
            {
                for (int y = 0; y < map[0].Count; y++)
                {
                    if (map[x][y] == -1)
                    {
                        Rectangle borderWall = new Rectangle()
                        {
                            Width = BLOCK_SIZE,
                            Height = BLOCK_SIZE,
                            Fill = Brushes.Yellow,
                            Stroke = Brushes.Yellow,
                            StrokeThickness = 2,
                        };
                        MainWindow.canvas.Children.Add(borderWall);
                        Canvas.SetLeft(borderWall, x * BLOCK_SIZE);
                        Canvas.SetTop(borderWall, y * BLOCK_SIZE);
                    }
                    else if (map[x][y] == 1)
                    {
                        Rectangle wall = new Rectangle()
                        {
                            Width = BLOCK_SIZE,
                            Height = BLOCK_SIZE,
                            Fill = Brushes.Red,
                            Stroke = Brushes.Red,
                            StrokeThickness = 2,
                        };
                        MainWindow.canvas.Children.Add(wall);
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
                        MainWindow.canvas.Children.Add(invisible);
                        Canvas.SetLeft(invisible, x * BLOCK_SIZE);
                        Canvas.SetTop(invisible, y * BLOCK_SIZE);
                    }
                }
            }
        }
    }    
}
