using DLuOvBamG.Models;
using System.Collections.Generic;

namespace DLuOvBamG.Services
{
    public class Tensorflow
    {
        private Dictionary<ScanOptionsEnum, List<Picture>> Pictures;

        public Tensorflow() {
            Pictures = new Dictionary<ScanOptionsEnum, List<Picture>>();
        }

        public void FillPictureLists(List<ScanOptionsEnum> options)
        {
            //Listen für alle ScanOptions
            foreach (ScanOptionsEnum option in options)
            {
                Pictures.Add(option, new List<Picture>());
            }


            //Listen mit Bildern füllen
            string[] stockImages = {
                "https://farm9.staticflickr.com/8625/15806486058_7005d77438.jpg",
                "https://farm5.staticflickr.com/4011/4308181244_5ac3f8239b.jpg",
                "https://farm8.staticflickr.com/7423/8729135907_79599de8d8.jpg",
                "https://farm3.staticflickr.com/2475/4058009019_ecf305f546.jpg",
                "https://farm6.staticflickr.com/5117/14045101350_113edbe20b.jpg",
                "https://farm8.staticflickr.com/7524/15620725287_3357e9db03.jpg",
                "https://farm9.staticflickr.com/8351/8299022203_de0cb894b0.jpg",
            };

            for (int i = 0; i < stockImages.Length; i++)
            {
                Picture pic = new Picture(stockImages[i], i.ToString());
                foreach (ScanOptionsEnum option in options)
                {
                    Pictures[option].Add(pic);
                }
            }
        }

        public List<Picture> GetImagesForDisplay(ScanOptionsEnum option)
        {
            List<Picture> displayImages = Pictures[option];

            if (displayImages.Count > 3)
            {
                displayImages.RemoveRange(3, displayImages.Count - 3);
            }

            return displayImages;
        }
    }
}
