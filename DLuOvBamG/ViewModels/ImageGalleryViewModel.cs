using DLuOvBamG.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Runtime.Serialization;
using DLuOvBamG.Views;
using DLuOvBamG.Services;
using System.IO;
using System.Linq;
using DLToolkit.Forms.Controls;
using FFImageLoading;

namespace DLuOvBamG.ViewModels
{
    public class ImageGalleryViewModel : INotifyPropertyChanged
    {
        public FlowObservableCollection<Grouping<string, Picture>> GroupedItems { get; set; }
        public List<Picture> Items { get; set; }

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
            Items = new List<Picture>();
            GroupedItems = new FlowObservableCollection<Grouping<string, Picture>>();
            CurrentPage = page;
            LoadImagesFromStorage();
        }

        async void LoadImagesFromStorage()
        {
            string[] stockImages = {
                "https://farm9.staticflickr.com/8625/15806486058_7005d77438.jpg",
                "https://farm5.staticflickr.com/4011/4308181244_5ac3f8239b.jpg",
                "https://farm8.staticflickr.com/7423/8729135907_79599de8d8.jpg",
                "https://farm3.staticflickr.com/2475/4058009019_ecf305f546.jpg",
                "https://farm6.staticflickr.com/5117/14045101350_113edbe20b.jpg",
                "https://farm8.staticflickr.com/7524/15620725287_3357e9db03.jpg",
                "https://farm9.staticflickr.com/8351/8299022203_de0cb894b0.jpg",
            };

            IPathService pathService = DependencyService.Get<IPathService>();
            FFImageLoading.IImageService imageService = ImageService.Instance;
            string dcimFolder = pathService.DcimFolder;
            dcimFolder += "/Camera";
            ImageFileStorage imageFileStorage = new ImageFileStorage();
            string[] imagePaths = await imageFileStorage.GetFilesFromDirectory(dcimFolder);

            if(imagePaths.Length == 0)
            {
                imagePaths = stockImages;
            }

            var pictureList = new List<Picture>();
            for (int i = 0; i < imagePaths.Length ; i++)
            {
                // Stream imageStream = await imageService.LoadFile(imagePaths[i]).DownSample(width: 200).AsJPGStreamAsync(quality: 80);
                Picture picture = new Picture(imagePaths[i], i.ToString());
                // picture.ImageSource = ImageSource.FromStream(() => imageStream);
                pictureList.Add(picture);
            }

            var sorted = pictureList
                .OrderBy(item => item.Date)
                .GroupBy(item => item.Date.Date.ToShortDateString())
                .Select(itemGroup => new Grouping<string, Picture>(itemGroup.Key, itemGroup))
                .ToList();

            Items = pictureList;
            GroupedItems = new FlowObservableCollection<Grouping<string, Picture>>(sorted);
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
                        if (picture.Id == Item.Id)
                        {
                            Console.WriteLine("tapped {0}", picture.Id);
                            var newPage = new ImageDetailPage(picture);
                            currentPage.Navigation.PushAsync(newPage, true);
                        }
                }

                });
            }
        }
    }
}