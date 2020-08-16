using System;
using ItemResult = SuccincT.Options.ValueOrError<Muhsin3Categories.Domain.Item, Muhsin3Categories.Core.Error>;

namespace Muhsin3Categories.Core
{
    public static class FunctionalExtensions
    {
        public static ItemResult ExecuteIfSuccess(this ItemResult i, Func<Domain.Item, ItemResult> func) => i.HasValue ? func.Invoke(i.Value) : i;
    }
}
