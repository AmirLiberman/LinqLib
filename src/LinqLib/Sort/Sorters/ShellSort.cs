using System.Collections.Generic;

namespace LinqLib.Sort.Sorters
{
  internal class ShellSort<TKey> : Sort<TKey>, ISort<TKey>
  {
    internal ShellSort(MapItem<TKey>[] map, IComparer<TKey> comparer, bool descending)
      : base(map, comparer, descending) { }

    public MapItem<TKey>[] Sort()
    {
      int len = Map.Length;
      int increment = 3;

      while (increment > 0)
      {
        for (int i = 0; i < len; i++)
        {
          int j = i;
          MapItem<TKey> temp = Map[i];

          while ((j >= increment) && (CompareKeys(Map[j - increment].Key, temp.Key) > 0))
          {
            Map[j] = Map[j - increment];
            j = j - increment;
          }

          Map[j] = temp;
        }

        int newIncrement = increment >> 1;

        if (newIncrement != 0)
          increment = newIncrement;
        else if (increment == 1)
          increment = 0;
        else
          increment = 1;
      }

      return Map;
    }
  }
}
