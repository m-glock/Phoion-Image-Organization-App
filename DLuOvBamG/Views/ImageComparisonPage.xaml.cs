using DLuOvBamG.Models;
using DLuOvBamG.Services.Gestures;
using DLuOvBamG.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DLuOvBamG.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageComparisonPage : ContentPage
    {
        private ImageComparisonViewModel VM;
        private Point point;
        private bool stop;

        public ImageComparisonPage(List<Picture> pictures, Picture mainPic)
        {
            VM = new ImageComparisonViewModel();
            
            Picture comparingPicture = mainPic;
            pictures.Remove(comparingPicture);
            VM.PictureList = pictures;
            VM.ComparingPictureUri = comparingPicture.Uri;
            VM.CurrentPictureUri = pictures[0].Uri;

            BindingContext = VM;
            InitializeComponent();

            VM.CarouselView = ImageCarouselView;
        }

        public void OnCurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            Picture currentPicture = (Picture)e.CurrentItem;
            VM.CurrentPictureUri = currentPicture.Uri;
        }

        public void ImageTouched(object sender, TouchActionEventArgs args)
        {
            Image currentPicture = sender as Image;

            switch (args.Type)
            {
                case TouchActionType.Moved:
                    //Console.WriteLine("Location: " + args.Location);
                    Point newPoint = args.Location;
                    if (point != null)
                    {
                        double diff = point.X - newPoint.X;
                        bool right = diff > 0;
                        bool enoughDiff = diff > 100;
                        if (right && enoughDiff) Console.WriteLine("swiped right");
                        //else Console.WriteLine("swiped left");
                    }
                    point = newPoint;
                    break;
                case TouchActionType.Pressed:
                    //Console.WriteLine("tap started");
                    stop = false;
                    ShowBasePic(currentPicture);
                    //currentPicture.Source = VM.ComparingPictureUri;
                    break;
                case TouchActionType.Released:
                    //Console.WriteLine("tap stopped");
                    currentPicture.Source = VM.CurrentPictureUri;
                    stop = true;
                    Console.WriteLine("stop is true");
                    //currentPicture.Source = VM.CurrentPictureUri;
                    break;
                default:
                    break;
            }
        }

        private async void ShowBasePic(Image currentPicture)
        {
            Console.WriteLine("start stopwatch");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (stopwatch.ElapsedMilliseconds >= 2000) //Timer
            {
                Console.WriteLine("time: " + stopwatch.ElapsedMilliseconds);
                if (stop)
                {
                    Console.WriteLine("stop loop");
                    break;
                }
            }

            stopwatch.Stop();
            if (!stop)
            {
                Console.WriteLine("change pic image");
                currentPicture.Source = VM.ComparingPictureUri;
            }
        }
    }
}