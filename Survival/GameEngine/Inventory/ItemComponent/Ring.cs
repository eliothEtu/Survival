using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Survival.GameEngine.Inventory.ItemComponent
{
    internal class Ring : Item
    {
        private Tuple<string, double> bonus;
        public Tuple<string, double> Bonus
        {
            get => bonus;
            set
            {
                if (value.Item2 < 0)
                {
                    throw new ArgumentException("Bonus cannot be negative");
                }
                bonus = value;
            }
        }

        public Ring(string name, string description, BitmapImage image, string type, Tuple<string, double> bonus) : base (name, description, image, type)
        {
            Bonus = bonus;
        }
    }
}
