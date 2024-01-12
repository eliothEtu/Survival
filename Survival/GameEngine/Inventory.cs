using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Survival.GameEngine
{
    internal class Inventory
    {
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
