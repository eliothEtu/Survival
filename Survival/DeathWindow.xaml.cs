using Survival.GameEngine;
using System;
using System.Collections;
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
    /// Logique d'interaction pour DeathWindow.xaml
    /// </summary>
    public partial class DeathWindow : Window
    {
        Label stat;
        public DeathWindow()
        {
            InitializeComponent();

            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;

            canvDeath.Width = SystemParameters.PrimaryScreenWidth;
            canvDeath.Height = SystemParameters.PrimaryScreenHeight;

            Label title = new Label()
            {
                Width = canvDeath.Width / 2,
                Height = 180,
                Content = "Vous êtes mort",
                FontSize = 90,
                FontWeight = FontWeights.Bold,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            canvDeath.Children.Add(title);
            Canvas.SetLeft(title, canvDeath.Width / 2 - title.Width / 2);
            Canvas.SetTop(title, 30);

            Label sectionStat = new Label()
            {
                Width = double.NaN,
                Height = double.NaN,
                Content = $"Récapitulatif de la partie :",
                FontSize = 30,
                FontWeight = FontWeights.Bold,
            };
            canvDeath.Children.Add(sectionStat);
            Canvas.SetLeft(sectionStat, 10);
            Canvas.SetTop(sectionStat, Canvas.GetTop(title) + title.Height + 100);

            stat = new Label()
            {
                Width = double.NaN,
                Height = double.NaN,
                Content = $"Nombre de mob tué : {Engine.Instance.Player.MobKill} \n\nNombre de projectile lancé :  {Engine.Instance.Player.ProjectileFire} \n\nArgent gagné : {Engine.Instance.Player.Money - Engine.Instance.MoneyStartgame} \n\nTemps en vie : {Math.Round((DateTime.Now - Engine.Instance.TimeStartGame).TotalSeconds)} secondes",
                FontSize = 25,
                FontWeight = FontWeights.Bold,
            };
            canvDeath.Children.Add(stat);
            Canvas.SetLeft(stat, 20);
            Canvas.SetTop(stat, Canvas.GetTop(sectionStat) + sectionStat.ActualHeight + 50);

            Button returnMenu = new Button()
            {
                Width = 200,
                Height = 70,
                Content = "Retourner au menu",
                FontSize = 18
            };
            returnMenu.Click += GoToMenu;
            canvDeath.Children.Add(returnMenu);
            Canvas.SetTop(returnMenu, canvDeath.Height - returnMenu.Height - 10);
            Canvas.SetLeft(returnMenu, canvDeath.Width - returnMenu.Width - 10);
        }

        private void GoToMenu(object sender, RoutedEventArgs e)
        {
            Engine.Instance.PlaySoundButton();
            ((MainWindow)Application.Current.MainWindow).ExitDeathWindow();
        }

        public void UpdateStat()
        {
            stat.Content = $"Nombre de mob tué : {Engine.Instance.Player.MobKill} \n\nNombre de projectile lancé :  {Engine.Instance.Player.ProjectileFire} \n\nArgent gagné : {Engine.Instance.Player.Money - Engine.Instance.MoneyStartgame} \n\nTemps en vie : {Math.Round((DateTime.Now - Engine.Instance.TimeStartGame).TotalSeconds)} secondes";
        }
    }
}
