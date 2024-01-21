using Survival.GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Logique d'interaction pour Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private readonly string CODE = "*";

        private Button exit, confirmSizeButton;
        private TextBox xValueText, yValueText;
        private Slider sliderDifficulty, sliderVolumeSound;
        private Label labelShowDifficulty, labelShowVolumeSound;
        private PasswordBox passwordCheat;

        //God Mod UI
        private Rectangle separator;
        private Label labelMoney;
        private TextBox moneyText;
        private Button addMoneyButton;
        private Label textInvincible;
        private CheckBox checkDamage;

        public Button Exit { get => Exit1; set => Exit1 = value; }
        public Button AddMoneyButton { get => AddMoneyButton1; set => AddMoneyButton1 = value; }
        public CheckBox CheckDamage { get => checkDamage; set => checkDamage = value; }
        public Label TextInvincible { get => textInvincible; set => textInvincible = value; }
        public Button AddMoneyButton1 { get => addMoneyButton; set => addMoneyButton = value; }
        public TextBox MoneyText { get => moneyText; set => moneyText = value; }
        public Label LabelMoney { get => labelMoney; set => labelMoney = value; }
        public Rectangle Separator { get => separator; set => separator = value; }
        public PasswordBox PasswordCheat { get => passwordCheat; set => passwordCheat = value; }
        public Label LabelShowDifficulty { get => labelShowDifficulty; set => labelShowDifficulty = value; }
        public Label LabelShowVolumeSound { get => labelShowVolumeSound; set => labelShowVolumeSound = value; }
        public Slider SliderDifficulty { get => sliderDifficulty; set => sliderDifficulty = value; }
        public Slider SliderVolumeSound { get => sliderVolumeSound; set => sliderVolumeSound = value; }
        public TextBox XValueText { get => xValueText; set => xValueText = value; }
        public TextBox YValueText { get => yValueText; set => yValueText = value; }
        public Button Exit1 { get => exit; set => exit = value; }
        public Button ConfirmSizeButton { get => confirmSizeButton; set => confirmSizeButton = value; }

        public Settings()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;
            this.IsVisibleChanged += ClearGodMod;

            canvSettings.Width = SystemParameters.PrimaryScreenWidth;
            canvSettings.Height = SystemParameters.PrimaryScreenHeight;
            canvSettings.Focus();

            Exit = new Button()
            {
                Width = 75,
                Height = 75,
                Background = new ImageBrush(Engine.imageExit),
                FocusVisualStyle = null
            };
            Exit.Click += ExitSettings;
            canvSettings.Children.Add(Exit);
            Canvas.SetLeft(Exit, canvSettings.Width - Exit.Width - 10);
            Canvas.SetTop(Exit, 10);

            Button exitGame = new Button()
            {
                Width = 150,
                Height = 75,
                Content = "Fermer le jeu",
                FontSize = 25
            };
            exitGame.Click += ExitGame;
            canvSettings.Children.Add(exitGame);
            Canvas.SetLeft(exitGame, 10);
            Canvas.SetTop(exitGame, 10);

            Label title = new Label()
            {
                Width = canvSettings.Width / 2,
                Height = 180,
                Content = "Paramètres",
                FontSize = 90,
                FontWeight = FontWeights.Bold,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            canvSettings.Children.Add(title);
            Canvas.SetLeft(title, canvSettings.Width/2 - title.Width / 2);
            Canvas.SetTop(title, 30);

            Label labelSize = new Label()
            {
                Width = 600,
                Height = 50,
                Content = "Taille de la map (base : x = 20 / y = 20) :",
                FontSize = 30,
                FontWeight = FontWeights.Bold,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            canvSettings.Children.Add(labelSize);
            Canvas.SetLeft(labelSize, 30);
            Canvas.SetTop(labelSize, Canvas.GetTop(title) + title.Height + 100);

            XValueText = new TextBox()
            {
                Width = 200,
                Height = 50,
                Text = "Multipliez la valeur X",
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0)),
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            XValueText.PreviewTextInput += TextInput;
            XValueText.GotFocus += GotFocusOnTextBox;
            canvSettings.Children.Add(XValueText);
            Canvas.SetLeft(XValueText, Canvas.GetLeft(labelSize) + labelSize.Width + 20);
            Canvas.SetTop(XValueText, Canvas.GetTop(labelSize));

            YValueText = new TextBox()
            {
                Width = 200,
                Height = 50,
                Text = "Multipliez la valeur Y",
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0)),
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            YValueText.PreviewTextInput += TextInput;
            YValueText.GotFocus += GotFocusOnTextBox;
            canvSettings.Children.Add(YValueText);
            Canvas.SetLeft(YValueText, Canvas.GetLeft(XValueText) + XValueText.Width + 20);
            Canvas.SetTop(YValueText, Canvas.GetTop(XValueText));

            ConfirmSizeButton = new Button() 
            {
                Width = 120,
                Height = 50,
                Content = "Confirmer",
                FontSize = 25
            };
            ConfirmSizeButton.Click += SetSize;
            canvSettings.Children.Add(ConfirmSizeButton);
            Canvas.SetLeft(ConfirmSizeButton, Canvas.GetLeft(YValueText) + YValueText.Width + 20);
            Canvas.SetTop(ConfirmSizeButton, Canvas.GetTop(YValueText));

            Label labelDifficulty = new Label()
            {
                Width = 500,
                Height = 50,
                Content = "Difficulté : ",
                FontSize = 30,
                FontWeight = FontWeights.Bold,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            canvSettings.Children.Add(labelDifficulty);
            Canvas.SetLeft(labelDifficulty, 30);
            Canvas.SetTop(labelDifficulty, Canvas.GetTop(labelSize) + labelSize.Height + 70);

            SliderDifficulty = new Slider()
            {
                Width = 500,
                Height = 50,
                Minimum = 2,
                Maximum = 10,
                TickFrequency = 1,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 10, 0, 0)
            };
            SliderDifficulty.ValueChanged += ChangeDifficulty;
            canvSettings.Children.Add(SliderDifficulty);
            Canvas.SetLeft(SliderDifficulty, Canvas.GetLeft(labelDifficulty) + labelDifficulty.Width + 20);
            Canvas.SetTop(SliderDifficulty, Canvas.GetTop(labelDifficulty));

            LabelShowDifficulty = new Label()
            {
                Width = 100,
                Height = 50,
                Content = "0",
                FontSize = 30,
                FontWeight = FontWeights.Bold,
                BorderBrush = Brushes.Black,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            canvSettings.Children.Add(LabelShowDifficulty);
            Canvas.SetLeft(LabelShowDifficulty, Canvas.GetLeft(SliderDifficulty) + SliderDifficulty.Width + 20);
            Canvas.SetTop(LabelShowDifficulty, Canvas.GetTop(labelDifficulty));

            Label labelExplainDifficulty = new Label()
            {
                Width = double.NaN,
                Height = 50,
                Content = "Nombre de mob qui spawn = numéro de la vague multiplié par la valeur du slider",
                Foreground = Brushes.LightGray,
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                BorderBrush = Brushes.Black,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            canvSettings.Children.Add(labelExplainDifficulty);
            Canvas.SetLeft(labelExplainDifficulty, Canvas.GetLeft(LabelShowDifficulty) + LabelShowDifficulty.Width + 20);
            Canvas.SetTop(labelExplainDifficulty, Canvas.GetTop(labelDifficulty));

            Label labelVolumeSound = new Label()
            {
                Width = 500,
                Height = 50,
                Content = "Volume des sons : ",
                FontSize = 30,
                FontWeight = FontWeights.Bold,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            canvSettings.Children.Add(labelVolumeSound);
            Canvas.SetLeft(labelVolumeSound, 30);
            Canvas.SetTop(labelVolumeSound, Canvas.GetTop(labelDifficulty) + labelDifficulty.Height + 70);

            SliderVolumeSound = new Slider()
            {
                Width = 500,
                Height = 50,
                Minimum = 0,
                Maximum = 100,
                TickFrequency = 1,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 10, 0, 0)
            };
            SliderVolumeSound.ValueChanged += ChangeVolume;
            canvSettings.Children.Add(SliderVolumeSound);
            Canvas.SetLeft(SliderVolumeSound, Canvas.GetLeft(labelVolumeSound) + labelVolumeSound.Width + 20);
            Canvas.SetTop(SliderVolumeSound, Canvas.GetTop(labelVolumeSound));

            LabelShowVolumeSound = new Label()
            {
                Width = 100,
                Height = 50,
                Content = "0",
                FontSize = 30,
                FontWeight = FontWeights.Bold,
                BorderBrush = Brushes.Black,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            canvSettings.Children.Add(LabelShowVolumeSound);
            Canvas.SetLeft(LabelShowVolumeSound, Canvas.GetLeft(SliderVolumeSound) + SliderVolumeSound.Width + 20);
            Canvas.SetTop(LabelShowVolumeSound, Canvas.GetTop(labelVolumeSound));

            Label labelCheat = new Label()
            {
                Width = 500,
                Height = 50,
                Content = "Entrer le code de triche : ",
                FontSize = 30,
                FontWeight = FontWeights.Bold,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            canvSettings.Children.Add(labelCheat);
            Canvas.SetLeft(labelCheat, 30);
            Canvas.SetTop(labelCheat, Canvas.GetTop(labelVolumeSound) + labelVolumeSound.Height + 70);

            PasswordCheat = new PasswordBox()
            {
                Width = 350,
                Height = 50,
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            canvSettings.Children.Add(PasswordCheat);
            Canvas.SetLeft(PasswordCheat, Canvas.GetLeft(labelCheat) + labelCheat.Width + 20);
            Canvas.SetTop(PasswordCheat, Canvas.GetTop(labelCheat));

            Button aboutButton = new Button()
            {
                Width = 100,
                Height = 75,
                Content = "A propos",
                FontSize = 20
            };
            aboutButton.Click += OpenHTP;
            canvSettings.Children.Add(aboutButton);
            Canvas.SetLeft(aboutButton, canvSettings.Width - aboutButton.Width - 10);
            Canvas.SetTop(aboutButton, canvSettings.Height - aboutButton.Height - 10);
        }

        private void ExitSettings(object sender, RoutedEventArgs e)
        {
            ConfirmSizeButton.Background = new SolidColorBrush(Color.FromRgb(221, 221, 221));
            if (AddMoneyButton != null)
            {
                AddMoneyButton.Background = new SolidColorBrush(Color.FromRgb(221, 221, 221));
            }
            Engine.Instance.PlaySoundButton();
            ((MainWindow)Application.Current.MainWindow).ExitSettings();
        }

        private void ExitGame(object sender, RoutedEventArgs e)
        {
            Engine.Instance.PlaySoundButton();
            ((MainWindow)Application.Current.MainWindow).ExitGame();
        }

        private void OpenHTP(object sender, RoutedEventArgs e)
        {
            Engine.Instance.PlaySoundButton();
            ((MainWindow)Application.Current.MainWindow).OpenHowToPlay();
        }

        private void canvSettings_KeyDown(object sender, KeyEventArgs e)        
        { 
            if (e.Key == Key.Enter)
            {
                if (PasswordCheat.Password == CODE)
                {
                    if (Separator == null)
                    {
                        GodModActivated();
                    } else
                    {
                        ShowGodMod();
                    }
                }
            }
        }

        private void GodModActivated()
        {
            Separator = new Rectangle()
            {
                Height = 8,
                Width = SystemParameters.PrimaryScreenWidth - 50,
                Fill = Brushes.Black,
                RadiusX = 10,
                RadiusY = 10
            };
            canvSettings.Children.Add(Separator);
            Canvas.SetLeft(Separator, 25);
            Canvas.SetTop(Separator, Canvas.GetTop(PasswordCheat) + PasswordCheat.Height + 70);

            LabelMoney = new Label()
            {
                Width = 500,
                Height = 50,
                Content = "Entrer une somme d'argent : ",
                FontSize = 30,
                FontWeight = FontWeights.Bold,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            canvSettings.Children.Add(LabelMoney);
            Canvas.SetLeft(LabelMoney, 30);
            Canvas.SetTop(LabelMoney, Canvas.GetTop(Separator) + Separator.Height + 70);

            MoneyText = new TextBox()
            {
                Width = 200,
                Height = 50,
                Text = "Argent à ajouter",
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0)),
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            MoneyText.GotFocus += GotFocusOnTextBox;
            canvSettings.Children.Add(MoneyText);
            Canvas.SetLeft(MoneyText, Canvas.GetLeft(LabelMoney) + LabelMoney.Width + 20);
            Canvas.SetTop(MoneyText, Canvas.GetTop(LabelMoney));

            AddMoneyButton = new Button()
            {
                Width = 120,
                Height = 50,
                Content = "Ajouter",
                FontSize = 25
            };
            AddMoneyButton.Click += AddMoney;
            canvSettings.Children.Add(AddMoneyButton);
            Canvas.SetLeft(AddMoneyButton, Canvas.GetLeft(MoneyText) + MoneyText.Width + 20);
            Canvas.SetTop(AddMoneyButton, Canvas.GetTop(MoneyText));

            TextInvincible = new Label()
            {
                Width = 500,
                Height = 50,
                Content = "Joueur invincible : ",
                FontSize = 30,
                FontWeight = FontWeights.Bold,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            canvSettings.Children.Add(TextInvincible);
            Canvas.SetLeft(TextInvincible, 30);
            Canvas.SetTop(TextInvincible, Canvas.GetTop(MoneyText) + MoneyText.Height + 70);

            CheckDamage = new CheckBox()
            {
                Content = "Permettre les dégats",
                RenderTransform = new ScaleTransform(3, 3),
                IsChecked = true,
            };
            CheckDamage.Click += ChangeCanDamagePlayer;
            canvSettings.Children.Add(CheckDamage);
            Canvas.SetLeft(CheckDamage, Canvas.GetLeft(TextInvincible) + TextInvincible.Width + 20);
            Canvas.SetTop(CheckDamage, Canvas.GetTop(TextInvincible));
        }

        public void ClearGodMod(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!this.IsVisible && Separator != null)
            {
                Separator.Visibility = Visibility.Hidden;
                LabelMoney.Visibility = Visibility.Hidden;
                MoneyText.Visibility = Visibility.Hidden;
                AddMoneyButton.Visibility = Visibility.Hidden;
                TextInvincible.Visibility = Visibility.Hidden;
                CheckDamage.Visibility = Visibility.Hidden;
                PasswordCheat.Password = "";
            }
        }

        public void ShowGodMod()
        {
            Separator.Visibility = Visibility.Visible;
            LabelMoney.Visibility = Visibility.Visible;
            MoneyText.Visibility = Visibility.Visible;
            AddMoneyButton.Visibility = Visibility.Visible;
            TextInvincible.Visibility = Visibility.Visible;
            CheckDamage.Visibility = Visibility.Visible;
        }

        public void AddMoney(object sender, RoutedEventArgs e)
        {
            int moneyToAdd;
            if (int.TryParse(MoneyText.Text, out moneyToAdd))
            {
                AddMoneyButton.Background = Brushes.LightGreen;
                Engine.Instance.Player.Money += moneyToAdd;
            }
        }
        private void SetSize(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(XValueText.Text, out Engine.xValue) && int.TryParse(YValueText.Text, out Engine.yValue))
            {
                ConfirmSizeButton.Background = Brushes.LightGreen;
            }
        }

        private void TextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        private void GotFocusOnTextBox(object sender, RoutedEventArgs e)
        {
            TextBox obj = sender as TextBox;
            obj.Text = "";
            obj.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
        }

        private void ChangeCanDamagePlayer(object sender, RoutedEventArgs e)
        {
            Engine.Instance.Player.BCanTakeDamage = (CheckDamage.IsChecked == true) ? true : false;
        }

        public void ChangeDifficulty(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int difficulty = (int)Math.Round(SliderDifficulty.Value);
            LabelShowDifficulty.Content = difficulty;
            Engine.Instance.MobSpawner.WaveMultiplier = difficulty;
        }

        public void ChangeVolume(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double volume = Math.Round(SliderVolumeSound.Value);
            LabelShowVolumeSound.Content = volume + "%";
            Engine.Instance.SoundVolume = volume;
            Engine.Instance.SetVolumeSound();
        }
    }
}