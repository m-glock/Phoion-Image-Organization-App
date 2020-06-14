using System;
using System.Diagnostics;
using System.Windows.Input;
using DLuOvBamG.Models;
using Xamarin.Forms;

namespace DLuOvBamG.ViewModels
{
    public class ImageDetailViewModel : ContentPage
    {
        public Picture Image { get; set; }
        public string debugString { get; set; } = "test";
        public ImageDetailViewModel(Picture item)
        {
           
            Image = item;
            debugString = "5";

        }


        public ICommand MenuTapped
        {
            get
            {
                return new Command((object componentIdentifier) =>
                {
                    onTabPressed(componentIdentifier);
                });
            }
        }

        public void onTabPressed(object componentIdentifier)
        {
            if (componentIdentifier is string)
            {
                debugtest((string)componentIdentifier);
                switch ((string)componentIdentifier)
                {
                    case "categories":
                        debugtest("1");
                         break;
                    case "info":
                        break;
                    case "delete":
                        break;
                    case "similar":
                        
                        break;
                }
            }
        }

        public void debugtest(string debugtext)
        {
            debugString = debugtext;
            
        }
    }
}