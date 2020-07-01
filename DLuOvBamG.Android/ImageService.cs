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

        public byte[] GetFileBytes(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new ArgumentException("file does not exists");
            }

            return File.ReadAllBytes(filePath);
        }
    }
}


                