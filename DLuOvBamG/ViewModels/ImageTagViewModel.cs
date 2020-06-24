using DLuOvBamG.Models;
using DLuOvBamG.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace DLuOvBamG.ViewModels
{
    class ImageTagViewModel : INotifyPropertyChanged
    {
        public Picture Picture;
        public ObservableCollection<CategoryTag> tags { get; set; }

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

        public ImageTagViewModel(int pictureId)
        {
            GetCategoryTags(pictureId);
        }

        async void GetCategoryTags(int pictureId)
        {
            ImageOrganizationDatabase db = App.Database;
            Picture dbPicture = await db.GetPictureAsync(pictureId);
            Tags = new ObservableCollection<CategoryTag>(dbPicture.CategoryTags);
            Picture = dbPicture;
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
