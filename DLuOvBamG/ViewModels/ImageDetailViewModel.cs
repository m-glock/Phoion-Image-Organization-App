using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using DLuOvBamG.Models;
using DLuOvBamG.Views;
using Xamarin.Forms;



namespace DLuOvBamG.ViewModels
{
    public class ImageDetailViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public Picture Image { get; set; }
        public string debugString { get; set; } = "test";
        public INavigation Navigation;


        public ImageDetailViewModel(Picture item)
        {

            Image = item;
        }


        public ICommand GetCategories => new Command(async () => {

        });
        public ICommand GetInfo => new Command(async () => {
            var newPage = new InfoPage(Image);
            Navigation.PushAsync(newPage, true);
        });

        public ICommand DeleteImage => new Command(async () => {

        });
        public ICommand GetSimilar => new Command(async () =>
        {

        });
    }
}