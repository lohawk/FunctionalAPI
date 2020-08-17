using FunctionalAPI.Core;
using FunctionalAPI.Domain;
using SuccincT.Functional;
using System.Collections.Concurrent;
using Xunit.Sdk;
using ItemResult = SuccincT.Options.ValueOrError<FunctionalAPI.Domain.Item, FunctionalAPI.Core.Error>;

namespace FunctionalAPI.Data
{
    public class CachedRepository : IManageItemState
    {
        private IManageItemState _backingRepository { get; }
        protected ConcurrentDictionary<int, Item> _cache = new ConcurrentDictionary<int, Item>();

        public CachedRepository(IManageItemState backingRepository)
        {
            _backingRepository = backingRepository;
        }

        public ItemResult GetItemById(int id) =>
            _cache.ContainsKey(id)
                ? ItemResult.WithValue(_cache[id])
                : _backingRepository.GetItemById(id)
                .SideEffectIfSuccess(item => _cache[id] = item);

        public ItemResult UpdateItem(Item item) =>
            _backingRepository.UpdateItem(item)
                .SideEffectIfSuccess(item => _cache[item.Id] = item);

        public ItemResult CreateItem(Item item) => 
            _backingRepository.CreateItem(item)
                .SideEffectIfSuccess(item => _cache[item.Id] = item);
    }
}
