using DLuOvBamG.Models;
using DLuOvBamG.Services.Gestures;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DLuOvBamG.ViewModels
{
    class ImageComparisonViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public List<CarouselViewItem> PictureList { get; set; }
        public CarouselView CarouselViewMain { get; set; }
        private int carouselViewPosition { get; set; }
        private List<CarouselViewItem> PicsToDelete { get; set; }
        private INavigation Navigation;
        private bool stop;
        public event PropertyChangedEventHandler PropertyChanged;
        //private double firstPoint = -1;
        //private int pointerCounter;

        //private bool pauseSwiping;

        #region PropertyChanged
        public int CarouselViewPosition
        {
            set
            {
                if (carouselViewPosition != value)
                {
                    carouselViewPosition = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CarouselViewPosition"));
                }
            }
            get
            {
                return carouselViewPosition;
            }
        }
        #endregion

        public ImageComparisonViewModel(INavigation navigation, List<CarouselViewItem> picsForCarousel)
        {
            Navigation = navigation;
            PictureList = picsForCarousel;
            PicsToDelete = new List<CarouselViewItem>();
        }

        public async void ShowAlertSelectionLost(Page imageComparionPage)
        {
            bool result = await imageComparionPage.DisplayAlert("Careful",
                "If you go back now without deleting the selected pictures, your selection will be lost.",
                "Go back", "Stay here");

            if (result)
            {
                await Navigation.PopAsync(true);
            }
        }

        public async Task OnPressedAsync(Image currentPicture)
        {
            stop = false;
            await ShowBasePic(currentPicture);
        }

        public void OnReleasedAsync(Image currentPicture)
        {
            CarouselViewItem currentItem = (CarouselViewItem)CarouselViewMain.CurrentItem;
            currentPicture.Source = currentItem.Uri;
            stop = true;
        }

        private async Task ShowBasePic(Image currentPicture)
        {
            await Task.Delay(1000);
            if (!stop)
            {
                Console.WriteLine("successful long tap");
                CarouselViewItem currentItem = (CarouselViewItem)CarouselViewMain.CurrentItem;
                currentPicture.Source = currentItem.ComparingPictureUri;
            } else
            {
                Console.WriteLine("unsuccessful long tap");
            }
                
        }

        public ICommand MarkPictureAsDeleted
        {
            get
            {
                return new Command(() =>
                {
                    CarouselViewItem currentPicture = (CarouselViewItem)CarouselViewMain.CurrentItem;
                    currentPicture.MarkForDeletion();
                    if (!PicsToDelete.Contains(currentPicture))
                    {
                        PicsToDelete.Add(currentPicture);
                    }

                });
            }
        }



        /*public async void OnSwiped(TouchActionEventArgs args)
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
                    await Task.Delay(1000);
                }
                firstPoint = -1;
                pointerCounter = 0;
            }
        }

        private void SwipeRight()
        {
            Console.WriteLine("swipe left " + CarouselViewPosition);
            if (CarouselViewPosition < PictureList.Count - 1)
            {
                CarouselViewPosition += 1;
            }
        }

        private void SwipeLeft()
        {
            Console.WriteLine("swipe right " + CarouselViewPosition);
            if (CarouselViewPosition > 0)
            {
                CarouselViewPosition -= 1;
            }
        }

        private void SwipeDown()
        {
            // Handle the swipe
            //CarouselViewItem picToDelete = PictureList.Find(pic => pic.Uri == CurrentPictureUri);
            //PicsToDelete.Add(picToDelete);
            //picToDelete.Id;
        }*/
    }
}
