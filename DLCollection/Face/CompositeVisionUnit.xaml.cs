using Tools.UI;
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
using System.Diagnostics;
using Microsoft.ProjectOxford.Face.Contract;

namespace DLCollection.Face
{
    public partial class CompositeVisionUnit : UserControl
    {
        private Style _defaultButtonStyle;
        private string _photoPath;
        private DirectionViewer3D _viewer;
        private List<Microsoft.ProjectOxford.Face.Contract.Face> _msResult;

        public CompositeVisionUnit()
        {
            InitializeComponent();
            InitStyles();
            Loaded += (s, e) => ShowFileBrowseBtn();
        }

        private void InitStyles()
        {
            _defaultButtonStyle = new Style
            {
                BasedOn = (Style)FindResource("MainButtonStyle"),
                TargetType = typeof(Button)
            };

            _defaultButtonStyle.Setters.Add(
              new Setter { Property = VerticalAlignmentProperty, Value = VerticalAlignment.Center });
            _defaultButtonStyle.Setters.Add(
              new Setter { Property = HorizontalContentAlignmentProperty, Value = HorizontalAlignment.Center });
            _defaultButtonStyle.Setters.Add(
             new Setter { Property = VerticalAlignmentProperty, Value = VerticalAlignment.Center });
            _defaultButtonStyle.Setters.Add(
              new Setter { Property = HorizontalAlignmentProperty, Value = HorizontalAlignment.Center });
            _defaultButtonStyle.Setters.Add(
              new Setter { Property = FontSizeProperty, Value = 32d });
            _defaultButtonStyle.Setters.Add(
              new Setter { Property = PaddingProperty, Value = new Thickness(30, 10, 30, 13) });
        }

        private void ShowFileBrowseBtn()
        {
            var btn = new Button()
            {
                Content = "Browse photo...",
                Style = _defaultButtonStyle
            };

            btn.Click += (s, e) => BrowseFile();
            grdContainer.Children.Add(btn);
        }

        private void BrowseFile()
        {
            var dialog = new CommonOpenFileDialog();
            CommonFileDialogResult result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Cancel || result == CommonFileDialogResult.None) return;
            if (result == CommonFileDialogResult.Ok)
            {
                _photoPath = dialog.FileName;
                grdContainer.Children.Clear();
                var btn = new Button()
                {
                    Content = $"Start image{Environment.NewLine}processing.",
                    Style = _defaultButtonStyle
                };

                btn.Click += (s, e) => CalculateDirection();
                grdContainer.Children.Add(btn);
            }
        }

        private async void CalculateDirection()
        {
            // Loading 
            grdContainer.Children.Clear();
            grdContainer.Children.Add(new LoadingPicture { Padding = new Thickness(0, 15, 0, 30) });

            // Send MS request
            var image = DirectionViewer3D.LoadPhotoFromFile(_photoPath);
            _msResult = await FaceRecognition.Core.MicrosoftAPIs.ComparationAPI.
                Commands.CommandsInstance.DetectFaceWithLandmarks(image);
            try
            {
                FaceRecognition.Core.ImageProcessing.ImageProcessingInstance.ClearCache();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            // Select what to show
            // Show new seletion window

            // Show resutls
            grdContainer.Children.Clear();
            _viewer = new DirectionViewer3D()
            {
                Photo = image
            };
            grdContainer.Children.Add(_viewer);

            // Render 3d graphics
            Render3D();

        }

        private void Render3D()
        {
            if (_msResult == null) return;

            // Draw landmarks
            // if landmarks
            foreach (var face in _msResult)
                DrawLandmarks(face);

            // Draw face rect
            // if rect
            foreach (var face in _msResult)
                DrawFaceRectangle(face);

        }

        private void DrawFaceRectangle(Microsoft.ProjectOxford.Face.Contract.Face face)
        {
            //_viewer.DrawVector
        }

        private void DrawLandmarks(Microsoft.ProjectOxford.Face.Contract.Face face)
        {
            DrawLandmark(face.FaceLandmarks.EyebrowLeftInner);
            DrawLandmark(face.FaceLandmarks.EyebrowLeftOuter);
            DrawLandmark(face.FaceLandmarks.EyebrowRightInner);
            DrawLandmark(face.FaceLandmarks.EyebrowRightOuter);
            DrawLandmark(face.FaceLandmarks.EyeLeftBottom);
            DrawLandmark(face.FaceLandmarks.EyeLeftInner);
            DrawLandmark(face.FaceLandmarks.EyeLeftOuter);
            DrawLandmark(face.FaceLandmarks.EyeLeftTop);
            DrawLandmark(face.FaceLandmarks.EyeRightBottom);
            DrawLandmark(face.FaceLandmarks.EyeRightInner);
            DrawLandmark(face.FaceLandmarks.EyeRightOuter);
            DrawLandmark(face.FaceLandmarks.EyeRightTop);
            DrawLandmark(face.FaceLandmarks.MouthLeft);
            DrawLandmark(face.FaceLandmarks.MouthRight);
            DrawLandmark(face.FaceLandmarks.NoseLeftAlarOutTip);
            DrawLandmark(face.FaceLandmarks.NoseLeftAlarTop);
            DrawLandmark(face.FaceLandmarks.NoseRightAlarOutTip);
            DrawLandmark(face.FaceLandmarks.NoseRightAlarTop);
            DrawLandmark(face.FaceLandmarks.NoseRootLeft);
            DrawLandmark(face.FaceLandmarks.NoseRootRight);
            DrawLandmark(face.FaceLandmarks.NoseTip);
            DrawLandmark(face.FaceLandmarks.PupilLeft);
            DrawLandmark(face.FaceLandmarks.PupilRight);
            DrawLandmark(face.FaceLandmarks.UnderLipBottom);
            DrawLandmark(face.FaceLandmarks.UnderLipTop);
            DrawLandmark(face.FaceLandmarks.UpperLipBottom);
            DrawLandmark(face.FaceLandmarks.UpperLipTop);
        }

        private void DrawLandmark(Microsoft.ProjectOxford.Face.Contract.FeatureCoordinate point)
        {
            if (point == null) return;
            _viewer.DrawPoint2D((point.X, point.Y), (255, 255, 255), radius: 5);
        }
    }
}
