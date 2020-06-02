using System;
using System.Collections.Generic;
using System.Text;

namespace DLuOvBamG.Services
{
    public interface IImageService
    {
        DateTime GetDateTaken(string filePath);

        byte[] ResizeImage(string filePath, float width, float height, int quality);
    }
}
