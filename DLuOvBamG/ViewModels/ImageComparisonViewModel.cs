using DLuOvBamG.Models;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace DLuOvBamG.ViewModels
{
    class ImageComparisonViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public List<Picture> PictureList { get; set; }
        public string currentPictureUri { get; set; }
        public string comparingPictureUri { get; set; }
        public CarouselView CarouselView { get; set; }
        public List<Picture> PicsToDelete { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        #region PropertyChanged
        public string CurrentPictureUri
        {
            set
            {
                if (currentPictureUri != value)
                {
                    currentPictureUri = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentPictureUri"));
                }
            }
            get
            {
                return currentPictureUri;
            }
        }

        public string ComparingPictureUri
        {
            set
            {
                if (comparingPictureUri != value)
                {
                    comparingPictureUri = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ComparingPictureUri"));
                }
            }
            get
            {
                return comparingPictureUri;
            }
        }
        #endregion

        public ImageComparisonViewModel()
        {

        }

        /*public ICommand CvCurrentItemChanged()
        {

        }*/

        public void OnSwipeLeft()
        {
            //TODO: carousel position -1
        }

        public void OnSwipeRight()
        {
            //TODO: carousel position +1
        }

        public void OnSwipeDown()
        {
            //TODO: mark picture for deletion
        }
    }
}
