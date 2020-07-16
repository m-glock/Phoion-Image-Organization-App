using DLToolkit.Forms.Controls;
using DLuOvBamG.Models;
using DLuOvBamG.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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
                    App.CurrentDirectory = "";
                    Navigation.PopAsync();
                }; 
            }
        }
    }
}