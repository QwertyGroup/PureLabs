using Tools.UI;

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

namespace DLCollection.MSvsCV
{
    public partial class MSvsCVMain : Window
    {
        public MSvsCVMain()
        {
            InitializeComponent();
            Loaded += (s, e) => OnLoaded();
        }

        SourceSelectorPage _selectorPage;
        private void OnLoaded()
        {
            // Can overlap
            Topmost = false;

            // Load selector page
            _selectorPage = new SourceSelectorPage();
            _selectorPage.OnPhotoCaptured += (s, e) => { Height = Height * 1.5; frMainFrame.Content = new FaceDetectPage(e); };
            frMainFrame.Content = _selectorPage;
        }

        private void CmdExit_Click(object sender, RoutedEventArgs e)
        {
            CmdBack_Click(null, null);
            Application.Current.Shutdown();
        }

        private void CmdBack_Click(object sender, RoutedEventArgs e)
        {
            _selectorPage.KillCapture();
            var wind = new MainWindow() { Topmost = true };
            wind.Show();
            Close();
        }

        private void GrdActionBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
