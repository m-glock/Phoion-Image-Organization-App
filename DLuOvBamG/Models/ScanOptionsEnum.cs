using DLuOvBamG.Views;
using System;

namespace DLuOvBamG.Models
{
	public enum ScanOptionsEnum
	{
		blurryPics, 
		darkPics, 
		similarPics
	}

    static class ScanOptionsMethods
    {

        public static String GetTextForDisplay(this ScanOptionsEnum s1)
        {
            switch (s1)
            {
                case ScanOptionsEnum.blurryPics:
                    return "Unscharfe Bilder";
                case ScanOptionsEnum.darkPics:
                    return "Dunkle Bilder";
                case ScanOptionsEnum.similarPics:
                    return "Ähnliche Bilder";
                default:
                    return null;
            }
        }

        public static String GetNameForGalleryPage(this ScanOptionsEnum s1)
        {
            switch (s1)
            {
                case ScanOptionsEnum.blurryPics:
                    return "openBlurryPicsPage";
                case ScanOptionsEnum.darkPics:
                    return "openDarkPicsPage";
                case ScanOptionsEnum.similarPics:
                    return "openSimilarPicsPage";
                default:
                    return null;
            }
        }
    }
}
