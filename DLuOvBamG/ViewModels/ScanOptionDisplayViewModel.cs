using System;
using DLuOvBamG.Models;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using DLuOvBamG.Views;

namespace DLuOvBamG.ViewModels
{
    public class ScanOptionDisplayViewModel : BaseViewModel, INotifyPropertyChanged
	{
		public ScanOptionsEnum Option;
		public List<List<Picture>> Pictures;
		public double precision;
		public event PropertyChangedEventHandler PropertyChanged;
		public INavigation Navigation;
		public Xamarin.Forms.Image SelectedImage;

        public double Precision
        {
            set
            {
                if (precision != value)
                {
                    precision = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Precision"));
                }
            }
            get
            {
                return precision;
            }
        }

		public ScanOptionDisplayViewModel()
        {
			Title = "Aufräumergebnisse";
		}

		public List<Picture> GetPictureListForGroup(int groupID)
		{
			if (groupID > Pictures.Count) return null;
			return Pictures[groupID];
		}

		public async void OpenComparisonPage(Picture pic, string groupID)
		{
			try
			{
				int id = Int32.Parse(groupID);
				List<Picture> pictures = App.tf.GetPicturesForOption(Option, id);
				await Navigation.PushAsync(new ImageComparisonPage(pictures, pic));
			}
			catch (FormatException)
			{
				//TODO: output to user?
				Console.WriteLine($"Unable to parse '{groupID}'");
			}
			
		}
	}
}