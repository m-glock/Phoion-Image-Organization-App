using DLuOvBamG.Models;
using DLuOvBamG.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DLuOvBamG.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoPage : ContentPage
    {
        
        ImageInfoViewModel IV;

        public InfoPage(Picture image)
        {
            IV = new ImageInfoViewModel(image); 
            InitializeComponent();
            BindingContext = IV;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            IV.GetPlacemarkAsync();
        }
    }
}