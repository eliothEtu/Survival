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
using Survival.GameEngine.world;

namespace Survival.GameEngine
{
    public class Renderer
    {
        private Vector2 sizeMap = new Vector2(500, 500);// new Vector2((float)Math.Round(((MainWindow)Application.Current.MainWindow).canv.Width / MapGenerator.BLOCK_SIZE), (float)Math.Round(((MainWindow)Application.Current.MainWindow).canv.Height / MapGenerator.BLOCK_SIZE));
        private Vector2 pos = Vector2.Zero;

        public Vector2 Pos
        {
            get => this.pos;
            set => this.pos = value;
        }

        public void UpdateCamera(Rectangle player, Vector2 targetPos)
        {
            this.pos = targetPos;
            this.pos.X = (float)Math.Clamp(pos.X, 0, 40 * 75 - ((MainWindow)Application.Current.MainWindow).canv.Width);
            this.pos.Y = (float)Math.Clamp(pos.Y, 0, 40 * 75 - ((MainWindow)Application.Current.MainWindow).canv.Height);
        }

        public void Draw(List<Entity> entities)
        {
            ((MainWindow)Application.Current.MainWindow).canv.Children.Clear();

            int xGap = (int)pos.X / MapGenerator.BLOCK_SIZE;
            int yGap = (int)pos.Y / MapGenerator.BLOCK_SIZE;
            for (int x = xGap - (int)Math.Round(SystemParameters.PrimaryScreenWidth / 100);  x < xGap + Math.Round(SystemParameters.PrimaryScreenWidth / MapGenerator.BLOCK_SIZE) + 1; x++)
            {
                for (int y = yGap - (int)Math.Round(SystemParameters.PrimaryScreenHeight / 100); y < yGap + Math.Round(SystemParameters.PrimaryScreenHeight / MapGenerator.BLOCK_SIZE) + 1; y++)
                {
                    if (Engine.Instance.MapGenerator.IsInMap(x, y))
                    {
                        if (Engine.Instance.MapGenerator.Map[x][y] == -1)
                        {
                            Rectangle borderWall = new Rectangle()
                            {
                                Width = MapGenerator.BLOCK_SIZE,
                                Height = MapGenerator.BLOCK_SIZE,
                                Fill = Brushes.Yellow,
                                Stroke = Brushes.Yellow,
                                StrokeThickness = 2,
                            };
                            ((MainWindow)Application.Current.MainWindow).canv.Children.Add(borderWall);
                            Canvas.SetLeft(borderWall, x * MapGenerator.BLOCK_SIZE - pos.X);
                            Canvas.SetTop(borderWall, y * MapGenerator.BLOCK_SIZE - pos.Y);
                        }
                        else if (Engine.Instance.MapGenerator.Map[x][y] == 1)
                        {
                            Rectangle wall = new Rectangle()
                            {
                                Width = MapGenerator.BLOCK_SIZE,
                                Height = MapGenerator.BLOCK_SIZE,
                                Fill = Brushes.Red,
                                Stroke = Brushes.Red,
                                StrokeThickness = 2,
                            };
                            ((MainWindow)Application.Current.MainWindow).canv.Children.Add(wall);
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
                            ((MainWindow)Application.Current.MainWindow).canv.Children.Add(invisible);
                            Canvas.SetLeft(invisible, x * MapGenerator.BLOCK_SIZE - pos.X);
                            Canvas.SetTop(invisible, y * MapGenerator.BLOCK_SIZE - pos.Y);
                        }
                    }
                }
            }
            
            foreach (Entity e in entities)
            {
                ((MainWindow)Application.Current.MainWindow).canv.Children.Add(e.Rectangle);
                Canvas.SetLeft(e.Rectangle, e.Position.X);
                Canvas.SetTop(e.Rectangle, e.Position.Y);
            }                
        }
    }
}
