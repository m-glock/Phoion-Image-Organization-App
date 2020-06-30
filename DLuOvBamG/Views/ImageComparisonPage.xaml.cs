using DLuOvBamG.Models;
using DLuOvBamG.Services.Gestures;
using DLuOvBamG.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DLuOvBamG.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageComparisonPage : ContentPage
    {
        private ImageComparisonViewModel VM;
        private double fistPoint = -1;
        private int pointerCounter;
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

        public async void ImageTouched(object sender, TouchActionEventArgs args)
        {
            Image currentPicture = sender as Image;

            switch (args.Type)
            {
                case TouchActionType.Moved:
                    pointerCounter++;
                    Console.WriteLine(pointerCounter);
                    if (fistPoint == -1)
                        fistPoint = args.Location.X;
                    
                    if (pointerCounter >= 10)
                    {
                        double currentPoint = args.Location.X;
                        double diff = fistPoint - currentPoint;
                        bool right = diff > 0;
                        double devicewidth = DeviceDisplay.MainDisplayInfo.Width;
                        bool enoughDiff = diff > devicewidth / 5;
                        if (right && enoughDiff)
                        {
                            VM.SwipeRight();
                        } 
                        else if (!right && enoughDiff)
                        {
                            VM.SwipeLeft();
                        }
                        fistPoint = -1;
                        pointerCounter = 0;
                    }

                    break;
                case TouchActionType.Pressed:
                    //Console.WriteLine("tap started");
                    stop = false;
                    await ShowBasePic(currentPicture);
                    //currentPicture.Source = VM.ComparingPictureUri;
                    break;
                case TouchActionType.Released:
                    //Console.WriteLine("tap stopped");
                    currentPicture.Source = VM.CurrentPictureUri;
                    stop = true;
                    //Console.WriteLine("stop is true");
                    //currentPicture.Source = VM.CurrentPictureUri;
                    break;
                default:
                    break;
            }
        }

        public async Task ShowBasePic(Image currentPicture)
        {
            await Task.Delay(1000);
            if(!stop)
                currentPicture.Source = VM.ComparingPictureUri;

        }

    }
}