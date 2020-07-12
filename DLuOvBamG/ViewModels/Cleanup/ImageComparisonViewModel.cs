﻿using DLuOvBamG.Models;
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
        public ImageButton BinImage {get; set;}
        private int carouselViewPosition { get; set; }
        private List<CarouselViewItem> PicsToDelete { get; set; }
        private ImageComparisonPage ImageComparisonPage;
        private bool stop;
        public event PropertyChangedEventHandler PropertyChanged;
        private double firstPoint = -1;
        private int pointerCounter;

        //private bool pauseSwiping;

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
            if (PicsToDelete.Count > 0) {
                bool result = await ImageComparisonPage.DisplayAlert("Careful",
                    "If you go back now without deleting the selected pictures, your selection will be lost.",
                    "Go back", "Stay here");

                if (result)
                {
                    await ImageComparisonPage.Navigation.PopAsync(true);
                }
            } else
            {
                await ImageComparisonPage.Navigation.PopAsync(true);
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

        /*
         * Displays the comparing image in the background if the touch has been long enough
         */
        private async Task ShowBasePic(Image currentPicture)
        {
            await Task.Delay(1000);
            if (!stop)
            {
                Console.WriteLine("successful long tap");
                CarouselViewItem currentItem = (CarouselViewItem)CarouselViewMain.CurrentItem;
                currentPicture.Source = currentItem.ComparingPictureUri;
            }
            else
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
                    // (un)mark current picture to be deleted and update the list of pictures to be deleted
                    CarouselViewItem currentPicture = (CarouselViewItem)CarouselViewMain.CurrentItem;
                    currentPicture.MarkForDeletion();
                    if (PicsToDelete.Contains(currentPicture))
                    {
                        PicsToDelete.Remove(currentPicture);
                    } 
                    else
                    {
                        PicsToDelete.Add(currentPicture);
                    }
                    //update the icon
                    BinImage.Source = currentPicture.IsMarkedForDeletion() ? "delete_restore_64px.png" : "delete_64px.png";
                });
            }
        }

        public ICommand DeletePictures
        {
            get
            {
                return new Command( async(object imageComparionPage) =>
                {
                    // ask user whether they are sure to delete all the images
                    bool result = await ((Page)imageComparionPage).DisplayAlert("Are you sure?",
                        "Do you really want to delete " + PicsToDelete.Count + " pictures?",
                            "Delete", "Go Back");

                    if (result)
                    {
                        //TODO Delete Pictures
                        // if set has now less than 3 pictures, remove set?
                    }
                });
            }
        }

        public async void OnSwiped(TouchActionEventArgs args)
        {
            //if (pauseSwiping) return;
            pointerCounter++;
            Console.WriteLine(pointerCounter);
            if (firstPoint == -1)
                firstPoint = args.Location.Y;
            if (pointerCounter >= 7)
            {
                double diff = firstPoint - args.Location.Y;
                double devicewidth = DeviceDisplay.MainDisplayInfo.Width;
                //Console.WriteLine(devicewidth + "device width");
                bool enoughDiff = diff < -30;
                Console.WriteLine(diff + " diff");
                if (enoughDiff) 
                {
                    Console.WriteLine("enough difference");
                    SwipeDown();
                    /*pauseSwiping = true;
                    await Task.Delay(1000);
                    pauseSwiping = false;
                    await Task.Delay(1000);*/
                }
                firstPoint = -1;
                pointerCounter = 0;
            }
        }

        /*private void SwipeRight()
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
        }*/

        private void SwipeDown()
        {
            // Handle the swipe
            //CarouselViewItem picToDelete = PictureList.Find(pic => pic.Uri == CurrentPictureUri);
            //PicsToDelete.Add(picToDelete);
            //picToDelete.Id;
            Console.WriteLine("swiped down.");
        }
    }
}