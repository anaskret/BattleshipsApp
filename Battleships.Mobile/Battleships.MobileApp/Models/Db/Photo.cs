using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Battleships.MobileApp.Models.Db
{
    public class Photo
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Image { get; set; }
    }
}
