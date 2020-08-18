using FunctionalAPI.Domain;
using System;
using ItemResult = SuccincT.Options.ValueOrError<FunctionalAPI.Domain.Item, FunctionalAPI.Core.Error>;

namespace FunctionalAPI.Commands
{
    public class UpdateItemCommand
    {
        public string Data { get; }
        public DateTime ModifiedAt { get; }
        public Func<ItemResult> GetFunc { get; }
        public Func<Item, ItemResult> SaveFunc { get; }

        public UpdateItemCommand(Func<ItemResult> getFunc, Func<Item, ItemResult> saveFunc, string data, DateTime modifiedAt)  {
            GetFunc = getFunc;
            SaveFunc = saveFunc;
            Data = data;
            ModifiedAt = modifiedAt;
        }
    }
}
