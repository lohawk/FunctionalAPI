using System;
using ItemResult = SuccincT.Options.ValueOrError<FunctionalAPI.Domain.Item, FunctionalAPI.Core.Error>;

namespace FunctionalAPI.Core
{
    public static class FunctionalExtensions
    {
        public static ItemResult ExecuteIfSuccess(this ItemResult i, Func<Domain.Item, ItemResult> func) => i.HasValue ? func.Invoke(i.Value) : i;
        public static ItemResult SideEffectIfSuccess(this ItemResult i, Action<Domain.Item> func)
        {
            if (i.HasValue) func.Invoke(i.Value);
            return i;
        }
    }
}
