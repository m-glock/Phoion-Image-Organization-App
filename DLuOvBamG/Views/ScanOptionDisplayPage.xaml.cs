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

		public ScanOptionDisplayPage(double optionValue, ScanOptionsEnum option)
		{
			InitializeComponent();
			VM = BindingContext as ScanOptionDisplayViewModel;
			VM.Navigation = Navigation;
			VM.Option = option;
			List<List<Picture>> pictures = App.tf.GetPicturesForOption(option);
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

			CollectionView colView = new CollectionView()
			{
				SelectionMode = SelectionMode.Single
			};
			colView.HeightRequest = 100;
			colView.ClassId = groupNumber.ToString();
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
			colView.SelectionChanged += OnCollectionViewSelectionChanged;

			StackLayout.Children.Add(colView);
		}

		void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			CollectionView view = (CollectionView)sender;
			string groupID = view.ClassId;
			Picture selectedPicture = (Picture)e.CurrentSelection[0];
			VM.OpenComparisonPage(selectedPicture, groupID);
		}

		private void ValueChanged(object sender, ValueChangedEventArgs e)
		{
			Slider optionElement = sender as Slider;
			VM.Precision = e.NewValue;
			//TODO: Change Content of page while Slider is moved or when movement is finished?
		}
	}
}