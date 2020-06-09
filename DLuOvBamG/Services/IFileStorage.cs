using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DLuOvBamG.Services
{
    public interface IImagagFileStorage
    {
        Task<string> ReadFileAsync(string path);
        Task DeleteFileAsync(string path);

        Task<string[]> GetFilesFromDirectory(string path);
    }
}
