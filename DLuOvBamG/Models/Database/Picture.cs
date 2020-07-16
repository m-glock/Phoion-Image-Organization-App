using DLuOvBamG.Services;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;

namespace DLuOvBamG.Models
{
    [Table("Pictures")]
    public class Picture
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        
        public string Uri { get; set; }

        [Ignore]
        public ImageSource ImageSource { get; set; }
        
        public DateTime Date { get; set; }

        public String Longitude { get; set; }

        public String Latitude { get; set; }

        public String Size { get; set; }

        public String Height { get; set; }

        public String Width { get; set; }

        public String DirectoryName { get; set; }

        [ManyToMany(typeof(PictureTags))]
        public List<CategoryTag> CategoryTags { get; set; }

        public double BlurryPrecision { get; set; }

        public double DarkPixelsPercent { get; set; }

        public double BrightPixelsPercent { get; set; }

        public Byte[] FeatureVector { get; set; }
        
        public Picture()
        {

        }

        public Picture(string Uri)
        {
            this.Uri = Uri;
            this.CategoryTags = new List<CategoryTag>();
        }
    }
}
