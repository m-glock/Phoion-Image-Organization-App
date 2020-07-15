using DLuOvBamG.Models;
using DLuOvBamG.Views;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace DLuOvBamG.ViewModels
{
    public class ScanResultViewModel : BaseViewModel
    {
        public INavigation Navigation;
        public Dictionary<ScanOptionsEnum, double> OptionValues;

        public ScanResultViewModel() { }

        public ICommand OpenScanOptionDisplayPage => new Command(async (object option) =>
        {
            ScanOptionsEnum chosenOption = (ScanOptionsEnum)option;
            int amountOfPictures = App.tf.GetAmountOfPicturesForOption(chosenOption);
            if (amountOfPictures > 0)
            {
                double value = OptionValues[chosenOption];
                await Navigation.PushAsync(new ScanOptionDisplayPage(value, chosenOption));
            }
        });
    }
}
