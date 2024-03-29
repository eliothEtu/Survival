﻿using System;
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

        Rectangle healthBar = new Rectangle()
        {
            Height = 30,
            Width = 250,
            Fill = Brushes.White,
            Stroke = Brushes.Black,
        };

        private ImageBrush grassTexture = new ImageBrush(new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\map\\grass2.png")));

        private ImageBrush stoneTexture = new ImageBrush(new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\map\\stone2.png")));

#if DEBUG
        public StackPanel overlayPanel = new StackPanel()
        {
            Width = double.NaN,
            Height = double.NaN,
            HorizontalAlignment = HorizontalAlignment.Left,
        };
#endif
        public Renderer()
        {
#if DEBUG
            overlayPanel.Children.Add(healthBar);
            ((MainWindow)Application.Current.MainWindow).canv.Children.Add(overlayPanel);
#endif
        }


        private Rect GetCameraRect()
        {
            double w = Math.Round(((MainWindow)Application.Current.MainWindow).canv.Width / MapGenerator.BLOCK_SIZE);
            double h = Math.Round(((MainWindow)Application.Current.MainWindow).canv.Height / MapGenerator.BLOCK_SIZE);

            return new Rect(this.Pos.X, this.Pos.Y, w, h);
        }

        public Vector2? GetPosOnCanvas(Vector2 worldPos)
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

        public Vector2 GetWorldPos(Vector2 canvasPos)
        {
            Rect cameraRect = this.GetCameraRect();

            float worldX = canvasPos.X.Remap(0, (float)((MainWindow)Application.Current.MainWindow).canv.Width, (float)(cameraRect.X - (cameraRect.Width / 2)), (float)(cameraRect.X + (cameraRect.Width / 2)));
            float worldY = canvasPos.Y.Remap(0, (float)((MainWindow)Application.Current.MainWindow).canv.Height, (float)(cameraRect.Y - (cameraRect.Height / 2)), (float)(cameraRect.Y + (cameraRect.Height / 2)));

            return new Vector2(worldX, worldY);
        }

        public void UpdateCamera(Vector2 targetPos)
        {
            this.pos = targetPos;
        }

        public void Draw(List<Entity> entities)
        {
            ((MainWindow)Application.Current.MainWindow).canv.Children.Clear();

            Rect cameraRect = this.GetCameraRect();
            for (int x = (int)Math.Max(0, cameraRect.X - (cameraRect.Width / 2)); x < (int)Math.Min(Engine.Instance.MapGenerator.SizeMap.X, cameraRect.X + (cameraRect.Width / 2) + MapGenerator.BLOCK_SIZE); x++)
            {
                for (int y = (int)Math.Max(0, cameraRect.Y - (cameraRect.Height / 2)); y < (int)Math.Min(Engine.Instance.MapGenerator.SizeMap.Y, cameraRect.Y + (cameraRect.Height / 2) + MapGenerator.BLOCK_SIZE); y++)
                {          
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
                                Fill = this.stoneTexture,
                            };
                            break;
                        default:
                            rec = new Rectangle()
                            {
                                Width = MapGenerator.BLOCK_SIZE,
                                Height = MapGenerator.BLOCK_SIZE,
                                Fill = this.grassTexture
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
#if DEBUG
            overlayPanel.Children.Clear();
            TextBlock moneytext = new TextBlock()
            {
                Height = double.NaN,
                Width = double.NaN,
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                Text = $"Money : {Engine.Instance.Player.Money})"
            };
            overlayPanel.Children.Add(moneytext);
#endif

            healthBar.Width = 25 * Engine.Instance.Player.Life;
            ((MainWindow)Application.Current.MainWindow).canv.Children.Add(healthBar);



            foreach (Entity e in entities)
            {
#if DEBUG
                TextBlock text = new TextBlock()
                {
                    Height = double.NaN,
                    Width = double.NaN,
                    FontSize = 20,
                    FontWeight = FontWeights.Bold,
                    Text = $"{e.Name} : {e.Position})"
                };
                overlayPanel.Children.Add(text);
#endif

                Vector2? canvasPos = this.GetPosOnCanvas(e.Position);

                if (canvasPos == null) continue;                
                ((MainWindow)Application.Current.MainWindow).canv.Children.Add(e.Rectangle);
                Canvas.SetLeft(e.Rectangle, (double)canvasPos?.X);
                Canvas.SetTop(e.Rectangle, (double)canvasPos?.Y);
            }
#if DEBUG
            ((MainWindow)Application.Current.MainWindow).canv.Children.Add(overlayPanel);
            Canvas.SetLeft(overlayPanel, ((MainWindow)Application.Current.MainWindow).canv.Width - overlayPanel.ActualWidth);
#endif
        }
    }
}
