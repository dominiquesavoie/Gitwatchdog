using System;
using SQLite;

namespace GitWatchdog.Model
{
    public class Item
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
    }
}
