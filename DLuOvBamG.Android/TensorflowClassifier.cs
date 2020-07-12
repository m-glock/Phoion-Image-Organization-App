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
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using Org.Tensorflow.Lite;
using Xamarin.Forms;

[assembly: Dependency(typeof(DLuOvBamG.Droid.TensorflowClassifier))]
namespace DLuOvBamG.Droid
{

    public class TensorflowClassifier : IClassifier
    {
        // TODO remove debug
        private bool testing = true;

        private Interpreter interpreter;
        private List<string> labels;

        private string[] modelFiles = { "modelBlur.tflite", "converted_modelKuhmuhMilch.tflite" };
        private string[] labelFiles = { "labelsBlur.txt", "labelsSqueezenet.txt" };

        private int thresholdBlurry;
        private int thresholdSimilar = 10;

        int IClassifier.ThresholdBlurry { get => thresholdBlurry; set => thresholdBlurry = value; }
        int IClassifier.ThresholdSimilar { get => thresholdSimilar; set => thresholdSimilar = value; }

        public event EventHandler<ClassificationEventArgs> ClassificationCompleted;

        public List<double[]> FeatureVectors = new List<double[]>();
        public Tuple<int, double>[][] featureMatrix;

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
            if (!testing) return "";
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
            while(tasks.Count > 0)
            {
                Task finishedTask = await Task.WhenAny(tasks);
                print("freeman");
                tasks.Remove(finishedTask);
            }

            print(stopWatch.ElapsedMilliseconds + " passed time classify for " + listAssets.Length + " items");

            featureMatrix = new Tuple<int, double>[FeatureVectors.Count][];

            for (int i = 0; i < FeatureVectors.Count; i++)
            {
                featureMatrix[i] = new Tuple<int, double>[FeatureVectors.Count];
                for (int j = 0; j <= i; j++)
                {
                    double d = Distance.Cosine(FeatureVectors[i], FeatureVectors[j]);
                    featureMatrix[i][j] = Tuple.Create(j, d);
                    featureMatrix[j][i] = Tuple.Create(i, d);
                    //System.Console.WriteLine(featureMatrix[i][j].Item2 + " item2");
                }
            }

            List<List<Tuple<int, double>>> allNeighbours = new List<List<Tuple<int, double>>>();
            for (int i = 0; i < featureMatrix.Length; i++)
            {
                allNeighbours.Add(featureMatrix[i].OrderBy(tupel => tupel.Item2).Take(10).ToList());
            }

            return returnVal;
        }
        #endregion

        public byte[] GetImageBytes(string path)
        {
            Stream stream;
            if (testing)
            {
                AssetFileDescriptor assetDescriptor = Android.App.Application.Context.Assets.OpenFd("stockImages/" + path);
                stream = assetDescriptor.CreateInputStream();
            }
            else
                stream = System.IO.File.OpenRead(path);

            //Stream stream = System.IO.File.OpenRead(path);
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
                return ms.ToArray();
            }
        }

        private /*float[][][][]*/ ByteBuffer ConvertBitmapToByteBuffer(byte[] bytes, int height, int width)
        {

            float IMAGE_STD = 128.0f;
            int IMAGE_MEAN = 128;
            // ERROR:  W/ServiceManagement(21703): getService: unable to call into hwbinder service for vendor.huawei.hardware.jpegdec@1.0::IJpegDecode/default.
            Bitmap bitmap = BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length);
            // ERROR: (21703): [ZeroHung]zrhung_get_config: Get config failed for wp[0x0008]
            Bitmap resizedBitmap = Bitmap.CreateScaledBitmap(bitmap, width, height, true);

            float[][][] shiftedPixels = new float[height][][];

            int[] pixels = new int[width * height];
            resizedBitmap.GetPixels(pixels, 0, resizedBitmap.Width, 0, 0, resizedBitmap.Width, resizedBitmap.Height);



            //for (int i = 0; i < height; i++)
            //{
            //    shiftedPixels[i] = new float[width][];
            //    for (int j = 0; j < width; j++)
            //    {
            //        int pixelVal = pixels[i * width + j];

            //        shiftedPixels[i][j] = new float[3];
            //        shiftedPixels[i][j][0] = ((((pixelVal >> 16) & 0xFF) - IMAGE_MEAN) / IMAGE_STD);
            //        shiftedPixels[i][j][1] = ((((pixelVal >> 8) & 0xFF) - IMAGE_MEAN) / IMAGE_STD);
            //        shiftedPixels[i][j][2] = ((((pixelVal) & 0xFF) - IMAGE_MEAN) / IMAGE_STD);

            //    }
            //}

            Byte[] bli = new byte[4 * width * height * 3];
            int counter = 0;
            foreach (var item in pixels)
            {
                for (int i = 2; i >= 0; i--)
                {
                    var blu = BitConverter.GetBytes(((((item >> i * 8) & 0xFF) - IMAGE_MEAN) / IMAGE_STD));

                    foreach (var muh in blu)
                    {
                        bli[counter] = muh;
                        counter++;
                    }

                }

            }

            return ByteBuffer.Wrap(bli);

            //float[][][][] tensorInputResult = new float[1][][][];
            //tensorInputResult[0] = shiftedPixels;

            //return tensorInputResult;
        }

        public async Task<List<ModelClassification>> ClassifySimilar(byte[] bytes)
        {
            // Debug
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            await Task.Delay(0);

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

            //System.Console.WriteLine(stopWatch.ElapsedMilliseconds + " passed tíme assignments");

            //float[][][][] inputs = ConvertBitmapToByteBuffer(bytes, width, height);
            ByteBuffer inputs = ConvertBitmapToByteBuffer(bytes, width, height);
            //System.Console.WriteLine(stopWatch.ElapsedMilliseconds + " passed tíme ConvertBitmapToByteBuffer");
            Java.Lang.Object[] finalInput = { /*Java.Lang.Object.FromArray(inputs)*/ inputs };

            interpreter.RunForMultipleInputsOutputs(finalInput, outputs);

            //System.Console.WriteLine(stopWatch.ElapsedMilliseconds + " passed time run");

            // Classification Results
            float[][] classificationResult = outputLabelsConverted.ToArray<float[]>();
            float[][][][] featureVectorResult = outputVectorsConverted.ToArray<float[][][]>();

            List<ModelClassification> result = new List<ModelClassification>();
            for (int i = 0; i < labels.Count; i++)
            {
                string label = labels[i];

                result.Add(new ModelClassification(label, classificationResult[0][i]));
            }

            //System.Console.WriteLine(stopWatch.ElapsedMilliseconds + " passed time objects");

            float[] featureVector = featureVectorResult[0][0][0];

            double[] doubleArray = Array.ConvertAll(featureVector, x => (double)x);
            //System.Console.WriteLine(stopWatch.ElapsedMilliseconds + " passed time convert");
            doubleArray = normalizeVector(doubleArray);
            //System.Console.WriteLine(stopWatch.ElapsedMilliseconds + " passed time normalize");
            FeatureVectors.Add(doubleArray);

            var sortedList = result.OrderByDescending(x => x.Probability).ToList();
            sortedList = sortedList.FindAll(x => System.Math.Round(x.Probability * 100, 2) > thresholdSimilar);
            //System.Console.WriteLine(stopWatch.ElapsedMilliseconds + " passed time order find");

            // Notify all listeners
            ClassificationCompleted?.Invoke(this, new ClassificationEventArgs(result));

            // Debug
            stopWatch.Stop();
            //System.Console.WriteLine(stopWatch.ElapsedMilliseconds + " passed time bla");

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

        public List<ModelClassification> ClassifyBlurry(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        //public List<ModelClassification> ClassifyBlurry(byte[] bytes)
        //{
        //    Tensor tensor = interpreter.GetInputTensor(0);

        //    int[] shape = tensor.Shape();
        //    int width = shape[1];
        //    int height = shape[2];
        //    //ByteBuffer byteBuffer = ConvertBitmapToByteBuffer(bytes, width, height);

        //    // Output Labels
        //    float[][] outputLabels = new float[1][] { new float[labels.Count] };
        //    var outputLabelsConverted = Java.Lang.Object.FromArray(outputLabels);


        //    interpreter.Run(ConvertBitmapToByteBuffer(bytes, width, height), outputLabelsConverted);

        //    // Classification Results
        //    float[][] classificationResult = outputLabelsConverted.ToArray<float[]>();

        //    List<ModelClassification> result = new List<ModelClassification>();
        //    for (int i = 0; i < labels.Count; i++)
        //    {
        //        string label = labels[i];

        //        result.Add(new ModelClassification(label, classificationResult[0][i]));
        //    }


        //    var sortedList = result.OrderByDescending(x => x.Probability).ToList();
        //    sortedList = sortedList.FindAll(x => System.Math.Round(x.Probability * 100, 2) > thresholdBlurry);

        //    // Notify all listeners
        //    ClassificationCompleted?.Invoke(this, new ClassificationEventArgs(result));

        //    return sortedList;
        //}

        public void print(string message)
        {
            System.Console.WriteLine(message);
        }

    }
}


