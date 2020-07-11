using Xamarin.Forms;
using DLuOvBamG.Services;
using DLuOvBamG.Views;
using System.Threading;
using System;

namespace DLuOvBamG
{
    public partial class App : Application
    {

        public static TensorflowExecutor tf;
        public App()
        {
            InitializeComponent();
            tf = new TensorflowExecutor();

            Device.SetFlags(new string[] { "Expander_Experimental" });
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
