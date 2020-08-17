using FunctionalAPI.Core;
using FunctionalAPI.Domain;
using SuccincT.Functional;
using System;
using ItemResult = SuccincT.Options.ValueOrError<FunctionalAPI.Domain.Item, FunctionalAPI.Core.Error>;

namespace FunctionalAPI.Business
{
    public static class ItemFunctions 
    {
        public static ItemResult UpdateItem(string data, DateTime modifiedAt, Item item) =>
            // Let's run the validation rules for this item
            ValidateItem(item, modifiedAt)
            // Then return the value if validation is successful
            .IfSuccess(item => ItemResult.WithValue(new Item(item)
            {
                Data = data,
                ModifiedAt = modifiedAt
            }));

        // Example of chaining validation rules together
        public static ItemResult ValidateItem(Item item, DateTime modifiedAt) =>
            // Check to make sure the modification date is valid
            ValidateModificationDate(item, modifiedAt)
            // Check to make sure the itemId is valid
            .IfSuccess(ValidateItemId);

        public static ItemResult ValidateItemId(Item item) =>
            item.Id > 0 ? ItemResult.WithValue(item) : ItemResult.WithError(new BusinessInvalidItemError());

        public static ItemResult ValidateModificationDate(Item item, DateTime modifiedAt) =>
            modifiedAt >= item.ModifiedAt ? ItemResult.WithValue(item) : ItemResult.WithError(new BusinessInvalidModificationDateError());
    }
}
