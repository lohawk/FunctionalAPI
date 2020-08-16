using FunctionalAPI.Domain;
using ItemResult = SuccincT.Options.ValueOrError<FunctionalAPI.Domain.Item, FunctionalAPI.Core.Error>;

namespace FunctionalAPI.Data
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
