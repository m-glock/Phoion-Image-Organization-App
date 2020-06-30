using DLuOvBamG.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
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
            PicsToDelete = new List<Picture>();
        }

        public ICommand SwipeLeft => new Command( () =>
        {
            Console.WriteLine("left swipe");
            CarouselView.Position = CarouselView.Position + 1;
        });

        public ICommand SwipeRight => new Command( () =>
        {
            Console.WriteLine("right swipe");
            CarouselView.Position = CarouselView.Position - 1;
        });

        public ICommand SwipeDown => new Command( () =>
        {
            // Handle the swipe
            Picture picToDelete = PictureList.Find(pic => pic.Uri == CurrentPictureUri);
            PicsToDelete.Add(picToDelete);
            Console.WriteLine("Down swipe");
        });
    }
}
