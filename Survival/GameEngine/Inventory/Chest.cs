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
            this.Price = price;
        }

        void OpenBox(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).shopWindow.ErrorMoney.Visibility = Visibility.Hidden;
            if (Engine.Instance.Player.Money >= this.Price)
            {
                Engine.Instance.Player.Money = Engine.Instance.Player.Money - this.Price;
                ((MainWindow)Application.Current.MainWindow).shopWindow.UpdateMoneyPlayer();
                ((MainWindow)Application.Current.MainWindow).shopWindow.ResetItemInfo();
                int tier = 0;
                for (int i = 0; i < random.Next(2, 10); i++)
                {
                    double chance = random.NextDouble();
                    if (chance < Rarity[2])
                    {
                        tier = 3;
                    }
                    else if (chance < Rarity[1])
                    {
                        tier = 2;
                    }
                    else
                    {
                        tier = 1;
                    }

                    List<Item> itemTier = Engine.Instance.Player.Inventory.GetItemByTierAndType(this.TypeItem, tier);
                    Item itemToAdd = new Item();
                    if (itemTier.Count > 0)
                    {
                        itemToAdd = itemTier[random.Next(itemTier.Count)];

                        if (!Engine.Instance.Player.Inventory.InventoryList.Contains(itemToAdd))
                        {
                            Engine.Instance.Player.Inventory.InventoryList.Add(itemToAdd);
                        }
                        else
                        {
                            Engine.Instance.Player.Inventory.InventoryList[Engine.Instance.Player.Inventory.InventoryList.IndexOf(itemToAdd)].Quantity++;
                        }
                    }
                    WrapPanel borderItemInfo = new WrapPanel()
                    {
                        Height = 90,
                        Width = ((MainWindow)Application.Current.MainWindow).shopWindow.ItemInfo.Width,
                        Orientation = Orientation.Horizontal,
                        Background = new SolidColorBrush(Color.FromArgb(180, 0, 0, 0)),
                        HorizontalAlignment = HorizontalAlignment.Center,
                    };
                    Rectangle itemRectangle = new Rectangle()
                    {
                        Width = itemToAdd.Rectangle.Width,
                        Height = itemToAdd.Rectangle.Height,
                        Fill = itemToAdd.Rectangle.Fill
                    };
                    Label nameItem = new Label()
                    {
                        Height = 90,
                        Width = double.NaN,
                        Content = itemToAdd.Name,
                        Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                        FontSize = 40,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        VerticalContentAlignment = VerticalAlignment.Center,
                    };
                    borderItemInfo.Children.Add(itemRectangle);
                    borderItemInfo.Children.Add(nameItem);

                    ((MainWindow)Application.Current.MainWindow).shopWindow.ItemInfo.Visibility = Visibility.Visible;
                    ((MainWindow)Application.Current.MainWindow).shopWindow.ItemInfo.Children.Insert(0, borderItemInfo);
                    borderItemInfo = null;
                }
            } else
            {
                ((MainWindow)Application.Current.MainWindow).shopWindow.ErrorMoney.Visibility = Visibility.Visible;
            }
        }
    }
}
