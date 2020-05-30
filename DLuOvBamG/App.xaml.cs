using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DLuOvBamG.Services;
using DLuOvBamG.Views;

namespace DLuOvBamG
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new NavigationPage(new ImageGrid());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
