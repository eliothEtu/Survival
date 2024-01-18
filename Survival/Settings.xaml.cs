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

        Button exit;
        TextBox xValueText, yValueText;
        Button confirmSizeButton;
        Slider sliderDifficulty;
        Label labelShowDifficulty;
        PasswordBox passwordCheat;

        //God Mod UI
        Rectangle separator;
        Label labelMoney;
        TextBox moneyText;
        Label texInvicible;
        CheckBox checkDamage;

        public Button Exit { get => exit; set => exit = value; }

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
                Width = 100,
                Height = 100,
                Content = "Exit",
                FontSize = 25
            };
            Exit.Click += ExitSettings;
            canvSettings.Children.Add(Exit);
            Canvas.SetLeft(Exit, canvSettings.Width - Exit.Width - 10);
            Canvas.SetTop(Exit, 10);

            Button exitGame = new Button()
            {
                Width = 150,
                Height = 100,
                Content = "Exit Game",
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
                Content = "Settings",
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
                Width = 500,
                Height = 50,
                Content = "Size map (base : x = 10 / y = 10) :",
                FontSize = 30,
                FontWeight = FontWeights.Bold,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            canvSettings.Children.Add(labelSize);
            Canvas.SetLeft(labelSize, 30);
            Canvas.SetTop(labelSize, Canvas.GetTop(title) + title.Height + 100);

            xValueText = new TextBox()
            {
                Width = 200,
                Height = 50,
                Text = "Multiply X Value",
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0)),
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            xValueText.PreviewTextInput += TextInput;
            xValueText.GotFocus += GotFocusOnTextBox;
            canvSettings.Children.Add(xValueText);
            Canvas.SetLeft(xValueText, Canvas.GetLeft(labelSize) + labelSize.Width + 20);
            Canvas.SetTop(xValueText, Canvas.GetTop(labelSize));

            yValueText = new TextBox()
            {
                Width = 200,
                Height = 50,
                Text = "Multiply Y Value",
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0)),
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            yValueText.PreviewTextInput += TextInput;
            yValueText.GotFocus += GotFocusOnTextBox;
            canvSettings.Children.Add(yValueText);
            Canvas.SetLeft(yValueText, Canvas.GetLeft(xValueText) + xValueText.Width + 20);
            Canvas.SetTop(yValueText, Canvas.GetTop(xValueText));

            confirmSizeButton = new Button()
            {
                Width = 100,
                Height = 50,
                Content = "Confirm Size",
                FontSize = 25
            };
            confirmSizeButton.Click += SetSize;
            canvSettings.Children.Add(confirmSizeButton);
            Canvas.SetLeft(confirmSizeButton, Canvas.GetLeft(yValueText) + yValueText.Width + 20);
            Canvas.SetTop(confirmSizeButton, Canvas.GetTop(yValueText));

            Label labelDifficulty = new Label()
            {
                Width = 500,
                Height = 50,
                Content = "Difficulty : ",
                FontSize = 30,
                FontWeight = FontWeights.Bold,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            canvSettings.Children.Add(labelDifficulty);
            Canvas.SetLeft(labelDifficulty, 30);
            Canvas.SetTop(labelDifficulty, Canvas.GetTop(labelSize) + labelSize.Height + 70);

            sliderDifficulty = new Slider()
            {
                Width = 500,
                Height = 50,
                Minimum = 2,
                Maximum = 10,
                TickFrequency = 1,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 10, 0, 0)
        };
            sliderDifficulty.ValueChanged += ChangeDifficulty;
            canvSettings.Children.Add(sliderDifficulty);
            Canvas.SetLeft(sliderDifficulty, Canvas.GetLeft(labelDifficulty) + labelDifficulty.Width + 20);
            Canvas.SetTop(sliderDifficulty, Canvas.GetTop(labelDifficulty));

            labelShowDifficulty = new Label()
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
            canvSettings.Children.Add(labelShowDifficulty);
            Canvas.SetLeft(labelShowDifficulty, Canvas.GetLeft(sliderDifficulty) + sliderDifficulty.Width + 20);
            Canvas.SetTop(labelShowDifficulty, Canvas.GetTop(labelDifficulty));

            Label labelCheat = new Label()
            {
                Width = 500,
                Height = 50,
                Content = "Enter Cheat Code : ",
                FontSize = 30,
                FontWeight = FontWeights.Bold,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            canvSettings.Children.Add(labelCheat);
            Canvas.SetLeft(labelCheat, 30);
            Canvas.SetTop(labelCheat, Canvas.GetTop(sliderDifficulty) + sliderDifficulty.Height + 70);

            passwordCheat = new PasswordBox()
            {
                Width = 350,
                Height = 50,
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            canvSettings.Children.Add(passwordCheat);
            Canvas.SetLeft(passwordCheat, Canvas.GetLeft(labelCheat) + labelCheat.Width + 20);
            Canvas.SetTop(passwordCheat, Canvas.GetTop(labelCheat));
        }

        private void ExitSettings(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).ExitSettings();
        }

        private void ExitGame(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).ExitGame();
        }

        private void SetSize(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(xValueText.Text, out Engine.xValue) && int.TryParse(xValueText.Text, out Engine.xValue))
            {
                confirmSizeButton.Background = Brushes.LightGreen;
            }
        }

        private void TextInput(object sender, TextCompositionEventArgs e)
        {
            // Vérifier si le texte saisi est un chiffre
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true; // Annuler l'événement si ce n'est pas un chiffre
            }
        }

        private void canvSettings_KeyDown(object sender, KeyEventArgs e)
        {
            if (((MainWindow)Application.Current.MainWindow).bSettings == true && e.Key == Key.Escape)
            {
                ((MainWindow)Application.Current.MainWindow).CloseSettingsInGame();
            }

            if (e.Key == Key.Enter)
            {
                if (passwordCheat.Password == CODE)
                {
                    if (separator == null)
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
            separator = new Rectangle()
            {
                Height = 8,
                Width = SystemParameters.PrimaryScreenWidth - 50,
                Fill = Brushes.Black,
                RadiusX = 10,
                RadiusY = 10
            };
            canvSettings.Children.Add(separator);
            Canvas.SetLeft(separator, 25);
            Canvas.SetTop(separator, Canvas.GetTop(passwordCheat) + passwordCheat.Height + 70);

            labelMoney = new Label()
            {
                Width = 500,
                Height = 50,
                Content = "Enter a amount of money : ",
                FontSize = 30,
                FontWeight = FontWeights.Bold,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            canvSettings.Children.Add(labelMoney);
            Canvas.SetLeft(labelMoney, 30);
            Canvas.SetTop(labelMoney, Canvas.GetTop(separator) + separator.Height + 70);

            moneyText = new TextBox()
            {
                Width = 200,
                Height = 50,
                Text = "Money To Add",
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0)),
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            moneyText.GotFocus += GotFocusOnTextBox;
            canvSettings.Children.Add(moneyText);
            Canvas.SetLeft(moneyText, Canvas.GetLeft(labelMoney) + labelMoney.Width + 20);
            Canvas.SetTop(moneyText, Canvas.GetTop(labelMoney));
            
            texInvicible = new Label()
            {
                Width = 500,
                Height = 50,
                Content = "Player Can Get Damage : ",
                FontSize = 30,
                FontWeight = FontWeights.Bold,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            canvSettings.Children.Add(texInvicible);
            Canvas.SetLeft(texInvicible, 30);
            Canvas.SetTop(texInvicible, Canvas.GetTop(moneyText) + moneyText.Height + 70);

            checkDamage = new CheckBox()
            {
                Content = "Allow Damage",
                RenderTransform = new ScaleTransform(3, 3),
                IsChecked = true,
            };
            checkDamage.Click += ChangeCanDamagePlayer;
            canvSettings.Children.Add(checkDamage);
            Canvas.SetLeft(checkDamage, Canvas.GetLeft(texInvicible) + texInvicible.Width + 20);
            Canvas.SetTop(checkDamage, Canvas.GetTop(texInvicible));
        }

        public void ClearGodMod(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!this.IsVisible && separator != null)
            {
                separator.Visibility = Visibility.Hidden;
                labelMoney.Visibility = Visibility.Hidden;
                moneyText.Visibility = Visibility.Hidden;
                texInvicible.Visibility = Visibility.Hidden;
                checkDamage.Visibility = Visibility.Hidden;
                passwordCheat.Password = "";
            }
        }

        public void ShowGodMod()
        {
            separator.Visibility = Visibility.Visible;
            labelMoney.Visibility = Visibility.Visible;
            moneyText.Visibility = Visibility.Visible;
            texInvicible.Visibility = Visibility.Visible;
            checkDamage.Visibility = Visibility.Visible;
        }

        private void GotFocusOnTextBox(object sender, RoutedEventArgs e)
        {
            TextBox obj = sender as TextBox;
            obj.Text = "";
            obj.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
        }

        private void ChangeCanDamagePlayer(object sender, RoutedEventArgs e)
        {
            Engine.Instance.Player.BCanTakeDamage = (checkDamage.IsChecked == true) ? true : false;
        }

        public void ChangeDifficulty(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int difficulty = (int)Math.Round(sliderDifficulty.Value);
            labelShowDifficulty.Content = difficulty;
            //Engine.Instance.MobSpawner.WaveMultiplier = value;
        }
    }
}