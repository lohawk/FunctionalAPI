using FunctionalAPI.Commands;
using FunctionalAPI.Core;
using ItemResult = SuccincT.Options.ValueOrError<FunctionalAPI.Domain.Item, FunctionalAPI.Core.Error>;

namespace FunctionalAPI.Business
{
    public static class ItemCommandExecutors
    {

        public static ItemResult Execute(UpdateItemCommand command) =>
            // Get the item we want to update
            command.GetFunc()
            // Make sure the updateItem is valid
            .IfSuccess(item => ItemFunctions.ValidateItem(item, command.ModifiedAt))
            // Update the item using business rules
            .IfSuccess(item => ItemFunctions.UpdateItem(item.Data, command.ModifiedAt, item))
            // Save the state
            .IfSuccess(item => command.SaveFunc(item));
    }
}
