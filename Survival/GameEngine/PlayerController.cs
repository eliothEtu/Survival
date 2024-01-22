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

        Player player = Engine.Instance.Player;

        public PlayerController() 
        { 


        }

        public void KeyDown(KeyEventArgs key)
        {
            if (!((MainWindow)Application.Current.MainWindow).bInventory)
            {
                if (key.Key == Key.Z && player.Velocity.Y != -1)
                {
                    player.Velocity = new Vector2(player.Velocity.X, -1);
                }
                if (key.Key == Key.S && player.Velocity.Y != 1)
                {
                    player.Velocity = new Vector2(player.Velocity.X, 1);
                }
                if (key.Key == Key.Q && player.Velocity.X != -1)
                {
                    player.Velocity = new Vector2(-1, player.Velocity.Y);
                }
                if (key.Key == Key.D && player.Velocity.X != 1)
                {
                    player.Velocity = new Vector2(1, player.Velocity.Y);
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
            Player player = Engine.Instance.Player;

            if (key.Key == Key.Z && player.Velocity.Y != 0)
            {
                player.Velocity = new Vector2(player.Velocity.X, 0);
            }
            if (key.Key == Key.S && player.Velocity.Y != 0)
            {
                player.Velocity = new Vector2(player.Velocity.X, 0);
            }
            if (key.Key == Key.Q && player.Velocity.X != 0)
            {
                player.Velocity = new Vector2(0, player.Velocity.Y);
            }
            if (key.Key == Key.D && player.Velocity.X != 0)
            {
                player.Velocity = new Vector2(0, player.Velocity.Y);
            }
        }
        public void MouseLeft(MouseEventArgs key)
        {
            Point mouse = key.GetPosition(((MainWindow)Application.Current.MainWindow).canv);
            Vector2 mouseV = Engine.Instance.Renderer.GetWorldPos(new Vector2((float)mouse.X, (float)mouse.Y));
            Engine.Instance.Player.Fire(mouseV);
        }
    }
}
