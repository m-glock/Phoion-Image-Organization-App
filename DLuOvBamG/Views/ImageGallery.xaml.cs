using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DLuOvBamG.ViewModels;
using DLToolkit.Forms.Controls;

namespace DLuOvBamG.Views{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageGallery : ContentPage{

        ImageGalleryViewModel vm { get; set; }
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
            vm.GetPictures();   
        }
    }
}