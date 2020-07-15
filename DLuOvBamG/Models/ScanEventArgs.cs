using System;
using System.Collections.Generic;
using System.Text;

namespace DLuOvBamG.Models
{
    public class ScanEventArgs
    {
        public ScanOptionsEnum Option;
        public ScanEventArgs(ScanOptionsEnum option)
        {
            Option = option;
        }
    }
}
