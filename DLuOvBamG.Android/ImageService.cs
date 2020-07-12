using Android.Graphics;
using Android.Media;
using Android.Net;
using Android.Content;
using Android.Database;
using Android.Provider;
using DLuOvBamG.Droid;
using DLuOvBamG.Services;
using System;
using System.IO;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using static Android.Provider.MediaStore;
using static Android.Provider.MediaStore.Images;
using System.Linq;
using Android.App;

[assembly: Dependency(typeof(ImageService))]
namespace DLuOvBamG.Droid
{
    class ImageService : IImageService
    {
        private static Android.Net.Uri InternalContentUri  = MediaStore.Images.Media.InternalContentUri;
        private static Android.Net.Uri ExternalContentUri = MediaStore.Images.Media.ExternalContentUri;
        private Context CurrentContext = Android.App.Application.Context;
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

        public string[] GetAllImagesFromDevice()
        {
            string[] internalImagePaths = GetImagesFromUri(InternalContentUri);

            string[] externalImagePaths = new string[0];
            Boolean isSDPresent = Android.OS.Environment.ExternalStorageState.Equals(Android.OS.Environment.MediaMounted);
            if (isSDPresent) {
                externalImagePaths = GetImagesFromUri(ExternalContentUri);
            }

            if(externalImagePaths.Length != 0)
            {
                string[] result = new string[internalImagePaths.Length + externalImagePaths.Length];
                internalImagePaths.CopyTo(result, 0);
                externalImagePaths.CopyTo(result, internalImagePaths.Length);
                return result;
            } else
            {
                return internalImagePaths;
            }
        }

        private string[] GetImagesFromUri(Android.Net.Uri uri)
        {
            // A list of which columns to return. Passing null will return all columns, which is inefficient.
            string[] projection = { 
                ImageColumns.Data,
                ImageColumns.BucketDisplayName,
                ImageColumns.Id
            };
            // How to order the rows, formatted as an SQL ORDER BY clause
            string orderBy = ImageColumns.Id;

            //Stores all the images from the gallery in Cursor
            ICursor cursor = CurrentContext.ContentResolver.Query(uri, projection, null, null, orderBy);
            //Total number of images
            int count = cursor.Count;

            //Create an array to store path to all the images
            string[] arrPath = new String[count];

            for (int i = 0; i < count; i++)
            {
                cursor.MoveToPosition(i);
                int dataColumnIndex = cursor.GetColumnIndex(ImageColumns.Data);
                //Store the path of the image
                arrPath[i] = cursor.GetString(dataColumnIndex);
            }
            // The cursor should be freed up after use with close()
            cursor.Close();
            return arrPath;
        }
    }
}


                