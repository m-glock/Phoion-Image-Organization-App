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
        public Dictionary<ScanOptionsEnum, double> ScanOptions;
        public List<Expander> expander { get; set; }
        public string SimilarPrecision { get; }
        public string DarkPrecision { get; }
        public string BlurryPrecision { get; }


        public CleanupViewModel()
        {
            Title = "Aufräumen";
            ScanOptions = new Dictionary<ScanOptionsEnum, double>();
        }

        public void UpdateScanOptions(ScanOptionsEnum option, Button scanButton, double sliderValue)
        {
            UpdateScanOptionsList(option, sliderValue);
            scanButton.IsEnabled = true;
        }

        private void UpdateScanOptionsList(ScanOptionsEnum option, double sliderValue)
        {
            if (ScanOptions.ContainsKey(option))
                ScanOptions.Remove(option);
            else
                ScanOptions.Add(option, sliderValue);
        }

        public void SetScanOptionSliderInitialValue(ScanOptionsEnum option, Slider slider)
        {
            int presicionValue = option.GetDefaultPresicionValue();
            ScanOptions[option] = presicionValue;
            slider.Value = presicionValue;
        }

        public void UpdateScanOptionSliderValue(ScanOptionsEnum option, double value)
        {
            if (ScanOptions.ContainsKey(option))
                ScanOptions[option] = value;
        }

        public void CheckToDisableScanButton(Button scanButton, List<Switch> optionSwitches)
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
            await Navigation.PushAsync(new ScanResultPage(ScanOptions));
        });

    }
}