using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using DLuOvBamG.Models;
using DLuOvBamG.ViewModels;
using System.Windows.Input;

namespace DLuOvBamG.Views
{
    [DesignTimeVisible(false)]
    public partial class ImageDetailPage : ContentPage
    {

        ImageDetailViewModel DV;
        

        public ImageDetailPage(Picture image)
        {
            InitializeComponent();
            DV = new ViewModels.ImageDetailViewModel(image);
            BindingContext = DV;
            DV.Navigation = Navigation;
        }
    }
}