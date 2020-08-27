using System;
using Xamarin.Forms;

namespace DLuOvBamG.Views
{
    /*
     * from: https://theconfuzedsourcecode.wordpress.com/2017/03/12/lets-override-navigation-bar-back-button-click-in-xamarin-forms/
     * Gets or Sets the Back button click overriden custom action
     */
    public class CustomBackButtonPage : ContentPage
    {
        public Action CustomBackButtonAction { get; set; }

        public static readonly BindableProperty EnableBackButtonOverrideProperty =
               BindableProperty.Create(
               nameof(EnableBackButtonOverride),
               typeof(bool),
               typeof(CustomBackButtonPage),
               false);

        /*
         * Gets or Sets Custom Back button overriding state
         */
        public bool EnableBackButtonOverride
        {
            get
            {
                return (bool)GetValue(EnableBackButtonOverrideProperty);
            }
            set
            {
                SetValue(EnableBackButtonOverrideProperty, value);
            }
        }
    }
}