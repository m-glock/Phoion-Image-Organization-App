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
using GalaSoft.MvvmLight.Messaging;
using System.Text.RegularExpressions;
using Xamarin.Forms.Internals;

namespace DLuOvBamG.ViewModels
{
    public delegate void PictureDeletedEventHandler(object source, PictureDeletedEvent e);
    public class ImageGalleryViewModel : INotifyPropertyChanged
    {
        readonly IImageService imageService = DependencyService.Get<IImageService>();
        readonly ImageFileStorage imageFileStorage = new ImageFileStorage();
        readonly IClassifier classifier = App.Classifier;
        readonly ImageOrganizationDatabase db = App.Database;

        public event PropertyChangedEventHandler PropertyChanged;

        private FlowObservableCollection<Grouping<string, Picture>> albumItems;
        private FlowObservableCollection<Grouping<string, Picture>> groupedItems;
        private string SelectedGroup { get; set; }

        public FlowObservableCollection<Grouping<string, Picture>> GroupedItems
        {
            set
            {
                groupedItems = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("GroupedItems"));
                }
            }

            get
            {
                return groupedItems;
            }
        }

        public FlowObservableCollection<Grouping<string, Picture>> AlbumItems
        {
            set
            {
                albumItems = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("AlbumItems"));
                }
            }

            get
            {
                return albumItems;
            }
        }

        public List<Picture> Items { get; set; }
        public INavigation Navigation;

        public ImageGalleryViewModel()
        {
            Items = new List<Picture>();
            GroupedItems = new FlowObservableCollection<Grouping<string, Picture>>();
            AlbumItems = new FlowObservableCollection<Grouping<string, Picture>>();
            SelectedGroup = "";
            Messenger.Default.Register<PictureDeletedEvent>(this, OnPictureDeleted);
        }

        public async void GetPictures()
        {
            // try to get pictures from db, if this fails load them and put them in db
            List<Picture> pictures = await LoadImagesFromDB();

            if (pictures.Count == 0)
            {
                Picture[] devicePictures = await imageFileStorage.GetPicturesFromDevice(AlbumItems, null);
                var saved = await SavePicturesInDB(devicePictures);
                if (saved)
                {
                    pictures = await LoadImagesFromDB();
                    var categoryTags = await SaveCategoryTagsInDB();
                    var classified = await ClassifyAllPictures(pictures);
                }
            }
            else
            {
                // set image sources and remove non exist pictures
                pictures = SetImageSources(pictures);
                // set album items
                GroupPicturesByDirectory(pictures);

                // get new pictures taken since last reading date
                DateTime dateFilter = imageFileStorage.GetAppPropertyReadingDate();
                Picture[] newDevicePictures = await imageFileStorage.GetPicturesFromDevice(AlbumItems, dateFilter);
                // update thumbnail of album
                OrderAllAlbumsByDate();

                var saved = await SavePicturesInDB(newDevicePictures);
                pictures = await LoadImagesFromDB();
                pictures.AddRange(newDevicePictures);
            }

            Items = pictures;
        }

        Task<List<Picture>> LoadImagesFromDB()
        {
            return db.GetPicturesAsync();
        }

        /// <summary>
        /// Sets ImageSource of all pictures. Deletes Picture from DB if file dont exist
        /// </summary>
        List<Picture> SetImageSources(List<Picture> pictures)
        {
            List<Picture> picturesWithSource = new List<Picture>();
            pictures.ForEach(async picture =>
            {
                // if file exists
                if (File.Exists(picture.Uri))
                {
                    picture.ImageSource = ImageSource.FromFile(picture.Uri);
                    picturesWithSource.Add(picture);
                }
                else
                {
                    // else delete image from db
                    int deletedPicture = await imageFileStorage.DeleteFileAsync(picture);
                }
            });
            return picturesWithSource;
        }

        void OrderAllAlbumsByDate()
        {
            AlbumItems.ForEach(album =>
            {
                List<Picture> orderd =  album.OrderByDescending(item => item.Date).ToList();
                album.Clear();
                album.AddRange(orderd);
            });
        }

        List<Grouping<string, Picture>> GroupPicturesByDate(List<Picture> pictures)
        {
            return pictures
                .OrderByDescending(item => item.Date)
                .GroupBy(item => item.Date.Date.ToShortDateString())
                .Select(itemGroup => new Grouping<string, Picture>(itemGroup.Key, itemGroup))
                .ToList();
        }

        void GroupPicturesByDirectory(List<Picture> pictures)
        {
            var sorted = pictures
                .OrderByDescending(item => item.Date)
                .GroupBy(item => item.DirectoryName)
                .Select(itemGroup => new Grouping<string, Picture>(itemGroup.Key, itemGroup, itemGroup.Count()))
                .OrderByDescending(item => item.ColumnCount)
                .ToList();
            AlbumItems = new FlowObservableCollection<Grouping<string, Picture>>(sorted);
        }

        async Task<bool> SavePicturesInDB(Picture[] pictures)
        {
            if (pictures.Length > 0)
            {
                var tasks = pictures.Select(picture => db.SavePictureAsync(picture));
                await Task.WhenAll(tasks);
                return true;
            }
            return false;
        }

        async Task<int[]> SaveCategoryTagsInDB()
        {
            IAssetsService assetsService = DependencyService.Get<IAssetsService>();
            List<string> labels = assetsService.LoadClassificationLabels();
            List<CategoryTag> categoryTags = labels.Select(label =>
                {
                    return new CategoryTag()
                    {
                        Name = label,
                        IsCustom = false
                    };
                }
            ).ToList();
            var categoryTagsTasks = categoryTags.Select(categoryTag => db.SaveCategoryTagAsync(categoryTag));
            var categoryTagIdArray = await Task.WhenAll(categoryTagsTasks);
            return categoryTagIdArray;
        }

        async Task<bool> ClassifyAllPictures(List<Picture> pictures)
        {
            if (pictures.Count > 0)
            {
                var classifyTasks = pictures.Select(picture => ClassifyPicture(picture));
                await Task.WhenAll(classifyTasks); // TODO bereits durchgeführte klassifizierungen speichern

                return true;
            }
            return false;
        }

        async Task ClassifyPicture(Picture picture)
        {
            // get classifications above 10% and put them in a list
            List<CategoryTag> categoryTags = new List<CategoryTag>();
            if (picture.CategoryTags is null)
            {
                picture.CategoryTags = new List<CategoryTag>();
            }
            byte[] fileBytes = imageService.GetFileBytes(picture.Uri);
            classifier.ChangeModel(ScanOptionsEnum.similarPics);
            // get classifications from classifier
            List<ModelClassification> modelClassifications = await classifier.ClassifySimilar(fileBytes);
            var currentVector = GetBytes(classifier.FeatureVectors[classifier.FeatureVectors.Count - 1]);


            // map strings to CategoryTag objects
            modelClassifications.ForEach(classification =>
            {
                CategoryTag categoryTag = new CategoryTag
                {
                    Name = classification.TagName
                };
                categoryTags.Add(categoryTag);
            });

            // find or insert all category tag objects
            categoryTags.ForEach(categoryTag => categoryTag.FindOrInsert());
            // add the categoryTags, now with id, to the picture and update it
            categoryTags.ForEach(categoryTag =>
            {
                picture.CategoryTags.Add(categoryTag);
            });
            picture.FeatureVector = currentVector;
            db.SavePictureAsync(picture);
        }

        public ICommand ItemTappedCommand
        {
            get
            {
                return new Command(async (sender) =>
                {
                    var Item = sender as Picture;

                    foreach (var picture in Items)
                    {
                        if (picture.Id == Item.Id)
                        {
                            Console.WriteLine("tapped {0}", picture.Id);
                            await Navigation.PushAsync(new ImageDetailPage(picture), true);
                        }
                    }

                });
            }
        }

        public ICommand GroupedItemTappedCommand
        {
            get
            {
                return new Command(async (sender) =>
                {
                    // get selected group
                    Grouping<string, Picture> selectedGroup = sender as Grouping<string, Picture>;
                    // get grouped item of selected album
                    List<Grouping<string, Picture>> grouped = GroupPicturesByDate(selectedGroup.ToList());
                    GroupedItems = new FlowObservableCollection<Grouping<string, Picture>>(grouped);
                    // set currently selected group
                    SelectedGroup = selectedGroup.Key;
                    // navigate to image grid
                    await Navigation.PushAsync(new ImageGrid(selectedGroup.Key), true);

                });
            }
        }
        public ICommand OpenCleanupPage => new Command(async () =>
        {
            await Navigation.PushAsync(new CleanupPage(SelectedGroup));
        });

        public void OnPictureDeleted(PictureDeletedEvent e)
        {
            int deletedPictureId = e.GetPictureId();
            // find picture object
            int pictureIndex = Items.FindIndex(pic => pic.Id == deletedPictureId);
            Picture picture = Items[pictureIndex];
            // delte picture from album
            string albumKey = picture.DirectoryName;
            AlbumItems.ForEach(group =>
            {
                // select correct album
                if (group.Key == albumKey)
                {
                    // get index of picture and delete
                    int groupIndex = group.ToList().FindIndex(pic => pic.Id == deletedPictureId);
                    group.RemoveAt(groupIndex);
                    // re-group pictures from album
                    List<Grouping<string, Picture>> grouped = GroupPicturesByDate(group.ToList());
                    GroupedItems = new FlowObservableCollection<Grouping<string, Picture>>(grouped);
                    return;
                }
            });
        }

        private byte[] GetBytes(double[] values)
        {
            return values.SelectMany(value => BitConverter.GetBytes(value)).ToArray();
        }
    }
}