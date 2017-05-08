using System.Collections.Generic;

namespace LinqLib.Sort.Sorters
{
  internal class MergeSort<TKey> : Sort<TKey>, ISort<TKey>
  {
    internal MergeSort(MapItem<TKey>[] map, IComparer<TKey> comparer, bool descending)
      : base(map, comparer, descending) { }

    public MapItem<TKey>[] Sort()
    {
      MapItem<TKey>[] tempMap = new MapItem<TKey>[Map.Length];
      Sort(0, Map.Length - 1, tempMap);
      return Map;
    }

    public void Sort(int left, int right, MapItem<TKey>[] tempMap)
    {
      if (right > left)
      {
        int mid = (right + left) >> 1;
        Sort(left, mid, tempMap);
        Sort(mid + 1, right, tempMap);
        Merge(left, mid + 1, right, tempMap);
      }
    }

    public void Merge(int left, int mid, int right, MapItem<TKey>[] tempMap)
    {
      int leftEnd = mid - 1;
      int tmpPos = left;
      int numElements = right - left + 1;

      while ((left <= leftEnd) && (mid <= right))
      {
        if (CompareKeys(Map[left].Key, Map[mid].Key) <= 0)
        {
          tempMap[tmpPos] = Map[left];
          left++;
        }
        else
        {
          tempMap[tmpPos] = Map[mid];
          mid++;
        }
        tmpPos++;
      }

      while (left <= leftEnd)
      {
        tempMap[tmpPos] = Map[left];
        left++;
        tmpPos++;
      }

      while (mid <= right)
      {
        tempMap[tmpPos] = Map[mid];
        mid++;
        tmpPos++;
      }

      for (int i = 0; i < numElements; i++)
      {
        Map[right] = tempMap[right];
        right--;
      }
    }
  }
}
