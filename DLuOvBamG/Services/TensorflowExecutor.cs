using DLuOvBamG.Models;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace DLuOvBamG.Services
{
    //TODO: merge with existing TF Service
    public class TensorflowExecutor
    {
        private Dictionary<ScanOptionsEnum, List<Picture>> Pictures;
        private IClassifier Classifier;

        public TensorflowExecutor() {
            Pictures = new Dictionary<ScanOptionsEnum, List<Picture>>();
            Classifier = DependencyService.Get<IClassifier>();
            // Debug
            Classifier.test();
        }

        public void FillPictureLists(List<ScanOptionsEnum> chosenOptions)
        {
            // TODO make it asynchronous
            List<Picture> pictureList = DatabaseImageRetriever.GetImagesFromDatabase().Result;

            foreach (ScanOptionsEnum option in chosenOptions)
            {
                if(option == ScanOptionsEnum.darkPics)
                {
                    // TODO
                }
                else if(option == ScanOptionsEnum.blurryPics)
                {
                    Classifier.ChangeModel(option);

                    List<Picture> blurryPics = new List<Picture>();
                    foreach (var picture in pictureList)
                    {
                        byte[] byteArray = Classifier.GetImageBytes(picture.Uri);
                        List<ModelClassification> modelClassificaton = Classifier.Classify(byteArray);
                        if (modelClassificaton[0].TagName == "Blurry")
                        {
                            blurryPics.Add(picture);
                        }
                    }
                    Pictures.Add(option, blurryPics);

                }
                else
                {

                }
                
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
