using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Survival.GameEngine.Inventory.ItemComponent
{
    internal class Artifact : Item
    {
        private TimeSpan coolDown;

        public TimeSpan CoolDown
        {
            get 
            { 
                return this.coolDown; 
            }
            set 
            { 
                this.coolDown = value; 
            }
        }


        public Artifact(string name, string description, BitmapImage image, string type,TimeSpan coolDown, int tier) : base(name, description, image, type, tier)
        {
            this.CoolDown = coolDown;
        }
    }
}
