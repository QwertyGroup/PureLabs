using _3DTools;
using FaceRecognition.Core;

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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DLCollection.Face
{
    public partial class DirectionViewer3D : UserControl
    {
        public System.Drawing.Image Photo { get; set; }
        public double PhotoWidth { get { return Photo.Width; } }
        public double PhotoHeight { get { return Photo.Height; } }

        public DirectionViewer3D()
        {
            InitializeComponent();
            Loaded += (s, e) => OnLoaded();
        }

        public static System.Drawing.Image LoadPhotoFromFile(string path)
        {
            return System.Drawing.Image.FromFile(path);
        }

        private void OnLoaded()
        {
            ShowPhoto();
        }

        public void ShowPhoto()
        {
            if (Photo != null)
                imgPhoto.Source = ImageProcessing.ImageProcessingInstance.ConvertImageToBitmapImage(Photo);

            DrawVector((414.7, 913.5, 1), (459.6, 1098.0, 1), (9, 85, 237), 4);
            DrawVector((0, 0, 1), (PhotoWidth, PhotoHeight, 1), (140, 85, 237), 4);
            DrawVector((0, 0, 0), (0, 0, 100), (240, 85, 0), 2);
        }

        public void DrawVector((double W, double H, double Depth) point1, (double W, double H, double Depth) point2,
            (byte R, byte G, byte B) rgbColor, int thickness = 2) // In photo resoulution
        {
            // reverse X coord 
            point1.Depth = -point1.Depth;
            point2.Depth = -point2.Depth;

            // adjust Y pos (reverse) 
            point1.W = PhotoWidth - point1.W;
            point2.W = PhotoWidth - point2.W;

            // Making points 
            var collection = new List<Point3D>
            {
                new Point3D(ConvertPXto3dUnits(point1.Depth,Dimention.PhDepth),
                ConvertPXto3dUnits(point1.W,Dimention.PhWidth),
                ConvertPXto3dUnits(point1.H,Dimention.PhHeight)),

                new Point3D(ConvertPXto3dUnits(point2.Depth,Dimention.PhDepth),
                ConvertPXto3dUnits(point2.W,Dimention.PhWidth),
                ConvertPXto3dUnits(point2.H,Dimention.PhHeight)),
            };

            // Drawing vector
            vp3dViewPort.Children.Add(new ScreenSpaceLines3D
            {
                Thickness = thickness,
                Color = Color.FromRgb(rgbColor.R, rgbColor.G, rgbColor.B),
                Points = new Point3DCollection(collection)
            });
        }

        public enum Dimention { PhWidth, PhHeight, PhDepth }
        private double ConvertPXto3dUnits(double val, Dimention dim)
        {
            var result = 0d;
            switch (dim)
            {
                case Dimention.PhWidth:
                    result = 10 * val / PhotoWidth;
                    result -= 5;
                    break;
                case Dimention.PhHeight:
                    result = 10 * val / PhotoHeight;
                    result -= 5;
                    break;
                case Dimention.PhDepth:
                    result = 10 * val / ((PhotoWidth + PhotoHeight) / 2);
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}
