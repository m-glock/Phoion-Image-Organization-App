using System;
using Xamarin.Forms;

namespace DLuOvBamG.Views
{
    public class CustomBackButtonPage : ContentPage
    {
        /// <summary>
        /// Gets or Sets the Back button click overriden custom action
        /// </summary>
        public Action CustomBackButtonAction { get; set; }

        public static readonly BindableProperty EnableBackButtonOverrideProperty =
               BindableProperty.Create(
               nameof(EnableBackButtonOverride),
               typeof(bool),
               typeof(CustomBackButtonPage),
               false);

        /// <summary>
        /// Gets or Sets Custom Back button overriding state
        /// </summary>
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