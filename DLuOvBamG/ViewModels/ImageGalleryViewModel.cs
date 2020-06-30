﻿using DLuOvBamG.Models;
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
        IImageService imageService = DependencyService.Get<IImageService>();
        IClassifier classifier = App.Classifier;
        ImageOrganizationDatabase db = App.Database;

        public event PropertyChangedEventHandler PropertyChanged;

        FlowObservableCollection<Grouping<string, Picture>> groupedItems;

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

        public List<Picture> Items { get; set; }
        public INavigation Navigation;


        public ImageGalleryViewModel()
        {
            Items = new List<Picture>();
            GroupedItems = new FlowObservableCollection<Grouping<string, Picture>>();
        }


        public async void GetPictures()
        {
            // try to get pictures from db, if this fails load them and put them in db
            List<Picture> pictures = await LoadImagesFromDB();

            if (pictures.Count == 0)
            {
                var storageImages = await LoadImagesFromStorage();
                var saved = await SavePicturesInDB(storageImages);
                if (saved)
                {
                    pictures = await LoadImagesFromDB();
                    var categoryTags = await SaveCategoryTagsInDB();
                    var classified = await ClassifyAllPictures(pictures);
                }
            }
            Items = pictures;
            pictures = SetImageSources(pictures);
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
            for (int i = 0; i < imagePaths.Length; i++)
            {
                Picture picture = new Picture(imagePaths[i]);
                pictureList.Add(picture);
            }
            return pictureList;
        }

        Task<List<Picture>> LoadImagesFromDB()
        {
            return db.GetPicturesAsync();
        }

        List<Picture> SetImageSources(List<Picture> pictures)
        {
            return pictures.Select(picture =>
            {
                // if file exists
                picture.ImageSource = ImageSource.FromFile(picture.Uri);
                // else delete image from db
                return picture;
            }).ToList();
        }

        void GroupPicturesByDate(List<Picture> pictures)
        {
            var sorted = pictures
                .OrderByDescending(item => item.Date)
                .GroupBy(item => item.Date.Date.ToShortDateString())
                .Select(itemGroup => new Grouping<string, Picture>(itemGroup.Key, itemGroup))
                .ToList();
            GroupedItems = new FlowObservableCollection<Grouping<string, Picture>>(sorted);
        }

        async Task<bool> SavePicturesInDB(List<Picture> pictures)
        {
            if (pictures.Count > 0)
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
                        Name = label 
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
                await Task.WhenAll(classifyTasks);
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

            // get classifications from classifier
            List<ModelClassification> modelClassifications = classifier.Classify(fileBytes);
            // filter classifications, to get only above 10% probability
            List<ModelClassification> topClassifications = modelClassifications.Where(classification => classification.Probability > 0.1f).ToList();

            // map strings to CategoryTag objects
            topClassifications.ForEach(classification =>
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
            db.SavePictureAsync(picture);
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
                            var newPage = new ImageTagPage(picture.Id);
                            // var newPage = new ImageDetailPage(picture);
                            Navigation.PushAsync(newPage, true);
                        }
                    }

                });
            }
        }

        public ICommand OpenCleanupPage => new Command(async () =>
        {
            await Navigation.PushAsync(new CleanupPage());
        });
    }
}