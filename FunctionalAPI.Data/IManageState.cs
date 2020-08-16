using Muhsin3Categories.Domain;
using SuccincT.Options;
using ItemResult = SuccincT.Options.ValueOrError<Muhsin3Categories.Domain.Item, Muhsin3Categories.Core.Error>;

namespace Muhsin3Categories.Data
{
    public interface IManageItemState
    {
        ItemResult GetItemById(int id);
        ItemResult UpdateItem(Item item);
        ItemResult CreateItem(Item item);
    }
}
