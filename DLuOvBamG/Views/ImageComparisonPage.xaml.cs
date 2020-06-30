using DLuOvBamG.Models;
using DLuOvBamG.Services.Gestures;
using DLuOvBamG.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DLuOvBamG.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageComparisonPage : ContentPage
    {
        private ImageComparisonViewModel VM;
        private Point point;

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
                    Console.WriteLine("Location: " + args.Location);
                    break;
                case TouchActionType.Pressed:
                    Console.WriteLine("tap started");
                    currentPicture.Source = VM.ComparingPictureUri;
                    break;
                case TouchActionType.Released:
                    Console.WriteLine("tap stopped");
                    currentPicture.Source = VM.CurrentPictureUri;
                    break;
                default:
                    break;
            }
        }
    }
}