using System.ComponentModel;

namespace DLuOvBamG.Models
{
    class CarouselViewItem : INotifyPropertyChanged
    {
        public string Uri { get; }
        public double markedForDeletion { get; set; }
        private string ComparingPictureUri;
        public string deleteIcon { get; set; }
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

        public string DeleteIcon
        {
            set
            {
                if (deleteIcon != value)
                {
                    deleteIcon = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DeleteIcon"));
                }
            }
            get
            {
                return deleteIcon;
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

        public CarouselViewItem(string uri, string comparingUri)
        {
            Uri = uri;
            CurrentUri = Uri;
            ComparingPictureUri = comparingUri;
            MarkedForDeletion = 1;
            DeleteIcon = "delete_64px.png";
        }

        public void MarkForDeletion()
        {
            MarkedForDeletion = MarkedForDeletion == 1 ? 0.6 : 1;
            DeleteIcon = MarkedForDeletion == 1 ? "delete_64px.png" : "delete_restore_64px.png";
        }

        public bool IsMarkedForDeletion()
        {
            return MarkedForDeletion != 1;
        }

        public void ChangeURIs()
        {
            if (CurrentUri.Equals(Uri))
            {
                CurrentUri = ComparingPictureUri;
            }
            else
            {
                CurrentUri = Uri;
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