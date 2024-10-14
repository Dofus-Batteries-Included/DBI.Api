namespace DBI.PathFinder.Extensions;

static class CollectionExtensions
{
    public static T MinBy<T>(this IEnumerable<T> enumerable, Func<T, int> minFunc) =>
        enumerable.Aggregate<T, (int CurrentMin, T? CurrentValue)>(
                (CurrentMin: int.MinValue, CurrentValue: default),
                (acc, value) =>
                {
                    int funcResult = minFunc(value);
                    return funcResult < acc.CurrentMin ? (funcResult, value) : acc;
                }
            )
            .CurrentValue
        ?? throw new ArgumentException("Collection is empty.");
}
