using SQLite;
using SQLiteNetExtensions.Attributes;

namespace DLuOvBamG.Models
{
    [Table("PictureTags")]

    public class PictureTags
    {
        [ForeignKey(typeof(Picture))]
        public int PictureId { get; set; }

        [ForeignKey(typeof(CategoryTag))]
        public int CategoryTag { get; set; }
    }
}
