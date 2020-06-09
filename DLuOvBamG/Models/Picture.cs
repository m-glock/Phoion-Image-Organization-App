using DLuOvBamG.Services;
using SQLite;
using SQLiteNetExtensions.Attributes;
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
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Uri { get; set; }

        public ImageSource ImageSource { get; set; }
        
        public DateTime Date { get; set; }

        [ManyToMany(typeof(PictureTags))]
        public List<CategoryTag> CategoryTags { get; set; }
        
        public Picture()
        {

        }
        public Picture(string Uri)
        {
            this.Uri = Uri;
            this.Date = GetDate(Uri);
        }

        public Picture(string Uri, Stream ImageData)
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
