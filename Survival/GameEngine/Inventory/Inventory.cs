using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Survival.GameEngine.Inventory.ItemComponent;

namespace Survival.GameEngine.Inventory
{
    internal class Inventory
    {
        public static readonly Dictionary<string, List<Item>> ITEMS_POSSIBLE = new Dictionary<string, List<Item>>()
        {
            { "Armor", new List<Item>
                {
                    new Armor("Helmet", "Helmet", "", new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\armor\\Helmet R.png")), "Armor", Tuple.Create("", 1)),
                    new Armor("Chestplate", "Chestplate", "", new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\armor\\ChestplateR.png")), "Armor", Tuple.Create("", 1)),
                    new Armor("Leggings", "Leggings", "", new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\tresor.png")), "Armor", Tuple.Create("", 1)),
                    new Armor("Boots", "Boots", "", new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\armor\\BootsR.png")), "Armor", Tuple.Create("", 1)),
                    new Armor("Gloves", "Gloves", "", new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\armor\\GloveR.png")), "Armor", Tuple.Create("", 1))
                }
            },
            { "Ring", new List<Item>
                {
                    new Ring("Ring of Fire", "", new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\anneauFeu.png")), "Ring", Tuple.Create("", 1.0)),
                    new Ring("Ring of Ice", "", new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\anneauFeu.png")), "Ring", Tuple.Create("", 1.0)),
                    new Ring("Ring of Sound", "", new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\anneauFeu.png")), "Ring", Tuple.Create("", 1.0)),
                }
            },
            { "Artifact", new List<Item>
                {
                new Artifact("fire","boule de feu",new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\attacks\\flamme.png")),"Artifact", TimeSpan.FromSeconds(1))
                }
            }
        };

        private List<Item> inventoryList;

        internal List<Item> InventoryList { get => inventoryList; set => inventoryList = value; }

        public Inventory()
        {
            InventoryList = new List<Item>();
        }

        public List<Item> GetItemByType(string type)
        {
            List<Item> list = new List<Item>();
            foreach (Item item in InventoryList)
            {
                if (item.Type == type)
                {
                    list.Add(item);
                }
            }

            return list;
        }

        public List<Item> GetItemByBonus(string bonus)
        {
            List<Item> list = new List<Item>();
            foreach (Ring item in GetItemByType("Ring"))
            {
                if (item.Bonus.Item1 == bonus)
                {
                    list.Add(item);
                }
            }

            foreach (Armor item in GetItemByType("Armor"))
            {
                if (item.Bonus.Item1 == bonus)
                {
                    list.Add(item);
                }
            }

            return list;
        }

        public List<Item> GetArmorByPart(string part)
        {
            List<Item> list = new List<Item>();
            foreach (Armor item in GetItemByType("Armor"))
            {
                if (item.Part == part)
                {
                    list.Add(item);
                }
            }
            return list;
        }

        public string GetDescriptionByItemName(string itemName)
        {
            foreach (Item item in InventoryList)
            {
                if (item.Name == itemName)
                {
                    return item.Description;
                }
            }
            return "";
        }
    }
}
