using Survival.GameEngine;
using Survival.GameEngine.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
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
using Survival.GameEngine.Inventory.ItemComponent;

namespace Survival
{
    /// <summary>
    /// Logique d'interaction pour Archives.xaml
    /// </summary>
    public partial class Archives : Window
    {
        public Archives()
        {
            InitializeComponent();

            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;

            canvArchives.Width = SystemParameters.PrimaryScreenWidth;
            canvArchives.Height = SystemParameters.PrimaryScreenHeight;

            Button exit = new Button()
            {
                Width = 75,
                Height = 75,
                Background = new ImageBrush(Engine.imageExit),
            };
            Canvas.SetZIndex(exit, 1);
            exit.Click += ExitArchives;
            canvArchives.Children.Add(exit);
            Canvas.SetLeft(exit, canvArchives.Width - exit.Width - 10);
            Canvas.SetTop(exit, 10);

            Label title = new Label()
            {
                Width = canvArchives.Width,
                Height = 200,
                Content = "Archives",
                FontSize = 90,
                FontWeight = FontWeights.Bold,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            canvArchives.Children.Add(title);
            Canvas.SetLeft(title, 0);
            Canvas.SetTop(title, 0);

            int heightItem = (int)(canvArchives.Height - title.Height - 100) / Inventory.ITEMS_POSSIBLE.Values.Sum(list => list.Count);
            int index = 0;
            foreach(List<Item> items in Inventory.ITEMS_POSSIBLE.Values)
            {
                foreach(Item item in items)
                {
                    Rectangle image = new Rectangle()
                    {
                        Width = heightItem,
                        Height = heightItem,
                        Fill = item.Texture
                    };
                    canvArchives.Children.Add(image);
                    Canvas.SetTop(image, title.Height + index * heightItem + 100);
                    if (item is Armor)
                    {
                        Armor a = item as Armor;
                        Label itemUI = new Label()
                        {
                            Width = canvArchives.Width,
                            Height = heightItem,
                            Content = $"{a.Name} : Tier  = {a.Tier} / Bonus = {a.Bonus.Item1} / Valeur = +{a.Bonus.Item2}",
                            FontWeight = FontWeights.Bold,
                            FontSize = 30
                        };
                        canvArchives.Children.Add(itemUI);
                        Canvas.SetTop(itemUI, title.Height + index * heightItem + 100);
                        Canvas.SetLeft(itemUI, image.Width);
                    }
                    if (item is Ring)
                    {
                        Ring r = item as Ring;
                        Label itemUI = new Label()
                        {
                            Width = canvArchives.Width,
                            Height = heightItem,
                            Content = $"{r.Name} : Tier  = {r.Tier} / Bonus = {r.Bonus.Item1} / Valeur = *{r.Bonus.Item2}",
                            FontWeight = FontWeights.Bold,
                            FontSize = 30
                        };
                        canvArchives.Children.Add(itemUI);
                        Canvas.SetTop(itemUI, title.Height + index * heightItem + 100);
                        Canvas.SetLeft(itemUI, image.Width);
                    }
                    if (item is Artifact)
                    {
                        Artifact a = item as Artifact;
                        Label itemUI = new Label()
                        {
                            Width = canvArchives.Width,
                            Height = heightItem,
                            Content = $"{a.Name} : Tier  = {a.Tier}",
                            FontWeight = FontWeights.Bold,
                            FontSize = 30
                        };
                        canvArchives.Children.Add(itemUI);
                        Canvas.SetTop(itemUI, title.Height + index * heightItem + 100);
                        Canvas.SetLeft(itemUI, image.Width);
                    }
                    index++;
                }
            }
        }

        private void ExitArchives(object sender, EventArgs e)
        {
            Engine.Instance.PlaySoundButton();
            ((MainWindow)Application.Current.MainWindow).ExitArchives();
        }
    }
}
