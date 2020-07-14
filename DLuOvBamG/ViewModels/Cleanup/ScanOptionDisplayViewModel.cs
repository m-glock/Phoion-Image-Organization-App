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
		public INavigation Navigation;
		public Image SelectedImage;
		public event PropertyChangedEventHandler PropertyChanged;

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
				List<List<Picture>> allPictures = App.tf.GetAllPicturesForOption(Option);
				List<Picture> pictures = allPictures[id];
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