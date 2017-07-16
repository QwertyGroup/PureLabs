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
using System.Windows.Shapes;

namespace DLCollection.Demo
{
    public partial class DemoMain : Window
    {
        public DemoMain()
        {
            InitializeComponent();
            Loaded += (s, e) => OnLoaded();
        }

        private void OnLoaded()
        {
            // Can overlap
            Topmost = false;
        }

        private void CmdExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void CmdBack_Click(object sender, RoutedEventArgs e)
        {
            var wind = new MainWindow() { Topmost = true };
            wind.Show();
            Close();
        }

        private void GrdActionBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
