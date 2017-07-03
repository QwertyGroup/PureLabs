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
            grdContainer2.Children.Add(new LoadingPicture());
            grdContainer1.Children.Add(new LoadingCIrcle());
        }
    }
}
