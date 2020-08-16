﻿using System;

namespace Muhsin3Categories.Domain
{
    public class Item
    {
        public int Id { get; set; }
        public string Data { get; set; }
        public DateTime ModifiedAt { get; set; }

        public int Version { get; set; }

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