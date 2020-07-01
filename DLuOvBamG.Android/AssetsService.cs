using Xamarin.Forms;
using DLuOvBamG.Droid;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DLuOvBamG.Services;

[assembly: Dependency(typeof(AssetsService))]
namespace DLuOvBamG.Droid
{
    class AssetsService : IAssetsService
    {
        List<string> IAssetsService.LoadClassificationLabels()
        {
            string path = "labelsSqueezenet.txt";
            StreamReader sr = new StreamReader(Android.App.Application.Context.Assets.Open(path));
            List<string> labels = sr.ReadToEnd().Split('\n').Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToList();
            return labels;
        }
    }
}