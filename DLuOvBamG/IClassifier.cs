using DLuOvBamG.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DLuOvBamG
{
    public interface IClassifier
    {
        event EventHandler<ClassificationEventArgs> ClassificationCompleted;

        void Classify(byte[] bytes);

        void test();
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
