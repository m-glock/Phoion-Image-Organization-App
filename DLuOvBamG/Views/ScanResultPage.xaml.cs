using DLuOvBamG.Models;
using DLuOvBamG.ViewModels;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DLuOvBamG.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ScanResultPage : ContentPage
	{
		private ScanResultViewModel VM;

		public ScanResultPage(Dictionary<ScanOptionsEnum, double> options)
		{
			Title = "Scanergebnisse";
			InitializeComponent();
			VM = BindingContext as ScanResultViewModel;
			VM.Navigation = Navigation;
			VM.Options = options;

            foreach(ScanOptionsEnum option in options.Keys)
            {
				showImageGroups(option);
            }
        }

		private void showImageGroups(ScanOptionsEnum option)
        {
            //TODO: how to find correct rows and columns if needed?
            //TODO: give them correct ClassID or correct binding method to know which one has been clicked -> enum
            Grid grid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(70) },
                    new RowDefinition { Height = new GridLength(5) }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition{ Width = new GridLength(5, GridUnitType.Star)},
                    new ColumnDefinition{ Width = new GridLength(45, GridUnitType.Star)},
                    new ColumnDefinition{ Width = new GridLength(42, GridUnitType.Star)},
                    new ColumnDefinition{ Width = new GridLength(3, GridUnitType.Star)},
                    new ColumnDefinition{ Width = new GridLength(5, GridUnitType.Star)},
                }
            };
            grid.Margin = new Thickness(0, 0, 0, 20);

            grid.Children.Add(new Image
            {
                Source = "https://farm9.staticflickr.com/8625/15806486058_7005d77438.jpg",
                Aspect = Aspect.AspectFill
            }, 1, 4, 0, 2) ;

            grid.Children.Add(new BoxView
            {
                BackgroundColor = Color.White,
                Opacity = 0.8
            }, 2, 4, 0, 2);

            grid.Children.Add(new Label
            {
                Text = option.GetTextForDisplay(),
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.End,
                FontAttributes = FontAttributes.Bold
            }, 2, 3, 0, 2) ;


            StackLayout.Children.Add(grid);
        }
	}
}