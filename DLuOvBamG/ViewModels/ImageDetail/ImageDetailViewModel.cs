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
        private ImageDetailPage Page;

        public ImageDetailViewModel(Picture item, ImageDetailPage page)
        {
            Image = item;
            Page = page;
        }

        private void OnPictureDeleted(int deletedId)
        {
            PictureDeletedEvent deletedEvent = new PictureDeletedEvent(deletedId);
            Messenger.Default.Send(deletedEvent);
        }

        public ICommand GetCategories => new Command(async () => {
            await Page.Navigation.PushAsync(new ImageTagPage(Image.Id), true);
        });

        public ICommand GetInfo => new Command(async () => {
            await Page.Navigation.PushAsync(new InfoPage(Image), true);
        });

        public ICommand DeleteImage => new Command(async () => {
            int deletedId = await imageFileStorage.DeleteFileAsync(Image);
            if(deletedId == -1)
            {
                OnPictureDeleted(Image.Id);
                await Page.Navigation.PopAsync();
            } else
            {
                await Page.DisplayAlert("Not possible", "Image cannot be deleted. It might be read only.", "Okay");
            }
            
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