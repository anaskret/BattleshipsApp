using Battleships.MobileApp.Models.Db;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.MobileApp.Services.SqliteService
{
    public static class SqliteService
    {
        static SQLiteAsyncConnection db;
        static async Task Init()
        {
            if(db != null)
                return;

            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Photos.Db");

            db = new SQLiteAsyncConnection(dbPath);

            await db.CreateTableAsync<Photo>();
        }

        public static async Task AddPhoto(string imageName)
        {
            await Init();
            var photo = new Photo()
            {
                Image = imageName
            };

            await db.InsertAsync(photo);
        }
        
        public static async Task RemovePhoto(int id)
        {
            await Init();

            await db.DeleteAsync<Photo>(id);
        }

        public static async Task<IEnumerable<Photo>> GetPhotos()
        {
            await Init();

            var photos = await db.Table<Photo>().ToListAsync();
            return photos;
        }
    }
}
