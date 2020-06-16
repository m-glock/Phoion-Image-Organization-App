using DLuOvBamG.Models;
using System;
using System.Collections.Generic;

namespace DLuOvBamG.Services
{
    public class Tensorflow
    {
        private Dictionary<ScanOptionsEnum, List<List<Picture>>> Pictures;

        public Tensorflow() {
            Pictures = new Dictionary<ScanOptionsEnum, List<List<Picture>>>();
        }

        public void FillPictureLists(List<ScanOptionsEnum> chosenOptions)
        {
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

            //alle Bilder in Liste speichern
            Picture[] picList = new Picture[stockImages.Length];
            for (int i = 0; i < stockImages.Length; i++)
            {
                picList[i] = new Picture(stockImages[i], i.ToString());
            }

            //zufällige Gruppen aus Bildern bilden
            Random r = new Random();
            foreach (ScanOptionsEnum option in chosenOptions)
            {
                List<List<Picture>>  pictures = new List<List<Picture>>();

                for (int i = 0; i < 2; i++) 
                {
                    List<Picture> pictureGroup = new List<Picture>();
                    int randomPicLength = r.Next(1, 5);
                    for (int j = 0; j < randomPicLength; j++)
                    {
                        int randomIndex = r.Next(0, picList.Length);
                        pictureGroup.Add(picList[randomIndex]);
                    }
                    pictures.Add(pictureGroup);
                }
                Pictures.Add(option, pictures);
            }
        }

        public Picture[] GetImagesForDisplay(ScanOptionsEnum option)
        {
            List<List<Picture>> picturesList = Pictures[option];
            if (picturesList[0] == null) return null;

            int picAmount = picturesList[0].Count > 2 ? 3 : picturesList[0].Count;
            Picture[] displayImages = new Picture[picAmount];
            picturesList[0].CopyTo(0, displayImages, 0, picAmount);

            return displayImages;
        }

        public List<List<Picture>> GetAllPicturesForOption(ScanOptionsEnum option)
        {
            return Pictures[option];
        }

        public int GetAmountOfSetsForOption(ScanOptionsEnum option)
        {
            return Pictures[option].Count;
        }

        public int GetAmountOfPicturesForOption(ScanOptionsEnum option)
        {
            List<List<Picture>> pictures = Pictures[option];
            int count = 0;
            foreach (List<Picture> list in pictures)
            {
                count += list.Count;
            }

            return count;
        }
    }
}
