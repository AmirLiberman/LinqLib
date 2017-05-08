using System.Collections.Generic;

namespace LinqLib.Sort.Sorters
{
  internal class SelectSort<TKey> : Sort<TKey>, ISort<TKey>
  {
    internal SelectSort(MapItem<TKey>[] map, IComparer<TKey> comparer, bool descending)
      : base(map, comparer, descending) { }

    public MapItem<TKey>[] Sort()
    {
      int lastItem = Map.Length - 1;

      for (int i = 0; i < lastItem; i++)
      {
        int min = i;

        for (int j = i + 1; j <= lastItem; j++)
          if (CompareKeys(j, min) < 0)
            min = j;

        Swap(i, min);
      }

      return Map;
    }
  }
}
