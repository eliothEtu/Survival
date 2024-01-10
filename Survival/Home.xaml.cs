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

namespace Survival
{
    /// <summary>
    /// Logique d'interaction pour Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        ImageBrush titleImage = new ImageBrush();
        public Home()
        {
            InitializeComponent();

            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;

            canvHome.Width = SystemParameters.PrimaryScreenWidth;
            canvHome.Height = SystemParameters.PrimaryScreenHeight;

            Button launchGame = new Button()
            {
                Height = 80,
                Width = SystemParameters.PrimaryScreenWidth / 4,
                Content = "Launch Game",
                FontSize = 30
            };
            launchGame.Click += LaunchGame;
            canvHome.Children.Add(launchGame);
            Canvas.SetTop(launchGame, SystemParameters.PrimaryScreenHeight/2 - launchGame.Height/2);
            Canvas.SetLeft(launchGame, SystemParameters.PrimaryScreenWidth/2 - launchGame.Width/2);

            Button openShop = new Button()
            {
                Height = 80,
                Width = SystemParameters.PrimaryScreenWidth / 4,
                Content = "Shop",
                FontSize = 30
            };
            canvHome.Children.Add(openShop);
            Canvas.SetTop(openShop, SystemParameters.PrimaryScreenHeight / 2 - openShop.Height / 2 + launchGame.Height);
            Canvas.SetLeft(openShop, SystemParameters.PrimaryScreenWidth / 2 - openShop.Width / 2);

            Button settingsButton = new Button()
            {
                Height = 80,
                Width = 80,
                Content = "Settings",
                FontSize = 24
            };
            canvHome.Children.Add(settingsButton);
            Canvas.SetTop(settingsButton, SystemParameters.PrimaryScreenHeight - settingsButton.Height - 20);
            Canvas.SetLeft(settingsButton, 20);

            titleImage.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "image\\title.png"));

            Rectangle title = new Rectangle()
            {
                Width = SystemParameters.PrimaryScreenWidth/1.5,
                Height = 200,
                Fill = titleImage
            };
            canvHome.Children.Add(title);
            Canvas.SetTop(title, 200);
            Canvas.SetLeft(title, SystemParameters.PrimaryScreenWidth - title.Width);
        }

        public void LaunchGame(object sender, RoutedEventArgs e)
        {
            if (Owner is MainWindow mainWindow)
            {
                mainWindow.LaunchGame();
            }
        }
    }
}
