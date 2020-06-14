using Xamarin.Forms;
using DLuOvBamG.Services;
using DLuOvBamG.Views;

namespace DLuOvBamG
{
    public partial class App : Application
    {
        static IClassifier classifier;
        public App()
        {
            InitializeComponent();

            Device.SetFlags(new string[] { "Expander_Experimental" });
            DependencyService.Register<MockDataStore>();
            MainPage = new NavigationPage(new ImageGrid());
            
            classifier = DependencyService.Get<IClassifier>();
            // Debug
            classifier.test();
            
        }

        public static IClassifier Classifier
        {
            get
            {
                return classifier;
            }
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
