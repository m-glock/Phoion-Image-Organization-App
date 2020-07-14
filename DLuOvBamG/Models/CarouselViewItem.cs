using System.ComponentModel;

namespace DLuOvBamG.Models
{
    class CarouselViewItem : INotifyPropertyChanged
    {
        public int Id;
        public string Uri { get; }
        // double value for opacity of image in UI
        public double markedForDeletion { get; set; }
        private string ComparingPictureUri;
        public string currentUri;
        public bool IsTouched { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        #region property changed
        public double MarkedForDeletion
        {
            set
            {
                if (markedForDeletion != value)
                {
                    markedForDeletion = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MarkedForDeletion"));
                }
            }
            get
            {
                return markedForDeletion;
            }
        }

        public string CurrentUri
        {
            set
            {
                if (currentUri != value)
                {
                    currentUri = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentUri"));
                }
            }
            get
            {
                return currentUri;
            }
        }
        #endregion

        public CarouselViewItem(int id, string uri, string comparingUri)
        {
            Id = id;
            Uri = uri;
            CurrentUri = Uri;
            ComparingPictureUri = comparingUri;
            MarkedForDeletion = 1;
        }

        public void MarkForDeletion()
        {
            MarkedForDeletion = MarkedForDeletion == 1 ? 0.6 : 1;
        }

        public bool IsMarkedForDeletion()
        {
            return MarkedForDeletion != 1;
        }

        public void ChangeURIs()
        {
            if (CurrentUri.Equals(Uri))
            {
                ChangeURIToComparingPicture();
            }
            else
            {
                ChangeURIBackToOriginal();
            }
        }

        public void ChangeURIBackToOriginal()
        {
            CurrentUri = Uri;
        }

        public void ChangeURIToComparingPicture()
        {
            CurrentUri = ComparingPictureUri;
        }
    }
}