using System.ComponentModel;
using Xamarin.Forms;
using DLuOvBamG.Models;
using DLuOvBamG.ViewModels;

namespace DLuOvBamG.Views
{
    [DesignTimeVisible(false)]
    public partial class ImageDetailPage : ContentPage
    {
        ImageDetailViewModel DV;

        public ImageDetailPage(Picture image)
        {
            InitializeComponent();
            DV = new ImageDetailViewModel(image, this);
            BindingContext = DV;
        }
    }
}