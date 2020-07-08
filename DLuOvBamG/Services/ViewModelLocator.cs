using DLuOvBamG.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace DLuOvBamG.Services
{
    public class ViewModelLocator
    {
        private ImageTagViewModel imageTagViewModel;
        public ImageTagViewModel ImageTagViewModel
        {
            get
            {
                if (imageTagViewModel == null)
                    imageTagViewModel = new ImageTagViewModel();

                return imageTagViewModel;
            }
        }

        public ViewModelLocator()
        {

        }
    }
}