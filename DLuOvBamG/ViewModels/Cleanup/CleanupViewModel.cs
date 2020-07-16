using DLuOvBamG.Models;
using DLuOvBamG.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace DLuOvBamG.ViewModels
{
    public class CleanupViewModel : BaseViewModel, INotifyPropertyChanged
    {

        private Dictionary<ScanOptionsEnum, double> ScanOptions;
        private double similarPrecision;
        private double darkPrecision;
        private double blurryPrecision;
        public event PropertyChangedEventHandler PropertyChanged;
        public INavigation Navigation { get; set; }

        // update number that appear next to the slider
        #region Precisions
        public double SimilarPrecision
        {
            set
            {
                if (similarPrecision != value)
                {
                    similarPrecision = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SimilarPrecision"));
                }
            }
            get
            {
                return similarPrecision;
            }
        }
        public double DarkPrecision
        {
            set
            {
                if (darkPrecision != value)
                {
                    darkPrecision = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DarkPrecision"));
                }
            }
            get
            {
                return darkPrecision;
            }
        }
        public double BlurryPrecision
        {
            set
            {
                if (blurryPrecision != value)
                {
                    blurryPrecision = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BlurryPrecision"));
                }
            }
            get
            {
                return blurryPrecision;
            }
        }
        #endregion

        public CleanupViewModel()
        {
            Title = "Cleanup";
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
            slider.Value = presicionValue;
        }

        /*
         * Update the value next to the slider
         */
        public void UpdateScanOptionSliderValue(ScanOptionsEnum option, double value)
        {
            switch (option)
            {
                case ScanOptionsEnum.blurryPics:
                    BlurryPrecision = value;
                    break;
                case ScanOptionsEnum.similarPics:
                    SimilarPrecision = value;
                    break;
                case ScanOptionsEnum.darkPics:
                    DarkPrecision = value;
                    break;
            }
            if (ScanOptions.ContainsKey(option))
                ScanOptions[option] = value;
        }

        /*
         * Scan button should only be enabled if at least one scan option is toggled
         * if none are toggled, the button is disabled
         */
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
            await App.tf.FillPictureLists(ScanOptions, App.CurrentDirectory);
        });

    }
}