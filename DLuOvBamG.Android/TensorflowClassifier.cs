using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
<<<<<<< Updated upstream

using Android.App;
using Android.Content;
=======
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.Res;
>>>>>>> Stashed changes
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
<<<<<<< Updated upstream
using Java.IO;
using Java.Nio;
using Java.Nio.Channels;

namespace DLuOvBamG.Droid
{
    public class TensorflowClassifier
    {
        //FloatSize is a constant with the value of 4 because a float value is 4 bytes
        const int FloatSize = 4;
        //PixelSize is a constant with the value of 3 because a pixel has three color channels: Red Green and Blue
        const int PixelSize = 3;

        public List<ImageClassificationModel> Classify(byte[] image)
        {
            var mappedByteBuffer = GetModelAsMappedByteBuffer();
            var interpreter = new Xamarin.TensorFlow.Lite.Interpreter(mappedByteBuffer);

            //To resize the image, we first need to get its required width and height
            var tensor = interpreter.GetInputTensor(0);
            var shape = tensor.Shape();

            var width = shape[1];
            var height = shape[2];

            var byteBuffer = GetPhotoAsByteBuffer(image, width, height);

            //use StreamReader to import the labels from labels.txt
            var streamReader = new StreamReader(Application.Context.Assets.Open("labels.txt"));

            //Transform labels.txt into List<string>
            var labels = streamReader.ReadToEnd().Split('\n').Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToList();

            //Convert our two-dimensional array into a Java.Lang.Object, the required input for Xamarin.TensorFlow.List.Interpreter
            var outputLocations = new float[1][] { new float[labels.Count] };
            var outputs = Java.Lang.Object.FromArray(outputLocations);

            interpreter.Run(byteBuffer, outputs);
            var classificationResult = outputs.ToArray<float[]>();

            //Map the classificationResult to the labels and sort the result to find which label has the highest probability
            var classificationModelList = new List<ImageClassificationModel>();

            for (var i = 0; i < labels.Count; i++)
            {
                var label = labels[i]; classificationModelList.Add(new ImageClassificationModel(label, classificationResult[0][i]));
            }

            return classificationModelList;
        }

        //Convert model.tflite to Java.Nio.MappedByteBuffer , the require type for Xamarin.TensorFlow.Lite.Interpreter
        private MappedByteBuffer GetModelAsMappedByteBuffer()
        {
            var assetDescriptor = Application.Context.Assets.OpenFd("model.tflite");
            var inputStream = new FileInputStream(assetDescriptor.FileDescriptor);

            var mappedByteBuffer = inputStream.Channel.Map(FileChannel.MapMode.ReadOnly, assetDescriptor.StartOffset, assetDescriptor.DeclaredLength);

            return mappedByteBuffer;
        }

        //Resize the image for the TensorFlow interpreter
        private ByteBuffer GetPhotoAsByteBuffer(byte[] image, int width, int height)
        {
            var bitmap = BitmapFactory.DecodeByteArray(image, 0, image.Length);
            var resizedBitmap = Bitmap.CreateScaledBitmap(bitmap, width, height, true);

            var modelInputSize = FloatSize * height * width * PixelSize;
            var byteBuffer = ByteBuffer.AllocateDirect(modelInputSize);
            byteBuffer.Order(ByteOrder.NativeOrder());

            var pixels = new int[width * height];
            resizedBitmap.GetPixels(pixels, 0, resizedBitmap.Width, 0, 0, resizedBitmap.Width, resizedBitmap.Height);

            var pixel = 0;

            //Loop through each pixels to create a Java.Nio.ByteBuffer
            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    var pixelVal = pixels[pixel++];

                    byteBuffer.PutFloat(pixelVal >> 16 & 0xFF);
                    byteBuffer.PutFloat(pixelVal >> 8 & 0xFF);
                    byteBuffer.PutFloat(pixelVal & 0xFF);
                }
            }

            bitmap.Recycle();

            return byteBuffer;
=======
using DLuOvBamG.Models;
using Java.IO;
using Java.Nio;
using Java.Nio.Channels;
using Org.Tensorflow.Lite;

namespace DLuOvBamG.Droid
{
    class TensorflowClassifier : IClassifier
    {
        Interpreter interpreter;
        List<string> labels;
        byte[] imageBytes;

        public event EventHandler<ClassificationEventArgs> ClassificationCompleted;

        public void test()
        {
            interpreter = new Interpreter(GetByteBuffer("mobilenet_v1_1.0_224.tflite"));
            labels = LoadLabelList();
            imageBytes = GetImageBytes();
            Classify();
        }

        private ByteBuffer GetByteBuffer(string path)
        {
            AssetFileDescriptor assetDescriptor = Android.App.Application.Context.Assets.OpenFd(path);
            FileInputStream inputStream = new FileInputStream(assetDescriptor.FileDescriptor);

            ByteBuffer mappedByteBuffer = inputStream.Channel.Map(FileChannel.MapMode.ReadOnly, assetDescriptor.StartOffset, assetDescriptor.DeclaredLength);

            return mappedByteBuffer;
        }

        private List<string> LoadLabelList()
        {
            StreamReader sr = new StreamReader(Android.App.Application.Context.Assets.Open("mobilenet_v1_1.0_224.txt"));
            List<string> labels = sr.ReadToEnd().Split('\n').Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToList();
            //System.Console.WriteLine(String.Join("\n", labels));
            return labels;
        }

        private byte[] GetImageBytes()
        {
            AssetFileDescriptor assetDescriptor = Android.App.Application.Context.Assets.OpenFd("catdog.jpg");
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

        private ByteBuffer convertBitmapToByteBuffer(byte[] bytes, int height, int width)
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

        private void Classify()
        {
            Tensor tensor = interpreter.GetInputTensor(0);
            int[] shape = tensor.Shape();
            int width = shape[1];
            int height = shape[2];
            //var byteBuffer = GetPhotoAsByteBuffer(imageBytes, width, height);
            ByteBuffer byteBuffer = convertBitmapToByteBuffer(imageBytes, width, height);
            float[][] outputLocations = new float[1][] { new float[labels.Count] };
            var outputs = Java.Lang.Object.FromArray(outputLocations);
            interpreter.Run(byteBuffer,

                outputs);
            float[][] ModelClassificationResult = outputs.ToArray<float[]>();
            List<ModelClassification> result = new List<ModelClassification>();
            for (int i = 0; i < labels.Count; i++)
            {
                string label = labels[i];
                result.Add(new ModelClassification(label, ModelClassificationResult[0][i]));
            }

            IOrderedEnumerable<ModelClassification> sortedList = result.OrderByDescending(x => x.Probability);
            ModelClassification top = sortedList.First();
            System.Console.WriteLine(top.TagName + " " + Math.Round(top.Probability * 100, 2) + "% ultra result");
            foreach (ModelClassification item in sortedList)
            {
                System.Console.WriteLine(item.TagName + " " + Math.Round(item.Probability * 100, 2) + "% result");
            }
        }

        public Task Classify(byte[] bytes)
        {
            throw new NotImplementedException();
>>>>>>> Stashed changes
        }
    }
}