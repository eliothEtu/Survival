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

        private Canvas invUi; ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        bool bInventory = false;

        public MainWindow()
        {
            InitializeComponent();
            invUi = inv; ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //ForceFocus.EnableLock();

            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;

            canv.Width = SystemParameters.PrimaryScreenWidth;
            canv.Height = SystemParameters.PrimaryScreenHeight;
            canv.Focus();

            invUi.Visibility = Visibility.Hidden;
            invUi.Width = SystemParameters.PrimaryScreenWidth - 300;
            invUi.Height = SystemParameters.PrimaryScreenHeight - 300;
            invUi.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#000000");
            invUi.Background.Opacity = 0.7;
            Canvas.SetLeft(invUi, SystemParameters.PrimaryScreenWidth / 2 - invUi.Width / 2);
            Canvas.SetTop(invUi, SystemParameters.PrimaryScreenHeight / 2 - invUi.Height / 2);

            // Engine's constructor define a static instance property in Engine
            new Engine();

            // ((MainWindow)Application.Current.MainWindow).canv

            // this.Show();

            preparationWindow = new PreparationWindow();
            //preparationWindow.Owner = this;
            //preparationWindow.Hide();

            homeUI = new Home();
            homeUI.ShowDialog();

            itemContainer.Width = invUi.Width - 400;
            itemContainer.Height = invUi.Height - 50;
            Canvas.SetTop(itemContainer, 50);

            itemDescriptionUI.Width = 400;
            itemDescriptionUI.Height = invUi.Height - 50;

            Canvas.SetLeft(itemDescriptionUI, itemContainer.Width);
            Canvas.SetTop(itemDescriptionUI, 50);

            Button but = new Button()
            {
                Height = 50,
                Width = 100,
                Content = "Close Inventory"
            };
            but.Click += CloseInventory;
            invUi.Children.Add(but);
            Canvas.SetLeft(but, invUi.Width - but.Width - 20);
            Canvas.SetTop(but, 20);

            LoadInventory();
        }

        public void OpenInventory()
        {
            Engine.Instance.timer.Stop(); ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            canv.Children.Add(invUi); ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bInventory = true;
            invUi.Visibility = Visibility.Visible;
            Canvas.SetZIndex(invUi, 1);
        }

        public void CloseInventory()
        {
            bInventory = false;
            invUi.Visibility = Visibility.Hidden;
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
            invUi.Visibility = Visibility.Hidden;
        }

        private void LoadInventory() ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        {
            foreach (Item item in Engine.Instance.Player.Inventory.InventoryList)
            {
                Button itemBut = new Button()
                {
                    Width = 100,
                    Height = 100,
                    Content = item.Name,
                    Background = item.Texture
                };
                itemBut.MouseEnter += ShowItemDescription;
                itemBut.MouseLeave += ClearItemDescription;
                itemContainer.Children.Add(itemBut);
            }
        }

        private void ShowItemDescription(object sender, MouseEventArgs e) ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        {
            Button butHover = sender as Button;
            string description = Engine.Instance.Player.Inventory.GetDescriptionByItemName(butHover.Content.ToString());
            Label itemD = new Label()
            {
                Width = double.NaN,
                Content = description,
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
                Engine.Instance.timer.Stop();
                this.Close();
            }
            Engine.Instance.Controller.KeyDown(e);
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
