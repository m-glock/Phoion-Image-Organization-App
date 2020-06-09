using DLuOvBamG.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            this.Tags = new ObservableCollection<CategoryTag>();

            Tags.Add(new CategoryTag { Name = "Lorem" });
            Tags.Add(new CategoryTag { Name = "ipsum" });
            Tags.Add(new CategoryTag { Name = "dolor" });
            Tags.Add(new CategoryTag { Name = "sit" });
            Tags.Add(new CategoryTag { Name = "amet" });
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
