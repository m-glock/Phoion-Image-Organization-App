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

			// change list to collection view so that it gets updated when a pictures is deleted
			List<List<Picture>> pictures = App.tf.GetAllPicturesForOption(option);
			ObservableCollection<ObservableCollection<Picture>> obsvPictures = new ObservableCollection<ObservableCollection<Picture>>();
			foreach (List<Picture> picturesList in pictures)
            {
				obsvPictures.Add(new ObservableCollection<Picture>(picturesList));
			}

			VM = new ScanOptionDisplayViewModel(option, obsvPictures, Navigation);
			BindingContext = VM;

			Slider.Value = optionValue;
			Title = option.GetTextForDisplay();

			// add an image grid or image slider for each collection
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
					label.Text = groupID == 0 ? "Dark Pictures" : "Bright Pictures";
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
			colView.SelectionMode = SelectionMode.Single;
			colView.ClassId = groupID.ToString();

			// initialize image grid depending on the scan option
			// blurry and dark pictures will be shown in three columns and x rows
			// similar pictures will be shown with one row and x columns
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
			colView.SelectionChanged += OnCollectionViewSelectionChanged;

			StackLayout.Children.Add(colView);
		}

		/*
		 * If one Image is clicked, either open comparison view or just image detail view
		 * */
		void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			CollectionView view = (CollectionView)sender;
			string groupID = view.ClassId;

			if (e.CurrentSelection.Count > 0)
			{
				Picture selectedPicture = (Picture)e.CurrentSelection[0];
				view.SelectedItem = null;
				if (Option.Equals(ScanOptionsEnum.similarPics))
				{
					VM.OpenComparisonPage(selectedPicture, groupID);
				}
				else
				{
					VM.OpenImageDetailViewPage(selectedPicture);
				}
			}
		}

		/*
		 * reload page if slider value has changed
		 * */
		private void ValueChanged(object sender, ValueChangedEventArgs e)
		{
			VM.Precision = Math.Round(e.NewValue);
		}
	}
}