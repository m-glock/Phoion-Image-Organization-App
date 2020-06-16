using DLuOvBamG.Models;
using DLuOvBamG.Views;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace DLuOvBamG.ViewModels
{
    public class ScanResultViewModel : BaseViewModel
    {
        public INavigation Navigation;
        public Dictionary<ScanOptionsEnum, double> Options;

        public ScanResultViewModel()
        {

        }

        public ICommand openBlurryPicsPage => new Command(async () =>
        {
            Console.WriteLine("blurry chosen");
            List<Picture> pictures = new List<Picture>();

            pictures.Add(new Picture("https://farm5.staticflickr.com/4011/4308181244_5ac3f8239b.jpg", "1"));
            pictures.Add(new Picture("https://farm9.staticflickr.com/8351/8299022203_de0cb894b0.jpg", "2"));
            pictures.Add(new Picture("https://farm6.staticflickr.com/5117/14045101350_113edbe20b.jpg", "3"));
            pictures.Add(new Picture("https://farm8.staticflickr.com/7423/8729135907_79599de8d8.jpg", "4"));
            pictures.Add(new Picture("https://farm5.staticflickr.com/4011/4308181244_5ac3f8239b.jpg", "5"));
            pictures.Add(new Picture("https://farm9.staticflickr.com/8351/8299022203_de0cb894b0.jpg", "6"));
            pictures.Add(new Picture("https://farm6.staticflickr.com/5117/14045101350_113edbe20b.jpg", "7"));
            pictures.Add(new Picture("https://farm8.staticflickr.com/7423/8729135907_79599de8d8.jpg", "8"));

            await Navigation.PushAsync(new ImageComparisonPage(pictures));
            //await Navigation.PushAsync(new ScanOptionDisplayPage());
        });

        public ICommand openDarkPicsPage => new Command(async () =>
        {
            Console.WriteLine("dark chosen");
            //await Navigation.PushAsync(new ScanOptionDisplayPage());
        });

        public ICommand openSimilarPicsPage => new Command(async () =>
        {
            Console.WriteLine("similar chosen");
            //await Navigation.PushAsync(new ScanOptionDisplayPage());
        });
        
        /*public ICommand ShowImages => new Command(async () => {
                //TODO: get correct Group (Enum), slidervalue and pictureList
                ScanOptionsEnum option = ScanOptionsEnum.blurryPics;
                double sliderValue = 0.5;
                List<List<Picture>> pictures = new List<List<Picture>>();
                pictures.Add(new List<Picture>());
                await Navigation.PushAsync(new ScanOptionDisplayPage(option, sliderValue, pictures));
        });*/
    }
}
