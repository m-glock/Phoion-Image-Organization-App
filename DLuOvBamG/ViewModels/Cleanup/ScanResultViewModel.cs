using DLuOvBamG.Models;
using DLuOvBamG.Services;
using DLuOvBamG.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace DLuOvBamG.ViewModels
{
    public class ScanResultViewModel : BaseViewModel
    {
        public INavigation Navigation;
        public Dictionary<ScanOptionsEnum, double> OptionValues;

        public ScanResultViewModel()
        {

        }

        public void FillPictureListsTF()
        {
            App.tf.FillPictureLists(new List<ScanOptionsEnum>(OptionValues.Keys));//TODO async
        }

        public ICommand openBlurryPicsPage => new Command(async () =>
        {
            ScanOptionsEnum option = ScanOptionsEnum.blurryPics;
            double value = OptionValues[option];
            await Navigation.PushAsync(new ScanOptionDisplayPage(value, option));
        });

        public ICommand openDarkPicsPage => new Command(async () =>
        {
            ScanOptionsEnum option = ScanOptionsEnum.darkPics;
            double value = OptionValues[option];
            await Navigation.PushAsync(new ScanOptionDisplayPage(value, option));
        });

        public ICommand openSimilarPicsPage => new Command(async () =>
        {
            ScanOptionsEnum option = ScanOptionsEnum.similarPics;
            double value = OptionValues[option];
            await Navigation.PushAsync(new ScanOptionDisplayPage(value, option));
        });
    }
}