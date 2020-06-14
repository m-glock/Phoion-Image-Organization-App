using System;

using DLuOvBamG.Models;
using System.Collections.Generic;
using DLuOvBamG.Services;

namespace DLuOvBamG.ViewModels
{
    public class ScanOptionDisplayViewModel : BaseViewModel
    {
		public List<List<Picture>> Pictures;

		public ScanOptionDisplayViewModel()
        {
			Title = "Aufräumergebnisse";

		}

		public void getImagesFromTensorflow()
        {

		}

		public List<Picture> GetPictureListName(int groupID)
		{
			return Pictures[groupID];
		}
	}
}