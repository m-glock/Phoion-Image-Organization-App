using DLuOvBamG.Models;
using System.Collections.Generic;
using Xamarin.Forms;

namespace DLuOvBamG.ViewModels
{
    class ImageComparisonViewModel
    {
        public List<Picture> PictureList { get; set; }
        /*public Picture CurrentPicture
        {
            get
            {
                return CurrentPicture;
            }
            set
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentPicture"));
            } 
        } */ //Propertychanged
        public string currentPictureUri { get; set; }
        public string comparingPictureUri { get; set; }
        public CarouselView CarouselView { get; set; }
        public List<Picture> PicsToDelete { get; set; }

        public ImageComparisonViewModel()
        {

        }

        /*public ICommand CvCurrentItemChanged()
        {

        }*/

        public void onSwipeLeft()
        {
            //TODO: carousel position -1
        }

        public void onSwipeRight()
        {
            //TODO: carousel position +1
        }

        public void onSwipeDown()
        {
            //TODO: mark picture for deletion
        }
    }
}
