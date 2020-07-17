using DLuOvBamG.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DLuOvBamG.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageGrid : CustomBackButtonPage
    {
        ImageGalleryViewModel vm { get; set; }
        public ImageGrid(string Title)
        {
            InitializeComponent();
            vm = App.ViewModelLocator.ImageGalleryViewModel;
            BindingContext = vm;
            this.Title = Title;

            // show safety alert when clicking the navigation back button
            if (EnableBackButtonOverride)
            {
                this.CustomBackButtonAction = () =>
                {
                    App.CurrentGroup = "";
                    Navigation.PopAsync();
                }; 
            }
        }
    }
}