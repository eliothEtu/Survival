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
        private Vector2 pos = Vector2.Zero;

        public Vector2 Pos
        {
            get => this.pos;
            set => this.pos = value;
        }

        TextBlock overlay = new TextBlock()
        {
            Height = double.NaN,
            Width = double.NaN,
            FontSize = 20,
            FontWeight = FontWeights.Bold
        };

        private Rect GetCameraRect()
        {
            double w = Math.Round(SystemParameters.PrimaryScreenWidth / MapGenerator.BLOCK_SIZE);
            double h = Math.Round(SystemParameters.PrimaryScreenHeight / MapGenerator.BLOCK_SIZE);

            return new Rect(this.Pos.X, this.Pos.Y, w, h);
        }

        private Vector2? GetPosOnCanvas(Vector2 worldPos)
        {
            Rect cameraRect = this.GetCameraRect();
            if (worldPos.X + MapGenerator.BLOCK_SIZE < cameraRect.X - (cameraRect.Width / 2) ||
                worldPos.X > cameraRect.X + (cameraRect.Width / 2) ||
                worldPos.Y + MapGenerator.BLOCK_SIZE < cameraRect.Y - (cameraRect.Height / 2) ||
                worldPos.Y > cameraRect.Y + (cameraRect.Height / 2))
            {
                return null;
            }

            float canvasX = worldPos.X.Remap((float)(cameraRect.X - (cameraRect.Width / 2)), (float)(cameraRect.X + (cameraRect.Width / 2)), 0, (float)((MainWindow)Application.Current.MainWindow).canv.Width);
            float canvasY = worldPos.Y.Remap((float)(cameraRect.Y - (cameraRect.Height / 2)), (float)(cameraRect.Y + (cameraRect.Height / 2)), 0, (float)((MainWindow)Application.Current.MainWindow).canv.Height);

            return new Vector2(canvasX, canvasY);
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

            Rect cameraRect = this.GetCameraRect();
            for (int x = 0; x < Engine.Instance.MapGenerator.SizeMap.X; x++)
            {
                if (x + MapGenerator.BLOCK_SIZE < cameraRect.X - (cameraRect.Width / 2) || x > cameraRect.X + (cameraRect.Width / 2)) continue;
                for (int y = 0; y < Engine.Instance.MapGenerator.SizeMap.Y; y++)
                {
                    if (y + MapGenerator.BLOCK_SIZE < cameraRect.Y - (cameraRect.Height / 2) || y > cameraRect.Y + (cameraRect.Height / 2)) continue;

                    Rectangle rec;
                    switch (Engine.Instance.MapGenerator.Map[x][y])
                    {
                        case -1:
                            rec = new Rectangle()
                            {
                                Width = MapGenerator.BLOCK_SIZE,
                                Height = MapGenerator.BLOCK_SIZE,
                                Fill = Brushes.Yellow,
                                Stroke = Brushes.Yellow,
                                StrokeThickness = 2,
                            };
                            break;
                        case 1:
                            rec = new Rectangle()
                            {
                                Width = MapGenerator.BLOCK_SIZE,
                                Height = MapGenerator.BLOCK_SIZE,
                                Fill = Brushes.Red,
                                Stroke = Brushes.Red,
                                StrokeThickness = 2,
                            };
                            break;
                        default:
                            rec = new Rectangle()
                            {
                                Width = MapGenerator.BLOCK_SIZE,
                                Height = MapGenerator.BLOCK_SIZE
                            };
                            break;
                    }

                    Vector2? canvasPos = this.GetPosOnCanvas(new Vector2(x, y));
                    if (canvasPos != null)
                    {
                        ((MainWindow)Application.Current.MainWindow).canv.Children.Add(rec);
                        Canvas.SetLeft(rec, (double)canvasPos?.X);
                        Canvas.SetTop(rec, (double)canvasPos?.Y);
                    }
                }
            }

            string textOveralay = "";
            foreach (Entity e in entities)
            {                
                Vector2? canvasPos = this.GetPosOnCanvas(e.Position);
           
                if (canvasPos == null) continue;
                ((MainWindow)Application.Current.MainWindow).canv.Children.Add(e.Rectangle);
                Canvas.SetLeft(e.Rectangle, (double)canvasPos?.X);
                Canvas.SetTop(e.Rectangle, (double)canvasPos?.Y);
                textOveralay += $"{e.Name} : {e.Position}";
            }

            ((MainWindow)Application.Current.MainWindow).canv.Children.Add(overlay);
            overlay.Text = entities[0].Position.ToString();

            /* int xGap = (int)pos.X / MapGenerator.BLOCK_SIZE;
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
             }   */
        }
    }
}
