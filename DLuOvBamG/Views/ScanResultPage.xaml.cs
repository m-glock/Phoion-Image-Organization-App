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
		ScanOptionResultsViewModel vm;
		private Dictionary<ScanOptionsEnum, double> options;

		public ScanResultPage(Dictionary<ScanOptionsEnum, double> options)
		{
			Title = "Option 1";
			InitializeComponent();
			this.options = options;
		}
	}
}