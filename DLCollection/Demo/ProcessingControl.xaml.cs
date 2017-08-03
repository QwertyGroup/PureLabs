using Emgu.CV;
using Emgu.CV.WPF;
using Emgu.CV.Structure;

using Tools.Libs;

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
    public partial class ProcessingControl : UserControl
    {
        private SettingsControlProperties _settings;
        private VideoCapture _capture;
        private MotionDetection _motionDetection = new MotionDetection();
        private bool IsProcessing = false;

        public ProcessingControl()
        {
            InitializeComponent();
        }

        public void Start(SettingsControlProperties settings)
        {
            _settings = settings;
            InitializeCapture();
            _capture.ImageGrabbed += (s, e) => FindMotion();

            _capture.Start();
            IsProcessing = true;
        }

        private void FindMotion()
        {
            if (!IsProcessing) return;
            Mat frame = new Mat();
            _capture.Retrieve(frame);

            var rects = _motionDetection.Detect(frame);
            foreach (var rect in rects)
                CvInvoke.Rectangle(frame, rect, new Bgr(System.Drawing.Color.White).MCvScalar);

            Dispatcher.Invoke(() => { imgScreen.Source = BitmapSourceConvert.ToBitmapSource(frame); });
        }

        private void InitializeCapture()
        {
            if (_capture != null) _capture.Dispose();
            if (_settings.SourceType == SettingsControlProperties.Source.Capture)
                _capture = new VideoCapture();
            else if (_settings.SourceType == SettingsControlProperties.Source.Browse)
                _capture = new VideoCapture(_settings.BrowsePath);
            _capture.FlipHorizontal = !_capture.FlipHorizontal;
            _capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth, 1280);
            _capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight, 720);
        }

        public void Stop()
        {
            IsProcessing = false;
            _capture.Stop();
            _capture.Dispose();
        }
    }
}
