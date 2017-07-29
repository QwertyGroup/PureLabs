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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DLCollection.Demo
{
    // TODO: FIX not valid data typed in (SettingsContol)
    public partial class CaptureViewerPage : Page
    {
        public CaptureViewerPage()
        {
            InitializeComponent();
            cSettings.OnNewSettingsVerified += (s, e) =>
            {
                lbDebug.Items.Add(e.CascadeType);

                lbDebug.Items.Add(e.SourceType);
                lbDebug.Items.Add(e.BrowsePath);

                lbDebug.Items.Add(e.ScaleFactor);
                lbDebug.Items.Add(e.MinNeighbors);

                lbDebug.Items.Add(e.MinSize);
                lbDebug.Items.Add(e.MaxSize);

                lbDebug.Items.Add(string.Empty);
            };
        }
    }
}
