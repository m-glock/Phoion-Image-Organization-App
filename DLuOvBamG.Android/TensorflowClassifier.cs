using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Android.Content.Res;
using Android.Graphics;
using DLuOvBamG.Models;
using Java.IO;
using Java.Nio;
using Java.Nio.Channels;
using MathNet.Numerics;
using Org.Tensorflow.Lite;
using Xamarin.Forms;

[assembly: Dependency(typeof(DLuOvBamG.Droid.TensorflowClassifier))]
namespace DLuOvBamG.Droid
{

    public class TensorflowClassifier : IClassifier
    {
        private Interpreter interpreter;
        private List<string> labels;

        private string[] modelFiles = { "modelBlur.tflite", "converted_modelKuhmuhMilch.tflite" };
        private string[] labelFiles = { "labelsBlur.txt", "labelsSqueezenet.txt" };

        private int thresholdBlurry;
        private int thresholdSimilar = 10;

        int IClassifier.ThresholdBlurry { get => thresholdBlurry; set => thresholdBlurry = value; }
        int IClassifier.ThresholdSimilar { get => thresholdSimilar; set => thresholdSimilar = value; }
        Tuple<int, double>[][] IClassifier.FeatureMatrix { get => FeatureMatrix; set => FeatureMatrix = value; }
        List<double[]> IClassifier.FeatureVectors { get => FeatureVectors; set => FeatureVectors = value; }

        public event EventHandler<ClassificationEventArgs> ClassificationCompleted;

        public List<double[]> FeatureVectors = new List<double[]>();
        // int -> index; double -> distance
        public Tuple<int, double>[][] FeatureMatrix;


        // TODO remove
        Stopwatch stopWatch = new Stopwatch();



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
            inputStream.Close();

            return mappedByteBuffer;
        }

        private List<string> LoadLabelList(string path)
        {
            StreamReader sr = new StreamReader(Android.App.Application.Context.Assets.Open(path));
            List<string> labels = sr.ReadToEnd().Split('\n').Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToList();
            sr.Close();
            return labels;
        }

        #region testing 

        public async Task classifyProcess(string entry)
        {
            //System.Console.WriteLine("thread of your life");
            byte[] image = GetImageBytes(entry);
            var sortedList = await ClassifySimilar(image);
            if (sortedList.Count > 0)
            {
                ModelClassification top = sortedList.First();
                print(top.TagName + " " + Math.Round(top.Probability * 100, 2) + "% ultra result for " + entry);
            }
            //foreach (ModelClassification item in sortedList)
            //{
            //    System.Console.WriteLine(item.TagName + " " + Math.Round(item.Probability * 100, 2) + "% result for " + entry);
            //}
        }

        public async Task<string> testAsync()
        {
            string returnVal = "";
            stopWatch.Start();
            ChangeModel(ScanOptionsEnum.similarPics);
            string[] listAssets = Android.App.Application.Context.Assets.List("stockImages");
            //Task[] tasks = new Task[listAssets.Length];
            List<Task> tasks = new List<Task>();

            for (int i = 0; i < listAssets.Length; i++)
            {
                tasks.Add(classifyProcess(listAssets[i]));
            }
            while (tasks.Count > 0)
            {
                Task finishedTask = await Task.WhenAny(tasks);
                print("freeman");
                tasks.Remove(finishedTask);
            }


            return returnVal;
        }
        #endregion

        public byte[] GetImageBytes(string path)
        {
            Stream stream = System.IO.File.OpenRead(path);
            byte[] fileBytes = ReadStream(stream);

            stream.Close();

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
                ms.Close();

                return ms.ToArray();
            }
        }

        private ByteBuffer ConvertBitmapToByteBuffer(byte[] bytes, int height, int width)
        {

            float IMAGE_STD = 128.0f;
            int IMAGE_MEAN = 128;

            Bitmap bitmap = BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length);
            Bitmap resizedBitmap = Bitmap.CreateScaledBitmap(bitmap, width, height, true);

            int[] pixels = new int[width * height];
            resizedBitmap.GetPixels(pixels, 0, resizedBitmap.Width, 0, 0, resizedBitmap.Width, resizedBitmap.Height);



            Byte[] imageBytes = new byte[4 * width * height * 3];
            int counter = 0;
            foreach (int pixel in pixels)
            {
                for (int i = 2; i >= 0; i--)
                {
                    byte[] pixelBytes = BitConverter.GetBytes(((((pixel >> i * 8) & 0xFF) - IMAGE_MEAN) / IMAGE_STD));

                    foreach (var pixelByte in pixelBytes)
                    {
                        imageBytes[counter] = pixelByte;
                        counter++;
                    }
                }
            }

            return ByteBuffer.Wrap(imageBytes);
        }

        public async Task<List<ModelClassification>> ClassifySimilar(byte[] bytes)
        {
            await Task.Delay(0); // await cheat, temporary

            Tensor tensor = interpreter.GetInputTensor(0);
            int[] shape = tensor.Shape();
            int width = shape[1];
            int height = shape[2];

            // Output Labels
            float[][] outputLabels = new float[1][] { new float[labels.Count] };
            var outputLabelsConverted = Java.Lang.Object.FromArray(outputLabels);

            // Output Vectors
            float[][][][] outputVectors = new float[1][][][];
            outputVectors[0] = new float[1][][];
            outputVectors[0][0] = new float[1][];
            outputVectors[0][0][0] = new float[1280];
            var outputVectorsConverted = Java.Lang.Object.FromArray(outputVectors);

            Dictionary<Java.Lang.Integer, Java.Lang.Object> outputs = new Dictionary<Java.Lang.Integer, Java.Lang.Object>
            {
                { (Java.Lang.Integer)0, outputLabelsConverted },
                { (Java.Lang.Integer)1, outputVectorsConverted }
            };

            ByteBuffer inputs = ConvertBitmapToByteBuffer(bytes, width, height);
            Java.Lang.Object[] finalInput = { inputs };

            interpreter.RunForMultipleInputsOutputs(finalInput, outputs);

            // Classification Results
            float[][] classificationResult = outputLabelsConverted.ToArray<float[]>();
            float[][][][] featureVectorResult = outputVectorsConverted.ToArray<float[][][]>();

            List<ModelClassification> classificationResults = new List<ModelClassification>();
            for (int i = 0; i < labels.Count; i++)
            {
                string label = labels[i];
                classificationResults.Add(new ModelClassification(label, classificationResult[0][i]));
            }

            // Storing feature vectors
            float[] featureVector = featureVectorResult[0][0][0];
            // conversion, because doubles are needed later on
            double[] featureVectorDouble = Array.ConvertAll(featureVector, x => (double)x);
            featureVectorDouble = normalizeVector(featureVectorDouble);
            FeatureVectors.Add(featureVectorDouble);


            var sortedList = classificationResults.OrderByDescending(x => x.Probability).ToList();
            // only select pictures that are bigger than a given threshold
            sortedList = sortedList.FindAll(x => System.Math.Round(x.Probability * 100, 2) > 40);

            // Notify all listeners
            //ClassificationCompleted?.Invoke(this, new ClassificationEventArgs(classificationResults));

            if (sortedList.Count > 0)
            {
                ModelClassification top = sortedList.First();
                print(top.TagName + " " + Math.Round(top.Probability * 100, 2) + "% ultra result for " + top);
            }

            return sortedList;
        }

        private double[] normalizeVector(double[] vector)
        {
            // Calculate magnitude
            double magnitude = 0;
            for (int i = 0; i < vector.Length; i++)
                magnitude += Math.Pow(vector[i], 2);
            magnitude = Math.Sqrt(magnitude);
            // normalize
            for (int i = 0; i < vector.Length; i++)
                vector[i] = (float)(vector[i] / magnitude);

            return vector;
        }

        public void FillFeatureVectorMatix()
        {
            stopWatch.Start();
            FeatureMatrix = new Tuple<int, double>[FeatureVectors.Count][];

            for (int i = 0; i < FeatureVectors.Count; i++)
            {
                FeatureMatrix[i] = new Tuple<int, double>[FeatureVectors.Count];
                for (int j = 0; j <= i; j++)
                {
                    double d = Distance.Cosine(FeatureVectors[i], FeatureVectors[j]);
                    FeatureMatrix[i][j] = Tuple.Create(j, d);
                    FeatureMatrix[j][i] = Tuple.Create(i, d);
                }
            }

            print(stopWatch.ElapsedMilliseconds + " time matrix");

            //List<List<Tuple<int, double>>> allNeighbours = new List<List<Tuple<int, double>>>();
            //for (int i = 0; i < featureMatrix.Length; i++)
            //{
            //    allNeighbours.Add(featureMatrix[i].OrderBy(tupel => tupel.Item2).Take(10).ToList());
            //}
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

        public void print(string message)
        {
            System.Console.WriteLine(message);
        }

    }
}


