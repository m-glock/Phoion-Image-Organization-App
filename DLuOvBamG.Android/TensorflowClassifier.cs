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

[assembly: Dependency(typeof(DLuOvBamG.Droid.TensorflowClassifier))]
namespace DLuOvBamG.Droid
{
    public class TensorflowClassifier : IClassifier
    {
        private readonly Interpreter interpreter;
        private readonly List<string> labels;

        private float AcceptableResultPercentage = 20;

        public event EventHandler<ClassificationEventArgs> ClassificationCompleted;

        public TensorflowClassifier()
        {
            interpreter = new Interpreter(GetByteBuffer("mobilenet_v1_1.0_224.tflite"));
            labels = LoadLabelList("mobilenet_v1_1.0_224.txt");
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

        public void test()
        {
            string[] listAssets = Android.App.Application.Context.Assets.List("stockImages");
            foreach (var entry in listAssets)
            {
                byte[] image = GetImageBytes(entry);
                var sortedList = Classify(image);

                if (sortedList.Count > 0)
                {
                    ModelClassification top = sortedList.First();
                    System.Console.WriteLine(top.TagName + " " + Math.Round(top.Probability * 100, 2) + "% ultra result");
                }
                foreach (ModelClassification item in sortedList)
                {
                    System.Console.WriteLine(item.TagName + " " + Math.Round(item.Probability * 100, 2) + "% result");
                }
            }
        }

        private byte[] GetImageBytes(string path)
        {
            AssetFileDescriptor assetDescriptor = Android.App.Application.Context.Assets.OpenFd("stockImages/" + path);
            Stream stream = assetDescriptor.CreateInputStream();
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

        #endregion


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

        public List<ModelClassification> Classify(byte[] bytes)
        {
            Tensor tensor = interpreter.GetInputTensor(0);
            int[] shape = tensor.Shape();
            int width = shape[1];
            int height = shape[2];
            ByteBuffer byteBuffer = ConvertBitmapToByteBuffer(bytes, width, height);
            float[][] outputLocations = new float[1][] { new float[labels.Count] };
            var outputs = Java.Lang.Object.FromArray(outputLocations);

            interpreter.Run(byteBuffer, outputs);

            float[][] classificationResult = outputs.ToArray<float[]>();
            List<ModelClassification> result = new List<ModelClassification>();
            for (int i = 0; i < labels.Count; i++)
            {
                string label = labels[i];
                result.Add(new ModelClassification(label, classificationResult[0][i]));
            }

            var sortedList = result.OrderByDescending(x => x.Probability).ToList();
            sortedList = sortedList.FindAll(x => Math.Round(x.Probability * 100, 2) > AcceptableResultPercentage);

            // Notify all listeners
            ClassificationCompleted?.Invoke(this, new ClassificationEventArgs(result));

            return sortedList;
        }

    }
}


