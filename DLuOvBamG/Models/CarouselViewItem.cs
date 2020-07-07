namespace DLuOvBamG.Models
{
    class CarouselViewItem
    {
        public string Uri { get; }
        public bool MarkedForDeletion { get; set; }
        public string ComparingPictureUri { get; }

        public CarouselViewItem(string uri, string comparingUri)
        {
            Uri = uri;
            ComparingPictureUri = comparingUri;
            MarkedForDeletion = false;
        }
    }
}
