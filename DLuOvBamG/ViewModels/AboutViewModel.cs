using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DLuOvBamG.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "Test";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://xamarin.com"));
        }

        public ICommand OpenWebCommand { get; }
    }
}