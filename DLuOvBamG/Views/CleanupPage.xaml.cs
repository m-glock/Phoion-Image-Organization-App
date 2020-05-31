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
        private ScanOptionViewGroup Similar;
        private ScanOptionViewGroup Blurry;
        private ScanOptionViewGroup Dark;
        private List<Switch> switchList;

        public CleanupPage()
        {
            InitializeComponent();
            VM = BindingContext as CleanupViewModel;
            VM.Navigation = Navigation;
            GetOptionElements();
            ScanButton.IsEnabled = false;
        }

        private void GetOptionElements()
        {
            Similar = new ScanOptionViewGroup(ScanOptionsEnum.similarPics, similarSwitch, similarExpander, similarSlider);
            Blurry = new ScanOptionViewGroup(ScanOptionsEnum.blurryPics, blurrySwitch, blurryExpander, blurrySlider);
            Dark = new ScanOptionViewGroup(ScanOptionsEnum.darkPics, darkSwitch, darkExpander, darkSlider);
            switchList = new List<Switch>() { similarSwitch, blurrySwitch, darkSwitch };
        }

        private void OptionToggled(object sender, ToggledEventArgs e)
        {
            Switch optionToggle = sender as Switch;
            ScanOptionViewGroup viewGroup = GetOptionElementsFromClassID(optionToggle.ClassId);
            VM.UpdateScanOptions(viewGroup.Option, ScanButton);
            if (!optionToggle.IsToggled)
            {
                VM.checkToDisableScanButton(ScanButton, switchList);
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

        private ScanOptionViewGroup GetOptionElementsFromClassID(String classID)
        {
            switch (classID)
            {
                case "blurryPics":
                    return Blurry;
                case "darkPics":
                    return Dark;
                case "similarPics":
                    return Similar;
                default:
                    return null;
            }
        }
    }
}