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

        public PlayerController() 
        { 


        }

        public void KeyDown(KeyEventArgs key)
        {
            Player player = Engine.Instance.Player;

            if (key.Key == Key.Z && player.Velocity.Y != -1)
            {
                player.Velocity = new Vector2(player.Velocity.X, -1);
            }
            if(key.Key == Key.S && player.Velocity.Y != 1)
            {
                player.Velocity = new Vector2(player.Velocity.X, 1);
            }
            if(key.Key == Key.Q && player.Velocity.X != -1)
            {
                player.Velocity = new Vector2(-1, player.Velocity.Y);
            }
            if(key.Key == Key.D && player.Velocity.X != 1)
            {
                player.Velocity = new Vector2(1, player.Velocity.Y);
            }

            if (key.Key == Key.A)
            {
                Mob mob = new Mob("Mob", 100, new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\face.png")), Engine.Instance.MapGenerator.GetMobSpawnPos(5, 10), new Vector2(0f, 0f));
                Engine.Instance.Entities.Add(mob);
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
    }
}
