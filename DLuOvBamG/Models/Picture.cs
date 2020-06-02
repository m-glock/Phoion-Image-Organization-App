using DLuOvBamG.Services;
using System;

using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace DLuOvBamG.Models
{
    public class Picture
    {
        public string Uri { get; set; }

        public ImageSource ImageSource { get; set; }
        public string Id { get; set; }

        public DateTime Date { get; set; }

        public Picture(string Uri, string Id)
        {
            this.Uri = Uri;
            this.Id = Id;
            this.Date = GetDate(Uri);
        }

        public Picture(string Uri, string Id,  Stream ImageData)
        {
            this.Uri = Uri;
            this.Id = Id;
            this.Date = GetDate(Uri);
            this.ImageSource = ImageSource.FromStream(() => ImageData);
        }

        private DateTime GetDate(string Uri)
        {
            try
            {
                IImageService imageService = DependencyService.Get<IImageService>();
                return imageService.GetDateTaken(Uri);
            }
            catch (ArgumentException)
            {
                // image is probably stock
                return DateTime.Now;
            }
        }
    }
}
