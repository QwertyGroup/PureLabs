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
        // ctor
        public SettingsControl()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                Settings.ScaleFactor = ScaleFactorUI;
                Settings.MinNeighbors = MinNeighborsUI;
                Settings.MinSize = MinSizeUI;
                Settings.MaxSize = MaxSizeUI;

                Settings.CascadeType = SettingsControlProperties.Cascade.Default;
                HighlightDefaultCascade();

                Settings.SourceType = SettingsControlProperties.Source.Capture;
                HighlightCapture();

                ExpandSettings();
            };
        }

        // UI methods
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
            _tempSettings = Settings.Copy();
            cmdOpenSettings.Visibility = Visibility.Collapsed;
            cntSourceSelector.Visibility = Visibility.Visible;
            cntApplySettings.Visibility = Visibility.Visible;
            cntDetectMethodParams.Visibility = Visibility.Visible;
            tbSettingsTitle.Visibility = Visibility.Visible;
        }

        private void CmdApply_Click(object sender, RoutedEventArgs e)
        {
            _tempSettings.ScaleFactor = ScaleFactorUI;
            _tempSettings.MinNeighbors = MinNeighborsUI;
            _tempSettings.MinSize = MinSizeUI;
            _tempSettings.MaxSize = MaxSizeUI;
            Settings = _tempSettings;
            CollapseSettings();
            OnNewSettingsVerified?.Invoke(this, Settings);
        }

        private void CmdD0_Click(object sender, RoutedEventArgs e)
        {
            _tempSettings.CascadeType = SettingsControlProperties.Cascade.Default;
            HighlightDefaultCascade();
        }

        private void HighlightDefaultCascade()
        {
            cmdD0.Style = FindResource("MainButtonInverseStyle") as Style;
            cmdA1.Style = FindResource("MainButtonStyle") as Style;
            cmdA2.Style = FindResource("MainButtonStyle") as Style;
        }

        private void CmdA1_Click(object sender, RoutedEventArgs e)
        {
            _tempSettings.CascadeType = SettingsControlProperties.Cascade.Alt1;
            HighlightAlt1Cascade();
        }

        private void HighlightAlt1Cascade()
        {
            cmdA1.Style = FindResource("MainButtonInverseStyle") as Style;
            cmdD0.Style = FindResource("MainButtonStyle") as Style;
            cmdA2.Style = FindResource("MainButtonStyle") as Style;
        }

        private void CmdA2_Click(object sender, RoutedEventArgs e)
        {
            _tempSettings.CascadeType = SettingsControlProperties.Cascade.Alt2;
            HighlightAlt2Cascade();
        }

        private void HighlightAlt2Cascade()
        {
            cmdA2.Style = FindResource("MainButtonInverseStyle") as Style;
            cmdD0.Style = FindResource("MainButtonStyle") as Style;
            cmdA1.Style = FindResource("MainButtonStyle") as Style;
        }

        private void CmdCancel_Click(object sender, RoutedEventArgs e)
        {
            ScaleFactorUI = Settings.ScaleFactor;
            MinNeighborsUI = Settings.MinNeighbors;
            MinSizeUI = Settings.MinSize;
            MaxSizeUI = Settings.MaxSize;

            switch (Settings.CascadeType)
            {
                case SettingsControlProperties.Cascade.None:
                    break;
                case SettingsControlProperties.Cascade.Default:
                    HighlightDefaultCascade();
                    break;
                case SettingsControlProperties.Cascade.Alt1:
                    HighlightAlt1Cascade();
                    break;
                case SettingsControlProperties.Cascade.Alt2:
                    HighlightAlt2Cascade();
                    break;
                default:
                    break;
            }

            switch (Settings.SourceType)
            {
                case SettingsControlProperties.Source.None:
                    break;
                case SettingsControlProperties.Source.Capture:
                    HighlightCapture();
                    break;
                case SettingsControlProperties.Source.Browse:
                    HighlightBrowse();
                    break;
                default:
                    break;
            }

            CollapseSettings();
        }

        private void CmdBrowse_Click(object sender, RoutedEventArgs e)
        {
            _tempSettings.BrowsePath = BrowseDialog();
            if (string.IsNullOrEmpty(_tempSettings.BrowsePath)) return;
            _tempSettings.SourceType = SettingsControlProperties.Source.Browse;
            HighlightBrowse();
        }

        private void HighlightBrowse()
        {
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
            _tempSettings.BrowsePath = string.Empty;
            HighlightCapture();
        }

        private void HighlightCapture()
        {
            cmdCapture.Style = FindResource("MainButtonInverseStyle") as Style;
            cmdCaptureIcon.Content = FindResource("TakePhotoIconWhite") as Path;
            cmdCaptureText.Style = FindResource("MainTextBlockInverseStyle") as Style;

            cmdBrowse.Style = FindResource("MainButtonStyle") as Style;
            cmdBrowseIcon.Content = FindResource("ImageIconBlack") as Path;
            cmdBrowseText.Style = FindResource("MainTextBlockStyle") as Style;
        }

        // props/fields
        public SettingsControlProperties Settings { get; set; } = new SettingsControlProperties();
        private SettingsControlProperties _tempSettings;

        // events
        public event EventHandler<SettingsControlProperties> OnNewSettingsVerified;

        // dps
        public double ScaleFactorUI
        {
            get { return (double)GetValue(ScaleFactorUIProperty); }
            set { SetValue(ScaleFactorUIProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ScaleFactorUI.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScaleFactorUIProperty =
            DependencyProperty.Register("ScaleFactorUI", typeof(double), typeof(SettingsControl), new PropertyMetadata(1.02));



        public int MinNeighborsUI
        {
            get { return (int)GetValue(MinNeighborsUIProperty); }
            set { SetValue(MinNeighborsUIProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinNeighborsUI.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinNeighborsUIProperty =
            DependencyProperty.Register("MinNeighborsUI", typeof(int), typeof(SettingsControl), new PropertyMetadata(7));


        public System.Drawing.Size MinSizeUI
        {
            get { return (System.Drawing.Size)GetValue(MinSizeUIProperty); }
            set { SetValue(MinSizeUIProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinSizeUI.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinSizeUIProperty =
            DependencyProperty.Register("MinSizeUI", typeof(System.Drawing.Size), typeof(SettingsControl), new PropertyMetadata(new System.Drawing.Size(20, 20)));



        public System.Drawing.Size MaxSizeUI
        {
            get { return (System.Drawing.Size)GetValue(MaxSIzeUIProperty); }
            set { SetValue(MaxSIzeUIProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxSIzeUI.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxSIzeUIProperty =
            DependencyProperty.Register("MaxSizeUI", typeof(System.Drawing.Size), typeof(SettingsControl), new PropertyMetadata(new System.Drawing.Size(50, 50)));


    }
}
