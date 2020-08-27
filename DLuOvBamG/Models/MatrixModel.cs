using System;

namespace DLuOvBamG.Models
{
    public class MatrixModel
    {
        public Tuple<int, double>[][] FeatureMatrix { get; set; }
        public MatrixModel() { }

        public void InitializeArray(int size)
        {
            FeatureMatrix = new Tuple<int, double>[size][];
        }

        public void FillArray(Tuple<int, double>[][] arr)
        {
            FeatureMatrix = arr;
        }

    }
}
