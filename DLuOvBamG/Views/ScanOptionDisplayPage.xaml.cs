﻿using DLuOvBamG.Models;
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

		public ScanOptionDisplayPage(ScanOptionsEnum option, double optionValue, List<List<Picture>> pictures)
		{
			InitializeComponent();
			VM = BindingContext as ScanOptionDisplayViewModel;
			VM.Pictures = pictures;

			Title = option.GetTextForDisplay();
            for (int  i = 0; i < pictures.Count; i++)
            {
				addCollectionViewToPage(i);
			}
		}

		public void addCollectionViewToPage(int groupNumber)
		{
			Label label = new Label();
			label.Text = "Gruppe " + groupNumber;
			label.FontAttributes = FontAttributes.Bold;
			label.Margin = new Thickness(10, 20, 0, 0);
			StackLayout.Children.Add(label);

			CollectionView colView = new CollectionView();
			colView.HeightRequest = 100;
			colView.ItemsLayout = LinearItemsLayout.Horizontal;
			//TODO: acces list in VM with list[groupNumber]?
			//colView.SetBinding(ItemsView.ItemsSourceProperty, VM.GetPictureListName(groupNumber));
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