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

namespace DLCollection.MSvsCV
{
    public partial class SourceSelectorPage : Page
    {
        public event EventHandler<System.Drawing.Image> OnPhotoCaptured;

        private System.Drawing.Image _img;

        public SourceSelectorPage()
        {
            InitializeComponent();
            Loaded += (s, e) => OnLoaded();
        }

        private void OnLoaded()
        {
            cvCameraView.OnCapture += (s, e) => { _img = e; LoadRecoPage(); };
            cmdBrowse.Click += (s, e) =>
            {
                if (BrowseFile())
                {
                    LoadRecoPage();
                    (s as Button).Content = "Selected.";
                }
            };
        }

        private async void LoadRecoPage()
        {
            await Task.Delay(700);
            OnPhotoCaptured?.Invoke(this, _img);
        }

        public void KillCapture()
        {
            cvCameraView.KillCapture();
        }

        private bool BrowseFile()
        {
            var dialog = new CommonOpenFileDialog();
            CommonFileDialogResult result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Cancel || result == CommonFileDialogResult.None) return false;
            if (result == CommonFileDialogResult.Ok)
            {
                _img = System.Drawing.Image.FromFile(dialog.FileName);
                return true;
            }
            return false;
        }
    }
}
