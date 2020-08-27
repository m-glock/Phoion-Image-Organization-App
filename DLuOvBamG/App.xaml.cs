using Xamarin.Forms;
using DLuOvBamG.Services;
using DLuOvBamG.Views;

namespace DLuOvBamG
{
    public partial class App : Application
    {
        static ImageOrganizationDatabase database;
        static IClassifier classifier;
        static ViewModelLocator viewModelLocator;
        public static TensorflowExecutor tf;
        public static string CurrentSortKey;
        public static string CurrentGroup;

        public App()
        {
            InitializeComponent();
            tf = new TensorflowExecutor();
            CurrentSortKey = "";
            CurrentGroup = "";
            Device.SetFlags(new string[] { "Expander_Experimental" });
            MainPage = new NavigationPage(new ImageGallery());
        }

        public static IClassifier Classifier
        {
            get
            {
                if(classifier == null)
                    classifier = DependencyService.Get<IClassifier>();
                return classifier;
            }
        }

        public static ImageOrganizationDatabase Database
        {
            get
            {
                if (database == null)
                    database = new ImageOrganizationDatabase();
                return database;
            }
        }

        public static ViewModelLocator ViewModelLocator
        {
            get 
            {
                if (viewModelLocator == null)
                    viewModelLocator = new ViewModelLocator();
                return viewModelLocator;
            }
        }

        protected override void OnStart() { }

        protected override void OnSleep() { }

        protected override void OnResume() { }
    }
}
