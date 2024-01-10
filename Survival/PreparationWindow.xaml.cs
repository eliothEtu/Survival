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
    /// Logique d'interaction pour PreparationWindow.xaml
    /// </summary>
    public partial class PreparationWindow : Window
    {
        ImageBrush playerImage = new ImageBrush();

        Dictionary<string, Button> equipments = new Dictionary<string, Button>();
        string[] nameSlot = new string[] {"Helmet", "Chestplate", "Leggings", "Boots", "Gloves", "Sword"};

        WrapPanel inventoryEquipment = new WrapPanel();
        public PreparationWindow()
        {
            InitializeComponent();

            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;

            canvPW.Width = SystemParameters.PrimaryScreenWidth;
            canvPW.Height = SystemParameters.PrimaryScreenHeight;

            Label title = new Label()
            {
                Height = 200,
                Width = SystemParameters.PrimaryScreenWidth,
                Content = "Choice your equipment",
                FontSize = 50,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                FontWeight = FontWeights.Bold
            };
            canvPW.Children.Add(title);
            Canvas.SetTop(title, 20);
            Canvas.SetLeft(title, 0);

            playerImage.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Image\\face (1).png"));

            Rectangle player = new Rectangle()
            {
                Height = SystemParameters.PrimaryScreenHeight / 2,
                Width = SystemParameters.PrimaryScreenHeight / 2,
                Fill = playerImage
            };
            canvPW.Children.Add(player);
            Canvas.SetTop(player, player.Height / 2);
            Canvas.SetLeft(player, 50);

            for (int i = 0; i < 6; i++)
            {
                Button rectangle = new Button()
                {
                    Width = 90,
                    Height = 90,
                    Content = nameSlot[i]
                };
                canvPW.Children.Add(rectangle);
                if (i < 5)
                {
                    Canvas.SetLeft(rectangle, 50 + player.Width + 20);
                    Canvas.SetTop(rectangle, player.Height / 2 - 10 + rectangle.Height * i + 10);
                } else
                {
                    Canvas.SetLeft(rectangle, 50 + player.Width + 20);
                    Canvas.SetTop(rectangle, player.Height / 2 - 10 + rectangle.Height * i + 60);
                }

                equipments[nameSlot[i]] = rectangle;
            }

            inventoryEquipment.Width = SystemParameters.PrimaryScreenWidth / 2;
            inventoryEquipment.Height = SystemParameters.PrimaryScreenHeight/1.5;
            inventoryEquipment.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#71FF0000");
            canvPW.Children.Add(inventoryEquipment);
            Canvas.SetTop(inventoryEquipment, SystemParameters.PrimaryScreenHeight / 2 - inventoryEquipment.Height / 2);
            Canvas.SetLeft(inventoryEquipment, SystemParameters.PrimaryScreenWidth / 2);

            Button startGame = new Button()
            {
                Width = 100,
                Height = 70,
                Content = "Start",
                FontSize = 18
            };
            startGame.Click += Start;
            canvPW.Children.Add(startGame);
            Canvas.SetTop(startGame, SystemParameters.PrimaryScreenHeight - 20 - startGame.Height);
            Canvas.SetLeft(startGame, SystemParameters.PrimaryScreenWidth - 20 - startGame.Width);
        }

        void Start(object sender, RoutedEventArgs e)
        {
            if (Owner is MainWindow mainWindow){
                mainWindow.StartGame();
            }
        }
    }
}
