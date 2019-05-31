using System.Collections.Generic;

namespace LinqLib.Sort.Sorters
{
  internal class QuickSort<TKey> : Sort<TKey>, ISort<TKey>
  {
    internal QuickSort(MapItem<TKey>[] map, IComparer<TKey> comparer, bool descending)
      : base(map, comparer, descending) { }

    public MapItem<TKey>[] Sort()
    {
      Sort(0, Map.Length - 1);
      return Map;
    }

    private void Sort(int left, int right)
    {
      TKey pivot = Map[(left + right) >> 1].Key;
      int l = left - 1;
      int r = right + 1;

      while (true)
      {
        do
        {
          r--;
        } while (CompareKeys(pivot, Map[r].Key) < 0);

        do
        {
          l++;
        } while (CompareKeys(pivot, Map[l].Key) > 0);

        if (l < r)
          Swap(l, r);
        else
          break;
      }
      int middle = r;               // Set middle index  

      if (left + 1 < right)
      {
        Sort(left, middle);         // Sort first section
        Sort(middle + 1, right);    // Sort second section
      }
    }
  }
}
