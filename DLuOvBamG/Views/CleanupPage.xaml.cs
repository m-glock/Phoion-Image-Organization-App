using DLuOvBamG.Models;
using DLuOvBamG.ViewModels;
using System;
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
        private Dictionary<Label, Switch> optionSwitches;

        public CleanupPage()
        {
            InitializeComponent(); 
            vm = BindingContext as CleanupViewModel;
            vm.Navigation = Navigation;
            vm.ScanButton = ScanButton;
            GetSwitches();
            vm.optionSwitches = optionSwitches;
            ScanButton.IsEnabled = false;
        }

        private void GetSwitches()
        {
            optionSwitches = new Dictionary<Label, Switch>();
            optionSwitches.Add(blurryLabel, blurrySwitch);
            optionSwitches.Add(darkLabel, darkSwitch);
            optionSwitches.Add(similarLabel, similarSwitch);
            optionSwitches.Add(duplicateLabel, duplicateSwitch);
            optionSwitches.Add(videoLabel, videoSwitch);
        }

        private void OptionToggled(object sender, ToggledEventArgs e)
        {
            Switch optionToggle = sender as Switch;
            vm.UpdateScanOptions(optionToggle.ClassId);
            if(!optionToggle.IsToggled)
            {
                vm.checkToDisableScanButton();
            }
        }

        private void OptionLabelTapped(object sender, EventArgs e)
        {
            Label optionLabel = sender as Label;
            Switch optionSwitch = optionSwitches[optionLabel];
            optionSwitch.IsToggled = !optionSwitch.IsToggled;
        }
    }
}