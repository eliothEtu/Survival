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
using Survival.GameEngine.Inventory;
using Survival.GameEngine.Inventory.ItemComponent;

namespace Survival
{
    /// <summary>
    /// Logique d'interaction pour Shop.xaml
    /// </summary>
    public partial class Shop : Window
    {
        public static List<List<double>> RARITY = new List<List<double>>() { new List<double> { 0.8, 0.2, 0.1}, new List<double> { 0.5, 0.35, 0.15}, new List<double> { 0.2, 0.5, 0.3 } };

        private BitmapImage imgButton = new BitmapImage();
        private string[] nameButton = new string[] { "Armor", "Ring", "Artifact"};
        public Shop()
        {
            InitializeComponent();

            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;

            canvShop.Width = SystemParameters.PrimaryScreenWidth;
            canvShop.Height = SystemParameters.PrimaryScreenHeight;
            
            canvContainer.Height = 0.85 * canvShop.Height + 35;
            canvContainer.Width = 10 + (canvContainer.Height - 20) / 3 * nameButton.Length * 2 + (canvContainer.Height - 20) / 3 / 2 * (nameButton.Length * 2) + 10;

            Canvas.SetTop(canvContainer, canvShop.Height/2 - canvContainer.Height/2);

            canvShop.Focus();

            imgButton = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\tresor.png"));

            Button exit = new Button()
            {
                Width = 50,
                Height = 50,
                Content = "Exit"
            };
            exit.Click += ExitShop;
            canvShop.Children.Add(exit);
            Canvas.SetLeft(exit, canvShop.Width - exit.Width - 10);
            Canvas.SetTop(exit, 10);

            int pos, tier = 0;

            for (int i = 0; i < (nameButton.Length)*2; i++)
            {
                for (int j = -1; j < 1; j++)
                {
                    if (tier < 3) { 
                        pos = (j < 0) ? -1 : 1;

                        Chest chest = new Chest(RARITY[tier], new Vector2((float)(canvContainer.Height - 20) / 3, (float)(canvContainer.Height - 20) / 3), imgButton, nameButton[i / 2], Chest.PRICE[tier + 1]);
                        canvContainer.Children.Add(chest.But);
                        Canvas.SetTop(chest.But, canvContainer.Height / 2 + j * chest.But.Height + pos * 80 + 35);
                        Canvas.SetLeft(chest.But, 10 + chest.But.Width * i + chest.But.Width / 2 * i);

                        tier++;
                        TextBlock nameBox = new TextBlock()
                        {
                            Width = (canvContainer.Height - 20) / 3,
                            Height = 35,
                            FontSize = 20,
                            TextAlignment = TextAlignment.Center,
                            Text = nameButton[i / 2] + " Tier " + tier,
                        };
                        canvContainer.Children.Add(nameBox);
                        Canvas.SetTop(nameBox, canvContainer.Height / 2 + j * chest.But.Height + pos * 80 + 35 + nameBox.Height + chest.But.Height);
                        Canvas.SetLeft(nameBox, 10 + chest.But.Width * i + chest.But.Width / 2 * i);
                    } else {
                        tier = 0;
                    }
                }
            }

            Image moneyImage = new Image()
            {
                Width = 50,
                Height = 50,
                Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\tresor.png"))
            };
            canvShop.Children.Add(moneyImage);
            Canvas.SetTop(moneyImage, 10);
            Canvas.SetLeft(moneyImage, 10);

            Label money = new Label()
            {
                Height = 50,
                Width = double.NaN,
                Content = ": " + Engine.Instance.Player.Money,
                FontSize = 30,
                FontWeight = FontWeights.Bold,
            };
            canvShop.Children.Add(money);
            Canvas.SetTop(money, 10);
            Canvas.SetLeft(money, Canvas.GetLeft(moneyImage) + moneyImage.Width + 10);
        }

        private void ExitShop(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).ExitShop();
        }

        private void canvShop_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            double newPos = 0;
            switch (e.Delta)
            {
                case -120:
                    newPos = Canvas.GetLeft(canvContainer) + e.Delta / 2;
                    break;

                case 120:
                    newPos = Canvas.GetLeft(canvContainer) + e.Delta / 2;
                    break;
            }
            newPos = Math.Clamp(newPos, -(canvContainer.Width/3), 30);
            Canvas.SetLeft(canvContainer,  newPos);
        }
    }
}
