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
        }

        public void DashaTest()
        {
            // Eyes
            DrawVector((913.5, 414.7, 1), (1098.0, 459.6, 1), (9, 85, 237), 2);
            DrawVector((0, 0, 1), (PhotoWidth, PhotoHeight, 1), (140, 85, 237), 4);

            // Corners
            DrawVector((0, 0, 0), (0, 0, 50), (240, 85, 0), 2);
            DrawVector((PhotoWidth, PhotoHeight, 0), (PhotoWidth, PhotoHeight, 100), (240, 85, 0), 2);
            DrawVector((0, PhotoHeight, 0), (0, PhotoHeight, 50), (240, 85, 0), 2);
            DrawVector((PhotoWidth, 0, 0), (PhotoWidth, 0, 50), (240, 85, 0), 2);

            // Green PX x(W):400 y(H):800
            DrawVector((400, 800, 15), (400, 800, 150), (30, 255, 30), 3);
        }

        public void DrawPoint((double W, double H, double Depth) point_in_px, (byte R, byte G, byte B) rgbColor, int thickness = 2)
        {
            var W = point_in_px.W;
            var H = point_in_px.H;
            var Depth = point_in_px.Depth;

            DrawVector((W, H, Depth), (W, H, Depth + 6), rgbColor, thickness);                 // o - origin

            //DrawVector((W, H + 1, Depth), (W, H + 1, Depth + 6), rgbColor, thickness);         //  #
            //DrawVector((W - 1, H, Depth), (W - 1, H, Depth + 6), rgbColor, thickness);         // #o#
            //DrawVector((W + 1, H, Depth), (W + 1, H, Depth + 6), rgbColor, thickness);         //  #
            //DrawVector((W, H - 1, Depth), (W, H - 1, Depth + 6), rgbColor, thickness);

            //DrawVector((W - 1, H + 1, Depth), (W - 1, H + 1, Depth + 6), rgbColor, thickness); // #o#
            //DrawVector((W + 1, H + 1, Depth), (W + 1, H + 1, Depth + 6), rgbColor, thickness); // ooo
            //DrawVector((W - 1, H - 1, Depth), (W - 1, H - 1, Depth + 6), rgbColor, thickness); // #o#
            //DrawVector((W + 1, H - 1, Depth), (W + 1, H - 1, Depth + 6), rgbColor, thickness);

            var sqSide = 2;
            for (int i = -sqSide; i <= sqSide; i++) for (int j = -sqSide; j <= sqSide; j++)
                    DrawVector((W + i, H + j, Depth), (W + i, H + j, Depth + 6), rgbColor, thickness);
        }

        public void DrawVector((double W, double H, double Depth) point1_in_px, (double W, double H, double Depth) point2_in_px,
            (byte R, byte G, byte B) rgbColor, int thickness = 2) // In photo resoulution
        {
            // reverse X coord 
            point1_in_px.Depth = -point1_in_px.Depth;
            point2_in_px.Depth = -point2_in_px.Depth;

            // adjust Y pos (reverse) 
            point1_in_px.H = PhotoHeight - point1_in_px.H;
            point2_in_px.H = PhotoHeight - point2_in_px.H;

            // Making points 
            var collection = new List<Point3D>
            {
                new Point3D(ConvertPXto3dUnits(point1_in_px.Depth,Dimention.PhDepth),
                ConvertPXto3dUnits(point1_in_px.H,Dimention.PhHeight),
                ConvertPXto3dUnits(point1_in_px.W,Dimention.PhWidth)),

                new Point3D(ConvertPXto3dUnits(point2_in_px.Depth,Dimention.PhDepth),
                ConvertPXto3dUnits(point2_in_px.H,Dimention.PhHeight),
                ConvertPXto3dUnits(point2_in_px.W,Dimention.PhWidth)),
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
