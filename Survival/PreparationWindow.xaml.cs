using Survival.GameEngine;
using Survival.GameEngine.Inventory.ItemComponent;
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

namespace Survival
{
    /// <summary>
    /// Logique d'interaction pour PreparationWindow.xaml
    /// </summary>
    public partial class PreparationWindow : Window
    {
        ImageBrush playerImage = new ImageBrush();
        Rectangle player;

        Dictionary<string, Rectangle> equipments = new Dictionary<string, Rectangle>();
        List<Item> itemEquiped = new List<Item>();
        string[] nameSlot = new string[] { "Helmet", "Chestplate", "Leggings", "Boots", "Gloves", "Ring", "Artifact" };

        WrapPanel inventoryEquipment = new WrapPanel();
        Label error = new Label();

        public Item dragObject = null;
        bool bArtefactSet, bObjectSet = false;

        Rectangle imagePower;

        public PreparationWindow()
        {
            InitializeComponent();

            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;

            canvPW.Width = SystemParameters.PrimaryScreenWidth;
            canvPW.Height = SystemParameters.PrimaryScreenHeight;

            Button exit = new Button()
            {
                Width = 75,
                Height = 75,
                Background = new ImageBrush(Engine.imageExit),
            };
            Canvas.SetZIndex(exit, 1);
            exit.Click += ExitPreparation;
            canvPW.Children.Add(exit);
            Canvas.SetLeft(exit, canvPW.Width - exit.Width - 10);
            Canvas.SetTop(exit, 10);

            Label title = new Label()
            {
                Height = 200,
                Width = SystemParameters.PrimaryScreenWidth,
                Content = "Choississez votre équipement",
                FontSize = 50,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                FontWeight = FontWeights.Bold
            };
            canvPW.Children.Add(title);
            Canvas.SetTop(title, 20);
            Canvas.SetLeft(title, 0);

            playerImage.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\player\\face.png"));

            player = new Rectangle()
            {
                Height = SystemParameters.PrimaryScreenHeight / 2,
                Width = SystemParameters.PrimaryScreenHeight / 2,
                Fill = playerImage
            };
            canvPW.Children.Add(player);
            Canvas.SetTop(player, player.Height / 2);
            Canvas.SetLeft(player, 50);

            for (int i = 0; i < nameSlot.Length; i++)
            {
                Rectangle rectangle = new Rectangle()
                {
                    Width = 90,
                    Height = 90,
                    Fill = Brushes.DarkGray,
                    Stroke = Brushes.Black
                };
                canvPW.Children.Add(rectangle);
                if (i < 5)
                {
                    Canvas.SetLeft(rectangle, 50 + player.Width + 20);
                    Canvas.SetTop(rectangle, player.Height / 2 - 10 + rectangle.Height * i + 10);
                }
                else
                {
                    Canvas.SetLeft(rectangle, 50 + player.Width + 20);
                    Canvas.SetTop(rectangle, player.Height / 2 - 10 + rectangle.Height * i + 60);
                }

                equipments[nameSlot[i]] = rectangle;
            }

            inventoryEquipment.Width = SystemParameters.PrimaryScreenWidth / 2;
            inventoryEquipment.Height = SystemParameters.PrimaryScreenHeight / 1.5;
            inventoryEquipment.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#71FF0000");
            canvPW.Children.Add(inventoryEquipment);
            Canvas.SetTop(inventoryEquipment, SystemParameters.PrimaryScreenHeight / 2 - inventoryEquipment.Height / 2);
            Canvas.SetLeft(inventoryEquipment, SystemParameters.PrimaryScreenWidth / 2);

            Button startGame = new Button()
            {
                Width = 100,
                Height = 70,
                Content = "Lancer",
                FontSize = 18
            };
            startGame.Click += Start;
            canvPW.Children.Add(startGame);
            Canvas.SetTop(startGame, SystemParameters.PrimaryScreenHeight - 20 - startGame.Height);
            Canvas.SetLeft(startGame, SystemParameters.PrimaryScreenWidth - 20 - startGame.Width);
        }

        private void ExitPreparation( object sender, EventArgs e)
        {
            Engine.Instance.PlaySoundButton();
            ((MainWindow)Application.Current.MainWindow).ExitPreparation();
        }

        void Start(object sender, RoutedEventArgs e)
        {
            Engine.Instance.PlaySoundButton();
            if (!bArtefactSet)
            {
                error = new Label()
                {
                    Width = canvPW.Width / 2,
                    Height = 180,
                    Content = "Vous n'avez pas équipé d'artéfacts. Veuillez en equiper un.",
                    FontSize = 40,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Color.FromRgb(225, 0, 0)),
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                };
                canvPW.Children.Add(error);
                Canvas.SetLeft(error, canvPW.Width / 2 - error.Width / 2);
                Canvas.SetTop(error, canvPW.Height - error.Height);
                return;
            }
            foreach (Item item in itemEquiped)
            {
                switch (item.Type)
                {
                    case "Armor":
                        Armor armor = item as Armor;
                        switch (armor.Bonus.Item1)
                        {
                            case "Health":
                                Engine.Instance.Player.Life += armor.Bonus.Item2;
                                break;
                            case "Damage":
                                Engine.Instance.Player.Damage += armor.Bonus.Item2;
                                break;
                            case "ProjectileVelocity":
                                Engine.Instance.Player.ProjectileVelocity += armor.Bonus.Item2;
                                break;
                            case "ProjectileLifeSpan":
                                Engine.Instance.Player.ProjectileLifeSpan += TimeSpan.FromSeconds(armor.Bonus.Item2);
                                break;
                        }
                        break;

                    case "Ring":
                        Ring ring = item as Ring;
                        switch (ring.Bonus.Item1)
                        {
                            case "Health":
                                Engine.Instance.Player.Life *= ring.Bonus.Item2;
                                break;
                            case "Damage":
                                Engine.Instance.Player.Damage *= ring.Bonus.Item2;
                                break;
                            case "ProjectileVelocity":
                                Engine.Instance.Player.ProjectileVelocity *= (float)ring.Bonus.Item2;
                                break;
                            case "ProjectileLifeSpan":
                                Engine.Instance.Player.ProjectileLifeSpan *= ring.Bonus.Item2;
                                break;
                        }
                        break;
                }
            }
            foreach (Item i in itemEquiped)
            {
                i.bCanDrag = true;
                canvPW.Children.Remove(i.Rectangle);
            }
            canvPW.Children.Remove(imagePower);
            ((MainWindow)Application.Current.MainWindow).StartGame();
        }
        public void LoadInventory()
        {
            inventoryEquipment.Children.Clear();
            foreach (Item i in Engine.Instance.Player.Inventory.InventoryList)
                inventoryEquipment.Children.Add(i.Rectangle);
        }

        private void canvPW_PreviewMouseMove(object sender, MouseEventArgs e) 
        {
            if (dragObject == null)
            {
                return;
            }
            if (inventoryEquipment.Children.Contains(dragObject.Rectangle))
            {
                inventoryEquipment.Children.Remove(dragObject.Rectangle);
                canvPW.Children.Add(dragObject.Rectangle);
            }
            Point position = e.GetPosition(sender as UIElement);
            Canvas.SetTop(dragObject.Rectangle, position.Y - dragObject.Rectangle.Height / 2);
            Canvas.SetLeft(dragObject.Rectangle, position.X - dragObject.Rectangle.Width / 2);
        }

        private void canvPW_PreviewMouseUp(object sender, MouseButtonEventArgs e) 
        {
            error.Content = string.Empty;
            bObjectSet = false;
            if (dragObject != null)
            {
                foreach (KeyValuePair<string, Rectangle> keyValuePair in equipments)
                {
                    Rect equipRect = new Rect(Canvas.GetLeft(keyValuePair.Value), Canvas.GetTop(keyValuePair.Value), keyValuePair.Value.Width, keyValuePair.Value.Height);
                    Rect dragRect = new Rect(Canvas.GetLeft(dragObject.Rectangle), Canvas.GetTop(dragObject.Rectangle), dragObject.Rectangle.Width, dragObject.Rectangle.Height);

                    if (equipRect.IntersectsWith(dragRect))
                    {
                        switch (dragObject.Type)
                        {
                            case "Armor":
                                Armor a = dragObject as Armor;
                                if (a.Part == keyValuePair.Key)
                                {
                                    foreach (Item i in itemEquiped)
                                    {
                                        Console.WriteLine(i.Quantity);
                                        if (i is Armor)
                                        {
                                            Armor equiped = i as Armor;
                                            if (equiped.Part == a.Part)
                                            {
                                                itemEquiped.Remove(i);
                                                Rectangle remove = i.Rectangle;
                                                canvPW.Children.Remove(remove);
                                                i.bCanDrag = true;
                                                inventoryEquipment.Children.Add(i.Rectangle);
                                                break;
                                            }
                                        }
                                    }
                                    dragObject.bCanDrag = false;
                                    Canvas.SetTop(dragObject.Rectangle, Canvas.GetTop(keyValuePair.Value));
                                    Canvas.SetLeft(dragObject.Rectangle, Canvas.GetLeft(keyValuePair.Value));
                                    itemEquiped.Add(dragObject);
                                    bObjectSet = true;
                                }
                                break;

                            case "Artifact":
                                Artifact artifact = dragObject as Artifact;
                                if (artifact.Type == keyValuePair.Key)
                                {
                                    dragObject.bCanDrag = false;
                                    Canvas.SetTop(dragObject.Rectangle, Canvas.GetTop(keyValuePair.Value));
                                    Canvas.SetLeft(dragObject.Rectangle, Canvas.GetLeft(keyValuePair.Value));
                                    itemEquiped.Add(dragObject);

                                    imagePower = new Rectangle()
                                    {
                                        Width = 400,
                                        Height = 400,
                                        Fill = dragObject.Texture
                                    };
                                    canvPW.Children.Add(imagePower);
                                    Canvas.SetTop(imagePower, Canvas.GetTop(player) + player.Height / 2 - 60);
                                    Canvas.SetLeft(imagePower, Canvas.GetLeft(player) + 20);
                                    Engine.Instance.Player.ItemEquiped = artifact;
                                    bArtefactSet = true;
                                    bObjectSet = true;
                                }
                                break;

                            case "Ring":
                                Ring ring = dragObject as Ring;
                                if (ring.Type == keyValuePair.Key)
                                {
                                    dragObject.bCanDrag = false;
                                    Canvas.SetTop(dragObject.Rectangle, Canvas.GetTop(keyValuePair.Value));
                                    Canvas.SetLeft(dragObject.Rectangle, Canvas.GetLeft(keyValuePair.Value));
                                    itemEquiped.Add(dragObject);
                                    bObjectSet = true;
                                }
                                break;
                        }
                    }
                }
            }
            if (!bObjectSet)
            {
                if (dragObject != null)
                {
                    canvPW.Children.Remove(dragObject.Rectangle);
                    inventoryEquipment.Children.Add(dragObject.Rectangle);
                }
            }
            dragObject = null;
            canvPW.ReleaseMouseCapture();
        }
    }
}
