using DLuOvBamG.Services;
using Android.App;
using Xamarin.Forms;
using DLuOvBamG.Droid;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Linq;

[assembly: Dependency(typeof(AssetsService))]
namespace DLuOvBamG.Droid
{
    class AssetsService : IAssetsService
    {
        List<string> IAssetsService.LoadClassificationLabels()
        {
            string path = "mobilenet_v1_1.0_224.txt";
            StreamReader sr = new StreamReader(Android.App.Application.Context.Assets.Open(path));
            List<string> labels = sr.ReadToEnd().Split('\n').Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToList();
            return labels;
        }
    }
}