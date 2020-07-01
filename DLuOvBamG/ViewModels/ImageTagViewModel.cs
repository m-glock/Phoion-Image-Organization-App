using DLuOvBamG.Models;
using DLuOvBamG.Services;
using DLuOvBamG.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace DLuOvBamG.ViewModels
{
    public class ImageTagViewModel : INotifyPropertyChanged
    {
        public INavigation Navigation;

        private Picture Picture;

        public string CustomTagInput { get; set; }
        public CategoryTag SelectedCustomTag { get; set; }

        public IList CustomTags { get; set; }

        private ImageOrganizationDatabase database = App.Database;

        private ObservableCollection<CategoryTag> tags { get; set; }

        public ObservableCollection<CategoryTag> Tags
        {
            set
            {

                tags = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Tags"));
                }
            }

            get
            {
                return tags;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ImageTagViewModel()
        {
        }

        public async void GetCategoryTagsOfPicture(int pictureId)
        {
            Picture dbPicture = await database.GetPictureAsync(pictureId);
            Tags = new ObservableCollection<CategoryTag>(dbPicture.CategoryTags);
            Picture = dbPicture;
        }

        async void GetCustomCategoryTags()
        {
            List<CategoryTag> customTags = await database.GetCustomCategoryTagsAsync();
            CustomTags = customTags;
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

        public ICommand SaveNewTag
        {
            get
            {
                return new Command(async (sender) =>
                {
                    CategoryTag newTag = null;
                    if (CustomTagInput != null)
                    {
                        // create new tag
                        newTag = new CategoryTag()
                        {
                            Name = CustomTagInput,
                            IsCustom = true
                        };
                        CustomTagInput = "";
                    } else if (SelectedCustomTag != null)
                    {
                        newTag = SelectedCustomTag;
                    }

                    if(newTag != null)
                    {
                        int id = await database.SaveCategoryTagAsync(newTag);
                        Picture.CategoryTags.Add(newTag);
                        await database.SavePictureAsync(Picture);
                        GetCategoryTagsOfPicture(Picture.Id);
                        await Navigation.PopModalAsync();
                    }
                    
                });
            }
        }
        
        public ICommand OpenEditPage
        {
            get
            {
                return new Command((sender) =>
                {
                    GetCustomCategoryTags();
                    var addTagPage = new AddTagPage();
                    Navigation.PushModalAsync(addTagPage);
                });
            }
        }

        public ICommand CloseEditPage
        {
            get 
            {
                return new Command((sender) =>
                {
                    Navigation.PopModalAsync();
                });
            }
        }
    }
}
