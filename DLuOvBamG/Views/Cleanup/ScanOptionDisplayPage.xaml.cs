using DLuOvBamG.Models;
using DLuOvBamG.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

			// change list ton collection view so that it gets updated when a pictures is deleted
			List<List<Picture>> pictures = App.tf.GetAllPicturesForOption(option);
			ObservableCollection<ObservableCollection<Picture>> obsvPictures = new ObservableCollection<ObservableCollection<Picture>>();
			foreach (List<Picture> picturesList in pictures)
            {
				obsvPictures.Add(new ObservableCollection<Picture>(picturesList));
			}

			VM = new ScanOptionDisplayViewModel(option, obsvPictures);
			BindingContext = VM;

			Slider.Value = optionValue;
			Title = option.GetTextForDisplay();

			for (int i = 0; i < pictures.Count; i++)
			{
				AddCollectionViewToPage(i);
			}
		}

		public void AddCollectionViewToPage(int groupID)
        {
			//get correct text for collectionview label
			if (Option.Equals(ScanOptionsEnum.darkPics) || Option.Equals(ScanOptionsEnum.similarPics))
			{
				Label label = new Label();
				if (Option.Equals(ScanOptionsEnum.darkPics))
				{
					label.Text = groupID == 0 ? "Dunkle Bilder" : "Helle Bilder";
				}
				else
				{
					label.Text = "Set " + (groupID + 1);
				}
				label.FontAttributes = FontAttributes.Bold;
				label.Margin = new Thickness(10, 20, 0, 0);
				StackLayout.Children.Add(label);
			}

			CollectionView colView = new CollectionView();
			if (Option.Equals(ScanOptionsEnum.similarPics))
			{
				colView.ItemsLayout = LinearItemsLayout.Horizontal; 
				colView.HeightRequest = 100;
			}
			else
			{
				colView.ItemsLayout = new GridItemsLayout(3, ItemsLayoutOrientation.Vertical);
				colView.HeightRequest = Math.Ceiling(VM.Pictures[groupID].Count / 3.0) * 100 + 20;
			}

			colView.SetBinding(ItemsView.ItemsSourceProperty, "Pictures[" + groupID + "]");

			colView.ItemTemplate = new DataTemplate(() =>
			{
				ContentView contentView = new ContentView();
				contentView.Padding = new Thickness(2);

				Image image = new Image { Aspect = Aspect.AspectFill};
                if (Option.Equals(ScanOptionsEnum.similarPics))
				{
					image.WidthRequest = 100;
				}
				else
                {
					image.HeightRequest = 100;

				}
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
		}
	}
}