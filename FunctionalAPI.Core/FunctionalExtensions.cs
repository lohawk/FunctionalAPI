using System;
using ItemResult = SuccincT.Options.ValueOrError<FunctionalAPI.Domain.Item, FunctionalAPI.Core.Error>;

namespace FunctionalAPI.Core
{
    public static class FunctionalExtensions
    {
        public static ItemResult ExecuteIfSuccess(this ItemResult i, Func<Domain.Item, ItemResult> func) => i.HasValue ? func.Invoke(i.Value) : i;
    }
}
