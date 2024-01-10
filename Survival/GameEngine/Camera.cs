using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
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

namespace Survival.GameEngine
{
    public class Camera
    {
        private Vector2 sizeMap = new Vector2(500, 500);// new Vector2((float)Math.Round(MainWindow.canvas.Width / MapGenerator.BLOCK_SIZE), (float)Math.Round(MainWindow.canvas.Height / MapGenerator.BLOCK_SIZE));
        public Vector2 pos = Vector2.Zero;

        public void Update(Rectangle player, Vector2 targetPos)
        {
            pos = targetPos;
            Draw(player, targetPos);
        }

        public void Draw(Rectangle player, Vector2 posPlayer)
        {
            MainWindow.canvas.Children.Clear();

            int xGap = (int)pos.X / MapGenerator.BLOCK_SIZE;
            int yGap = (int)pos.Y / MapGenerator.BLOCK_SIZE;
            for (int x = xGap - (int)Math.Round(SystemParameters.PrimaryScreenWidth / 100);  x < xGap + Math.Round(SystemParameters.PrimaryScreenWidth / MapGenerator.BLOCK_SIZE) + 1; x++)
            {
                for (int y = yGap - (int)Math.Round(SystemParameters.PrimaryScreenHeight / 100); y < yGap + Math.Round(SystemParameters.PrimaryScreenHeight / MapGenerator.BLOCK_SIZE) + 1; y++)
                {
                    if (MainWindow.map.IsInMap(x, y))
                    {
                        if (MainWindow.map.map[x][y] == -1)
                        {
                            Rectangle borderWall = new Rectangle()
                            {
                                Width = MapGenerator.BLOCK_SIZE,
                                Height = MapGenerator.BLOCK_SIZE,
                                Fill = Brushes.Yellow,
                                Stroke = Brushes.Yellow,
                                StrokeThickness = 2,
                            };
                            MainWindow.canvas.Children.Add(borderWall);
                            Canvas.SetLeft(borderWall, x * MapGenerator.BLOCK_SIZE - pos.X);
                            Canvas.SetTop(borderWall, y * MapGenerator.BLOCK_SIZE - pos.Y);
                        }
                        else if (MainWindow.map.map[x][y] == 1)
                        {
                            Rectangle wall = new Rectangle()
                            {
                                Width = MapGenerator.BLOCK_SIZE,
                                Height = MapGenerator.BLOCK_SIZE,
                                Fill = Brushes.Red,
                                Stroke = Brushes.Red,
                                StrokeThickness = 2,
                            };
                            MainWindow.canvas.Children.Add(wall);
                            Canvas.SetLeft(wall, x * MapGenerator.BLOCK_SIZE - pos.X);
                            Canvas.SetTop(wall, y * MapGenerator.BLOCK_SIZE - pos.Y);
                        }
                        else
                        {
                            Rectangle invisible = new Rectangle()
                            {
                                Width = MapGenerator.BLOCK_SIZE,
                                Height = MapGenerator.BLOCK_SIZE
                            };
                            MainWindow.canvas.Children.Add(invisible);
                            Canvas.SetLeft(invisible, x * MapGenerator.BLOCK_SIZE - pos.X);
                            Canvas.SetTop(invisible, y * MapGenerator.BLOCK_SIZE - pos.Y);
                        }
                    }
                }
            }
            
            MainWindow.canvas.Children.Add(player);
            Canvas.SetLeft(player, posPlayer.X);
            Canvas.SetTop(player, posPlayer.Y);
        }
    }
}
