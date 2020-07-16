using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DLuOvBamG.ViewModels;
using DLToolkit.Forms.Controls;
using System;
using Android;

namespace DLuOvBamG.Views{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageGallery : ContentPage{

        ImageGalleryViewModel vm { get; set; }

        private bool firstTry = true;
        public ImageGallery(){
            InitializeComponent();
            FlowListView.Init();
            vm = App.ViewModelLocator.ImageGalleryViewModel;
            BindingContext = vm;
            vm.Navigation = Navigation;                
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (firstTry)
            {
                vm.GetPictures();
                firstTry = false;
            }
             
        }

        async void SortClickedAsync(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Group Options", "Cancel", null, "Directory", "Date", "Location", "Category");
            vm.OnGroupOptionsSelected(action);
        }
    }
}