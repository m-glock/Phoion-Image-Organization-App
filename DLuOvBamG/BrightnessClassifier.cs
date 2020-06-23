using Android.Graphics;
using Java.Nio;
using System;
using System.Collections.Generic;
using System.Text;

namespace DLuOvBamG
{
    public class BrightnessClassifier
    {

        private int width = 128; 
        private int height = 128;

        public float[] Classify(byte[] bytes)
        {

            Bitmap bitmap = BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length);
            Bitmap resizedBitmap = Bitmap.CreateScaledBitmap(bitmap, width, height, true);

 
            int[] intValues = new int[width * height];
            resizedBitmap.GetPixels(intValues, 0, resizedBitmap.Width, 0, 0, resizedBitmap.Width, resizedBitmap.Height);

            int pixel = 0;
            int darkPixels = 0;
            int brightPixels = 0;
            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < height; ++j)
                {
                    int val = intValues[pixel++];

                    int red = (val >> 16) & 0x000000FF;
                    int green = (val >> 8) & 0x000000FF;
                    int blue = (val) & 0x000000FF;

                    if (red + green + blue < 30)
                        darkPixels++;
                    if (red + green + blue > 730)
                        brightPixels++;

                }
            }

            float darkQuotient = (float)darkPixels / (float)(width * height);
            float brightQuotient = (float)brightPixels / (float)(width * height);

            return new float[] { darkQuotient, brightQuotient }; 

        }




    }
}
