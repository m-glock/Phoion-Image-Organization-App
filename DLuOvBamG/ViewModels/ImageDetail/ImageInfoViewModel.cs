using Android.Media;
using DLuOvBamG.Models;
using DLuOvBamG.Services;
using System;
using System.ComponentModel;
using System.Linq;
using Xamarin.Essentials;

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
        public Placemark Placemark { get; private set; }
        private Picture Picture { get; }

        public ImageInfoViewModel(Picture image)
        {
            Picture = image;
            Dimensions = image.Height + " x " + image.Width;
            Path = image.Uri;
            Name = image.Uri.Split('/').Last();
            Date = image.Date.ToString();
            Location = image.Latitude.Replace(',','.') + " , " + image.Longitude.Replace(',', '.');
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

        public async void GetPlacemarkAsync()
        {
            GeoService geoService = new GeoService();
            double latitude = Convert.ToDouble(Picture.Latitude);
            double longitude = Convert.ToDouble(Picture.Longitude);
            Placemark placemark = await geoService.GetPlacemark(latitude, longitude);
            Placemark = placemark;
        }
    }
}
