using Survival.GameEngine;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;


namespace Survival.GameEngine
{
    public class Chest
    {
        List<double> rarity = new List<double>();
        List<Item> itemDrop = new List<Item>();
        Button but = new Button();

        public List<double> Rarity { get => rarity; set => rarity = value; }
        public List<Item> ItemDrop { get => itemDrop; set => itemDrop = value; }
        public Button But { get => but; set => but = value; }

        public Chest(List<double> rarity, List<Item> itemDrop, Vector2 Size, BitmapImage texture)
        {
            Rarity = rarity;
            ItemDrop = itemDrop;

            BitmapImage text = texture;
            ImageBrush brush = new ImageBrush();

            brush.Stretch = Stretch.Uniform;
            brush.TileMode = TileMode.None;
            brush.ImageSource = text;

            this.But = new Button
            {
                Width = Size.X,
                Height = Size.Y,
                Background = brush
            };
            this.But.Click += OpenBox;
        }

        void OpenBox(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Box opened succesfully");
            foreach(Item i in ItemDrop)
            {
                Console.WriteLine($"{i.Name}");
                Engine.Instance.Player.Inventory.InventoryList.Add(i);
            }
        }
    }
}
