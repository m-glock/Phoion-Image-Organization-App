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
        public static string GetTextForDisplay(this ScanOptionsEnum s1)
        {
            switch (s1)
            {
                case ScanOptionsEnum.blurryPics:
                    return "Blurry Pictures";
                case ScanOptionsEnum.darkPics:
                    return "Dark Pictures";
                case ScanOptionsEnum.similarPics:
                    return "Similar Pictures";
                default:
                    return null;
            }
        }

        public static int GetDefaultPresicionValue(this ScanOptionsEnum s1)
        {
            switch (s1)
            {
                case ScanOptionsEnum.blurryPics:
                    return 5;
                case ScanOptionsEnum.darkPics:
                    return 3;
                case ScanOptionsEnum.similarPics:
                    return 5;
                default:
                    return 0;
            }
        }
    }
}
