using DLuOvBamG.Models;
using System;
using System.ComponentModel;
using System.Linq;

namespace DLuOvBamG.ViewModels
{
    class ImageInfoViewModel : BaseViewModel
    {
        public string Name { get; }
        public string Location { get; }
        public string Dimensions { get; }
        public string Date { get; }
        public string Path { get;  }

        public string Size { get; }

        public ImageInfoViewModel(Picture image)
        {
           
            Dimensions = image.Width + " x " + image.Height;
            Path = image.Uri;
            Name = image.Uri.Split('/').Last();
            Date = image.Date.ToString();
            Location = image.Latitude + " - " + image.Longitude;
            Size = calcSizeInMB(image.Size);
        }

        private string calcSizeInMB(string size)
        {
            if(String.IsNullOrEmpty(size))
            {
                return "";
            }

            double sizeInMB = (Convert.ToDouble(size) / 1000000);
            return sizeInMB.ToString("0.##") + " MB";
        }
    }
}
