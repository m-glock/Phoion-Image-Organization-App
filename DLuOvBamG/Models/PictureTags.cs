using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DLuOvBamG.Models
{
    public class PictureTags
    {
        [ForeignKey(typeof(Picture))]
        public int PictureId { get; set; }

        [ForeignKey(typeof(CategoryTag))]
        public int CategoryTag { get; set; }
    }
}
