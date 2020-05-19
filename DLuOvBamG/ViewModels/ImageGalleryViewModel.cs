using DLuOvBamG.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace DLuOvBamG.ViewModels
{
    public class ImageGalleryViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Picture> Items { get; set; }

        private ContentPage currentPage;
        public ContentPage CurrentPage
        {
            get
            {
                return currentPage;
            }

            set
            {
                currentPage = value;
            }

        }

        [DataContract]
        class ImageList
        {
            [DataMember(Name = "photos")]
            public List<string> Photos = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ImageGalleryViewModel(ContentPage page)
        {
            Items = new ObservableCollection<Picture>() { };

            string[] images = {
                "https://farm9.staticflickr.com/8625/15806486058_7005d77438.jpg",
                "https://farm5.staticflickr.com/4011/4308181244_5ac3f8239b.jpg",
                "https://farm8.staticflickr.com/7423/8729135907_79599de8d8.jpg",
                "https://farm3.staticflickr.com/2475/4058009019_ecf305f546.jpg",
                "https://farm6.staticflickr.com/5117/14045101350_113edbe20b.jpg",
                "https://farm8.staticflickr.com/7524/15620725287_3357e9db03.jpg",
                "https://farm9.staticflickr.com/8351/8299022203_de0cb894b0.jpg",
            };

            int number = 0;
            for (int n = 0; n < 20; n++)
            {
                for (int i = 0; i < images.Length; i++)
                {
                    number++;
                    Picture picture = new Picture
                    {
                        Id = i.ToString(),
                        Uri = images[i]
                    };
                    Items.Add(picture);
                }
            }
        }

        public ICommand ItemTappedCommand
        {
            get
            {
                return new Command((sender) =>
                {
                    var Item = sender as Picture;

                    foreach (var picture in Items)
                    {
                    // if (picture.Id == )Item.Id)
                    // {
                    // TODO: show detail page
                    // var newPage = new xxxContentPage(Item);
                    // currentPage.Navigation.PushAsync(newPage, true);
                    //}
                }

                });
            }
        }
    }
}