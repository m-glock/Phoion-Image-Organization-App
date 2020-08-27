using DLuOvBamG.Models;
using DLuOvBamG.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;

namespace DLuOvBamG.Views
{
    [DesignTimeVisible(false)]
    public partial class CleanupPage : ContentPage
    {
        private CleanupViewModel VM;
        private Dictionary<string, ScanOptionViewGroup> ViewGroups;
        private List<Switch> SwitchList;

        public CleanupPage()
        {
            InitializeComponent();
            VM = BindingContext as CleanupViewModel;
            VM.Navigation = Navigation;

            GetOptionElements();
            foreach (ScanOptionViewGroup viewGroup in ViewGroups.Values)
            {
                VM.SetScanOptionSliderInitialValue(viewGroup.Option, viewGroup.OptionSlider);
            }
            ScanButton.IsEnabled = false;
        }

        /** 
         * create groups of UI elements for each scan option for easier access
         */
        private void GetOptionElements()
        {
            ViewGroups = new Dictionary<string, ScanOptionViewGroup>();
            ViewGroups.Add("similarPics", new ScanOptionViewGroup(ScanOptionsEnum.similarPics, similarSwitch, similarExpander, similarSlider));
            ViewGroups.Add("blurryPics", new ScanOptionViewGroup(ScanOptionsEnum.blurryPics, blurrySwitch, blurryExpander, blurrySlider));
            ViewGroups.Add("darkPics", new ScanOptionViewGroup(ScanOptionsEnum.darkPics, darkSwitch, darkExpander, darkSlider));
            SwitchList = new List<Switch>() { similarSwitch, blurrySwitch, darkSwitch };
        }

        private void OptionToggled(object sender, ToggledEventArgs e)
        {
            Switch optionToggle = sender as Switch;
            ScanOptionViewGroup viewGroup = ViewGroups[optionToggle.ClassId];
            VM.UpdateScanOptions(viewGroup.Option, ScanButton, viewGroup.OptionSlider.Value);
            if (!optionToggle.IsToggled)
            {
                VM.CheckToDisableScanButton(ScanButton, SwitchList);
                viewGroup.OptionExpander.IsExpanded = false;
            }
            else
            {
                viewGroup.OptionExpander.IsExpanded = true;
            }
        }

        private void OptionTapped(object sender, EventArgs e)
        {
            Element optionElement = sender as Element;
            ScanOptionViewGroup viewGroup = ViewGroups[optionElement.ClassId];
            if (viewGroup != null)
            {
                Switch optionSwitch = viewGroup.OptionSwitch;
                optionSwitch.IsToggled = !optionSwitch.IsToggled;
            }
        }

        /**
         * Update slider value
         */
        private void ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Element optionElement = sender as Element;
            ScanOptionViewGroup viewGroup = ViewGroups[optionElement.ClassId]; 
            var newStep = Math.Round(e.NewValue);

            viewGroup.OptionSlider.Value = newStep;

            VM.UpdateScanOptionSliderValue(viewGroup.Option, newStep);
        }
    }
}