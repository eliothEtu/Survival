using Survival.MalwareStuff;
using Survival.GameEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Survival
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Home homeUI;
        public PreparationWindow preparationWindow;

        private string[] itemName = new string[5];
        private Dictionary<string, string> itemDescription = new Dictionary<string, string>();

        bool bInventory = false;

        public MainWindow()
        {
            InitializeComponent();
            //ForceFocus.EnableLock();

            //WindowState = WindowState.Maximized;
            //WindowStyle = WindowStyle.None;

            //canv.Width = SystemParameters.PrimaryScreenWidth;
            //canv.Height = SystemParameters.PrimaryScreenHeight;
            canv.Focus();

            inv.Visibility = Visibility.Hidden;
            inv.Width = SystemParameters.PrimaryScreenWidth - 300;
            inv.Height = SystemParameters.PrimaryScreenHeight - 300;
            inv.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#000000");
            inv.Background.Opacity = 0.7;
            Canvas.SetLeft(inv, SystemParameters.PrimaryScreenWidth / 2 - inv.Width / 2);
            Canvas.SetTop(inv, SystemParameters.PrimaryScreenHeight / 2 - inv.Height / 2);

            // Engine's constructor define a static instance property in Engine
            new Engine();

            // ((MainWindow)Application.Current.MainWindow).canv

            // this.Show();

            preparationWindow = new PreparationWindow();
           // preparationWindow.Owner = this;
            //preparationWindow.Hide();

            homeUI = new Home();
            homeUI.ShowDialog();

            itemContainer.Width = inv.Width - 400;
            itemContainer.Height = inv.Height - 50;
            Canvas.SetTop(itemContainer, 50);

            itemDescriptionUI.Width = 400;
            itemDescriptionUI.Height = inv.Height - 50;

            Canvas.SetLeft(itemDescriptionUI, itemContainer.Width);
            Canvas.SetTop(itemDescriptionUI, 50);

            Button but = new Button()
            {
                Height = 50,
                Width = 100,
                Content = "Close Inventory"
            };
            but.Click += CloseInventory;
            inv.Children.Add(but);
            Canvas.SetLeft(but, inv.Width - but.Width - 20);
            Canvas.SetTop(but, 20);

            itemName[0] = "Iron";
            itemName[1] = "Stone";
            itemName[2] = "Copper";
            itemName[3] = "Sword";
            itemName[4] = "Bow";

            itemDescription[itemName[0]] = "Just a iron ingot";
            itemDescription[itemName[1]] = "Classic stone";
            itemDescription[itemName[2]] = "Copper rust";
            itemDescription[itemName[3]] = "A sword find in a dungeon";
            itemDescription[itemName[4]] = "Bow find on a mob";

            LoadInventory();
        }

        public void OpenInventory()
        {
            bInventory = true;
            inv.Visibility = Visibility.Visible;
            Canvas.SetZIndex(inv, 2);
        }

        public void CloseInventory()
        {
            bInventory = false;
            inv.Visibility = Visibility.Hidden;
        }

        public void LaunchGame()
        {
            homeUI.Hide();
            preparationWindow.ShowDialog();
        }

        public void StartGame()
        {
            preparationWindow.Hide();
            Engine.Instance.Start();
        }

        public void CloseInventory(object sender, RoutedEventArgs e)
        {
            bInventory = false;
            inv.Visibility = Visibility.Hidden;
        }

        private void LoadInventory()
        {
            foreach (string item in itemName)
            {
                Button itemBut = new Button()
                {
                    Width = 100,
                    Height = 100,
                    Content = item
                };
                itemBut.MouseEnter += ShowItemDescription;
                itemBut.MouseLeave += ClearItemDescription;
                itemContainer.Children.Add(itemBut);
            }
        }

        private void ShowItemDescription(object sender, MouseEventArgs e)
        {
            Button butHover = sender as Button;
            Label itemD = new Label()
            {
                Width = double.NaN,
                Content = itemDescription[butHover.Content.ToString()],
                FontSize = 30,
                Foreground = new SolidColorBrush(Colors.White)
            };
            itemDescriptionUI.Children.Add(itemD);
        }

        private void ClearItemDescription(object sender, MouseEventArgs e)
        {
            itemDescriptionUI.Children.Clear();
        }

        private void canv_KeyDown(object sender, KeyEventArgs e)
        {
            if (!bInventory && e.Key == Key.I)
            {
                OpenInventory();
            }
            else
            {
                CloseInventory();
            }

            if (e.Key == Key.Escape)
            {
                this.Close();
            }
            Engine.Instance.Controller.KeyDown(e);
            /*if (e.Key == Key.Z)
            {
                Canvas.SetTop(player, Canvas.GetTop(player) - MapGenerator.BLOCK_SIZE);
                //cam.pos.Y -= MapGenerator.BLOCK_SIZE;
            }
            if (e.Key == Key.S)
            {
                Canvas.SetTop(player, Canvas.GetTop(player) + MapGenerator.BLOCK_SIZE);
                //cam.pos.Y += MapGenerator.BLOCK_SIZE;
            }
            if (e.Key == Key.Q)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) - MapGenerator.BLOCK_SIZE);
                //cam.pos.X -= MapGenerator.BLOCK_SIZE;
            }
            if (e.Key == Key.D)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) + MapGenerator.BLOCK_SIZE);
                //cam.pos.X += MapGenerator.BLOCK_SIZE;
            }*/
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            ForceFocus.DisableLock();
        }

        public void Exit()
        {
            // we need to kill the launcher first so it won't restart the game
            Process[] processes = Process.GetProcessesByName("Launcher.exe");
            foreach(Process process in processes)
            {
                process.Kill();
            }

            // then we can kill the game
            Application.Current.Shutdown();
        }

        private void canv_KeyUp(object sender, KeyEventArgs e)
        {
            Engine.Instance.Controller.KeyUp(e);
        }
    }
}
