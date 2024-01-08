using Survival.GameEngine;
using System;
using System.Collections.Generic;
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
        public static Canvas canvas;

        public MapGenerator map = new MapGenerator();
        public MainWindow()
        {
            InitializeComponent();

            WindowState = WindowState.Maximized;

            canv.Width = SystemParameters.FullPrimaryScreenWidth;
            canv.Height = SystemParameters.FullPrimaryScreenHeight;

            Console.WriteLine(canv.Width + " " + canv.Height); 

            canvas = canv;
            map.CreateMap();
            map.SmoothMap(5);
            map.ShowMap();
        }
    }
}
