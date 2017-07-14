using Emgu.CV;
using Emgu.CV.WPF;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceRecognition.Core.EmguCvAPIs
{
    public class FaceTools
    {
        public Mat ImageToMat(System.Drawing.Image img)
        {
            return BitmapSourceConvert.ToMat(
                ImageProcessing.ImageProcessingInstance.ConvertImageToBitmapImage(img));
        }

        public Image MatToImage(Mat mat)
        {
            return mat.Bitmap;
        }

        public TimeSpan LastFindObjProcessTime { get; set; } = TimeSpan.FromSeconds(0);
        private Stopwatch _sw = new Stopwatch();
        public List<Rectangle> FindObjByCascade(Mat inputImg, CascadeClassifier cascade,
            double scaleFactor = 1.02, int minNeighbors = 7,
            Size minSize = default(Size), Size maxSize = default(Size))
        {
            _sw.Reset();
            _sw.Start();
            var faces = new List<Rectangle>();

            using (cascade)
            {
                using (UMat ugray = new UMat())
                {
                    CvInvoke.CvtColor(inputImg, ugray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);

                    //normalizes brightness and increases contrast of the image
                    CvInvoke.EqualizeHist(ugray, ugray);

                    //Detect the faces  from the gray scale image and store the locations as rectangle
                    //The first dimensional is the channel
                    //The second dimension is the index of the rectangle in the specific channel                     
                    Rectangle[] facesDetected = cascade.DetectMultiScale(ugray, scaleFactor: scaleFactor,
                        minNeighbors: minNeighbors, minSize: minSize, maxSize: maxSize);

                    faces.AddRange(facesDetected);

                    // Detect Eyes (тут есть выделение региона)
                    //foreach (Rectangle f in facesDetected)
                    //{
                    //    //Get the region of interest on the faces
                    //    using (UMat faceRegion = new UMat(ugray, f))
                    //    {
                    //        Rectangle[] eyesDetected = eye.DetectMultiScale(
                    //           faceRegion,
                    //           1.1,
                    //           10,
                    //           new Size(20, 20));

                    //        foreach (Rectangle e in eyesDetected)
                    //        {
                    //            Rectangle eyeRect = e;
                    //            eyeRect.Offset(f.X, f.Y);
                    //            eyes.Add(eyeRect);
                    //        }
                    //    }
                    //}
                }
            }

            _sw.Stop();
            LastFindObjProcessTime = _sw.Elapsed;

            return faces;
        }
    }
}
