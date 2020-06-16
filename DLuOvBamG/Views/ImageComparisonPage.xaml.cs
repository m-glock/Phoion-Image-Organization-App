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

        public ImageComparisonPage(List<Picture> similarPictures)
        {
            VM = new ImageComparisonViewModel();
            VM.SimilarPictures = similarPictures;
            BindingContext = VM;
            InitializeComponent();
            //VM.CurrentPicture = (Picture)ImageCarouselView.CurrentItem;
        }
    }
}