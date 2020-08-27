using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.OS;
using DLuOvBamG.Views;
using System.Linq;

namespace DLuOvBamG.Droid
{
    [Activity(Label = "DLuOvBamG", Icon = "@mipmap/logo", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Android.Glide.Forms.Init(this);
            LoadApplication(new App());

            // for custom back button access
            Android.Support.V7.Widget.Toolbar toolbar = this.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        /*
         * code for custom back button action
         * from: https://theconfuzedsourcecode.wordpress.com/2017/03/12/lets-override-navigation-bar-back-button-click-in-xamarin-forms/
         */
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == 16908332)
            {
                try
                {
                    var currentpage = (CustomBackButtonPage)Xamarin.Forms.Application.Current.MainPage.Navigation.NavigationStack.LastOrDefault();
                    if (currentpage?.CustomBackButtonAction != null)
                        currentpage?.CustomBackButtonAction.Invoke();
                }
                catch (InvalidCastException ex)
                {
                    // Do nothing
                };

                return base.OnOptionsItemSelected(item);
            }
            else
            {
                return base.OnOptionsItemSelected(item);
            }
        }

        /*
         * code for custom back button action for the hardware button
         */
        public override void OnBackPressed()
        {
            try
            {
                var currentpage = (CustomBackButtonPage)Xamarin.Forms.Application.Current.MainPage.Navigation.NavigationStack.LastOrDefault();

                if (currentpage?.CustomBackButtonAction != null)
                    currentpage?.CustomBackButtonAction.Invoke();
                else
                    base.OnBackPressed();
            }
            catch (InvalidCastException e)
            {
                base.OnBackPressed();
            }
        }
    }
}