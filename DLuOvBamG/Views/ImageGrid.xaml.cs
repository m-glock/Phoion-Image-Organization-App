using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DLuOvBamG.ViewModels;
using DLToolkit.Forms.Controls;

namespace DLuOvBamG.Views{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageGrid : ContentPage{ 
        
        public ImageGrid(){
            InitializeComponent();
            FlowListView.Init();
            ImageGalleryViewModel vm = BindingContext as ImageGalleryViewModel;
            vm.Navigation = Navigation;                
        } 
    }
}