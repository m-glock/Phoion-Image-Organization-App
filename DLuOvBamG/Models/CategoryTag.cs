using DLuOvBamG.Services;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DLuOvBamG.Models
{
    [Table("CategoryTags")]
    public class CategoryTag
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }

        [ManyToMany(typeof(PictureTags))]
        public List<Picture> Pictures { get; set; }

        public int FindOrInsert()
        {
            ImageOrganizationDatabase db = App.Database;
            CategoryTag dbObject = db.GetCategoryTagByName(Name);

            if (dbObject is null)
            {
                int id = db.SaveCategoryTag(this);
                Id = id;
            } else
            {
                Id = dbObject.Id;
            }
            return Id;
        }
    }
}
