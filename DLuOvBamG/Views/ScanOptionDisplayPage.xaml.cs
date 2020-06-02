using DLuOvBamG.Models;
using DLuOvBamG.ViewModels;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DLuOvBamG.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ScanOptionDisplayPage : ContentPage
	{
		ScanOptionResultsViewModel vm;

		public ScanOptionDisplayPage(Dictionary<ScanOptionsEnum, double> options)
		{
			InitializeComponent();
			vm = BindingContext as ScanOptionResultsViewModel;
            foreach (KeyValuePair<ScanOptionsEnum, double> pair in options)
			{
				addCollectionViewToPage(pair.Key);
				Console.WriteLine("Option: " + pair.Key.ToString() + " with value " + pair.Value);
			}
		}

		void ImageSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Console.WriteLine("selection has changed.");
			if (vm != null)
			{

			}
		}

		public void addCollectionViewToPage(ScanOptionsEnum option)
		{
			Label label = new Label();
			label.Text = option.GetTextForDisplay();
			label.Margin = new Thickness(10, 20, 0, 0);
			StackLayout.Children.Add(label);

			CollectionView colView = new CollectionView();
			colView.HeightRequest = 100;
			colView.ItemsLayout = LinearItemsLayout.Horizontal;
			colView.SetBinding(ItemsView.ItemsSourceProperty, vm.GetPictureListName(option));
			StackLayout.Children.Add(colView);

			colView.ItemTemplate = new DataTemplate(() =>
			{
				ContentView contentView = new ContentView();
				contentView.Padding = new Thickness(2);

				Image image = new Image { Aspect = Aspect.AspectFill, WidthRequest = 100 };
				image.SetBinding(Image.SourceProperty, "Uri");

				contentView.Content = image;
				return contentView;
			});
		}
	}
}