using Survival.GameEngine;
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
                Content = "Se préparer pour le combat",
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
                Content = "Boutique",
                FontSize = 30
            };
            openShop.Click += OpenShop;
            canvHome.Children.Add(openShop);
            Canvas.SetTop(openShop, Canvas.GetTop(launchGame) + launchGame.Height);
            Canvas.SetLeft(openShop, SystemParameters.PrimaryScreenWidth / 2 - openShop.Width / 2);

            Button archivesButton = new Button()
            {
                Height = 80,
                Width = SystemParameters.PrimaryScreenWidth / 4,
                Content = "Archives",
                FontSize = 30
            };
            archivesButton.Click += OpenArchives;
            canvHome.Children.Add(archivesButton);
            Canvas.SetTop(archivesButton, Canvas.GetTop(openShop) + openShop.Height);
            Canvas.SetLeft(archivesButton, SystemParameters.PrimaryScreenWidth / 2 - archivesButton.Width / 2);

            Button creditButton = new Button()
            {
                Height = 80,
                Width = SystemParameters.PrimaryScreenWidth / 4,
                Content = "Crédits",
                FontSize = 30
            };
            creditButton.Click += OpenCredits;
            canvHome.Children.Add(creditButton);
            Canvas.SetTop(creditButton, Canvas.GetTop(archivesButton) + archivesButton.Height);
            Canvas.SetLeft(creditButton, SystemParameters.PrimaryScreenWidth / 2 - creditButton.Width / 2);

            Button settingsButton = new Button()
            {
                Height = 80,
                Width = 80,
                Background = new ImageBrush(new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\settings.png"))),
                FocusVisualStyle = null,
                FontSize = 24
            };
            settingsButton.Click += OpenSettings;
            canvHome.Children.Add(settingsButton);
            Canvas.SetTop(settingsButton, 20);
            Canvas.SetLeft(settingsButton, SystemParameters.PrimaryScreenWidth - settingsButton.Width - 20);

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

        private void LaunchGame(object sender, RoutedEventArgs e)
        {
            Engine.Instance.PlaySoundButton();
            ((MainWindow)Application.Current.MainWindow).OpenPreparation();
        }
        private void OpenShop(object sender, RoutedEventArgs e)
        {
            Engine.Instance.PlaySoundButton();
            ((MainWindow)Application.Current.MainWindow).OpenShop();
        }

        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            Engine.Instance.PlaySoundButton();
            ((MainWindow)Application.Current.MainWindow).OpenSettings();
        }

        private void OpenCredits(object sender, RoutedEventArgs e)
        {
            Engine.Instance.PlaySoundButton();
            ((MainWindow)Application.Current.MainWindow).OpenCredits();
        }

        private void OpenArchives(object sender, RoutedEventArgs e)
        {
            Engine.Instance.PlaySoundButton();
            ((MainWindow)Application.Current.MainWindow).OpenArchives();
        }
    }
}
