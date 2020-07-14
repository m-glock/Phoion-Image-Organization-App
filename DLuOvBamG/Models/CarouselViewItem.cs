using System.ComponentModel;

namespace DLuOvBamG.Models
{
    class CarouselViewItem : INotifyPropertyChanged
    {
        public int Id;
        public string Uri { get; }
        public double markedForDeletion { get; set; }
        public string ComparingPictureUri { get; }
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
        #endregion

        public CarouselViewItem(int id, string uri, string comparingUri)
        {
            Id = id; 
            Uri = uri;
            ComparingPictureUri = comparingUri;
            MarkedForDeletion = 1;
        }

        public void MarkForDeletion()
        {
            MarkedForDeletion = 0.6;
        }

        public void UnmarkForDeletion()
        {
            MarkedForDeletion = 1;
        }

        public bool IsMarkedForDeletion()
        {
            return MarkedForDeletion != 1;
        }
    }
}
