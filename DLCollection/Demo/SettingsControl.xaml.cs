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
    public partial class SettingsControl : UserControl
    {
        public SettingsControl()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                brdSettings.Visibility = Visibility.Visible;
                cntSourceSelector.Visibility = Visibility.Collapsed;
                cntApplySettings.Visibility = Visibility.Collapsed;
                cntDetectMethodParams.Visibility = Visibility.Collapsed;
            };
        }

        private void CmdSettings_Click(object sender, RoutedEventArgs e)
        {
            brdSettings.Visibility = Visibility.Collapsed;
            cntSourceSelector.Visibility = Visibility.Visible;
            cntApplySettings.Visibility = Visibility.Visible;
            cntDetectMethodParams.Visibility = Visibility.Visible;
        }
    }
}
