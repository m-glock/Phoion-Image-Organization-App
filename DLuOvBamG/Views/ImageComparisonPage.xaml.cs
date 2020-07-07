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
    public partial class ImageComparisonPage : ContentPage
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
            VM.PictureList = picsForCarousel;
            VM.ComparingPictureUri = comparingPicture.Uri;
            VM.CurrentPictureUri = pictures[0].Uri;
            BindingContext = VM;

            InitializeComponent();

            /*CarouselView carouselView = new CarouselView();
            carouselView.SetBinding(ItemsView.ItemsSourceProperty, "PictureList");
            carouselView.SetBinding(CarouselView.PositionProperty, "CarouselViewPosition");
            carouselView.CurrentItemChanged += OnCurrentItemChanged;
            carouselView.ItemTemplate = new DataTemplate(() =>
            {
                TouchImage touchImage = new TouchImage();
                touchImage.SetBinding(TouchImage.RegularBackgroundImageSourceProperty, "Uri");
                touchImage.SetBinding(TouchImage.PressedBackgroundImageSourceProperty, "ComparingPictureUri");
                touchImage. = TouchEff;

                return contentView;
            });
            Grid.Children.Add(carouselView);
            Grid.SetRow(carouselView, 1);*/

        }


        public void OnCurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            CarouselViewItem currentPicture = (CarouselViewItem)e.CurrentItem;
            VM.CurrentPictureUri = currentPicture.Uri;
        }

        public void ImageTouched(object sender, TouchActionEventArgs args)
        {
            Image currentPicture = sender as Image;

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