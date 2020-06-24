using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using DLuOvBamG.Models;
using DLuOvBamG.ViewModels;

namespace DLuOvBamG.Views
{
    public partial class ImageTagPage : ContentPage
    {
        public ImageTagPage(int pictureId)
        {
            InitializeComponent();
            BindingContext = new ImageTagViewModel(pictureId);
        }
    }
}