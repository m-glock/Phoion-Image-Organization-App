using DLuOvBamG.Models;
using DLuOvBamG.ViewModels;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;

namespace DLuOvBamG.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class CleanupPage : ContentPage
    {
        private CleanupViewModel vm;
        private Dictionary<ScanOptionsEnum, Switch> optionSwitches;

        public CleanupPage()
        {
            InitializeComponent(); 
            vm = BindingContext as CleanupViewModel;
            vm.Navigation = Navigation;
            GetSwitches();
            ScanButton.IsEnabled = false;
        }

        private void GetSwitches()
        {
            optionSwitches = new Dictionary<ScanOptionsEnum, Switch>();
            optionSwitches.Add(ScanOptionsEnum.blurryPics, blurrySwitch);
            optionSwitches.Add(ScanOptionsEnum.darkPics, darkSwitch);
            optionSwitches.Add(ScanOptionsEnum.similarPics, similarSwitch);
            optionSwitches.Add(ScanOptionsEnum.duplicates, duplicateSwitch);
            optionSwitches.Add(ScanOptionsEnum.longVideos, videoSwitch);
        }

        private void OptionToggled(object sender, ToggledEventArgs e)
        {
            Switch optionToggle = sender as Switch;

            if (optionToggle.IsToggled && optionToggle != null)
            {
                vm.UpdateScanOptions(optionSwitches.FirstOrDefault(x => x.Value.Equals(optionToggle)).Key);
                ScanButton.IsEnabled = true;
            }
            else
            {
                bool optionsChosen = false;
                foreach(Switch optionSwitch in optionSwitches.Values) {
                    if (optionSwitch.IsToggled) optionsChosen = true;
                }
                ScanButton.IsEnabled = optionsChosen;
            }
        }
    }
}