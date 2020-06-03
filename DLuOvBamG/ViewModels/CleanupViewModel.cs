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
        public Button ScanButton;
        public Dictionary<Label, Switch> optionSwitches;

        public CleanupViewModel()
        {
            Title = "Aufräumen";
            scanOptions = new List<ScanOptionsEnum>();
        }

        public void UpdateScanOptions(String optionName)
        {
            //TODO: better way of accessing the right Enum?
            var values = Enum.GetValues(typeof(ScanOptionsEnum));
            foreach(Enum enumOption in values)
            {
                if (enumOption.ToString() == optionName)
                {
                    updateScanOptionsList((ScanOptionsEnum)enumOption);
                    break;
                }
            }
            ScanButton.IsEnabled = true;
        }

        private void updateScanOptionsList(ScanOptionsEnum option)
        {
            if (scanOptions.Contains(option))
                scanOptions.Remove(option);
            else
                scanOptions.Add(option);  
        }

        public void checkToDisableScanButton()
        {
            bool optionsChosen = false;
            foreach (Switch optionSwitch in optionSwitches.Values)
            {
                if (optionSwitch.IsToggled) optionsChosen = true;
            }
            ScanButton.IsEnabled = optionsChosen;
        }

        public ICommand StartScan => new Command(async () => {
            await Navigation.PushAsync(new ScanResultPage(scanOptions));
        });

    }
}