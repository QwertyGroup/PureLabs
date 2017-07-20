using Microsoft.WindowsAPICodePack.Dialogs;

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
                ExpandSettings();
            };
        }

        private void CmdSettings_Click(object sender, RoutedEventArgs e)
        {
            ExpandSettings();
        }

        private void CollapseSettings()
        {
            cmdOpenSettings.Visibility = Visibility.Visible;
            cntSourceSelector.Visibility = Visibility.Collapsed;
            cntApplySettings.Visibility = Visibility.Collapsed;
            cntDetectMethodParams.Visibility = Visibility.Collapsed;
            tbSettingsTitle.Visibility = Visibility.Collapsed;
        }

        private void ExpandSettings()
        {
            _tempSettings = Settings;
            cmdOpenSettings.Visibility = Visibility.Collapsed;
            cntSourceSelector.Visibility = Visibility.Visible;
            cntApplySettings.Visibility = Visibility.Visible;
            cntDetectMethodParams.Visibility = Visibility.Visible;
            tbSettingsTitle.Visibility = Visibility.Visible;
        }

        private void CmdApply_Click(object sender, RoutedEventArgs e)
        {
            Settings = _tempSettings;
            CollapseSettings();
        }

        private void CmdD0_Click(object sender, RoutedEventArgs e)
        {
            _tempSettings.CascadeType = SettingsControlProperties.Cascade.Default;
            cmdD0.Style = FindResource("MainButtonInverseStyle") as Style;
            cmdA1.Style = FindResource("MainButtonStyle") as Style;
            cmdA2.Style = FindResource("MainButtonStyle") as Style;
        }

        private void CmdA1_Click(object sender, RoutedEventArgs e)
        {
            _tempSettings.CascadeType = SettingsControlProperties.Cascade.Alt1;
            cmdA1.Style = FindResource("MainButtonInverseStyle") as Style;
            cmdD0.Style = FindResource("MainButtonStyle") as Style;
            cmdA2.Style = FindResource("MainButtonStyle") as Style;
        }

        private void CmdA2_Click(object sender, RoutedEventArgs e)
        {
            _tempSettings.CascadeType = SettingsControlProperties.Cascade.Alt2;
            cmdA2.Style = FindResource("MainButtonInverseStyle") as Style;
            cmdD0.Style = FindResource("MainButtonStyle") as Style;
            cmdA1.Style = FindResource("MainButtonStyle") as Style;
        }

        private void CmdCancel_Click(object sender, RoutedEventArgs e)
        {
            CollapseSettings();
        }

        private void CmdBrowse_Click(object sender, RoutedEventArgs e)
        {
            _tempSettings.SourceType = SettingsControlProperties.Source.Browse;
            _tempSettings.BrowsePath = BrowseDialog();
            if (string.IsNullOrEmpty(_tempSettings.BrowsePath)) return;

            cmdBrowse.Style = FindResource("MainButtonInverseStyle") as Style;
            cmdBrowseIcon.Content = FindResource("ImageIconWhite") as Path;
            cmdBrowseText.Style = FindResource("MainTextBlockInverseStyle") as Style;

            cmdCapture.Style = FindResource("MainButtonStyle") as Style;
            cmdCaptureIcon.Content = FindResource("TakePhotoIconBlack") as Path;
            cmdCaptureText.Style = FindResource("MainTextBlockStyle") as Style;
        }

        private string BrowseDialog()
        {
            var dialog = new CommonOpenFileDialog();
            CommonFileDialogResult result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Cancel || result == CommonFileDialogResult.None) return null;
            if (result == CommonFileDialogResult.Ok) return dialog.FileName;
            return null;
        }

        private void CmdCapture_Click(object sender, RoutedEventArgs e)
        {
            _tempSettings.SourceType = SettingsControlProperties.Source.Capture;

            cmdCapture.Style = FindResource("MainButtonInverseStyle") as Style;
            cmdCaptureIcon.Content = FindResource("TakePhotoIconWhite") as Path;
            cmdCaptureText.Style = FindResource("MainTextBlockInverseStyle") as Style;

            cmdBrowse.Style = FindResource("MainButtonStyle") as Style;
            cmdBrowseIcon.Content = FindResource("ImageIconBlack") as Path;
            cmdBrowseText.Style = FindResource("MainTextBlockStyle") as Style;
        }

        public SettingsControlProperties Settings { get; set; } = new SettingsControlProperties();
        private SettingsControlProperties _tempSettings;
    }
}
