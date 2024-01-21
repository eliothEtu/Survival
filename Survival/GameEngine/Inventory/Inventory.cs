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
            { "Armure", new List<Item>
                {
                    new Armor("Casque", "Helmet", "Simple helmet", new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\armor\\Tier1Helmet.png")), "Armor", Tuple.Create("ProjectileVelocity", 0.5), 1),
                    new Armor("Plastron", "Chestplate", "Simple Chestplate", new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\armor\\Tier1Chestplate.png")), "Armor", Tuple.Create("Health", 2.0), 1),
                    new Armor("Gants", "Gloves", "Simple Gloves", new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\armor\\Tier1Glove.png")), "Armor", Tuple.Create("Damage", 1.0), 1),
                    new Armor("Chaussures", "Boots", "Simple Boots", new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\armor\\Tier1Boots.png")), "Armor", Tuple.Create("ProjectileLifeSpan", 1.0), 1),

                    new Armor("Casque", "Helmet", "Simple helmet", new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\armor\\Tier2Helmet.png")), "Armor", Tuple.Create("ProjectileVelocity", 1.0), 2),
                    new Armor("Plastron", "Chestplate", "Simple Chestplate", new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\armor\\Tier2Chestplate.png")), "Armor", Tuple.Create("Health", 5.0), 2),
                    new Armor("Gants", "Gloves", "Simple Gloves", new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\armor\\Tier2Glove.png")), "Armor", Tuple.Create("Damage", 2.0), 2),
                    new Armor("Chaussures", "Boots", "Simple Boots", new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\armor\\Tier2Boots.png")), "Armor", Tuple.Create("ProjectileLifeSpan", 2.0), 2),

                    new Armor("Casque", "Helmet", "Simple helmet", new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\armor\\Tier3Helmet.png")), "Armor", Tuple.Create("ProjectileVelocity", 1.5), 3),
                    new Armor("Plastron", "Chestplate", "Simple Chestplate", new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\armor\\Tier3Chestplate.png")), "Armor", Tuple.Create("Health", 16.0), 3),
                    new Armor("Gants", "Gloves", "Simple Gloves", new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\armor\\Tier3Glove.png")), "Armor", Tuple.Create("Damage", 3.0), 3),
                    new Armor("Chaussures", "Boots", "Simple Boots", new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\armor\\Tier3Boots.png")), "Armor", Tuple.Create("ProjectileLifeSpan", 3.0), 3),
                }
            },
            { "Anneau", new List<Item>
                {
                    new Ring("Bague de feu", "Fire Ring", new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\item\\Tier1Ring.png")), "Ring", Tuple.Create("ProjectileVelocity", 1.5), 1),
                    new Ring("Bague de glace", "Ice Ring", new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\item\\Tier2Ring.png")), "Ring", Tuple.Create("Health", 5.0), 2),
                    new Ring("Bague de corruption", "Sound Ring", new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\item\\Tier3Ring.png")), "Ring", Tuple.Create("Damage", 3.0), 3),
                }
            },
            { "Artéfact", new List<Item>
                {
                new Artifact("Flamme","boule de feu",new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\attacks\\Tier1Artifact.png")), new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\attacks\\flame.png")),"Artifact", TimeSpan.FromSeconds(1), 1),
                new Artifact("Glace","boule de feu",new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\attacks\\Tier2Artifact.png")), new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\attacks\\ice.png")),"Artifact", TimeSpan.FromSeconds(1), 2),
                new Artifact("Corruption","boule de feu",new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\attacks\\Tier3Artifact.png")), new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\attacks\\corupt.png")),"Artifact", TimeSpan.FromSeconds(1), 3)
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

        public Item GetItemByName(string name)
        {
            foreach (Item item in InventoryList)
            {
                if (item.Name == name)
                {
                    return item;
                }
            }
            return new Item();
        }

        public List<Item> GetItemByTierAndType(string type, int tier)
        {
            List<Item> list = new List<Item>();
            foreach (Item item in ITEMS_POSSIBLE[type])
            {
                if (item.Tier == tier)
                {
                    list.Add(item);
                }
            }
            return list; 
        }
    }
}
