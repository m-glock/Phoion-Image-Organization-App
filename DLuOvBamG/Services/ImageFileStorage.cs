using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DLuOvBamG.Services
{
    public class ImageFileStorage
    {
        public Task DeleteFileAsync(string filePath)
        {
            throw new NotImplementedException();
        }

        public async Task<string[]> GetFilesFromDirectory(string folderPath)
        {
            var status = await CheckAndRequestExternalStoragePermissionAsync();
            if (status != PermissionStatus.Granted)
            {
                // Notify user permission was denied
                string[] empty = new string[] { };
                return empty;
            }

            string[] filePaths = Directory.GetFiles(folderPath, "*.jpg");
            return filePaths;
        }

        public async Task<string[]> GetImagePathsFromDevice()
        {
            var status = await CheckAndRequestExternalStoragePermissionAsync();
            if (status != PermissionStatus.Granted)
            {
                // Notify user permission was denied
                string[] empty = new string[] { };
                return empty;
            }

            IImageService imageService = DependencyService.Get<IImageService>();
            string[] filePaths = imageService.GetAllImagesFromDevice();
            return filePaths;
        }

        public Task<string> ReadFileAsync(string filePath)
        {
            throw new NotImplementedException();
        }

        private async Task<PermissionStatus> CheckAndRequestExternalStoragePermissionAsync()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.StorageRead>();
            }

            return status;
        }
    }
}
