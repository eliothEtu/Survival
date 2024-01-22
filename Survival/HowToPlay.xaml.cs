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
    /// Logique d'interaction pour HowToPlay.xaml
    /// </summary>
    public partial class HowToPlay : Window
    {
        public HowToPlay()
        {
            InitializeComponent();

            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;

            canvHTP.Width = SystemParameters.PrimaryScreenWidth;
            canvHTP.Height = SystemParameters.PrimaryScreenHeight;
            canvHTP.Focus();

            Label title = new Label()
            {
                Width = canvHTP.Width / 2,
                Height = 180,
                Content = "Bienvenue !",
                FontSize = 90,
                FontWeight = FontWeights.Bold,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            canvHTP.Children.Add(title);
            Canvas.SetLeft(title, canvHTP.Width / 2 - title.Width / 2);
            Canvas.SetTop(title, 30);

            Label aboutGame = new Label()
            {
                Width = canvHTP.Width,
                Height = double.NaN,
                Content = "A propos du jeu ? \n" +
                "Bienvenue dans Survival\r\n\r\nSurvivez dans un monde hostile, équipez-vous et affrontez les dangers qui vous entourent. Voici comment jouer :\r\n\r\nObjectif du Jeu :\r\nSurvivez aussi longtemps que possible dans ce monde dangereux. Équipez-vous soigneusement et affrontez des ennemis redoutables.\r\n\r\nContrôles :\r\n\r\nDéplacement : Utilisez les touches Z, Q, S, D pour vous déplacer dans toutes les directions et I pour ouvrir votre inventaire une fois le jeu lancé.\r\nTir : Clique gauche pour lancer une attaque et éliminer votre ennemis.\r\nÉquipement :\r\nVisitez la boutique pour acheter des objets essentiels à votre survie, tels que des armures, des anneaux et des artéfacts. Assurez-vous de vous équiper soigneusement avant de \npartir à l'aventure.\r\n\r\nConseils de Survie :\r\n\r\nGardez un oeil sur vôtre vie.\r\nRestez à l'affût des ennemis et choisissez vos combats avec précaution.\r\n\n\n Bonne chance, survivant.",
                FontSize = 30,
                FontWeight = FontWeights.Bold,

            };
            canvHTP.Children.Add(aboutGame);
            Canvas.SetLeft(aboutGame, 30);
            Canvas.SetTop(aboutGame, Canvas.GetTop(title) + title.Height + 100);

            Button exit = new Button()
            {
                Width = 100,
                Height = 75,
                Content = "Continuer",
                FontSize = 20
            };
            exit.Click += ExitHTP;
            canvHTP.Children.Add(exit);
            Canvas.SetLeft(exit, canvHTP.Width - exit.Width - 10);
            Canvas.SetTop(exit, canvHTP.Height - exit.Height - 10);
        }

        private void ExitHTP(object sender, EventArgs e)
        {
            Engine.Instance.PlaySoundButton();
            ((MainWindow)Application.Current.MainWindow).ExitHowToPlay();
        }
    }
}
