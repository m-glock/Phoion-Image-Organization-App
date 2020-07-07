namespace DLuOvBamG.Models
{
    class CarouselViewItem
    {
        public string Uri { get; }
        public bool MarkedForDeletion { get; set; }

        public CarouselViewItem(string uri)
        {
            Uri = uri;
            MarkedForDeletion = false;
        }
    }
}
