using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DLuOvBamG.Services
{
    public interface IAssetsService
    {
        List<string> LoadClassificationLabels();
    }
}
