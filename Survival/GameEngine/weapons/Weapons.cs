using Survival.GameEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Survival.GameEngine.weapons
{
    internal class Weapons
    {
        private string name;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (value.Length == 0)
                {
                    throw new ArgumentException("Weapon must have a name");
                }
                name = value;
            }
        }

        private Rectangle rectange;

        public Rectangle Rectangle
        {
            get
            {
                return rectange;
            }
            set { rectange = value; }
        }

        private string description;

        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }

        private string malus;

        public string Malus
        {
            get
            {
                return malus;
            }
            set
            {
                malus = value;
            }
        }


        private int damage;

        public int Damage
        {
            get
            {
                return damage;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Damage must be positive");
                }
                damage = value;
            }
        }

        private int timeBeforeNextShot;

        public int TimebeforeNextShot
        {
            get
            {
                return timeBeforeNextShot;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Time must be positive");
                }
                timeBeforeNextShot = value;
            }
        }



    }
}
