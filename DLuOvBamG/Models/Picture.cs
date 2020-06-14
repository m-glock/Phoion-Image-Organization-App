using DLuOvBamG.Services;
using System;
using System.Collections.Generic;
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
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public List<CategoryTag> CategoryTags { get; set; }

        public Picture()
        {

        }
        public Picture(string Uri, int Id)
        {
            this.Uri = Uri;
            this.Id = Id;
            this.Date = GetDate(Uri);
            this.CategoryTags = new List<CategoryTag>();
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
