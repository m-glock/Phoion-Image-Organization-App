using DLuOvBamG.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace DLuOvBamG.ViewModels
{
    public class ScanOptionDisplayViewModel : BaseViewModel, INotifyPropertyChanged
	{
		public List<List<Picture>> Pictures;
		public double precision;
		public event PropertyChangedEventHandler PropertyChanged;
        private ScanOptionsEnum Option;

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

		public ScanOptionDisplayViewModel(ScanOptionsEnum option)
        {
			Title = "Aufräumergebnisse";
            Option = option;
		}

		public List<Picture> GetPictureListForGroup(int groupID)
		{
			if (groupID > Pictures.Count) return null;
			return Pictures[groupID];
		}

        public ICommand UpdatePicturesAfterValueChange => new Command(async () =>
        {
            Dictionary<ScanOptionsEnum, double> dictChangedValue = new Dictionary<ScanOptionsEnum, double>();
            dictChangedValue.Add(Option, Precision);
            App.tf.FillPictureLists(dictChangedValue);
            Pictures = App.tf.GetAllPicturesForOption(Option);
            // TODO: updates automatically? Activity indicator?
        });
    }
}