using DLuOvBamG.Models;
using DLuOvBamG.Services.Gestures;
using DLuOvBamG.Views;
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
        public int amountOfDeletedPics { get; set; }
        private ImageComparisonPage ImageComparisonPage;
        public event PropertyChangedEventHandler PropertyChanged;

        #region PropertyChanged
        // update carousel view position for both carousel views at the same time
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

        public int AmountOfDeletedPics
        {
            set
            {
                if (amountOfDeletedPics != value)
                {
                    amountOfDeletedPics = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AmountOfDeletedPics"));
                }
            }
            get
            {
                return amountOfDeletedPics;
            }
        }
        #endregion
        public ImageComparisonViewModel(ImageComparisonPage page, List<CarouselViewItem> picsForCarousel)
        {
            ImageComparisonPage = page;
            PictureList = picsForCarousel;
            PicsToDelete = new List<CarouselViewItem>();
        }

        public async void ShowAlertSelectionLost()
        {
            // if there are pictures to be deleted, make sure to let the user know his selection willbe lost if they leave the site
            if (PicsToDelete.Count > 0)
            {
                bool result = await ImageComparisonPage.DisplayAlert("Careful",
                    "If you go back now without deleting the selected pictures, your selection will be lost.",
                    "Go back", "Stay here");

                if (result)
                {
                    await ImageComparisonPage.Navigation.PopAsync(true);
                }
            }
            else
            {
                await ImageComparisonPage.Navigation.PopAsync(true);
            }
        }

        public async Task OnPressedAsync(CarouselViewItem current)
        {
            current.IsTouched = true;
            await ShowBasePic(current);
        }

        public void OnReleasedAsync(CarouselViewItem current)
        {
            CarouselViewItem currentItem = (CarouselViewItem)CarouselViewMain.CurrentItem;
            currentItem.ChangeURIBackToOriginal();
            current.IsTouched = false;
        }

        /*
         * Displays the comparing image in the background if the touch has been long enough
         */
        private async Task ShowBasePic(CarouselViewItem current)
        {
            await Task.Delay(1000);
            if (current.IsTouched)
            {
                Console.WriteLine("successful long tap");
                CarouselViewItem currentItem = (CarouselViewItem)CarouselViewMain.CurrentItem;
                currentItem.ChangeURIToComparingPicture();
            }
            else
            {
                Console.WriteLine("unsuccessful long tap");
            }
        }

        public void AddMarkedPictureToDeleteList(CarouselViewItem item)
        {
            if (PicsToDelete.Contains(item)) PicsToDelete.Remove(item);
            else PicsToDelete.Add(item);
            AmountOfDeletedPics = PicsToDelete.Count;
        }

        public ICommand DeletePictures
        {
            get
            {
                return new Command(async () =>
                {
                    if (PicsToDelete.Count > 0)
                    {
                        // ask user whether they are sure to delete all the images
                        bool result = await ImageComparisonPage.DisplayAlert("Are you sure?",
                            "Do you really want to delete " + PicsToDelete.Count + " pictures?",
                                "Delete", "Go Back");

                        if (result)
                        {
                            // TODO Delete Pictures (-> Picture, not CarouselViewItem)

                            PictureList.RemoveAll(item => PicsToDelete.Contains(item));
                            PicsToDelete.Clear();

                            // if set has now less than 2 pictures (+ comparison picture), go back to previous page
                            if (PictureList.Count < 2)
                            {
                                // TODO: remove this set 
                                await ImageComparisonPage.Navigation.PopAsync();
                            }
                        }
                    }
                });
            }
        }
    }
}