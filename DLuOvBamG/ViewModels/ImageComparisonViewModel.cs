using DLuOvBamG.Models;
using DLuOvBamG.Services.Gestures;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DLuOvBamG.ViewModels
{
    class ImageComparisonViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public List<Picture> PictureList { get; set; }
        public string currentPictureUri { get; set; }
        public string comparingPictureUri { get; set; }
        public CarouselView CarouselView { get; set; }
        public List<Picture> PicsToDelete { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        private double firstPoint = -1;
        private int pointerCounter;
        private bool stop;
        private bool pauseSwiping;

        #region PropertyChanged
        public string CurrentPictureUri
        {
            set
            {
                if (currentPictureUri != value)
                {
                    currentPictureUri = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentPictureUri"));
                }
            }
            get
            {
                return currentPictureUri;
            }
        }

        public string ComparingPictureUri
        {
            set
            {
                if (comparingPictureUri != value)
                {
                    comparingPictureUri = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ComparingPictureUri"));
                }
            }
            get
            {
                return comparingPictureUri;
            }
        }
        #endregion

        public ImageComparisonViewModel()
        {
            PicsToDelete = new List<Picture>();
        }

        public async Task OnPressedAsync(Image currentPicture)
        {
            stop = false;
            await ShowBasePic(currentPicture);
        }

        public async Task OnReleasedAsync(Image currentPicture)
        {
            await Task.Delay(500); 
            currentPicture.Source = CurrentPictureUri;
            stop = true;
        }

        public async void OnSwiped(TouchActionEventArgs args)
        {
            if (pauseSwiping) return;
            pointerCounter++;
            Console.WriteLine(pointerCounter);
            if (firstPoint == -1)
                firstPoint = args.Location.X;

            if (pointerCounter >= 7)
            {
                double diff = firstPoint - args.Location.X;
                double devicewidth = DeviceDisplay.MainDisplayInfo.Width;
                //Console.WriteLine(devicewidth + "device width");
                bool enoughDiff = Math.Abs(diff) > 30;
                Console.WriteLine(diff + " diff");
                if (enoughDiff) 
                {
                    if (diff > 0) SwipeRight();
                    else SwipeLeft();
                    pauseSwiping = true;
                    await Task.Delay(1000);
                    pauseSwiping = false;
                }
                firstPoint = -1;
                pointerCounter = 0;
            }
        }

        private async Task ShowBasePic(Image currentPicture)
        {
            await Task.Delay(1000);
            if (!stop)
                currentPicture.Source = ComparingPictureUri;
        }

        private void SwipeRight()
        {
            Console.WriteLine("swipe left " + CarouselView.Position);
            if (CarouselView.Position < PictureList.Count - 1)
            {
                CarouselView.Position = CarouselView.Position + 1;
            }
        }

        private void SwipeLeft()
        {
            Console.WriteLine("swipe right " + CarouselView.Position);
            if (CarouselView.Position > 0)
            {
                CarouselView.Position = CarouselView.Position - 1;
            }
        }

        private void SwipeDown()
        {
            // Handle the swipe
            Picture picToDelete = PictureList.Find(pic => pic.Uri == CurrentPictureUri);
            PicsToDelete.Add(picToDelete);
        }
    }
}
