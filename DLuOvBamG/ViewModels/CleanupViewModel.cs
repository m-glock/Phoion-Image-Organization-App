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
        public List<ScanOptionsEnum> scanOptions;
        public List<Expander> expander { get; set; }

        public CleanupViewModel()
        {
            Title = "Aufräumen";
            scanOptions = new List<ScanOptionsEnum>();
        }

        public void UpdateScanOptions(ScanOptionsEnum option, Button ScanButton)
        {
            updateScanOptionsList(option);
            ScanButton.IsEnabled = true;
        }

        private void updateScanOptionsList(ScanOptionsEnum option)
        {
            if (scanOptions.Contains(option))
                scanOptions.Remove(option);
            else
                scanOptions.Add(option);  
        }

        public void checkToDisableScanButton(Button ScanButton, List<Switch> optionSwitches)
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
            ScanButton.IsEnabled = optionsChosen;
        }

        public ICommand StartScan => new Command(async () => {
            await Navigation.PushAsync(new ScanResultPage(scanOptions));
        });

    }
}