using System.IO;
using Android.Graphics;

namespace DLuOvBamG
{
    public static class ImageResizer
    {
        public static byte[] ResizeImage(byte[] imageData, float width, float height)
        {
            return ResizeImageAndroid(imageData, width, height);
        }
    
        public static byte[] ResizeImageAndroid(byte[] imageData, float width, float height)
        {
            // Load the bitmap 
            Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
            
            float DesiredHeight = 0;
            float DesiredWidth = 0;
            
            var OriginalHeight = originalImage.Height;
            var OriginalWidth = originalImage.Width;
            
            if (OriginalHeight > OriginalWidth) // height (71 for avatar) is master
            {
                DesiredHeight = height;
                float divider = OriginalHeight / height;
                DesiredWidth = OriginalWidth / divider;
            }
            else // width (61 for avatar) is master
            {
                DesiredWidth = width;
                float divider = OriginalWidth / width;
                DesiredHeight = OriginalHeight / divider;
            }
            
            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)DesiredWidth, (int)DesiredHeight, false);
            
            using (MemoryStream ms = new MemoryStream())
            {
                resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
                return ms.ToArray();
            }
        }
    }
}