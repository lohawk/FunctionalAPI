using FunctionalAPI.Domain;
using SuccincT.Options;
using ItemResult = SuccincT.Options.ValueOrError<FunctionalAPI.Domain.Item, FunctionalAPI.Core.Error>;

namespace FunctionalAPI.Data
{
    public interface IManageItemState
    {
        ItemResult GetItemById(int id);
        ItemResult UpdateItem(Item item);
        ItemResult CreateItem(Item item);
    }
}
