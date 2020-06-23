using DLuOvBamG.Models;
using DLuOvBamG.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DLuOvBamG.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageComparisonPage : ContentPage
    {
        private ImageComparisonViewModel VM;

        //TODO: what if list only has one picture?
        public ImageComparisonPage(List<Picture> pictures, Picture mainPic)
        {
            VM = new ImageComparisonViewModel();
            VM.PictureList = pictures;
            
            //TODO: remove comparing picture from list
            Picture comparingPicture = mainPic;
            VM.ComparingPictureUri = comparingPicture.Uri;
            VM.CurrentPictureUri = pictures[0].Uri;

            BindingContext = VM;
            InitializeComponent();

            VM.CarouselView = ImageCarouselView;
        }

        public void OnCurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            Picture currentPicture = (Picture)e.CurrentItem;
            VM.CurrentPictureUri = currentPicture.Uri;
        }
    }
}