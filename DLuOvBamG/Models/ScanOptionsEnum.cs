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

        //TODO: finde passende Defaultwerte für alle Optionen
        public static int GetDefaultPresicionValue(this ScanOptionsEnum s1)
        {
            switch (s1)
            {
                case ScanOptionsEnum.blurryPics:
                    return 4;
                case ScanOptionsEnum.darkPics:
                    return 9;
                case ScanOptionsEnum.similarPics:
                    return 1;
                default:
                    return 0;
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
