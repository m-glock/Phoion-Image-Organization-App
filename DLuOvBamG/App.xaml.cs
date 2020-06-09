using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DLuOvBamG.Services;
using DLuOvBamG.Views;

namespace DLuOvBamG
{
    public partial class App : Application
    {
        static ImageOrganizationDatabase database;
        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new NavigationPage(new ImageGrid());
        }

        public static ImageOrganizationDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new ImageOrganizationDatabase();
                }
                return database;
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
