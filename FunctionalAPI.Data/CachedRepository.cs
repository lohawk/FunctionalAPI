using Muhsin3Categories.Domain;
using System.Collections.Concurrent;
using ItemResult = SuccincT.Options.ValueOrError<Muhsin3Categories.Domain.Item, Muhsin3Categories.Core.Error>;

namespace Muhsin3Categories.Data
{
    public class CachedRepository : IManageItemState
    {
        private IManageItemState _backingRepository { get; }
        protected ConcurrentDictionary<int, Item> _cache = new ConcurrentDictionary<int, Item>();

        public CachedRepository(IManageItemState backingRepository)
        {
            _backingRepository = backingRepository;
        }

        public ItemResult GetItemById(int id)
        {
            if (!_cache.ContainsKey(id))
            {
                var backingResult = _backingRepository.GetItemById(id);
                if (!backingResult.HasValue)
                    return backingResult;

                _cache[id] = backingResult.Value;
            }

            return ItemResult.WithValue(_cache[id]);
        }

        public ItemResult UpdateItem(Item item)
        {
            var backingResult = _backingRepository.UpdateItem(item);
            if (backingResult.HasValue)
                _cache[item.Id] = backingResult.Value;

            return backingResult;
        }

        public ItemResult CreateItem(Item item)
        {
            var backingResult = _backingRepository.CreateItem(item);
            if (backingResult.HasValue)
                _cache[item.Id] = backingResult.Value;

            return backingResult;
        }
    }
}
