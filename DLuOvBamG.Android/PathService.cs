using DLuOvBamG.Services;
using Android.App;
using Xamarin.Forms;
using DLuOvBamG.Droid;

[assembly: Dependency(typeof(PathService))]
namespace DLuOvBamG.Droid
{
    public class PathService : IPathService
    {
        public string InternalFolder
        {
            get
            {
                return Android.App.Application.Context.FilesDir.AbsolutePath;
            }
        }

        public string PublicExternalFolder
        {
            get
            {
                return Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            }
        }

        public string PrivateExternalFolder
        {
            get
            {
                return Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath;
            }
        }

        public string DcimFolder { 
            get
            {
                string directoryType = Android.OS.Environment.DirectoryDcim;
                return Android.OS.Environment.GetExternalStoragePublicDirectory(directoryType).AbsolutePath;
            }
        }

        public string PictureFolder
        {
            get
            {
                string directoryType = Android.OS.Environment.DirectoryPictures;
                return Android.OS.Environment.GetExternalStoragePublicDirectory(directoryType).AbsolutePath;
            }
        }
    }
}