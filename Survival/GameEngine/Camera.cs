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
        private readonly Vector2 SIZE_MAP = MapGenerator.SIZE_MAP;// new Vector2((float)Math.Round(((MainWindow)Application.Current.MainWindow).canv.Width / MapGenerator.BLOCK_SIZE), (float)Math.Round(((MainWindow)Application.Current.MainWindow).canv.Height / MapGenerator.BLOCK_SIZE));
        public Vector2 pos = Vector2.Zero;

        public List<Rect> posBlock = new List<Rect>(); ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Update(Rectangle player, Vector2 targetPos)
        {
            pos = targetPos;
            pos.X = (float)Math.Clamp(pos.X, 0, 40 * 75 - ((MainWindow)Application.Current.MainWindow).canv.Width);
            pos.Y = (float)Math.Clamp(pos.Y, 0, 40 * 75 - ((MainWindow)Application.Current.MainWindow).canv.Height);
        }

        public void Draw(List<Entity> entities)
        {
            ((MainWindow)Application.Current.MainWindow).canv.Children.Clear();
            posBlock.Clear(); ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

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
                            posBlock.Add(new Rect(Canvas.GetLeft(borderWall), Canvas.GetTop(borderWall), borderWall.Width, borderWall.Height)); ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
                            posBlock.Add(new Rect(Canvas.GetLeft(wall), Canvas.GetTop(wall), wall.Width, wall.Height)); ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
