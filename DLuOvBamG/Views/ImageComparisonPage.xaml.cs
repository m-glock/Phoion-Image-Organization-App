using Android.Views;
using DLuOvBamG.Models;
using DLuOvBamG.Services.Gestures;
using DLuOvBamG.ViewModels;
using System;
using System.Collections.Generic;
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
                if(!pic.Equals(mainPic)) picsForCarousel.Add(new CarouselViewItem(pic.Uri, comparingPicture.Uri));
            }
            
            VM = new ImageComparisonViewModel();
            BindingContext = VM;
            VM.PictureList = picsForCarousel;
            VM.ComparingPictureUri = comparingPicture.Uri;
            VM.CurrentPictureUri = pictures[0].Uri;

            InitializeComponent();

            VM.CarouselViewMain = ImageMainView;

            if (EnableBackButtonOverride)
            {
                this.CustomBackButtonAction = async () =>
                {
                    //TODO: shorten and only display if images to delet contains elements
                    bool result = await this.DisplayAlert("Careful",
                        "If you go back now, your selection of images to delete will be lost. The images themselves will remain, but you might have to select them again for deletion.",
                        "Go back", "Stay here");

                    if (result)
                    {
                        await Navigation.PopAsync(true);
                    }
                };
            }
        }


        public void OnCurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            CarouselViewItem currentPicture = (CarouselViewItem)e.CurrentItem;
            VM.CurrentPictureUri = currentPicture.Uri;
        }

        public void ImageTouched(object sender, TouchActionEventArgs args)
        {
            Image currentPicture = sender as Image;
            CarouselViewItem currentPictureItem = (CarouselViewItem)ImageMainView.CurrentItem;

            if (!currentPictureItem.IsMarkedForDeletion()) {
                switch (args.Type)
                {
                    /*case TouchActionType.Moved:
                        Console.WriteLine("moved");
                        break;*/
                    case TouchActionType.Pressed:
                        //Console.WriteLine("tap started"); 
                        VM.OnPressedAsync(currentPicture);
                        break;
                    case TouchActionType.Released:
                    case TouchActionType.Cancelled:
                    case TouchActionType.Exited:
                        //Console.WriteLine("tap stopped");
                        VM.OnReleasedAsync(currentPicture);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}