using System;

using DLuOvBamG.Models;

namespace DLuOvBamG.ViewModels
{
    public class ImageDetailViewModel : BaseViewModel
    {
        public Picture Image { get; set; }
        public ImageDetailViewModel(Picture item)
        {
            Image = item;
        }
    }
}
