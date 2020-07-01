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
    public partial class InfoPage : ContentPage
    {
        
        InfoViewModel IV;


        public InfoPage(Picture image)
        {
            InitializeComponent();
            IV = new InfoViewModel(image);
            BindingContext = IV;
        }
    }
}