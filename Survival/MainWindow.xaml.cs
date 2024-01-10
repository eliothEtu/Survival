using Survival.MalwareStuff;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Survival
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ForceFocus.EnableLock();
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
