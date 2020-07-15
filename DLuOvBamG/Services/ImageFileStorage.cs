using Android.Graphics;
using DLToolkit.Forms.Controls;
using DLuOvBamG.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DLuOvBamG.Services
{
    public class ImageFileStorage
    {
        // returns id of deleted files if this fails returns -1
        public async Task<int> DeleteFileAsync(Models.Picture picture)
        {
            ImageOrganizationDatabase db = App.Database;
            IImageService imageService = DependencyService.Get<IImageService>();

            var status = await CheckAndRequestExternalStorageWritePermissionAsync();
            if (status != PermissionStatus.Granted)
            {
                return -1;
            }
            imageService.DeleteImage(picture.Uri);
            return await db.DeletePictureAsync(picture);
        }

        public async Task<string[]> GetFilesFromDirectory(string folderPath)
        {
            var status = await CheckAndRequestExternalStorageReadPermissionAsync();
            if (status != PermissionStatus.Granted)
            {
                // Notify user permission was denied
                string[] empty = new string[] { };
                return empty;
            }

            string[] filePaths = Directory.GetFiles(folderPath, "*.jpg");
            return filePaths;
        }

        public async Task<Models.Picture[]> GetPicturesFromDevice(FlowObservableCollection<Grouping<string, Models.Picture>> collection, DateTime? dateFilter)
        {
            var status = await CheckAndRequestExternalStorageReadPermissionAsync();
            if (status != PermissionStatus.Granted)
            {
                // Notify user permission was denied
                Models.Picture[] empty = new Models.Picture[] { };
                return empty;
            }

            IImageService imageService = DependencyService.Get<IImageService>();
            Models.Picture[] pictures = imageService.GetAllImagesFromDevice(collection, dateFilter);
            return pictures;
        }

        public Task<string> ReadFileAsync(string filePath)
        {
            throw new NotImplementedException();
        }

        private async Task<PermissionStatus> CheckAndRequestExternalStorageReadPermissionAsync()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.StorageRead>();
            }

            return status;
        }

        private async Task<PermissionStatus> CheckAndRequestExternalStorageWritePermissionAsync()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.StorageWrite>();
            }

            return status;
        }
    }
}
