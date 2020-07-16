using DLuOvBamG.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DLuOvBamG.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageGrid : ContentPage
    {
        ImageGalleryViewModel vm { get; set; }
        public ImageGrid(string Title)
        {
            InitializeComponent();
            vm = App.ViewModelLocator.ImageGalleryViewModel;
            BindingContext = vm;
            this.Title = Title;
        }
    }
}