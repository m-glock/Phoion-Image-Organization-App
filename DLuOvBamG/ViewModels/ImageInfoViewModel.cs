using DLuOvBamG.Models;
using System.ComponentModel;

namespace DLuOvBamG.ViewModels
{
    class ImageInfoViewModel : BaseViewModel
    {
        public string InfoName { get; };
        public string InfoLocation { get; }
        public string[] InfoTagsArray { get; }
        public string InfoTags { get; }
        public string InfoDate { get; }

        public ImageInfoViewModel(Picture image)
        {
            //TODO: read out info from DB


            /* = "test";
            infoLocation = "Berlin";
            _infoTags = new string[] { "Hund", "Berlin", "Urlaub" };
            for (int i = 0; i < _infoTags.Length; i++)
            {
                infoTags = infoTags + _infoTags[i] + ", ";
            }
            infoDate = "01.02.2010";*/
        }
    }
}
