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

        event EventHandler<ClassificationEventArgs> ClassificationCompleted;

        List<ModelClassification> ClassifySimilar(byte[] bytes);
        List<ModelClassification> ClassifyBlurry(byte[] bytes);

        byte[] GetImageBytes(string path);

        void ChangeModel(ScanOptionsEnum type);
        //void test();
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
