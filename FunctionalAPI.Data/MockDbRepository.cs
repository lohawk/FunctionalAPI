using FunctionalAPI.Domain;
using System;
using System.Collections.Concurrent;
using System.Threading;
using ItemResult = SuccincT.Options.ValueOrError<FunctionalAPI.Domain.Item, FunctionalAPI.Core.Error>;

namespace FunctionalAPI.Data
{
    public class MockDbRepository : IManageItemState
    {
        private static readonly object _lock = new object();
        private static readonly ConcurrentDictionary<int, Item> _mockDb = new ConcurrentDictionary<int, Item>();

        public MockDbRepository()
        {
            _mockDb[42] = new Item()
            {
                Version = 2,
                Id = 42,
                Data = "Hello World",
                ModifiedAt = new DateTime(2020, 8, 13, 18, 0, 0)
            };
        }

        public ItemResult CreateItem(Item item)
        {
            // Because databases are slow
            Thread.Sleep(TimeSpan.FromSeconds(1));

            lock (_lock)
            {
                // Happy to have this here, as a real db would throw with a primary key exception
                if (_mockDb.ContainsKey(item.Id)) return ItemResult.WithError(new RepositoryOptimisticConcurrencyError(item.Id));

                var creationDatetime = DateTime.Now;
                _mockDb[item.Id] = new Item(item)
                {
                    ModifiedAt = creationDatetime,
                    Version = 1
                };
                return ItemResult.WithValue(_mockDb[item.Id]);
            }
        }

        public ItemResult GetItemById(int id)
        {
            // Because databases are slow
            Thread.Sleep(TimeSpan.FromSeconds(1));

            lock (_lock)
            {
                return _mockDb.ContainsKey(id)
                    ? ItemResult.WithValue(_mockDb[id])
                    : ItemResult.WithError(new RepositoryNotFoundError(id));
            }
        }

        public ItemResult UpdateItem(Item item)
        {
            // Because databases are slow
            Thread.Sleep(TimeSpan.FromSeconds(1));

            lock (_lock)
            {
                if (_mockDb.ContainsKey(item.Id))
                    ItemResult.WithError(new RepositoryNotFoundError(item.Id));

                _mockDb[item.Id] = item;
                return ItemResult.WithValue(item);
            }
        }
    }

}
