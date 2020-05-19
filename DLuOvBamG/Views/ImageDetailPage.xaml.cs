using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using DLuOvBamG.Models;
using DLuOvBamG.ViewModels;

namespace DLuOvBamG.Views
{
    [DesignTimeVisible(false)]
    public partial class ImageDetailPage : ContentPage
    {
        public ImageDetailPage(Picture image)
        {
            InitializeComponent();
            BindingContext = new ImageDetailViewModel(image);
        }

    }
}