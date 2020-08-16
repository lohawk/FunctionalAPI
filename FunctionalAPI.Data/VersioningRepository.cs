using Muhsin3Categories.Domain;
using ItemResult = SuccincT.Options.ValueOrError<Muhsin3Categories.Domain.Item, Muhsin3Categories.Core.Error>;

namespace Muhsin3Categories.Data
{
    public class VersioningRespository : IManageItemState
    {
        private IManageItemState _backingRepository { get; }
        private static readonly object _lock = new object();

        public VersioningRespository(IManageItemState backingRepository)
        {
            _backingRepository = backingRepository;
        }

        public ItemResult CreateItem(Item item) => _backingRepository.CreateItem(item);
        public ItemResult GetItemById(int id) => _backingRepository.GetItemById(id);
        public ItemResult UpdateItem(Item item) =>
            _backingRepository.UpdateItem(new Item(item)
            {
                Version = item.Version + 1
            });
    }
}
