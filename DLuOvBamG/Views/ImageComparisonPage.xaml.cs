using DLuOvBamG.Models;
using DLuOvBamG.Services.Gestures;
using DLuOvBamG.ViewModels;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DLuOvBamG.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageComparisonPage : ContentPage
    {
        private ImageComparisonViewModel VM;

        public ImageComparisonPage(List<Picture> pictures, Picture mainPic)
        {
            Picture comparingPicture = mainPic;
            List<Picture> pics = new List<Picture>(pictures);
            pics.Remove(comparingPicture); 
            
            VM = new ImageComparisonViewModel();
            VM.PictureList = pics;
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
                    VM.OnSwiped(args);
                    break;
                case TouchActionType.Pressed:
                    //Console.WriteLine("tap started");
                    VM.OnPressedAsync(currentPicture);
                    break;
                case TouchActionType.Released:
                    //Console.WriteLine("tap stopped");
                    VM.OnReleasedAsync(currentPicture);
                    break;
                default:
                    break;
            }
        }
    }
}