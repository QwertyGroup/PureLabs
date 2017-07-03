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

namespace DLCollection.Face
{
    public partial class FaceMain : Window
    {
        public FaceMain()
        {
            InitializeComponent();
            Loaded += (s, e) => InitializeControls();
        }

        private void InitializeControls()
        {
            Topmost = false;

            grdContainer0.Children.Add(new DirectionViewer3D { Photo = DirectionViewer3D.LoadPhotoFromFile("Photos/Dasha.jpg") });
            //grdContainer1.Children.Add(new LoadingCIrcle());
            grdContainer2.Children.Add(new LoadingPicture { Padding = new Thickness(0, 15, 0, 30) });
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
            DragMove();
        }
    }
}
