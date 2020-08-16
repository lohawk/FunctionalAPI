using FunctionalAPI.Core;

namespace FunctionalAPI.Data
{
    public abstract class RepositoryError : Error { }
    public class RepositoryNotFoundError : RepositoryError
    {
        public RepositoryNotFoundError(int id)
        {
            ErrorMessage = $"Cannot find item with id: {id}";
        }
    }
    public class RepositoryOptimisticConcurrencyError : RepositoryError
    {
        public RepositoryOptimisticConcurrencyError(int actualVersion, int expectedVersion)
        {
            ErrorMessage = $"Database version ({expectedVersion}) does not match object version ({actualVersion})";
        }
        public RepositoryOptimisticConcurrencyError(int id)
        {
            ErrorMessage = $"Database already contains an item with Id ({id})";
        }
    }
}
