using DLuOvBamG.Models;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DLuOvBamG.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ScanResultPage : ContentPage
	{
		public ScanResultPage(List<ScanOptionsEnum> options)
		{
			Console.WriteLine("new page");
			InitializeComponent();
			String text = "";
			foreach (ScanOptionsEnum option in options)
			{
				text += (option.ToString() + ",");
			}

			Label.Text = text;
		}
	}
}