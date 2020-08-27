using DLuOvBamG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DLuOvBamG.Services
{
    public class TensorflowExecutor
    {
        private Dictionary<ScanOptionsEnum, List<List<Picture>>> pictures;
        private IClassifier classifier;
        private BrightnessClassifier brightnessClassifier;
        private string DirectoryPath;
        private Dictionary<ScanOptionsEnum, double> oldOptions;
        public event EventHandler<ScanEventArgs> ScanWasFinished;

        public TensorflowExecutor()
        {
            DirectoryPath = "";
            pictures = new Dictionary<ScanOptionsEnum, List<List<Picture>>>();
            classifier = DependencyService.Get<IClassifier>();
            classifier.ThresholdBlurry = ScanOptionsEnum.blurryPics.GetDefaultPresicionValue() * 10;
            classifier.ThresholdSimilar = ScanOptionsEnum.similarPics.GetDefaultPresicionValue() * 10;
            brightnessClassifier = new BrightnessClassifier();
            oldOptions = new Dictionary<ScanOptionsEnum, double>();
        }

        public async Task FillPictureLists(Dictionary<ScanOptionsEnum, double> options, string pictureKey, string pictureValue)
        {
            List<Picture> pictureList;
            if (pictureKey != "")
            {
                pictureList = await App.Database.GetPictursByValueAsync(pictureKey, pictureValue);
            }
            else
            {
                pictureList = App.Database.GetPicturesAsync().Result;
            }

            foreach (ScanOptionsEnum option in options.Keys.ToList())
            {
                
                int threshold = (int)options[option] * 10;

                // when there is already an entry && the entry has the same slider value
                if (DirectoryPath.Equals(pictureValue) && pictures.ContainsKey(option) && oldOptions[option].Equals(options[option]))
                {
                    ScanWasFinished?.Invoke(this, new ScanEventArgs(option));
                    continue;
                }
                // when there is already a value for the 'option key' -> empty it
                else if (pictures.ContainsKey(option))
                {
                    pictures[option] = new List<List<Picture>>();
                }

                List<List<Picture>> outputList = new List<List<Picture>>();


                if (option == ScanOptionsEnum.darkPics)
                {
                    GetAllPicturesWithBadLighting(pictureList, threshold, outputList);
                }
                else if (option == ScanOptionsEnum.blurryPics)
                {
                    GetAllBlurryPictures(option, options, pictureList, threshold, outputList);
                }
                else if (option == ScanOptionsEnum.similarPics)
                {
                    GetAllSimilarPictures(pictureList, threshold, outputList);
                }

                pictures[option] = outputList;
                ScanWasFinished?.Invoke(this, new ScanEventArgs(option));
                oldOptions[option] = options[option];
            }

            DirectoryPath = pictureValue;
        }

        private void GetAllPicturesWithBadLighting(
            List<Picture> pictureList, 
            int threshold, 
            List<List<Picture>> outputList)
        {
            //brightnessClassifier.Threshold = (int)options[option] * 10; 
            List<Picture> darkPictures = new List<Picture>();
            List<Picture> brightPictures = new List<Picture>();

            foreach (var picture in pictureList)
            {
                if (picture.Uri.Contains("product/media/ims")) continue;
                double darkPixelsPercent = 0;
                double brightPixelsPercent = 0;
                if (picture.DarkPixelsPercent > 0)
                {
                    darkPixelsPercent = picture.DarkPixelsPercent;
                    brightPixelsPercent = picture.BrightPixelsPercent;
                }
                else
                {
                    byte[] byteArray = classifier.GetImageBytes(picture.Uri);
                    // 0 = dark, 1 = bright
                    double[] result = brightnessClassifier.Classify(byteArray);
                    picture.DarkPixelsPercent = result[0];
                    picture.BrightPixelsPercent = result[1];
                    App.Database.SavePictureAsync(picture);

                    darkPixelsPercent = result[0];
                    brightPixelsPercent = result[1];
                }


                if (darkPixelsPercent > threshold)
                {
                    darkPictures.Add(picture);
                }
                if (brightPixelsPercent > threshold)
                {
                    brightPictures.Add(picture);
                }
            }

            outputList.Add(darkPictures);
            outputList.Add(brightPictures);
        }

        private void GetAllBlurryPictures(
            ScanOptionsEnum option,
            Dictionary<ScanOptionsEnum, double> options,
            List<Picture> pictureList, 
            int threshold,
            List<List<Picture>> outputList)
        {
            classifier.ChangeModel(option);
            classifier.ThresholdBlurry = (int)options[option] * 10;
            List<Picture> blurryPics = new List<Picture>();
            foreach (var picture in pictureList)
            {
                if (picture.Uri.Contains("product/media/ims")) continue;

                // when picture was scanned already
                if (picture.BlurryPrecision != 0)
                {
                    if (picture.BlurryPrecision > (float)(threshold / 100f))
                        blurryPics.Add(picture);
                    continue;
                }

                byte[] byteArray = classifier.GetImageBytes(picture.Uri);
                List<ModelClassification> modelClassificaton = classifier.ClassifyBlurry(byteArray);
                if (modelClassificaton.Count > 0 && modelClassificaton[0].TagName == "Blurry")
                {
                    picture.BlurryPrecision = modelClassificaton[0].Probability;
                    if (modelClassificaton[0].Probability > (float)(threshold / 100f))
                    {
                        blurryPics.Add(picture);
                    }
                }
                else
                {
                    picture.BlurryPrecision = -1;
                }
                App.Database.SavePictureAsync(picture);
            }
            outputList.Add(blurryPics);
        }

        private void GetAllSimilarPictures(
            List<Picture> pictureList,
            int threshold,
            List<List<Picture>> outputList)
        {
            var matrix = classifier.FeatureMatrix;
            float portion = 1 - (Math.Abs((float)(threshold / 10f) - 1) / 8);
            float newThreshold = portion * 0.2f + 0.4f;

            for (int i = 0; i < pictureList.Count; i++)
            {
                if (pictureList[i].Uri.Contains("product/media/ims")) continue;
                var similarPics = matrix[i].Where(picture => picture.Item2 < newThreshold).Select(picture => pictureList[picture.Item1]).ToList();
                if (similarPics.Count < 3) continue;
                bool addToOutput = true;
                for (int j = outputList.Count - 1; j >= 0; j--) // go through all lists in outputlist
                {
                    var list = outputList[j];
                    var biggerList = list.Count > similarPics.Count ? list : similarPics;
                    var smallerList = list.Count < similarPics.Count ? list : similarPics;
                    var exclusiveList = biggerList.Except(smallerList).ToList();
                    if (exclusiveList.Count <= biggerList.Count * 0.50f)
                    {
                        // when output contains the smaller of the similar lists -> remove it 
                        if (outputList.Contains(smallerList))
                            outputList.Remove(smallerList);
                        else
                            addToOutput = false;
                    }
                }
                if (addToOutput)
                {
                    outputList.Add(similarPics);
                }
                addToOutput = true;
            }
        }

        /*
         * depending on how many pictures were found, get the apropriate amount of pictures to preview
         */
        public Picture[] GetImagesForDisplay(ScanOptionsEnum option)
        {
            List<List<Picture>> picturesList = pictures[option];
            if (picturesList[0] == null || picturesList[0].Count < 1)
            {
                return new Picture[] { new Picture() };
            }

            int picAmount = picturesList[0].Count > 2 ? 3 : picturesList[0].Count;
            Picture[] displayImages = new Picture[picAmount];
            picturesList[0].CopyTo(0, displayImages, 0, picAmount);

            return displayImages;
        }

        public List<List<Picture>> GetAllPicturesForOption(ScanOptionsEnum option)
        {
            return pictures[option];
        }

        public int GetAmountOfSetsForOption(ScanOptionsEnum option)
        {
            if (!pictures.ContainsKey(option) || pictures[option] == null) return 0;
            return pictures[option].Count;
        }

        public int GetAmountOfPicturesForOption(ScanOptionsEnum option)
        {
            if (!pictures.ContainsKey(option) || pictures[option] == null) return 0;
            List<List<Picture>> localPics = pictures[option];
            int count = 0;
            foreach (List<Picture> list in localPics)
            {
                count += list.Count;
            }

            return count;
        }

        public double[] ByteToDoubleArray(byte[] byteArr)
        {
            double[] values = new double[byteArr.Length / 8];
            Buffer.BlockCopy(byteArr, 0, values, 0, values.Length * 8);
            return values;
        }

        public List<Picture> GetNeighboursForPicture(int id)
        {
            id--;
            List<Picture> returnPics = new List<Picture>();
            var allNeighbours = classifier.FeatureMatrix[id].OrderBy(tupel => tupel.Item2);
            var nearestNeighbors = allNeighbours.Where(tuple => tuple.Item2 < 0.5f).ToList();
            if(nearestNeighbors.Count == 0) return returnPics;

            foreach (var neighbour in nearestNeighbors)
            {
                //if (neighbour.Item1 == 0) continue; 
                returnPics.Add(App.Database.GetPictureAsync(neighbour.Item1 + 1).Result);
            }
            
            return returnPics;
        }

    }
}
