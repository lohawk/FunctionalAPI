using Muhsin3Categories.Core;

namespace Muhsin3Categories.Business
{
    public abstract class BusinessError : Error { }
    public class BusinessInvalidModificationDateError : BusinessError
    {
        public BusinessInvalidModificationDateError()
        {
            ErrorMessage = "Cannot modify an item earlier than it's most current modification date";
        }
    }
    public class BusinessInvalidItemError : BusinessError
    {
        public BusinessInvalidItemError()
        {
            ErrorMessage = "The item specified has invalid property values";
        }
    }
}
