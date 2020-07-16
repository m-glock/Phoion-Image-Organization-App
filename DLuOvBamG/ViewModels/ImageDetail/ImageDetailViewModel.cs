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

        public ICommand GetCategories => new Command(async () =>
        {
            await Page.Navigation.PushAsync(new ImageTagPage(Image.Id), true);
        });

        public ICommand GetInfo => new Command(async () =>
        {
            await Page.Navigation.PushAsync(new InfoPage(Image), true);
        });

        public ICommand DeleteImage => new Command(async () =>
        {
            int deletedId = await imageFileStorage.DeleteFileAsync(Image);
            if (deletedId != -1)
            {
                OnPictureDeleted(Image.Id);
                await Page.Navigation.PopAsync();
            }
            else
            {
                await Page.DisplayAlert("Not possible", "Image cannot be deleted. It might be read only.", "Okay");
            }

        });

        public ICommand GetSimilar => new Command(async () =>
        {
            List<Picture> listOfPics = App.tf.GetNeighboursForPicture(Image.Id);
            if (listOfPics.Count < 2)
            {
                await Page.DisplayAlert("", "No similar pictures were found", "Okay");
            }
            else
                await Page.Navigation.PushAsync(new ImageComparisonPage(listOfPics, Image), true);
        });
    }
}