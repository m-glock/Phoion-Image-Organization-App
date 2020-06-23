using DLuOvBamG.Models;
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
        public String infoName { get; } = "test";
        public String infoLocation { get; }
        public String[] _infoTags { get; }
        public String infoTags { get; }
        public String infoDate { get; }

        public InfoPage(Picture image)
        {
            infoName = "test";
            infoLocation = "Berlin";
            _infoTags = new string[]{"Hund","Berlin","Urlaub"};
            for(int i = 0; i<_infoTags.Length; i++)
            {
                infoTags = infoTags + _infoTags[i] + ", ";
            }
            infoDate = "01.02.2010";
            InitializeComponent();
            BindingContext = this;
        }
    }
}