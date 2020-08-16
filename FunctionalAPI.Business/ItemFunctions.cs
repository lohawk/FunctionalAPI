using Muhsin3Categories.Core;
using Muhsin3Categories.Domain;
using SuccincT.Functional;
using System;
using ItemResult = SuccincT.Options.ValueOrError<Muhsin3Categories.Domain.Item, Muhsin3Categories.Core.Error>;

namespace Muhsin3Categories.Business
{
    public static class ItemFunctions 
    {
        public static ItemResult UpdateItem(string data, DateTime modifiedAt, Item item) =>
            // Let's run the validation rules for this item
            ValidateItem(item, modifiedAt)
            // Then return the value if validation is successful
            .Into(r => r.ExecuteIfSuccess(item => ItemResult.WithValue(new Item(item)
            {
                Data = data,
                ModifiedAt = modifiedAt
            })));

        // Example of chaining validation rules together
        public static ItemResult ValidateItem(Item item, DateTime modifiedAt) =>
            // Check to make sure the modification date is valid
            ValidateModificationDate(item, modifiedAt)
            // Check to make sure the itemId is valid
            .Into(r => r.ExecuteIfSuccess(ValidateItemId));

        public static ItemResult ValidateItemId(Item item) =>
            item.Id > 0 ? ItemResult.WithValue(item) : ItemResult.WithError(new BusinessInvalidItemError());

        public static ItemResult ValidateModificationDate(Item item, DateTime modifiedAt) =>
            modifiedAt >= item.ModifiedAt ? ItemResult.WithValue(item) : ItemResult.WithError(new BusinessInvalidModificationDateError());
    }
}
