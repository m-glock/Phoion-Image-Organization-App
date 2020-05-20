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


        public CleanupViewModel()
        {
            Title = "Test";
            scanOptions = new List<ScanOptionsEnum>();
        }

        public void UpdateScanOptions(ScanOptionsEnum option)
        {
            if (scanOptions.Contains(option))
                scanOptions.Remove(option);
            else
                scanOptions.Add(option);
        }

        public ICommand StartScan => new Command(async () => { 
            Console.WriteLine("clicked");
            await Navigation.PushModalAsync(new NavigationPage(new ScanResultPage(scanOptions)));
            });

    }
}