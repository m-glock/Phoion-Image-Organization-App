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
            ScanOptionViewGroup viewGroup = GetOptionElementsFromClassID(optionToggle.ClassId);
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
            ScanOptionViewGroup viewGroup = GetOptionElementsFromClassID(optionElement.ClassId);
            if (viewGroup != null)
            {
                Switch optionSwitch = viewGroup.OptionSwitch;
                optionSwitch.IsToggled = !optionSwitch.IsToggled;
            }
        }

        private void ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Element optionElement = sender as Element;
            ScanOptionViewGroup viewGroup = GetOptionElementsFromClassID(optionElement.ClassId);
            double value = e.NewValue;
            Console.WriteLine("Value of " + viewGroup.Option.ToString() + " has changed to " + value);
            VM.UpdateScanOptionSliderValue(viewGroup.Option, value);
        }

        private ScanOptionViewGroup GetOptionElementsFromClassID(String classID)
        {
            switch (classID)
            {
                case "blurryPics":
                    return ViewGroups["blurryPics"];
                case "darkPics":
                    return ViewGroups["darkPics"];
                case "similarPics":
                    return ViewGroups["similarPics"];
                default:
                    return null;
            }
        }
    }
}