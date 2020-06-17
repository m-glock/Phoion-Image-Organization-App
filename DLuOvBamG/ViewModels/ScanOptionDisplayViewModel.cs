using System;

using DLuOvBamG.Models;
using System.Collections.Generic;
using DLuOvBamG.Services;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace DLuOvBamG.ViewModels
{
    public class ScanOptionDisplayViewModel : BaseViewModel, INotifyPropertyChanged
	{
		public List<List<Picture>> Pictures;
		public double precision;
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
	}
}