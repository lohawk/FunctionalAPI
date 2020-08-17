using SuccincT.Functional;
using System;
using ItemResult = SuccincT.Options.ValueOrError<FunctionalAPI.Domain.Item, FunctionalAPI.Core.Error>;

namespace FunctionalAPI.Core
{
    public static class FunctionalExtensions
    {
        public static ItemResult IfSuccess(this ItemResult i, Func<Domain.Item, ItemResult> func) => i.HasValue ? i.Value.Into(func) : i;
        public static ItemResult SideEffectIfSuccess(this ItemResult i, Action<Domain.Item> func)
        {
            if (i.HasValue) i.Value.Into(func);
            return i;
        }
    }
}
