using System;
using System.Collections.Generic;
using System.Text;

namespace DLuOvBamG.Models
{
    public class ModelClassification
    {   
        public float Probability { get; set; }
        public string TagName { get; set; }
        public float FeatureVector { get; set; }

        public ModelClassification(string tagName, float probability)
        {
            TagName = tagName;
            Probability = probability;
        }
    }

    public class PredictionResult
    {
        public List<ModelClassification> Predictions { get; set; }
    }
}
