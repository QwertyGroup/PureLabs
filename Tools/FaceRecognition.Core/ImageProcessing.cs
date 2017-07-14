using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

using BigDataCore;

namespace FaceRecognition.Core
{
    public class ImageProcessing
    {
        private ImageProcessing() { }
        private static Lazy<ImageProcessing> _imageProcessingInstance = new Lazy<ImageProcessing>(() => new ImageProcessing());
        public static ImageProcessing ImageProcessingInstance { get { return _imageProcessingInstance.Value; } }

        private Bitmap ConvertImageToBitmap(Image img)
        {
            return new Bitmap(img);
        }

        private Image ConvertBitmapToImage(Bitmap bm)
        {
            return bm;
        }

        private BitmapImage ConvertBitmapToBitmapImage(Bitmap bm)
        {
            var memstr = new MemoryStream();
            bm.Save(memstr, ImageFormat.Png);
            var bitmapImg = new BitmapImage();
            bitmapImg.BeginInit();
            memstr.Seek(0, SeekOrigin.Begin);
            bitmapImg.StreamSource = memstr;
            bitmapImg.EndInit();
            return bitmapImg;
        }

        public BitmapImage ConvertImageToBitmapImage(Image img)
        {
            return ConvertBitmapToBitmapImage(ConvertImageToBitmap(img));
        }

        private Bitmap ConvertBitmapImagetoBitmap(BitmapImage bitmapImg)
        {
            using (MemoryStream memstr = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImg));
                enc.Save(memstr);
                Bitmap bitmap = new Bitmap(memstr);
                return new Bitmap(bitmap);
            }
        }

        private Bitmap ConvertCroppedBitmapToBitmap(CroppedBitmap croppedImg)
        {
            using (MemoryStream memstr = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(croppedImg));
                enc.Save(memstr);
                Bitmap bitmap = new Bitmap(memstr);
                return new Bitmap(bitmap);
            }
        }

        public Image ConvertBitmapImageToImage(BitmapImage bitmapImg)
        {
            return ConvertBitmapToImage(ConvertBitmapImagetoBitmap(bitmapImg));
        }

        private string _saveDirectory = string.Empty;
        public string SaveDirectory { get => _saveDirectory; set => _saveDirectory = value.CheckDirectory(); }
        public string SaveImageToFile(string filename, Image img)
        {
            return SaveImageToFile(filename, img, img.RawFormat);
        }

        public string SaveImageToFile(string filename, Image img, ImageFormat imgformat)
        {
            var fullpath = UnitePathWithDir(filename) + GetExtention(imgformat);
            img.Save(fullpath, imgformat);
            return fullpath;
        }

        private string GetExtention(ImageFormat imgformat)
        {
            try
            {
                return ImageCodecInfo.GetImageEncoders()
                    .First(x => x.FormatID == imgformat.Guid)
                    .FilenameExtension
                    .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .First()
                    .Trim('*')
                    .ToLower();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return ".unknown";
            }
        }

        private string UnitePathWithDir(string filename)
        {
            if (SaveDirectory != string.Empty)
            {
                if (!Directory.Exists(SaveDirectory))
                    Directory.CreateDirectory(SaveDirectory);
                filename = SaveDirectory + @"\" + filename;
            }
            return filename;
        }

        public Image LoadImageFromFile(string filename)
        {
            filename = UnitePathWithDir(filename);
            return Image.FromFile(filename);
        }

        public Stream ImageToStream(Image img, ImageFormat imgformat)
        {
            var guid = Guid.NewGuid();
            var oldDir = SaveDirectory;
            SaveDirectory = _cacheDir;
            var path = SaveImageToFile(guid.ToString(), img);
            var fs = File.OpenRead(path);
            SaveDirectory = oldDir;
            return fs;
        }

        public Stream ImageToStream(Image img)
        {
            return ImageToStream(img, img.RawFormat);
        }

        private string _cacheDir = "Cache";
        public void ClearCache()
        {
            if (!Directory.Exists(_cacheDir))
                return;
            foreach (var file in new DirectoryInfo(_cacheDir).GetFiles())
                file.Delete();
        }

        public Image CropImage(Image img, Microsoft.ProjectOxford.Face.Contract.FaceRectangle rect)
        {
            var croppedImg = new CroppedBitmap(ConvertImageToBitmapImage(img), new Int32Rect(rect.Left, rect.Top, rect.Width, rect.Height));
            return ConvertBitmapToImage(ConvertCroppedBitmapToBitmap(croppedImg));
        }

        public enum FileNameSetting { WithExtension, WithoutExtension }
        public List<string> GetFilesInSaveDir(FileNameSetting setting)
        {
            var fileNames = new List<string>();
            var filePathes = Directory.GetFiles(SaveDirectory).ToList();
            foreach (var file in filePathes)
            {
                switch (setting)
                {
                    case FileNameSetting.WithExtension:
                        fileNames.Add(Path.GetFileName(file));
                        break;
                    case FileNameSetting.WithoutExtension:
                        fileNames.Add(Path.GetFileNameWithoutExtension(file));
                        break;
                    default:
                        break;
                }
            }
            return fileNames;
        }
    }
}
