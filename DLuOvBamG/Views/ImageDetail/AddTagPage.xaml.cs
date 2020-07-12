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
    public partial class AddTagPage : ContentPage
    {
        public AddTagPage()
        {
            InitializeComponent();
            BindingContext = App.ViewModelLocator.ImageTagViewModel;
        }
    }
}