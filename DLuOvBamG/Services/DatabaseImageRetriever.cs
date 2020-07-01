using DLuOvBamG.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DLuOvBamG.Services
{
    class DatabaseImageRetriever
    {
        // TODO get images from database
        public static async Task<List<Picture>> GetImagesFromDatabase()
        {
            IPathService pathService = DependencyService.Get<IPathService>();
            string dcimFolder = pathService.DcimFolder;
            dcimFolder += "/Camera";
            ImageFileStorage imageFileStorage = new ImageFileStorage();
            string[] imagePaths = await imageFileStorage.GetFilesFromDirectory(dcimFolder);

            var pictureList = new List<Picture>();
            for (int i = 0; i < imagePaths.Length; i++)
            {
                Picture picture = new Picture(imagePaths[i]);
                picture.ImageSource = ImageSource.FromFile(imagePaths[i]);
                pictureList.Add(picture);
            }

            return pictureList;
        }
    }
}
