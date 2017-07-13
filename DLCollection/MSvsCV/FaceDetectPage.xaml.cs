using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.WPF;

using FaceRecognition.Core;

using System;
using System.Collections.Generic;
using System.Drawing;
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
    public partial class FaceDetectPage : Page
    {
        public FaceDetectPage()
        {
            InitializeComponent();
        }

        private System.Drawing.Image _img;
        public FaceDetectPage(System.Drawing.Image img) : this()
        {
            _img = img;
            Loaded += (s, e) => OnLoaded();
        }

        private void OnLoaded()
        {
            ProcessImageMS();
            new Task(() => ProcessImagesEmguCv()).Start();
        }

        private void ProcessImagesEmguCv()
        {
            ProcessImageEmguCv("Cascades/haarcascade_frontalface_default.xml", imgRes2);
            ProcessImageEmguCv("Cascades/haarcascade_frontalface_alt.xml", imgRes3);
            ProcessImageEmguCv("Cascades/haarcascade_frontalface_alt2.xml", imgRes4);
        }

        private FaceRecognition.Core.EmguCvAPIs.FaceTools _faceTools =
             new FaceRecognition.Core.EmguCvAPIs.FaceTools();
        private void ProcessImageEmguCv(string cascadePath, System.Windows.Controls.Image imgRes)
        {
            var img = _img;
            var frame = _faceTools.ImageToMat(img);

            var cascade = new CascadeClassifier(cascadePath);
            var faces = _faceTools.FindObjByCascade(frame, cascade);
            DrawRects(faces, frame);

            Dispatcher.Invoke(() =>
            {
                imgRes.Source = BitmapSourceConvert.ToBitmapSource(frame);
            });
        }

        private async void ProcessImageMS()
        {
            var img = _img;

            var faces =
                await FaceRecognition.Core.MicrosoftAPIs.ComparationAPI.Commands.CommandsInstance.DetectFace(img);

            Mat frame = _faceTools.ImageToMat(img);
            DrawRects(faces.Select(f =>
            new System.Drawing.Rectangle(
                f.FaceRectangle.Left, f.FaceRectangle.Top, f.FaceRectangle.Width,
                f.FaceRectangle.Height)).ToList(), frame);

            Dispatcher.Invoke(() =>
            {
                imgRes1.Source = BitmapSourceConvert.ToBitmapSource(frame);
            });
        }

        private void DrawRects(List<System.Drawing.Rectangle> rects, Mat frame)
        {
            foreach (var rect in rects)
                CvInvoke.Rectangle(frame, rect, new Bgr(System.Drawing.Color.LimeGreen).MCvScalar, thickness: 3);
        }
    }
}
