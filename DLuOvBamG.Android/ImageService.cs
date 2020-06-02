using Android.Graphics;
using Android.Media;
using DLuOvBamG.Droid;
using DLuOvBamG.Services;
using System;
using System.IO;
using System.Text.RegularExpressions;
using Xamarin.Forms;

[assembly: Dependency(typeof(ImageService))]
namespace DLuOvBamG.Droid
{
    class ImageService : IImageService
    {
        private static readonly Regex r = new Regex(":");
        public DateTime GetDateTaken(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new ArgumentException("file does not exists");
            }
            ExifInterface exif = new ExifInterface(filePath);
            string dateTaken = exif.GetAttribute(ExifInterface.TagDatetime);
            dateTaken = r.Replace(dateTaken, "-", 2);
            return DateTime.Parse(dateTaken);
        }

        public byte[] ResizeImage(string filePath, float width, float height, int quality)
        {
            if (!File.Exists(filePath))
            {
                throw new ArgumentException("file does not exists");
            }

            byte[] imageData = File.ReadAllBytes(filePath);

            // Load the bitmap
            Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);

            float oldWidth = (float)originalImage.Width;
            float oldHeight = (float)originalImage.Height;
            float scaleFactor = 0f;

            if (oldWidth > oldHeight)
            {
                scaleFactor = width / oldWidth;
            }
            else
            {
                scaleFactor = height / oldHeight;
            }

            float newHeight = oldHeight * scaleFactor;
            float newWidth = oldWidth * scaleFactor;

            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)newWidth, (int)newHeight, false);

            using (MemoryStream ms = new MemoryStream())
            {
                resizedImage.Compress(Bitmap.CompressFormat.Jpeg, quality, ms);
                return ms.ToArray();
            }
        }
    }
}


                