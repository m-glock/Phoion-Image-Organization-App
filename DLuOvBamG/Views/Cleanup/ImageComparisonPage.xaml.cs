using DLuOvBamG.Models;
<<<<<<< HEAD
using DLuOvBamG.Services.Gestures;
using DLuOvBamG.ViewModels;
using System.Collections.Generic;
=======
using DLuOvBamG.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
>>>>>>> develop
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DLuOvBamG.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
<<<<<<< HEAD
    public partial class ImageComparisonPage : CustomBackButtonPage
=======
    public partial class ImageComparisonPage : ContentPage
>>>>>>> develop
    {
        private ImageComparisonViewModel VM;

        public ImageComparisonPage(List<Picture> pictures, Picture mainPic)
        {
            Picture comparingPicture = mainPic;
<<<<<<< HEAD
            List<CarouselViewItem> picsForCarousel = new List<CarouselViewItem>();
            
            foreach (Picture pic in pictures)
            {
                if(!pic.Equals(mainPic)) picsForCarousel.Add(new CarouselViewItem(pic.Uri, comparingPicture.Uri));
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
=======
            List<Picture> temp = new List<Picture>(pictures);
            //List<CarouselViewItem> pics = temp.Cast()
            //pics.Remove(comparingPicture); 
            
            VM = new ImageComparisonViewModel();
            //VM.PictureList = pics;
            VM.ComparingPictureUri = comparingPicture.Uri;
            VM.CurrentPictureUri = pictures[0].Uri;
            BindingContext = VM;
            
            InitializeComponent();
        }


        public void OnCurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            Picture currentPicture = (Picture)e.CurrentItem;
            VM.CurrentPictureUri = currentPicture.Uri;
        }

        /*public void ImageTouched(object sender, TouchActionEventArgs args)
        {
            Image currentPicture = sender as Image;
            
            switch (args.Type)
            {
                case TouchActionType.Moved:
                    Console.WriteLine("moved");
                    VM.OnSwiped(args);
                    break;
                case TouchActionType.Pressed:
                    //Console.WriteLine("tap started");
                    VM.OnPressedAsync(currentPicture);
                    break;
                case TouchActionType.Released:
                //case TouchActionType.Cancelled:
                case TouchActionType.Exited:
                    Console.WriteLine("tap stopped");
                    VM.OnReleasedAsync(currentPicture);
                    break;
                default:
                    break;
            }
        }*/
>>>>>>> develop
    }
}