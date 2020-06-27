using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Android.Content.Res;
using Android.Graphics;
using DLuOvBamG.Models;
using Java.IO;
using Java.Nio;
using Java.Nio.Channels;
using Org.Tensorflow.Lite;
using Xamarin.Forms;
using System.Collections;
using static DLuOvBamG.IClassifier;

[assembly: Dependency(typeof(DLuOvBamG.Droid.TensorflowClassifier))]
namespace DLuOvBamG.Droid
{

    public class TensorflowClassifier : IClassifier
    {

        private Interpreter interpreter;
        private List<string> labels;

        private string[] modelFiles = { "modelBlur.tflite", "converted_model.tflite" };
        private string[] labelFiles = { "labelsBlur.txt", "labelsSqueezenet.txt" };

        private int thresholdBlurry;
        private int thresholdSimilar;

        int IClassifier.ThresholdBlurry { get => thresholdBlurry; set => thresholdBlurry = value; }
        int IClassifier.ThresholdSimilar { get => thresholdSimilar; set => thresholdSimilar = value; }

        public event EventHandler<ClassificationEventArgs> ClassificationCompleted;


        public void ChangeModel(ScanOptionsEnum type)
        {
            int finalType = -1;
            switch (type)
            {
                case ScanOptionsEnum.similarPics:
                    finalType = 1;
                    break;
                case ScanOptionsEnum.blurryPics:
                    finalType = 0;
                    break;
                default:
                    break;
            }
            interpreter = new Interpreter(GetByteBuffer(modelFiles[finalType]));
            labels = LoadLabelList(labelFiles[finalType]);
        }

        // For tflite file
        private ByteBuffer GetByteBuffer(string path)
        {
            AssetFileDescriptor assetDescriptor = Android.App.Application.Context.Assets.OpenFd(path);
            FileInputStream inputStream = new FileInputStream(assetDescriptor.FileDescriptor);

            ByteBuffer mappedByteBuffer = inputStream.Channel.Map(FileChannel.MapMode.ReadOnly, assetDescriptor.StartOffset, assetDescriptor.DeclaredLength);

            return mappedByteBuffer;
        }

        private List<string> LoadLabelList(string path)
        {
            StreamReader sr = new StreamReader(Android.App.Application.Context.Assets.Open(path));
            List<string> labels = sr.ReadToEnd().Split('\n').Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToList();
            //System.Console.WriteLine(String.Join("\n", labels));
            return labels;
        }

        #region testing 

        //public void test()
        //{
        //    string[] listAssets = Android.App.Application.Context.Assets.List("stockImages");
        //    foreach (var entry in listAssets)
        //    {
        //        System.Console.WriteLine("reading image " + entry);
        //        byte[] image = GetImageBytes(entry);
        //        var sortedList = Classify(image);

        //        if (sortedList.Count > 0)
        //        {
        //            ModelClassification top = sortedList.First();
        //            System.Console.WriteLine(top.TagName + " " + Math.Round(top.Probability * 100, 2) + "% ultra result for " + entry);
        //        }
        //        foreach (ModelClassification item in sortedList)
        //        {
        //            System.Console.WriteLine(item.TagName + " " + Math.Round(item.Probability * 100, 2) + "% result for " + entry);
        //        }
        //    }
        //}



        #endregion

        public byte[] GetImageBytes(string path)
        {
            //AssetFileDescriptor assetDescriptor = Android.App.Application.Context.Assets.OpenFd("stockImages/" + path);

            Stream stream = System.IO.File.OpenRead(path);
            byte[] fileBytes = ReadStream(stream);

            return fileBytes;
        }

        public byte[] ReadStream(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        private ByteBuffer ConvertBitmapToByteBuffer(byte[] bytes, int height, int width)
        {
            float IMAGE_STD = 128.0f;
            int IMAGE_MEAN = 128;

            Bitmap bitmap = BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length);
            Bitmap resizedBitmap = Bitmap.CreateScaledBitmap(bitmap, width, height, true);

            ByteBuffer byteBuffer;
            int modelInputSize = 4 * height * width * 3;

            byteBuffer = ByteBuffer.AllocateDirect(modelInputSize);

            byteBuffer.Order(ByteOrder.NativeOrder());
            int[] intValues = new int[width * height];
            resizedBitmap.GetPixels(intValues, 0, resizedBitmap.Width, 0, 0, resizedBitmap.Width, resizedBitmap.Height);
            int pixel = 0;
            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < height; ++j)
                {
                    int val = intValues[pixel++];
                    byteBuffer.PutFloat((((val >> 16) & 0xFF) - IMAGE_MEAN) / IMAGE_STD);
                    byteBuffer.PutFloat((((val >> 8) & 0xFF) - IMAGE_MEAN) / IMAGE_STD);
                    byteBuffer.PutFloat((((val) & 0xFF) - IMAGE_MEAN) / IMAGE_STD);
                }
            }
            return byteBuffer;
        }

        public List<ModelClassification> ClassifySimilar(byte[] bytes)
        {
            Tensor tensor = interpreter.GetInputTensor(0);

            // TODO get second output as veature vector
            //Tensor outputTensor = interpreter.GetOutputTensor(1);
            //System.Console.WriteLine("tensor output " + outputTensor.NumDimensions());
            //bool[] brightness = new BrightnessClassifier().Classify(bytes);
            //if (brightness[0])
            //    System.Console.WriteLine("darkness quotient " + brightness[0]);
            //if (brightness[1])
            //    System.Console.WriteLine("brightness quotient " + brightness[1]);

            int[] shape = tensor.Shape();
            int width = shape[1];
            int height = shape[2];
            ByteBuffer byteBuffer = ConvertBitmapToByteBuffer(bytes, width, height);

            // Output Labels
            float[][] outputLabels = new float[1][] { new float[labels.Count] };
            var outputLabelsConverted = Java.Lang.Object.FromArray(outputLabels);

            // Output Vectors
            float[][][][] outputVectors = new float[1][][][];
            outputVectors[0] = new float[1][][];
            outputVectors[0][0] = new float[1][];
            outputVectors[0][0][0] = new float[512];
            var outputVectorsConverted = Java.Lang.Object.FromArray(outputVectors);

            Dictionary<Java.Lang.Integer, Java.Lang.Object> outputs = new Dictionary<Java.Lang.Integer, Java.Lang.Object>
            {
                { (Java.Lang.Integer)0, outputLabelsConverted },
                { (Java.Lang.Integer)1, outputVectorsConverted }
            };

            Java.Lang.Object[] inputs = { byteBuffer };

            interpreter.RunForMultipleInputsOutputs(inputs, outputs);

            // Classification Results
            float[][] classificationResult = outputLabelsConverted.ToArray<float[]>();
            float[][][][] featureVectorResult = outputVectorsConverted.ToArray<float[][][]>();

            List<ModelClassification> result = new List<ModelClassification>();
            for (int i = 0; i < labels.Count; i++)
            {
                string label = labels[i];

                result.Add(new ModelClassification(label, classificationResult[0][i]));
            }

            double n2 = 0;
            for (int i = 0; i < 512; i++)
            {
                n2 += Math.Pow(featureVectorResult[0][0][0][i], 2);
                
            }
            n2 = Math.Sqrt(n2);
            System.Console.WriteLine(n2 + " feature vector");

            var sortedList = result.OrderByDescending(x => x.Probability).ToList();
            sortedList = sortedList.FindAll(x => System.Math.Round(x.Probability * 100, 2) > thresholdSimilar);

            // Notify all listeners
            ClassificationCompleted?.Invoke(this, new ClassificationEventArgs(result));

            return sortedList;
        }

        public List<ModelClassification> ClassifyBlurry(byte[] bytes)
        {
            Tensor tensor = interpreter.GetInputTensor(0);

            int[] shape = tensor.Shape();
            int width = shape[1];
            int height = shape[2];
            ByteBuffer byteBuffer = ConvertBitmapToByteBuffer(bytes, width, height);

            // Output Labels
            float[][] outputLabels = new float[1][] { new float[labels.Count] };
            var outputLabelsConverted = Java.Lang.Object.FromArray(outputLabels);

    
            interpreter.Run(byteBuffer, outputLabelsConverted);

            // Classification Results
            float[][] classificationResult = outputLabelsConverted.ToArray<float[]>();
 
            List<ModelClassification> result = new List<ModelClassification>();
            for (int i = 0; i < labels.Count; i++)
            {
                string label = labels[i];

                result.Add(new ModelClassification(label, classificationResult[0][i]));
            }


            var sortedList = result.OrderByDescending(x => x.Probability).ToList();
            sortedList = sortedList.FindAll(x => System.Math.Round(x.Probability * 100, 2) > thresholdBlurry);

            // Notify all listeners
            ClassificationCompleted?.Invoke(this, new ClassificationEventArgs(result));

            return sortedList;
        }
    }
}


