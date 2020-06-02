using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using DLuOvBamG.Models;
using DLuOvBamG.Views;
using System.Collections.Generic;

namespace DLuOvBamG.ViewModels
{
    public class ScanResultViewModel : BaseViewModel
    {
		public List<Picture> BlurryGalleryImages { get; private set; }
		public List<Picture> DarkGalleryImages { get; private set; }
		public List<Picture> SimilarGalleryImages { get; private set; }
		public List<Picture> DuplicateGalleryImages { get; private set; }
		public List<Picture> Videos { get; private set; }

		public ScanResultViewModel()
        {
			BlurryGalleryImages = new List<Picture>();
			DarkGalleryImages = new List<Picture>();
			SimilarGalleryImages = new List<Picture>();
			DuplicateGalleryImages = new List<Picture>();
			Videos = new List<Picture>(); ;

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
				BlurryGalleryImages.Add(picture);
				DarkGalleryImages.Add(picture);
				SimilarGalleryImages.Add(picture);
				DuplicateGalleryImages.Add(picture);
				Videos.Add(picture);
			}
		}

		public String GetPictureListName(ScanOptionsEnum option)
		{
			switch (option)
			{
				case ScanOptionsEnum.blurryPics:
					return "BlurryGalleryImages";
				case ScanOptionsEnum.darkPics:
					return "DarkGalleryImages";
				case ScanOptionsEnum.similarPics:
					return "SimilarGalleryImages";
				case ScanOptionsEnum.duplicatePics:
					return "DuplicateGalleryImages";
				case ScanOptionsEnum.longVideos:
					return "Videos";
				default:
					return "";
			}
		}
	}
}