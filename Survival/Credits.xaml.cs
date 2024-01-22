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
    /// Logique d'interaction pour Credits.xaml
    /// </summary>
    public partial class Credits : Window
    {
        public Credits()
        {
            InitializeComponent();

            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;

            canvCredits.Width = SystemParameters.PrimaryScreenWidth;
            canvCredits.Height = SystemParameters.PrimaryScreenHeight;
            canvCredits.Focus();

            Button exit = new Button()
            {
                Width = 75,
                Height = 75,
                Background = new ImageBrush(Engine.imageExit),
                FocusVisualStyle = null
            };
            exit.Click += ExitCredits;
            canvCredits.Children.Add(exit);
            Canvas.SetLeft(exit, canvCredits.Width - exit.Width - 10);
            Canvas.SetTop(exit, 10);

            Label title = new Label()
            {
                Width = canvCredits.Width / 2,
                Height = 180,
                Content = "Crédits",
                FontSize = 90,
                FontWeight = FontWeights.Bold,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            canvCredits.Children.Add(title);
            Canvas.SetLeft(title, canvCredits.Width / 2 - title.Width / 2);
            Canvas.SetTop(title, 30);

            Label devName = new Label()
            {
                Width = canvCredits.Width,
                Height = 400,
                Content = "BETRIX Thomas \t Développeur \nROB Elioth \t Développeur \nTROUBA Maël \t Développeur",
                FontSize = 40,
                FontWeight = FontWeights.Bold,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            canvCredits.Children.Add(devName);
            Canvas.SetLeft(devName, 0);
            Canvas.SetTop(devName, Canvas.GetTop(title) + title.Height + 100);

            TextBlock designName = new TextBlock()
            {
                Width = canvCredits.Width,
                Height = double.NaN,
                Text = "ROB Elioth \t Graphisme \n Certaine ressources visuel ont été obtenu depuis internet via le site Flaticon \nPixel perfect \t Designer \nFreepik \t Designer",
                FontSize = 40,
                FontWeight = FontWeights.Bold,        
                TextAlignment = TextAlignment.Center,
            };
            canvCredits.Children.Add(designName);
            Canvas.SetLeft(designName, 0);
            Canvas.SetTop(designName, Canvas.GetTop(devName) + devName.Height + 100);
        }

        private void ExitCredits(object sender, RoutedEventArgs e)
        {
            Engine.Instance.PlaySoundButton();
            ((MainWindow)Application.Current.MainWindow).ExitCredits();
        }
    }
}
