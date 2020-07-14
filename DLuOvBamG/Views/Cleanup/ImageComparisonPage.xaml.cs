using DLuOvBamG.Models;
using DLuOvBamG.Services.Gestures;
using DLuOvBamG.ViewModels;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DLuOvBamG.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageComparisonPage : CustomBackButtonPage
    {
        private ImageComparisonViewModel VM;

        public ImageComparisonPage(List<Picture> pictures, Picture mainPic)
        {
            Picture comparingPicture = mainPic;

            List<CarouselViewItem> picsForCarousel = new List<CarouselViewItem>();
            foreach (Picture pic in pictures)
            {
                if (!pic.Equals(mainPic)) picsForCarousel.Add(new CarouselViewItem(pic.Id, pic.Uri, comparingPicture.Uri));
            }

            VM = new ImageComparisonViewModel(this, picsForCarousel);
            BindingContext = VM;

            InitializeComponent();

            // get carousel view to proper size depending on the screen size
            DisplayInfo mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            double width = mainDisplayInfo.Width / mainDisplayInfo.Density;
            ImageCarouselView.PeekAreaInsets = width < 350 ? 100 : 135;

            VM.CarouselViewMain = ImageMainView;

            // show safety alert when clicking the navigation back button
            if (EnableBackButtonOverride)
            {
                this.CustomBackButtonAction = () =>
                {
                    VM.ShowAlertSelectionLost();
                };
            }
        }

        /*
         * check what kind of touch has been registered and call the right method in ViewModel
         */
        public void ImageTouched(object sender, TouchActionEventArgs args)
        {
            CarouselViewItem currentPictureItem = (CarouselViewItem)ImageMainView.CurrentItem;

            if (!currentPictureItem.IsMarkedForDeletion())
            {
                switch (args.Type)
                {
                    case TouchActionType.Pressed:
                        VM.OnPressedAsync(currentPictureItem);
                        break;
                    case TouchActionType.Released:
                    case TouchActionType.Cancelled:
                        VM.OnReleasedAsync(currentPictureItem);
                        break;
                    default:
                        break;
                }
            }
        }

        /*
         * always revert from comparing picture back to actual image when swiping to the next element
         * change trash icon depending on whether the current image is marked to be deleted or not
         * delete_64px.png: 
         * delete_restore_64px.png: customisation of delete_64px.png
         */
        private void CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            // make sure that picture from set is displayed after swipe
            CarouselViewItem previousPicture = (CarouselViewItem)e.PreviousItem;
            if (previousPicture != null)
            {
                previousPicture.ChangeURIBackToOriginal();
                previousPicture.IsTouched = false;
            }

            // decide whether to show checkbox for deletion as amrked or unmarked
            CarouselViewItem currentItem = (CarouselViewItem)e.CurrentItem;
            DeleteCheckbox.CheckedChanged -= DeleteCheckboxChecked;
            DeleteCheckbox.IsChecked = currentItem.IsMarkedForDeletion();
            DeleteCheckbox.CheckedChanged += DeleteCheckboxChecked;
            
        }

        /*
         * mark or unmark the picture for deletion
         */
        private void DeleteCheckboxChecked(object sender, EventArgs e)
        {
            CarouselViewItem currentPicture = (CarouselViewItem)ImageMainView.CurrentItem;
            currentPicture.MarkForDeletion();
            VM.AddMarkedPictureToDeleteList(currentPicture);
        }
    }
}