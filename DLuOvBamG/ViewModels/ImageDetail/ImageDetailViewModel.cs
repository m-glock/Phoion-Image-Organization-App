using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using Android.App;
using DLuOvBamG.Models;
using DLuOvBamG.Services;
using DLuOvBamG.Views;
using GalaSoft.MvvmLight.Messaging;
using Xamarin.Forms;



namespace DLuOvBamG.ViewModels
{
    public class ImageDetailViewModel : BaseViewModel, INotifyPropertyChanged
    {
        readonly ImageFileStorage imageFileStorage = new ImageFileStorage();
        public Picture Image { get; set; }
        public INavigation Navigation;

        public ImageDetailViewModel(Picture item)
        {
            Image = item;
        }

        private void OnPictureDeleted(int deletedId)
        {
            PictureDeletedEvent deletedEvent = new PictureDeletedEvent(deletedId);
            Messenger.Default.Send(deletedEvent);
        }

        public ICommand GetCategories => new Command(async () => {
            await Navigation.PushAsync(new ImageTagPage(Image.Id), true);
        });

        public ICommand GetInfo => new Command(async () => {
            await Navigation.PushAsync(new InfoPage(Image), true);
        });

        public ICommand DeleteImage => new Command(async () => {
            int deletedId = await imageFileStorage.DeleteFileAsync(Image);
            OnPictureDeleted(Image.Id);
            await Navigation.PopAsync();
        });

        public ICommand GetSimilar => new Command(async () =>
        {
            // TODO finish when Tensorflow functionality in merged
            // how to search similar images only for one specific image instead of comparin all images against each other?

            /*ScanOptionsEnum option = ScanOptionsEnum.similarPics;
            List<ScanOptionsEnum> options = new List<ScanOptionsEnum> { option };
            App.tf.FillPictureLists(options);
            double optionValue = option.GetDefaultPresicionValue();
            await Navigation.PushAsync(new ScanOptionDisplayPage(optionValue, option), true);*/
        });
    }
}