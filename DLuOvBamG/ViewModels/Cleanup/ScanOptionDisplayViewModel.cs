using DLuOvBamG.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace DLuOvBamG.ViewModels
{
    public class ScanOptionDisplayViewModel : BaseViewModel, INotifyPropertyChanged
	{
        public ObservableCollection<ObservableCollection<Picture>> pictures;
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
        public ObservableCollection<ObservableCollection<Picture>> Pictures
        {
            set
            {
                pictures = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Pictures"));
                }
            }
            get
            {
                return pictures;
            }
        }

        public ScanOptionDisplayViewModel(ScanOptionsEnum option, ObservableCollection<ObservableCollection<Picture>> pictures)
        {
			Title = "Aufräumergebnisse";
            Option = option;
            Pictures = pictures;
        }

		public ObservableCollection<Picture> GetPictureListForGroup(int groupID)
		{
			if (groupID > Pictures.Count) return null;
			return Pictures[groupID];
		}

        public ICommand UpdatePicturesAfterValueChange => new Command(async () =>
        {
            Dictionary<ScanOptionsEnum, double> dictChangedValue = new Dictionary<ScanOptionsEnum, double>();
            dictChangedValue.Add(Option, Precision);
            App.tf.FillPictureLists(dictChangedValue);

            List<List<Picture>> pictures = App.tf.GetAllPicturesForOption(Option);
            ObservableCollection<ObservableCollection<Picture>> obsvPictures = new ObservableCollection<ObservableCollection<Picture>>();
            foreach (List<Picture> picturesList in pictures)
            {
                obsvPictures.Add(new ObservableCollection<Picture>(picturesList));
            }
            Pictures = obsvPictures;
            //Pictures = pictures;
        });
    }
}