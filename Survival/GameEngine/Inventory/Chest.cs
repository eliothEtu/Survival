using Survival.GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace Survival.GameEngine.Inventory.ItemComponent
{
    public class Chest
    {
        public static readonly Dictionary<int, int> PRICE = new Dictionary<int, int>() { { 1, 100 }, { 2, 500 }, { 3, 1000 } };

        private List<double> rarity = new List<double>();
        private List<Item> itemDrop = new List<Item>();
        private Button but = new Button();
        private string typeItem;
        private int price;

        public List<double> Rarity { get => rarity; set => rarity = value; }
        public Button But { get => but; set => but = value; }
        public string TypeItem { get => typeItem; set => typeItem = value; }
        public int Price { get => price; set => price = value; }

        Random random = new Random();

        public Chest(List<double> rarity, Vector2 Size, BitmapImage texture, string typeItem, int price)
        {
            Rarity = rarity;

            BitmapImage text = texture;
            ImageBrush brush = new ImageBrush();

            brush.Stretch = Stretch.Uniform;
            brush.TileMode = TileMode.None;
            brush.ImageSource = text;

            this.But = new Button
            {
                Width = Size.X,
                Height = Size.Y,
                Background = brush,
            };
            this.But.Click += OpenBox;

            this.TypeItem = typeItem;
        }

        void OpenBox(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 2; i++)
            {
                double chance = random.NextDouble();
                if (chance < Rarity[2])
                {
                    List<Item> itemTier3 = Engine.Instance.Player.Inventory.GetItemByTierAndType(this.TypeItem, 3);
                    Item itemToAdd = new Item();
                    if (itemTier3.Count > 0)
                    {
                        itemToAdd = itemTier3[random.Next(itemTier3.Count)];

                        if (!Engine.Instance.Player.Inventory.InventoryList.Contains(itemToAdd))
                        {
                            Engine.Instance.Player.Inventory.InventoryList.Add(itemToAdd);
                        }
                        else
                        {
                            Engine.Instance.Player.Inventory.InventoryList[Engine.Instance.Player.Inventory.InventoryList.IndexOf(itemToAdd)].Quantity++;
                        }
                    }
                }
                else if (chance < Rarity[1])
                {
                    List<Item> itemTier2 = Engine.Instance.Player.Inventory.GetItemByTierAndType(this.TypeItem, 2);
                    if (itemTier2.Count > 0)
                    {
                        Item itemToAdd = itemTier2[random.Next(itemTier2.Count)];
                        if (!Engine.Instance.Player.Inventory.InventoryList.Contains(itemToAdd))
                        {
                            Engine.Instance.Player.Inventory.InventoryList.Add(itemToAdd);
                        }
                        else
                        {
                            Engine.Instance.Player.Inventory.InventoryList[Engine.Instance.Player.Inventory.InventoryList.IndexOf(itemToAdd)].Quantity++;
                        }
                    }
                }
                else
                {
                    List<Item> itemTier1 = Engine.Instance.Player.Inventory.GetItemByTierAndType(this.TypeItem, 1);
                    Item itemToAdd = new Item();
                    if (itemTier1.Count > 0)
                    {
                        itemToAdd = itemTier1[random.Next(itemTier1.Count)];
                        if (!Engine.Instance.Player.Inventory.InventoryList.Contains(itemToAdd))
                        {
                            Engine.Instance.Player.Inventory.InventoryList.Add(itemToAdd);
                        }
                        else
                        {
                            Engine.Instance.Player.Inventory.InventoryList[Engine.Instance.Player.Inventory.InventoryList.IndexOf(itemToAdd)].Quantity++;
                        }
                    }                    
                }
            }
        }
    }
}
