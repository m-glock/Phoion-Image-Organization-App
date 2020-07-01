using DLuOvBamG.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DLuOvBamG.ViewModels
{
    class InfoViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public String infoName { get; } = "test";
        public String infoLocation { get; }
        public String[] _infoTags { get; }
        public String infoTags { get; }
        public String infoDate { get; }

        public InfoViewModel(Picture image)
        {
            //infoName = "test";
            //infoLocation = "Berlin";
            //_infoTags = new string[] { "Hund", "Berlin", "Urlaub" };
            //for (int i = 0; i < _infoTags.Length; i++)
            //{
            //    infoTags = infoTags + _infoTags[i] + ", ";
            //}
            //infoDate = "01.02.2010";


        }
    }
}
