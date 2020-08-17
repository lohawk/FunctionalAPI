using FunctionalAPI.Core;
using FunctionalAPI.Domain;
using SuccincT.Functional;
using System;
using ItemResult = SuccincT.Options.ValueOrError<FunctionalAPI.Domain.Item, FunctionalAPI.Core.Error>;

namespace FunctionalAPI.Data
{
    public class ValidatingRepository : IManageItemState
    {
        private IManageItemState _backingRepository { get; }
        private static readonly object _lock = new object();

        public ValidatingRepository(IManageItemState backingRepository)
        {
            _backingRepository = backingRepository;
        }

        public ItemResult CreateItem(Item item) => _backingRepository.CreateItem(item);
        public ItemResult GetItemById(int id) => _backingRepository.GetItemById(id);

        public ItemResult UpdateItem(Item item)
        {
            lock (_lock)
            {   
                // Setup the validation pipeline
                return 
                    // Get the item
                    _backingRepository.GetItemById(item.Id)
                    // validate the version
                    .IfSuccess(oldItem => ValidateOnVersion(oldItem, item))
                    // example next validator
                    .IfSuccess(ValidateOnAlwaysTrue)
                    // update the item in the backing repository
                    .IfSuccess(_backingRepository.UpdateItem);
            }
        }

        public static ItemResult ValidateOnVersion(Item oldItem, Item newItem) =>
            (oldItem.Version == newItem.Version)
            ? ItemResult.WithValue(newItem)
            : ItemResult.WithError(new RepositoryOptimisticConcurrencyError(oldItem.Version, newItem.Version));

        public static ItemResult ValidateOnAlwaysTrue(Item item) => ItemResult.WithValue(item);
    }

}
