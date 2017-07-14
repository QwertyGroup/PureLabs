using Emgu.CV;
using Emgu.CV.WPF;

using FaceRecognition.Core;

using System;
using System.Collections.Generic;
using System.Diagnostics;
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


namespace Tools.UI
{
    public partial class CameraV : UserControl
    {
        public event EventHandler<System.Drawing.Image> OnCapture;

        private VideoCapture _capt;
        private Mat _frame;
        private Mat _grabbedFrame;

        public CameraV()
        {
            InitializeComponent();

            cmdCapture.Visibility = Visibility.Hidden;
            imgPreview.Visibility = Visibility.Hidden;
            WaitCaptureLoad();

            Loaded += (s, e) => new Task(() => OnLoaded()).Start();

            OnCapture += (s, e) => KillCapture();
        }

        private void WaitCaptureLoad()
        {
            var tg = new TransformGroup();
            var rt = new RotateTransform()
            {
                Angle = -90
            };
            tg.Children.Add(rt);
            var lding = new LoadingPicture()
            {
                LayoutTransform = tg
            };
            grdMainContainer.Children.Add(lding);
        }

        private void OnLoaded()
        {
            _frame = new Mat();
            cmdCapture.Click += (s, e) =>
            {
                // This
                _grabbedFrame = new Mat();
                _capt.Grab();
                _capt.Retrieve(_grabbedFrame, 0);
                // Or this
                //_grabbedFrame = _capt.QueryFrame();

                if (!_grabbedFrame.IsEmpty)
                {
                    ImageProcessing.ImageProcessingInstance.SaveDirectory = "Gallery/";
                    ImageProcessing.ImageProcessingInstance.SaveImageToFile(new Func<string>(() =>
                    {
                        var files = ImageProcessing.ImageProcessingInstance.GetFilesInSaveDir(ImageProcessing.FileNameSetting.WithoutExtension);
                        if (files.Count == 0) return 0.ToString();
                        return (Convert.ToInt32(files.Last()) + 1).ToString();
                    }).Invoke(), _grabbedFrame.Bitmap, System.Drawing.Imaging.ImageFormat.Jpeg);

                    OnCapture?.Invoke(this, _grabbedFrame.Bitmap);
                }
            };

            try
            {
                _capt = new VideoCapture(0);

                _capt.ImageGrabbed += (s, e) =>
                {
                    if (_capt == null) return;
                    if (_capt.Ptr == IntPtr.Zero) return;
                    _capt.Retrieve(_frame, 0);
                    if (_frame.IsEmpty) return;
                    if (_frame.Bitmap == null) return;
                    //TryFindFace(_frame);
                    try
                    {
                        CvInvoke.PyrDown(_frame, _frame);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }

                    Dispatcher.Invoke(() =>
                    {
                        imgPreview.Source = BitmapSourceConvert.ToBitmapSource(_frame);
                    });
                };

                _capt.FlipHorizontal = !_capt.FlipHorizontal;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Err", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            //_capt.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps, 15); // Camera settings
            SetHighResolution();

            //_capt.Start();

            new Task(async () =>
            {
                await Task.Delay(1100);
                Dispatcher.Invoke(() =>
                {
                    grdMainContainer.Children.RemoveAt(grdMainContainer.Children.Count - 1);
                    imgPreview.Visibility = Visibility.Visible;
                    cmdCapture.Visibility = Visibility.Visible;
                });
            }).Start();
        }

        private int _framesConter;
        private void TryFindFace(Mat frame)
        {
            _framesConter++;
            if (_framesConter == 20)
            {
                _framesConter = 0;
                new Task(() => FindFace(frame)).Start();
                //FindFace(frame);
            }
        }

        private void FindFace(Mat frame)
        {
            if (frame.IsEmpty) return;
            var faceRects = new List<System.Drawing.Rectangle>();
            var eyeRects = new List<System.Drawing.Rectangle>();
            FaceDetection.DetectFace.Detect(_frame, "Cascades/haarcascade_frontalface_default.xml",
                "Cascades/haarcascade_eye.xml", faceRects, eyeRects, out long processTime);

            Dispatcher.Invoke(() =>
                {
                    if (faceRects.Count == 0) return;
                    cnvCanvas.Children.Clear();
                    foreach (var r in faceRects)
                        DrawRectOnCanvas(r, cnvCanvas.ActualWidth / frame.Width, cnvCanvas.ActualHeight / frame.Height);
                });
        }

        private void DrawRectOnCanvas(System.Drawing.Rectangle r, double widthCf, double heightCf)
        {
            var rect = new Rectangle()
            {
                Width = r.Width * widthCf,
                Height = r.Height * heightCf,
                StrokeThickness = 2,
                Stroke = new SolidColorBrush(Colors.Blue)
            };

            cnvCanvas.Children.Add(rect);
            Canvas.SetLeft(rect, r.Left * widthCf);
            Canvas.SetTop(rect, r.Top * heightCf);
        }

        private void SetLowResolution()
        {
            _capt.Stop();
            _capt.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth, 1280 / 2);
            _capt.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight, 720 / 2);
            _capt.Start();
        }

        private void SetHighResolution()
        {
            _capt.Stop();
            _capt.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth, 1920);
            _capt.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight, 1080);
            _capt.Start();
        }

        public void KillCapture()
        {
            if (_capt == null) return;
            _capt.Stop();
            _capt = null;
        }
    }
}
