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

namespace Survival.GameEngine
{
    internal class PlayerController
    {

        public PlayerController() 
        { 


        }

        public void KeyDown(KeyEventArgs key)
        {
            if (key.Key == Key.Z && Engine.Instance.Player.Velocity.Y != -1)
            {
                Engine.Instance.Player.Velocity = new Vector2(0, -1);
            }
            if(key.Key == Key.S && Engine.Instance.Player.Velocity.Y != 1)
            {
                Engine.Instance.Player.Velocity = new Vector2(0, 1);
            }
            if(key.Key == Key.Q && Engine.Instance.Player.Velocity.X != -1)
            {
                Engine.Instance.Player.Velocity = new Vector2(-1, 0);
            }
            if(key.Key == Key.D && Engine.Instance.Player.Velocity.X != 1)
            {
                Engine.Instance.Player.Velocity = new Vector2(1,0);
            }
        }

        public void KeyUp(KeyEventArgs key)
        {
            Engine.Instance.Player.Velocity = Vector2.Zero;
        }
    }
}
