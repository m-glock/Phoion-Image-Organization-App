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
        public string Id { get; set; }

        public DateTime Date { get; set; }

        public Picture(string Uri, string Id)
        {
            this.Uri = Uri;
            this.Id = Id;

            try
            {
                IImageService imageService = DependencyService.Get<IImageService>();
                this.Date = imageService.GetDateTaken(Uri);
            } catch (ArgumentException)
            {
                // image is probably stock
            }

            
        }
    }
}
