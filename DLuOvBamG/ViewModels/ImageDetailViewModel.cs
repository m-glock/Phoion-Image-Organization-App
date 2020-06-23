using System;
using System.Diagnostics;
using System.Windows.Input;
using DLuOvBamG.Models;
using DLuOvBamG.Views;
using Xamarin.Forms;



namespace DLuOvBamG.ViewModels
{
    public class ImageDetailView : ContentPage
    {
        public Picture Image { get; set; }
        public string debugString { get; set; } = "test";
        public INavigation Navigation;

        public ImageDetailView(Picture item)
        {
            
            Image = item;
        }

        void OnImageButtonClicked(object sender, EventArgs args)
        {
            debugString = "pressed";
        }

        public void getInfo()
        {
            var newPage = new InfoPage(Image);
            Navigation.PushAsync(newPage, true);
        }

        public void deleteImage()
        {

        }

        public void getCategories()
        {

        }

        public void similarImages()
        {

        }
    }
}