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

        private Dictionary<ScanOptionsEnum, double> oldOptions;

        public event EventHandler<ScanEventArgs> ScanWasFinished;

        //public void EventTest(object sender, ScanEventArgs e)
        //{
        //    Console.WriteLine(e.Option.ToString() + " super fancy");
        //}

        public TensorflowExecutor()
        {
            //ScanWasFinished += EventTest;
            pictures = new Dictionary<ScanOptionsEnum, List<List<Picture>>>();
            classifier = DependencyService.Get<IClassifier>();
            classifier.ThresholdBlurry = ScanOptionsEnum.blurryPics.GetDefaultPresicionValue() * 10;
            classifier.ThresholdSimilar = ScanOptionsEnum.similarPics.GetDefaultPresicionValue() * 10;
            brightnessClassifier = new BrightnessClassifier();
            oldOptions = new Dictionary<ScanOptionsEnum, double>();
        }

        public void FillPictureLists(Dictionary<ScanOptionsEnum, double> options)
        {

            // TODO make it asynchronous
            List<Picture> pictureList = App.Database.GetPicturesAsync().Result;

            foreach (ScanOptionsEnum option in options.Keys.ToList())
            {
                // when there is already an entry && the entry has the same slider value
                if (pictures.ContainsKey(option) && oldOptions[option].Equals(options[option]))
                    continue;
                else
                {
                    // when there is already a value for the 'option key' -> empty it
                    if (pictures.ContainsKey(option))
                        pictures[option] = new List<List<Picture>>();
                }

                List<List<Picture>> outputList = new List<List<Picture>>();

                if (option == ScanOptionsEnum.darkPics)
                {
                    brightnessClassifier.Threshold = (int)options[option] * 10;
                    List<Picture> darkPictures = new List<Picture>();
                    List<Picture> brightPictures = new List<Picture>();

                    foreach (var picture in pictureList)
                    {
                        byte[] byteArray = classifier.GetImageBytes(picture.Uri);
                        bool[] result = brightnessClassifier.Classify(byteArray);
                        if (result[0])
                        {
                            darkPictures.Add(picture);
                        }
                        if (result[1])
                        {
                            brightPictures.Add(picture);
                        }
                    }

                    outputList.Add(darkPictures);
                    outputList.Add(brightPictures);

                    ScanWasFinished?.Invoke(this, new ScanEventArgs(ScanOptionsEnum.darkPics));
                }
                else if (option == ScanOptionsEnum.blurryPics)
                {
                    classifier.ChangeModel(option);
                    classifier.ThresholdBlurry = (int)options[option] * 10;
                    List<Picture> blurryPics = new List<Picture>();
                    foreach (var picture in pictureList)
                    {
                        byte[] byteArray = classifier.GetImageBytes(picture.Uri);
                        List<ModelClassification> modelClassificaton = classifier.ClassifyBlurry(byteArray);
                        if (modelClassificaton.Count > 0 && modelClassificaton[0].TagName == "Blurry")
                        {
                            blurryPics.Add(picture);
                        }
                    }

                    outputList.Add(blurryPics);

                    ScanWasFinished?.Invoke(this, new ScanEventArgs(ScanOptionsEnum.blurryPics));
                }
                else if (option == ScanOptionsEnum.similarPics)
                {
                    classifier.FeatureVectors = pictureList.Select(picture => ByteToDoubleArray(picture.FeatureVector)).ToList();
                    classifier.FillFeatureVectorMatix();
                    var matrix = classifier.FeatureMatrix;

                    for (int i = 0; i < pictureList.Count; i++)
                    {
                        var similarPics = matrix[i].Where(picture => picture.Item2 < 0.5f).Select(picture => pictureList[picture.Item1]).ToList(); // TODO work with threshold
                        if (similarPics.Count < 3) continue;
                        bool addToOutput = true;
                        for (int j = outputList.Count - 1; j >= 0; j--) // go through all lists in outputlist
                        {
                            Console.WriteLine(outputList.Count + " current count");
                            var list = outputList[j];
                            var biggerList = list.Count > similarPics.Count ? list : similarPics;
                            var smallerList = list.Count < similarPics.Count ? list : similarPics;
                            var exclusiveList = biggerList.Except(smallerList).ToList();
                            if (exclusiveList.Count <= biggerList.Count * 0.15f) // when both lists are too similar
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

                    ScanWasFinished?.Invoke(this, new ScanEventArgs(ScanOptionsEnum.similarPics));
                }

                // TODO Event when one of the scans is ready, for each

                pictures[option] = outputList;

                oldOptions[option] = options[option];
            }

        }

        public Picture[] GetImagesForDisplay(ScanOptionsEnum option)
        {
            List<List<Picture>> picturesList = pictures[option];
            if (picturesList[0] == null) return null;

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
            return pictures[option].Count;
        }

        public int GetAmountOfPicturesForOption(ScanOptionsEnum option)
        {
            List<List<Picture>> localPics = pictures[option];
            int count = 0;
            foreach (List<Picture> list in localPics)
            {
                count += list.Count;
            }

            return count;
        }

        private double[] ByteToDoubleArray(byte[] byteArr)
        {
            double[] values = new double[byteArr.Length / 8];
            Buffer.BlockCopy(byteArr, 0, values, 0, values.Length * 8);
            return values;
        }

    }
}
