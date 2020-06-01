using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using DLuOvBamG.Models;
using DLuOvBamG.Views;
using System.Collections.Generic;

//TODO: change Image class to Picture class
namespace DLuOvBamG.ViewModels
{
    public class ScanResultViewModel : BaseViewModel
    {

		public List<Picture> GalleryImages { get; private set; }
		public List<Picture> GalleryImages2 { get; private set; }
		public IList<Picture> SelectedImages { get; set; }

		public ScanResultViewModel()
        {
			GalleryImages = new List<Picture>();
			GalleryImages2 = new List<Picture>();
			SelectedImages = new List<Picture>();

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
				Picture picture = new Picture(images[i], i.ToString());
				GalleryImages.Add(picture);
				GalleryImages2.Add(picture);
			}

			Console.WriteLine("Created gallery images.");
		}

		public void changeSelection(IList<Picture> selected)
		{
			SelectedImages = selected;
		}
	}
}