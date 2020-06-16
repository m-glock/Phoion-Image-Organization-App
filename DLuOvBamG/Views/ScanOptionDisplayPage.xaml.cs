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
		ScanOptionDisplayViewModel VM;

		public ScanOptionDisplayPage(double optionValue, ScanOptionsEnum option)
		{
			InitializeComponent();
			VM = BindingContext as ScanOptionDisplayViewModel;
			List<List<Picture>> pictures = App.tf.GetAllPicturesForOption(option);
			VM.Pictures = pictures;
			slider.Value = optionValue;

			Title = option.GetTextForDisplay();

            for (int i = 0; i < pictures.Count; i++)
            {
				AddCollectionViewToPage(i);
            }
		}

		public void AddCollectionViewToPage(int groupNumber)
		{
			Label label = new Label();
			label.Text = "Set " + (groupNumber + 1);
			label.FontAttributes = FontAttributes.Bold;
			label.Margin = new Thickness(10, 20, 0, 0);
			StackLayout.Children.Add(label);

			CollectionView colView = new CollectionView();
			colView.HeightRequest = 100;
			colView.ItemsLayout = LinearItemsLayout.Horizontal;
			colView.ItemsSource = VM.GetPictureListForGroup(groupNumber);
			
			colView.ItemTemplate = new DataTemplate(() =>
			{
				ContentView contentView = new ContentView();
				contentView.Padding = new Thickness(2);

				Image image = new Image { Aspect = Aspect.AspectFill, WidthRequest = 100 };
				image.SetBinding(Image.SourceProperty, "Uri");

				contentView.Content = image;
				return contentView;
			});

			StackLayout.Children.Add(colView);
		}
	}
}