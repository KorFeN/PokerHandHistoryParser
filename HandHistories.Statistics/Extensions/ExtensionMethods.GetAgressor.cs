using HandHistories.Objects.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Statistics.Extensions
{
    public static class ExtensionMethods
    {
        public static HandAction Aggressor(this IEnumerable<HandAction> Actions)
        {
            return Actions.LastOrDefault(p => p.IsAggressiveAction);
        }
    }
}
