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

		public List<Models.Image> GalleryImages { get; private set; }
		public List<Models.Image> GalleryImages2 { get; private set; }
		public IList<Models.Image> SelectedImages { get; set; }

		public ScanResultViewModel()
        {
			SelectedImages = new List<Models.Image>();
			GalleryImages = new List<Models.Image>
			{
				new Models.Image("bla", "katzen.jpg"),
				new Models.Image("bla", "FB4_IMI.jpg"),
				new Models.Image("bla", "squirrel.jpg"),
				new Models.Image("bla", "sdp.jpg"),
				new Models.Image("bla", "volleyball.png"),
				new Models.Image("bla", "katzen2.jpg"),
			};

			GalleryImages2 = new List<Models.Image>
			{
				new Models.Image("bla", "volleyball.png"),
				new Models.Image("bla", "sdp.jpg"),
				new Models.Image("bla", "katzen2.jpg"),
				new Models.Image("bla", "katzen.jpg"),
				new Models.Image("bla", "squirrel.jpg"),
				new Models.Image("bla", "FB4_IMI.jpg"),
				
			};

			Console.WriteLine("Created gallery images.");
		}

		public void printSelectedItems()
		{
			foreach (Models.Image image in SelectedImages)
			{
				Console.WriteLine(image.Name + " is currently selected.");
			}
		}

		public void changeSelection(IList<Models.Image> selected)
		{
			SelectedImages = selected;
		}
	}
}