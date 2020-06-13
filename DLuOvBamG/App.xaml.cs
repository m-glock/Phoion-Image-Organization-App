using Xamarin.Forms;
using DLuOvBamG.Services;
using DLuOvBamG.Views;

namespace DLuOvBamG
{
    public partial class App : Application
    {
        public static Tensorflow tf;
        public App()
        {
            InitializeComponent();
            tf = new Tensorflow();

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
