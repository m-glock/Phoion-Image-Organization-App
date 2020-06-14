﻿using Xamarin.Forms;
using DLuOvBamG.Services;
using DLuOvBamG.Views;

namespace DLuOvBamG
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            Device.SetFlags(new string[] { "Expander_Experimental" });
            DependencyService.Register<MockDataStore>();
            MainPage = new NavigationPage(new ImageGrid());
            
            IClassifier classifier = DependencyService.Get<IClassifier>();
            // Debug
            classifier.test();
            
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
