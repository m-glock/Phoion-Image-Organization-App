using DLuOvBamG.Models;
using DLuOvBamG.Views;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace DLuOvBamG.ViewModels
{
    public class CleanupViewModel : BaseViewModel
    {
        public INavigation Navigation { get; set; }
        public Dictionary<ScanOptionsEnum, double> scanOptions;
        //public List<ScanOptionsEnum> scanOptions;
        public List<Expander> expander { get; set; }

        public CleanupViewModel()
        {
            Title = "Aufräumen";
            //scanOptions = new List<ScanOptionsEnum>();
            scanOptions = new Dictionary<ScanOptionsEnum, double>();
        }

        public void UpdateScanOptions(ScanOptionsEnum option, Button scanButton, double sliderValue)
        {
            updateScanOptionsList(option, sliderValue);
            scanButton.IsEnabled = true;
        }

        private void updateScanOptionsList(ScanOptionsEnum option, double sliderValue)
        {
            if (scanOptions.ContainsKey(option))
                scanOptions.Remove(option);
            else
                scanOptions.Add(option, sliderValue);
            /*if (scanOptions.Contains(option))
                scanOptions.Remove(option);
            else
                scanOptions.Add(option);  */
        }

        //TODO: how to not change them all the time
        public void updateScanOptionSliderValue(ScanOptionsEnum option, double value)
        {
            if (scanOptions.ContainsKey(option))
                scanOptions[option] = value;
        }

        public void checkToDisableScanButton(Button scanButton, List<Switch> optionSwitches)
        {
            bool optionsChosen = false;
            foreach (Switch optionSwitch in optionSwitches)
            {
                if (optionSwitch.IsToggled)
                {
                    optionsChosen = true;
                    break;
                }
            }
            scanButton.IsEnabled = optionsChosen;
        }

        public ICommand StartScan => new Command(async () => {
            await Navigation.PushAsync(new ScanResultPage(scanOptions));
        });

    }
}