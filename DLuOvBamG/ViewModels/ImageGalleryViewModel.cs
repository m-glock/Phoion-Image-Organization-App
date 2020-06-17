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
using System.Threading.Tasks;

namespace DLuOvBamG.ViewModels
{
    public class ImageGalleryViewModel : INotifyPropertyChanged
    {
        static string CAMERA_PATH = "/Camera";
        public FlowObservableCollection<Grouping<string, Picture>> GroupedItems { get; set; }
        public List<Picture> Items { get; set; }
        public INavigation Navigation;

        public event PropertyChangedEventHandler PropertyChanged;
        public ImageGalleryViewModel()
        {
            Items = new List<Picture>();
            GroupedItems = new FlowObservableCollection<Grouping<string, Picture>>();
            GetPictures();
        }


        async void GetPictures()
        {
            // try to get pictures from db, if this fails load them and put them in db
            List<Picture> pictures = await LoadImagesFromDB();

            if (pictures.Count == 0)
            {
                pictures = await LoadImagesFromStorage();
                SavePicturesInDB(pictures);
            }

            Items = pictures;
            GroupPicturesByDate(pictures);
        }

        async Task<List<Picture>> LoadImagesFromStorage()
        {
            IPathService pathService = DependencyService.Get<IPathService>();
            string dcimFolder = pathService.DcimFolder;
            dcimFolder += CAMERA_PATH;
            ImageFileStorage imageFileStorage = new ImageFileStorage();
            string[] imagePaths = await imageFileStorage.GetFilesFromDirectory(dcimFolder);

            var pictureList = new List<Picture>();
            for (int i = 0; i < imagePaths.Length ; i++)
            {
                Picture picture = new Picture(imagePaths[i]);
                picture.ImageSource = ImageSource.FromFile(imagePaths[i]);
                picture.Id = i;
                pictureList.Add(picture);
            }
            return pictureList;
        }

        async Task<List<Picture>> LoadImagesFromDB()
        {
            ImageOrganizationDatabase db = App.Database;
            List<Picture> pictures = new List<Picture>();
            // This breaks the display of images, dont know why
            // List<Picture> pictures = await db.GetPicturesAsync();
            return pictures;
        }

        void GroupPicturesByDate(List<Picture> pictures)
        {
            var sorted = pictures
                .OrderByDescending(item => item.Date)
                .GroupBy(item => item.Date.Date.ToShortDateString())
                .Select(itemGroup => new Grouping<string, Picture>(itemGroup.Key, itemGroup))
                .ToList();
            GroupedItems =  new FlowObservableCollection<Grouping<string, Picture>>(sorted);
        }

        async void SavePicturesInDB(List<Picture> pictures)
        {
            if(pictures.Count > 0)
            {
                ImageOrganizationDatabase db = App.Database;

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
                        if (picture.Id == Item.Id)
                        {
                            Console.WriteLine("tapped {0}", picture.Id);
                            var newPage = new ImageTagPage(picture);
                            // var newPage = new ImageDetailPage(picture);
                            Navigation.PushAsync(newPage, true);
                        }
                    }

                });
            }
        }

        public ICommand OpenCleanupPage => new Command(async () => {
            await Navigation.PushAsync(new CleanupPage());
        });
    }
}