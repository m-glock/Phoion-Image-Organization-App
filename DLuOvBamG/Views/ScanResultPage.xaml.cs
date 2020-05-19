using DLuOvBamG.Models;
using DLuOvBamG.ViewModels;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DLuOvBamG.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ScanResultPage : ContentPage
	{
		ScanResultViewModel vm;
		
		public ScanResultPage(List<ScanOptionsEnum> options)
		{
			InitializeComponent();
			vm = BindingContext as ScanResultViewModel;
		}

		void ImageSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Console.WriteLine("selection has changed.");
			if (vm != null)
			{
				// TODO: template o.ä. für CollectionView erstellen und für jede ausgewählte Option einfügen
				// Labeltext und Binding müssen jeweils angepasst werden
			}
		}
	}
}