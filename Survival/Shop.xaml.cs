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
        private string[] nameButton = new string[] { "Armure", "Anneau", "Artéfact"};

        WrapPanel itemInfo;
        public WrapPanel ItemInfo { get => itemInfo; set => itemInfo = value; }

        Label errorMoney, money, rarity;
        public Label ErrorMoney { get => errorMoney; set => errorMoney = value; }
        public Label Money { get => money; set => money = value; }
        public Label Rarity { get => rarity; set => rarity = value; }

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

            imgButton = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\item\\tresor.png"));

            Button exit = new Button()
            {
                Width = 75,
                Height = 75,
                Background = new ImageBrush(Engine.imageExit),
                FocusVisualStyle = null,
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
                        Label nameBox = new Label()
                        {
                            Width = (canvContainer.Height - 20) / 3,
                            Height = 70,
                            FontSize = 20,
                            Content = nameButton[i / 2] + " Tier " + tier + "\nPrix : " + Chest.PRICE[tier],
                            FontWeight = FontWeights.Bold,
                            Foreground = new SolidColorBrush(Color.FromRgb(255 ,255, 255)),
                            VerticalContentAlignment = VerticalAlignment.Top,
                            HorizontalContentAlignment = HorizontalAlignment.Center,
                        };
                        canvContainer.Children.Add(nameBox);
                        Canvas.SetTop(nameBox, canvContainer.Height / 2 + j * chest.But.Height + pos * 80 + 55 + chest.But.Height);
                        Canvas.SetLeft(nameBox, 10 + chest.But.Width * i + chest.But.Width / 2 * i);
                    } else {
                        tier = 0;
                    }
                }
            }

            ItemInfo = new WrapPanel()
            {
                Width = 90 + 400, //90 taille image item 400 text;
                Height = canvShop.Height / 2,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            canvShop.Children.Add(ItemInfo);
            Canvas.SetTop(ItemInfo, canvShop.Height / 2 - ItemInfo.Height / 2);
            Canvas.SetLeft(ItemInfo, canvShop.Width/2 - ItemInfo.Width / 2);
            ItemInfo.Visibility = Visibility.Hidden;

            Button clearItemInfo = new Button()
            {
                Width = 100,
                Height = 50,
                Content = "Ok",
                Margin = new Thickness(195, 0, 0, 0)
            };
            clearItemInfo.Click += ClearItemInfo;
            ItemInfo.Children.Add(clearItemInfo);

            Image moneyImage = new Image()
            {
                Width = 50,
                Height = 50,
                Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\money.png"))
            };
            canvShop.Children.Add(moneyImage);
            Canvas.SetTop(moneyImage, 10);
            Canvas.SetLeft(moneyImage, 10);

            Money = new Label()
            {
                Height = 50,
                Width = double.NaN,
                Content = ": " + Engine.Instance.Player.Money,
                FontSize = 30,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.White),
            };
            canvShop.Children.Add(Money);
            Canvas.SetTop(Money, 10);
            Canvas.SetLeft(Money, Canvas.GetLeft(moneyImage) + moneyImage.Width + 10);

            ErrorMoney = new Label()
            {
                Height = 50,
                Width = canvShop.Width/2,
                Content = "Pas assez d'argent",
                FontSize = 30,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.Red),
                HorizontalContentAlignment = HorizontalAlignment.Center,
            };
            canvShop.Children.Add(ErrorMoney);
            Canvas.SetTop(ErrorMoney, canvShop.Height - ErrorMoney.Height - 10);
            Canvas.SetLeft(ErrorMoney, canvShop.Width/2 - ErrorMoney.Width / 2);
            ErrorMoney.Visibility = Visibility.Hidden;
        }

        public void UpdateMoneyPlayer()
        {
            Money.Content = ": " + Engine.Instance.Player.Money;
        }

        private void ClearItemInfo(object sender, EventArgs e)
        {
            ItemInfo.Visibility = Visibility.Hidden;
            ResetItemInfo();
        }

        public void ResetItemInfo()
        {
            if (ItemInfo.Children.Count > 1)
            {
                for (int i = 1; i < ItemInfo.Children.Count; i++)
                {
                    ItemInfo.Children.RemoveAt(0);
                }
            }
        }

        private void ExitShop(object sender, RoutedEventArgs e)
        {
            Engine.Instance.PlaySoundButton();
            ErrorMoney.Visibility = Visibility.Hidden;
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

        public void ShowChestRarity(List<double> rarity)
        {
            string text = "";
            for (int i = 0 ; i < rarity.Count; i++) { text += $"Tier {i + 1} : {rarity[i] * 100} % / "; }
            this.Rarity = new Label()
            {
                Width = canvShop.Width,
                Height = 50,
                Content = text,
                FontSize = 30,
                Foreground = new SolidColorBrush(Colors.White),
                FontWeight = FontWeights.Bold,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            canvShop.Children.Add(this.Rarity);
            Canvas.SetTop(Rarity, canvShop.Height - Rarity.Height - 10);
            Canvas.SetLeft(Rarity, 0);
        }

        public void ClearShowChestRarity()
        {
            canvShop.Children.Remove(this.Rarity);
            this.Rarity = null;
        }
    }
}
