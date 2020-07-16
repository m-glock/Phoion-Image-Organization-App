using DLuOvBamG.Models;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DLuOvBamG.Services
{
    public class ImageOrganizationDatabase
    {
        // static field to ensure that a single database connection is used for the lifetime of the APP
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteConnection SynchronousDB = new SQLiteConnection(Constants.DatabasePath);

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public ImageOrganizationDatabase()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        // check if tables already exists, else create them
        async Task InitializeAsync()
        {
            if (!initialized)
            {
                // create tables if needed
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(Picture).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(Picture)).ConfigureAwait(false);
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(CategoryTag)).ConfigureAwait(false);
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(PictureTags)).ConfigureAwait(false);
                    initialized = true;
                }
            }
        }

        public Task<List<Picture>> GetPicturesAsync()
        {
            return Database.Table<Picture>().ToListAsync();
        }

        public Task<List<Picture>> GetPicturesByDirectoryAsync(string directory)
        {
            return Database.Table<Picture>().Where(picture => picture.DirectoryName == directory).ToListAsync();
        }

        public Task<Picture> GetPictureAsync(int id)
        {
            return Database.GetWithChildrenAsync<Picture>(id);
        }

        public Task SavePictureAsync(Picture picture)
        {
            if (picture.Id != 0)
            {
                return Database.UpdateWithChildrenAsync(picture);
            }
            else
            {
                return Database.InsertAsync(picture);
            }
        }

        public Task<int> DeletePictureAsync(Picture picture)
        {
            return Database.DeleteAsync(picture);
        }

        public Task<List<CategoryTag>> GetCategoryTagsAsync()
        {
            return Database.Table<CategoryTag>().ToListAsync();
        }


        public Task<CategoryTag> GetCategoryTagAsync(int id)
        {
            return Database.Table<CategoryTag>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<List<CategoryTag>> GetCustomCategoryTagsAsync()
        {
            return Database.Table<CategoryTag>().Where(i => i.IsCustom == true).ToListAsync();
        }

        public CategoryTag GetCategoryTagByName(string name)
        {
            return SynchronousDB.Table<CategoryTag>().Where(i => i.Name.Equals(name)).FirstOrDefault();
        }

        public int SaveCategoryTag(CategoryTag categoryTag)
        {
            if (categoryTag.Id != 0)
            {
                return SynchronousDB.Update(categoryTag);
            }
            else
            {
                return SynchronousDB.Insert(categoryTag);
            }
        }

        public Task<int> SaveCategoryTagAsync(CategoryTag categoryTag)
        {
            if (categoryTag.Id != 0)
            {
                return Database.UpdateAsync(categoryTag);
            }
            else
            {
                return Database.InsertAsync(categoryTag);
            }
        }

        public Task<int> DeleteCategoryTagAsync(CategoryTag categoryTag)
        {
            return Database.DeleteAsync(categoryTag);
        }
    }
}
