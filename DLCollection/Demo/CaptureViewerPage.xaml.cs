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

            //cSettings.OnNewSettingsVerified += (s, e) =>
            //{
            //    if (e.Result != SettingsControl.SettingsReturnResult.Apply) return;

            //    lbDebug.Items.Add(e.Settings.CascadeType);

            //    lbDebug.Items.Add(e.Settings.SourceType);
            //    lbDebug.Items.Add(e.Settings.BrowsePath);

            //    lbDebug.Items.Add(e.Settings.ScaleFactor);
            //    lbDebug.Items.Add(e.Settings.MinNeighbors);

            //    lbDebug.Items.Add(e.Settings.MinSize);
            //    lbDebug.Items.Add(e.Settings.MaxSize);

            //    lbDebug.Items.Add(string.Empty);
            //};

            cSettings.OnNewSettingsVerified += (s, e) =>
            {
                cProcessing.Start(e.Settings);
            };

            cSettings.OnSettingsOpened += (s, e) =>
            {
                cProcessing.Stop();
            };
        }
    }
}
