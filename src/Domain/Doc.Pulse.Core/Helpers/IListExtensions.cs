namespace Doc.Pulse.Core.Helpers;

internal static class IListExtensions
{
    public static bool RemoveAll<T>(this IList<T> list, Func<T, bool> predicate)
    {
        var itemsToRemove = list.Where(predicate);
        var expected = itemsToRemove.Count();

        foreach (var item in itemsToRemove)
        {
            if (list.Remove(item))
                expected--;
        }

        return expected == 0;

        //int count = 0;

        //for (int i = list.Count - 1; i >= 0; i--)
        //{
        //    if (match(list[i]))
        //    {
        //        ++count;
        //        list.RemoveAt(i);
        //    }
        //}

        //return count;
    }
}
