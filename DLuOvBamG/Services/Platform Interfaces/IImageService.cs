using DLToolkit.Forms.Controls;
using DLuOvBamG.Models;
using System;


namespace DLuOvBamG.Services
{
    public interface IImageService
    {
        DateTime GetDateTaken(string filePath);
        byte[] GetFileBytes(string filePath);

        void DeleteImage(string filePath);
        Picture[] GetAllImagesFromDevice(FlowObservableCollection<Grouping<string, Models.Picture>> collection);
    }
}
