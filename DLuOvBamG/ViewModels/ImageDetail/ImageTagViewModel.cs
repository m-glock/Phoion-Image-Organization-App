using DLuOvBamG.Models;
using DLuOvBamG.Services;
using DLuOvBamG.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace DLuOvBamG.ViewModels
{
    public class ImageTagViewModel : INotifyPropertyChanged
    {
        private ImageOrganizationDatabase database = App.Database;
        public INavigation Navigation;
        private Picture Picture;
        public string CustomTagInput { get; set; }
        public CategoryTag SelectedCustomTag { get; set; }

        private ObservableCollection<CategoryTag> customTags { get; set; }
        private ObservableCollection<CategoryTag> selectOptions { get; set; }
        private ObservableCollection<CategoryTag> tags { get; set; }

        #region propertychanged
        public ObservableCollection<CategoryTag> SelectOptions
        {
            set
            {
                selectOptions = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectOptions"));
            }

            get
            {
                return selectOptions;
            }
        } 

        public ObservableCollection<CategoryTag> CustomTags
        {
            set
            {
                customTags = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CustomTags"));
            }

            get
            {
                return customTags;
            }
        }

        public ObservableCollection<CategoryTag> Tags
        {
            set
            {
                tags = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Tags"));
            }

            get
            {
                return tags;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        public ImageTagViewModel() { }

        public async void GetCategoryTagsOfPicture(int pictureId)
        {
            Picture dbPicture = await database.GetPictureAsync(pictureId);
            Tags = new ObservableCollection<CategoryTag>(dbPicture.CategoryTags);
            Picture = dbPicture;
        }

        async void GetCustomCategoryTags()
        {
            List<CategoryTag> customTags = await database.GetCustomCategoryTagsAsync();
            CustomTags = new ObservableCollection<CategoryTag>(customTags);
            SetSelectionOptions();
        }

        void SetSelectionOptions()
        {
            // remove tags of picture from custom tags
            List<CategoryTag> customTags = CustomTags.ToList();
            List<CategoryTag> pictureTags = Tags.ToList();
            List<CategoryTag> options = customTags.Except(pictureTags).ToList();
            SelectOptions = new ObservableCollection<CategoryTag>(options);
        }

        public ICommand DeleteButtonClicked
        {
            get
            {
                return new Command(async (sender) =>
                {
                    var Item = sender as CategoryTag;
                    Tags.Remove(Item);
                    Picture.CategoryTags.Remove(Item);
                    await database.SavePictureAsync(Picture);
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
