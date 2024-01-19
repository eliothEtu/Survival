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
using Survival.GameEngine.Inventory.ItemComponent;

namespace Survival
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public HowToPlay howTPWindow;
        public Home homeWindow;
        public Credits creditsWindow;        
        public PreparationWindow preparationWindow;
        public Shop shopWindow;
        public Settings settingsWindow;
        public DeathWindow deathWindow;

        private string[] itemName = new string[5];
        private Dictionary<string, string> itemDescription = new Dictionary<string, string>();

        private Canvas invUi;

        public bool bInventory = false, bSettings = false;

        public MainWindow()
        {
            InitializeComponent();

            //ForceFocus.EnableLock();
            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;

            invUi = inv;

            canv.Width = SystemParameters.PrimaryScreenWidth;
            canv.Height = SystemParameters.PrimaryScreenHeight;
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

            howTPWindow = new HowToPlay();
            creditsWindow = new Credits();
            preparationWindow = new PreparationWindow();
            shopWindow = new Shop();
            settingsWindow = new Settings();
            deathWindow = new DeathWindow();

            howTPWindow.ShowDialog();

            homeWindow = new Home();
            homeWindow.ShowDialog();

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
        }
        
        public void OpenHowToPlay()
        {
            howTPWindow.ShowDialog();
        }

        public void ExitHowToPlay()
        {
            howTPWindow.Hide();
        }

        public void OpenCredits()
        {
            homeWindow.Hide();
            creditsWindow.ShowDialog();
        }

        public void ExitCredits()
        {
            creditsWindow.Hide();
            homeWindow.ShowDialog();
        }

        public void OpenInventory()
        {
            Engine.Instance.timer.Stop();
            canv.Children.Add(invUi);
            bInventory = true;
            LoadInventory();
            invUi.Visibility = Visibility.Visible;
            Canvas.SetZIndex(invUi, 1);
        }
        
        public void CloseInventory(object sender, RoutedEventArgs e)
        {
            bInventory = false;
            inv.Visibility = Visibility.Hidden;
            Engine.Instance.timer.Start();
        }

        public void CloseInventory()
        {
            bInventory = false;
            inv.Visibility = Visibility.Hidden;
            Engine.Instance.timer.Start();
        }

        public void OpenPreparation()
        {
            homeWindow.Hide();
            preparationWindow.LoadInventory();
            preparationWindow.ShowDialog();
        }

        public void ExitPreparation()
        {
            preparationWindow.Hide();
            homeWindow.ShowDialog();
        }

        public void OpenShop()
        {
            homeWindow.Hide();
            shopWindow.UpdateMoneyPlayer();
            shopWindow.ShowDialog();
        }

        public void ExitShop()
        {
            shopWindow.Hide();
            homeWindow.ShowDialog();
        }

        public void OpenSettings()
        {
            homeWindow.Hide();
            settingsWindow.ShowDialog();
        }

        public void ExitSettings()
        {
            settingsWindow.Hide();
            homeWindow.ShowDialog();
        }

        public void OpenSettingsInGame()
        {
            bSettings = true;
            settingsWindow.Exit.Visibility = Visibility.Hidden;
            settingsWindow.ShowDialog();
        }

        public void CloseSettingsInGame()
        {
            bSettings = false;
            settingsWindow.Exit.Visibility = Visibility.Visible;
            settingsWindow.Hide();
        }

        public void StartGame()
        {
            preparationWindow.Hide();
            Engine.Instance.Start();
        }

        public void ExitGame()
        {
            Engine.Instance.timer.Stop();
            Application.Current.Shutdown();
        }

        public void OpenDeathWindow()
        {
            itemContainer.Children.Clear();
            deathWindow.UpdateStat();
            deathWindow.ShowDialog();
        }

        public void ExitDeathWindow()
        {
            deathWindow.Hide();
            homeWindow.ShowDialog();
        }

        private void LoadInventory()
        {
            itemContainer.Children.Clear();
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

        private void ShowItemDescription(object sender, MouseEventArgs e)
        {
            Button butHover = sender as Button;
            string description = $"{Engine.Instance.Player.Inventory.GetDescriptionByItemName(butHover.Content.ToString())} \n Quantity : {Engine.Instance.Player.Inventory.GetItemByName(butHover.Content.ToString()).Quantity} \n Tier : {Engine.Instance.Player.Inventory.GetItemByName(butHover.Content.ToString()).Tier}";
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
            Engine.Instance.Controller.KeyDown(e);
        }

        private void canv_KeyUp(object sender, KeyEventArgs e)
        {
            Engine.Instance.Controller.KeyUp(e);
        }

        private void canv_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Engine.Instance.Controller.MouseLeft(e);
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
    }
}
