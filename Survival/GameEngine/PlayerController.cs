using Survival.MalwareStuff;
using Survival.GameEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
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
using Survival.GameEngine.entities.ai;
using Survival.GameEngine.world;

namespace Survival.GameEngine
{
    internal class PlayerController
    {
        private Player player = Engine.Instance.Player;

        public PlayerController() 
        { 


        }

        public void KeyDown(KeyEventArgs key)
        {
            if (!((MainWindow)Application.Current.MainWindow).bInventory)
            {
                if (key.Key == Key.Z && this.player.Velocity.Y != -1)
                {
                    this.player.Velocity = new Vector2(this.player.Velocity.X, -1);
                }
                if (key.Key == Key.S && this.player.Velocity.Y != 1)
                {
                    this.player.Velocity = new Vector2(this.player.Velocity.X, 1);
                }
                if (key.Key == Key.Q && this.player.Velocity.X != -1)
                {
                    this.player.Velocity = new Vector2(-1, this.player.Velocity.Y);
                }
                if (key.Key == Key.D && this.player.Velocity.X != 1)
                {
                    this.player.Velocity = new Vector2(1, this.player.Velocity.Y);
                }

                if (key.Key == Key.F11)
                {
                    MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                    if (mainWindow.WindowState == WindowState.Maximized)
                    {
                        mainWindow.WindowState = WindowState.Normal;
                        mainWindow.WindowStyle = WindowStyle.SingleBorderWindow;
                    }
                    else
                    {
                        mainWindow.WindowState = WindowState.Maximized;
                        mainWindow.WindowStyle = WindowStyle.None;
                    }
                }
            }            

            if (!((MainWindow)Application.Current.MainWindow).bInventory && key.Key == Key.I)
            {
                ((MainWindow)Application.Current.MainWindow).OpenInventory();
            }
            else if (((MainWindow)Application.Current.MainWindow).bInventory && key.Key == Key.I)
            {
                ((MainWindow)Application.Current.MainWindow).CloseInventory();
            }
        }

        public void KeyUp(KeyEventArgs key)
        {
            if (key.Key == Key.Z && this.player.Velocity.Y != 0)
            {
                this.player.Velocity = new Vector2(this.player.Velocity.X, 0);
            }
            if (key.Key == Key.S && this.player.Velocity.Y != 0)
            {
                this.player.Velocity = new Vector2(this.player.Velocity.X, 0);
            }
            if (key.Key == Key.Q && this.player.Velocity.X != 0)
            {
                this.player.Velocity = new Vector2(0, this.player.Velocity.Y);
            }
            if (key.Key == Key.D && this.player.Velocity.X != 0)
            {
                this.player.Velocity = new Vector2(0, this.player.Velocity.Y);
            }
        }
        public void MouseLeft(MouseEventArgs key)
        {
            Point mouse = key.GetPosition(((MainWindow)Application.Current.MainWindow).canv);
            Vector2 mouseV = Engine.Instance.Renderer.GetWorldPos(new Vector2((float)mouse.X, (float)mouse.Y));
            this.player.Fire(mouseV);
        }
    }
}
