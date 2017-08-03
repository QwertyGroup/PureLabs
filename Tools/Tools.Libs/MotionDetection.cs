using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.VideoSurveillance;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Libs
{
    public class MotionDetection
    {
        private MotionHistory _motionHistory;
        private BackgroundSubtractor _foregroundDetector;
        private Mat _segMask = new Mat();
        private Mat _foregroundMask = new Mat();

        public MotionDetection()
        {
            _motionHistory = new MotionHistory(1.0, 0.05, 0.5);
        }

        public List<Rectangle> Detect(Mat image) // earlier retrieved image
        {
            if (_foregroundDetector == null)
            {
                _foregroundDetector = new BackgroundSubtractorMOG2();
            }

            _foregroundDetector.Apply(image, _foregroundMask);

            //update the motion history
            _motionHistory.Update(_foregroundMask);

            #region get a copy of the motion mask and enhance its color
            _motionHistory.Mask.MinMax(out double[] minValues, out double[] maxValues,
                out Point[] minLoc, out Point[] maxLoc);
            Mat motionMask = new Mat();
            using (ScalarArray sa = new ScalarArray(255.0 / maxValues[0]))
                CvInvoke.Multiply(_motionHistory.Mask, sa, motionMask, 1, DepthType.Cv8U);
            //Image<Gray, Byte> motionMask = _motionHistory.Mask.Mul(255.0 / maxValues[0]);
            #endregion

            //create the motion image 
            Mat motionImage = new Mat(motionMask.Size.Height, motionMask.Size.Width, DepthType.Cv8U, 3);
            motionImage.SetTo(new MCvScalar(0));
            //display the motion pixels in blue (first channel)
            //motionImage[0] = motionMask;
            CvInvoke.InsertChannel(motionMask, motionImage, 0);

            //Threshold to define a motion area, reduce the value to detect smaller motion
            double minArea = 50;

            //storage.Clear(); //clear the storage
            Rectangle[] rects;
            using (VectorOfRect boundingRect = new VectorOfRect())
            {
                _motionHistory.GetMotionComponents(_segMask, boundingRect);
                rects = boundingRect.ToArray();
            }

            var resultRects = new List<Rectangle>();
            //iterate through each of the motion component
            foreach (Rectangle comp in rects)
            {
                int area = comp.Width * comp.Height;
                //reject the components that have small area;
                //if (area < minArea) continue;

                resultRects.Add(comp);
                // find the angle and motion pixel count of the specific area
                //double angle, motionPixelCount;
                //_motionHistory.MotionInfo(_foregroundMask, comp, out angle, out motionPixelCount);

                ////reject the area that contains too few motion
                //if (motionPixelCount < area * 0.05) continue;

                ////Draw each individual motion in red
                ////DrawMotion(motionImage, comp, angle, new Bgr(Color.Red));
            }

            return resultRects;

            //// find and draw the overall motion angle
            //double overallAngle, overallMotionPixelCount;

            //_motionHistory.MotionInfo(_foregroundMask, new Rectangle(Point.Empty, motionMask.Size), out overallAngle, out overallMotionPixelCount);
            //DrawMotion(motionImage, new Rectangle(Point.Empty, motionMask.Size), overallAngle, new Bgr(Color.Green));

            //if (this.Disposing || this.IsDisposed)
            //    return;

            //capturedImageBox.Image = image;
            //forgroundImageBox.Image = _foregroundMask;

            ////Display the amount of motions found on the current image
            //UpdateText(String.Format("Total Motions found: {0}; Motion Pixel count: {1}", rects.Length, overallMotionPixelCount));

            ////Display the image of the motion
            //motionImageBox.Image = motionImage;

        }
    }
}
