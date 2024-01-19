using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Survival.GameEngine.Inventory.ItemComponent
{
    internal class Armor : Item
    {
        private readonly string[] PARTS_POSSIBLE = new string[] { "Helmet", "Chestplate", "Boots", "Gloves"};

        private Tuple<string, int> bonus;
        public Tuple<string, int> Bonus
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

        private string part;
        public string Part
        {
            get => part;
            set
            {
                if (!PARTS_POSSIBLE.Contains(value))
                {
                    throw new ArgumentException("Part is invalide");
                }
                part = value;
            }
        }

        public Armor(string name, string part, string description, BitmapImage image, string type, Tuple<string, int> bonus, int tier) : base(name, description, image, type, tier)
        {
            Bonus = bonus;
            Part = part;
        }
    }
}
