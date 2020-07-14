using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.OS;
using DLuOvBamG.Views;
using System.Linq;
using System;

namespace DLuOvBamG.Droid
{
    [Activity(Label = "DLuOvBamG", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
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

            Android.Support.V7.Widget.Toolbar toolbar = this.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			// check if the current item id 
			// is equals to the back button id
			if (item.ItemId == 16908332)
			{
				// retrieve the current xamarin forms page instance
				try
				{
					var currentpage = (CustomBackButtonPage)Xamarin.Forms.Application.Current.MainPage.Navigation.NavigationStack.LastOrDefault();

					// check if the page has subscribed to 
					// the custom back button event
					if (currentpage?.CustomBackButtonAction != null)
					{
						// invoke the Custom back button action
						currentpage?.CustomBackButtonAction.Invoke();
						// and disable the default back button action
						return false;
					}
				}
				catch (InvalidCastException ex) { }
				// if its not subscribed then go ahead 
				// with the default back button action
				return base.OnOptionsItemSelected(item);
			}
			else
			{
				// since its not the back button 
				//click, pass the event to the base
				return base.OnOptionsItemSelected(item);
			}
		}

		public override void OnBackPressed()
		{
			// this is not necessary, but in Android user 
			// has both Nav bar back button and
			// physical back button its safe 
			// to cover the both events

			try
			{
				// retrieve the current xamarin forms page instance
				var currentpage = (CustomBackButtonPage)Xamarin.Forms.Application.Current.MainPage.Navigation.NavigationStack.LastOrDefault();

				// check if the page has subscribed to 
				// the custom back button event
				if (currentpage?.CustomBackButtonAction != null)
				{
					currentpage?.CustomBackButtonAction.Invoke();
				} else
                {
					base.OnBackPressed();
				}
			}
			catch (InvalidCastException ex) { base.OnBackPressed(); }
		}
	}
}