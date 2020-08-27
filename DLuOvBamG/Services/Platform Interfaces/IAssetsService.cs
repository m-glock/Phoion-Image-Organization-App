using System.Collections.Generic;

namespace DLuOvBamG.Services
{
    public interface IAssetsService
    {
        List<string> LoadClassificationLabels();
    }
}
