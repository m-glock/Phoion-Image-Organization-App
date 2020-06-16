using System;

using DLuOvBamG.Models;
using System.Collections.Generic;
using DLuOvBamG.Services;
using System.Text.RegularExpressions;

namespace DLuOvBamG.ViewModels
{
    public class ScanOptionDisplayViewModel : BaseViewModel
    {
		public List<List<Picture>> Pictures;

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