﻿using DLuOvBamG.Services;
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

        [ManyToMany(typeof(PictureTags))]
        public List<CategoryTag> CategoryTags { get; set; }
        
        public Picture()
        {

        }

        public Picture(string Uri)
        {
            this.Uri = Uri;
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