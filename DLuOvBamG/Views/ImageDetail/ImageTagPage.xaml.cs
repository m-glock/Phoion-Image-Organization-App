using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using DLuOvBamG.Models;
using DLuOvBamG.ViewModels;

namespace DLuOvBamG.Views
{
    public partial class ImageTagPage : ContentPage
    {
        ImageTagViewModel vm { get; set; }
        public ImageTagPage(int pictureId)
        {
            InitializeComponent();
            vm = App.ViewModelLocator.ImageTagViewModel;
            BindingContext = vm;
            vm.Navigation = Navigation;
            vm.GetCategoryTagsOfPicture(pictureId);
        }
    }
}