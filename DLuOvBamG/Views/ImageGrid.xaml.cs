using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DLuOvBamG.ViewModels;
using DLToolkit.Forms.Controls;

namespace DLuOvBamG.Views{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageGrid : ContentPage{

        ImageGalleryViewModel vm { get; set; }
        public ImageGrid(){
            InitializeComponent();
            FlowListView.Init();
            vm = new ImageGalleryViewModel();
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