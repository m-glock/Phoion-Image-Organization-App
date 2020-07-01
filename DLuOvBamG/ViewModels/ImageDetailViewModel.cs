using System;
using System.Collections.Generic;
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
            await Navigation.PushAsync(new InfoPage(Image), true);
        });

        public ICommand DeleteImage => new Command(async () => {

        });

        public ICommand GetSimilar => new Command(async () =>
        {
            // TODO finish when Tensorflow functionality in merged
            // how to search similar images only for one specific image instead of comparin all images against each other?

            /*ScanOptionsEnum option = ScanOptionsEnum.similarPics;
            List<ScanOptionsEnum> options = new List<ScanOptionsEnum> { option };
            App.tf.FillPictureLists(options);
            double optionValue = option.GetDefaultPresicionValue();
            await Navigation.PushAsync(new ScanOptionDisplayPage(optionValue, option), true);*/
        });
    }
}