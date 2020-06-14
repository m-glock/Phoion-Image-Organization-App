using DLuOvBamG.Models;
using DLuOvBamG.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace DLuOvBamG.ViewModels
{
    class ImageTagViewModel
    {
        public Picture image;
        public ObservableCollection<CategoryTag> Tags { get; set; }
        
        public ImageTagViewModel(Picture image)
        {
            this.image = image;
            this.Tags = GetCategoryTags(image);
        }

        private ObservableCollection<CategoryTag> GetCategoryTags(Picture image)
        {
            IImageService imageService = DependencyService.Get<IImageService>();
            IClassifier classifier = App.Classifier;
            ObservableCollection<CategoryTag> categoryTags = new ObservableCollection<CategoryTag>();
            byte[] fileBytes = imageService.GetFileBytes(image.Uri);

            List<ModelClassification> modelClassifications = classifier.Classify(fileBytes);
            List<ModelClassification> topClassifications = modelClassifications.Where(classification => classification.Probability > 0.1f).ToList();
            topClassifications.ForEach(classification =>
            {
                CategoryTag categoryTag = new CategoryTag
                {
                    Name = classification.TagName + ' ' + classification.Probability
                };
                categoryTags.Add(categoryTag);
            });

            return categoryTags;
        }

        public ICommand DeleteButtonClicked
        {
            get
            {
                return new Command((sender) =>
                {
                    var Item = sender as CategoryTag;
                });
            }
        }

        public ICommand OpenEditPage
        {
            get
            {
                return new Command((sender) =>
                {
                    var Item = sender as CategoryTag;
                });
            }
        }
    }
}
