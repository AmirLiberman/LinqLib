using System.Collections.Generic;

namespace LinqLib.Sort.Sorters
{
  internal class BubbleSort<TKey> : Sort<TKey>, ISort<TKey>
  {
    internal BubbleSort(MapItem<TKey>[] map, IComparer<TKey> comparer, bool descending)
      : base(map, comparer, descending) { }

    public MapItem<TKey>[] Sort()
    {
      int itemsCount = Map.Length;

      for (int i = 0; i <= itemsCount; i++)
        // Loop a second time from the back of the array
        for (int j = itemsCount - 1; j > i; j--)
          // Swap the elements if necessary
          if (CompareKeys(j - 1, j) > 0)
            Swap(j - 1, j);

      return Map;
    }
  }
}
