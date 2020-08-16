using System;

namespace FunctionalAPI.Domain
{
    public class Item : VersionedItem
    {
        public int Id { get; set; }
        public string Data { get; set; }
        public DateTime ModifiedAt { get; set; }

        public Item() { }

        public Item(Item i)
        {
            Id = i.Id;
            Data = i.Data;
            ModifiedAt = i.ModifiedAt;
            Version = i.Version;
        }
    }
}
