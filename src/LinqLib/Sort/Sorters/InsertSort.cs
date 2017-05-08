using System.Collections.Generic;

namespace LinqLib.Sort.Sorters
{
  internal class InsertSort<TKey> : Sort<TKey>, ISort<TKey>
  {
    internal InsertSort(MapItem<TKey>[] map, IComparer<TKey> comparer, bool descending)
      : base(map, comparer, descending) { }

    public MapItem<TKey>[] Sort()
    {
      int lastindex = Map.Length - 1;
        // Loop through all elements in the original array from the second element
      for (int j = 1; j <= lastindex; j++)
      {
        MapItem<TKey> item = Map[j];
        int i = j - 1;
        // Loop through all elements from the key to the start
        // Check if the current element is smaller than the key
        while (i >= 0 && CompareKeys(Map[i].Key, item.Key) > 0)
        {
          // Move the current element backward
          Map[i + 1] = Map[i];
          i--;
        }
        // Finally move the key
        Map[i + 1] = item;
      }

      return Map;
    }
  }
}
