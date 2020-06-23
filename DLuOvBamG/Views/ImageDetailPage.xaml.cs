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

        ImageDetailView DV;
        

        public ImageDetailPage(Picture image)
        {
            InitializeComponent();
            DV =new ViewModels.ImageDetailView(image);
            BindingContext = DV;
            DV.Navigation = Navigation;
        }

        void OnImageButtonClicked(object sender, EventArgs args)
        {
            
            Console.WriteLine("test");
           
        }

        void categoriesBtn(object sender, EventArgs args)
        {
            DV.getCategories();
            Console.WriteLine("categries");
        }
        void infoBtn(object sender, EventArgs args)
        {
            DV.getInfo();
            Console.WriteLine("info");
        }
        void deleteBtn(object sender, EventArgs args)
        {
            DV.deleteImage();
            Console.WriteLine("delete");
        }

        void similarBtn(object sender, EventArgs args)
        {
            DV.similarImages();
            Console.WriteLine("similar");
        }

    }
}