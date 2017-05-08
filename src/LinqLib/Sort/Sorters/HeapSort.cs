using System.Collections.Generic;

namespace LinqLib.Sort.Sorters
{
  internal class HeapSort<TKey> : Sort<TKey>, ISort<TKey>
  {
    internal HeapSort(MapItem<TKey>[] map, IComparer<TKey> comparer, bool descending)
      : base(map, comparer, descending) { }

    public MapItem<TKey>[] Sort()
    {
      int lastindex = Map.Length - 1;
      // Build the initial heap
      for (int i = lastindex / 2; i >= 0; i--)
        HeapSortAdjust(i, lastindex);

      // Swap root node and the last heap node
      for (int i = lastindex; i >= 1; i--)
      {
        Swap(0, i);
        HeapSortAdjust(0, i - 1);
      }

      return Map;
    }

    private void HeapSortAdjust(int index, int length)
    {
      MapItem<TKey> temp = Map[index];
      int j = index * 2 + 1;

      while (j <= length)
      {
        if (j < length)
          if (CompareKeys(j, j + 1) < 0)
            j++;

        // Compare roots and the older children
        if (CompareKeys(temp.Key, Map[j].Key) < 0)
        {
          Map[index] = Map[j];
          index = j;
          j = 2 * index + 1;
        }
        else
        {
          j = length + 1;
        }
      }
      Map[index] = temp;
    }
  }
}
