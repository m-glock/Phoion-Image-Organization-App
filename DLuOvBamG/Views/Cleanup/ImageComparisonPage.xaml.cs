using DLuOvBamG.Models;
using DLuOvBamG.Services.Gestures;
using DLuOvBamG.ViewModels;
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
                if(!pic.Equals(mainPic)) picsForCarousel.Add(new CarouselViewItem(pic.Id, pic.Uri, comparingPicture.Uri));
            }
            
            VM = new ImageComparisonViewModel(Navigation, picsForCarousel);
            BindingContext = VM;

            InitializeComponent();

            VM.CarouselViewMain = ImageMainView;

            if (EnableBackButtonOverride)
            {
                this.CustomBackButtonAction = () =>
                {
                    VM.ShowAlertSelectionLost(this);
                };
            }
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