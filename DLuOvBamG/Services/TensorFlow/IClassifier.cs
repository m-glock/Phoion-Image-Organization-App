using DLuOvBamG.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DLuOvBamG
{
    public interface IClassifier
    {
        public int ThresholdBlurry { get; set; }
        public int ThresholdSimilar { get; set; }

        public List<double[]> FeatureVectors { get; set; }
        public Tuple<int, double>[][] FeatureMatrix { get; set; }

        event EventHandler<ClassificationEventArgs> ClassificationCompleted;

        Task<List<ModelClassification>> ClassifySimilar(byte[] bytes);
        List<ModelClassification> ClassifyBlurry(byte[] bytes);

        void FillFeatureVectorMatix();

        byte[] GetImageBytes(string path);

        void ChangeModel(ScanOptionsEnum type);
        Task<string> testAsync();
    }

    public class ClassificationEventArgs : EventArgs
    {
        public List<ModelClassification> Predictions { get; private set; }

        public ClassificationEventArgs(List<ModelClassification> predictions)
        {
            Predictions = predictions;
        }
    }
}
