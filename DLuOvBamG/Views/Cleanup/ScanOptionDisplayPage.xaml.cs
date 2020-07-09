using DLToolkit.Forms.Controls;
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
		private ScanOptionDisplayViewModel VM;
		private ScanOptionsEnum Option;

		public ScanOptionDisplayPage(double optionValue, ScanOptionsEnum option)
		{
			InitializeComponent();
			Option = option;
			VM = BindingContext as ScanOptionDisplayViewModel;
			List<List<Picture>> pictures = App.tf.GetAllPicturesForOption(option);
			VM.Pictures = pictures;
			Slider.Value = optionValue;

			Title = option.GetTextForDisplay();

			AdaptViewToScanOption();
		}

		private void AdaptViewToScanOption()
        {
            switch (Option)
            {
                case ScanOptionsEnum.blurryPics:
					// add one image grid for all blurry pictures
					AddCollectionViewGridToPage(0);
					break;
                case ScanOptionsEnum.darkPics:
					// add two image grids, one for dark and one for bright pictures
                    for (int i = 0; i < VM.Pictures.Count; i++)
                    {
						AddCollectionViewGridToPage(i);
					}
                    break;
                case ScanOptionsEnum.similarPics:
					// add one collectionView for each set of similar pictures found
					for (int i = 0; i < VM.Pictures.Count; i++)
					{
						AddCollectionViewToPage(i);
					}
					break;
            }
        }

		public void AddCollectionViewGridToPage(int groupID) {
			
            if (Option.Equals(ScanOptionsEnum.darkPics)){
				Label label = new Label();
				label.Text = groupID == 0 ? "Dunkle Bilder" : "Helle Bilder";
				label.FontAttributes = FontAttributes.Bold;
				label.Margin = new Thickness(10, 20, 0, 0);
				StackLayout.Children.Add(label);
			}

			CollectionView colView = new CollectionView();
			colView.ItemsLayout = new GridItemsLayout(3, ItemsLayoutOrientation.Vertical);
			colView.HeightRequest = 100;

			colView.ItemsSource = VM.GetPictureListForGroup(groupID);

			colView.ItemTemplate = new DataTemplate(() =>
			{
				ContentView contentView = new ContentView();
				contentView.Padding = new Thickness(2);

				Image image = new Image { Aspect = Aspect.AspectFill, HeightRequest = 100 };
				image.SetBinding(Image.SourceProperty, "Uri");

				contentView.Content = image;
				return contentView;
			});

			StackLayout.Children.Add(colView);
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

		private void ValueChanged(object sender, ValueChangedEventArgs e)
		{
			Slider optionElement = sender as Slider;
			VM.Precision = e.NewValue;
			//TODO: Change Content of page while Slider is moved or when movement is finished?
		}
	}
}