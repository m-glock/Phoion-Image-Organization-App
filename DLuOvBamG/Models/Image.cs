using System;
using System.Collections.Generic;
using System.Text;

namespace DLuOvBamG.Models
{
	// TODO: remove and replace in ScanResultViewModel.cs
	public class Image
	{
		public string Name { get; set; }
		public string ImageUrl { get; set; }

		public Image(string Name, string URL)
		{
			this.Name = Name;
			ImageUrl = URL;
		}
	}
}
