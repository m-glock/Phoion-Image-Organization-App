﻿using System;

using DLuOvBamG.Models;
using System.Collections.Generic;

namespace DLuOvBamG.ViewModels
{
    public class ScanOptionDisplayViewModel : BaseViewModel
    {
		public List<List<Picture>> Pictures;

		public ScanOptionDisplayViewModel()
        {
			Title = "Aufräumergebnisse"; 
			
			string[] images = {
				"https://farm9.staticflickr.com/8625/15806486058_7005d77438.jpg",
				"https://farm5.staticflickr.com/4011/4308181244_5ac3f8239b.jpg",
				"https://farm8.staticflickr.com/7423/8729135907_79599de8d8.jpg",
				"https://farm3.staticflickr.com/2475/4058009019_ecf305f546.jpg",
				"https://farm6.staticflickr.com/5117/14045101350_113edbe20b.jpg",
				"https://farm8.staticflickr.com/7524/15620725287_3357e9db03.jpg",
				"https://farm9.staticflickr.com/8351/8299022203_de0cb894b0.jpg",
			};

			for (int i = 0; i < images.Length; i++)
			{
				Picture picture = new Picture
				{
					Id = i.ToString(),
					Uri = images[i]
				};
			}
		}

		public List<Picture> GetPictureListName(int groupID)
		{
			return Pictures[groupID];
		}
	}
}