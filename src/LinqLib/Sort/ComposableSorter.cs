using LinqLib.Sort.Sorters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqLib.Sort
{
  internal abstract class ComposableSorter<TElement>
  {
    internal ComposableSorter<TElement> Next;

    internal abstract IEnumerator<TElement> Sort(IEnumerable<TElement> source);
  }

  internal class ComposableSorter<TElement, TKey> : ComposableSorter<TElement>
  {
    private readonly bool descending;
    private readonly SortType sortType;

    private readonly Func<TElement, TKey> keySelector;
    private readonly IComparer<TKey> comparer;

    private MapItem<TKey>[] map;

    internal ComposableSorter(Func<TElement, TKey> keySelector, SortType sortType, IComparer<TKey> comparer, bool descending)
    {
      this.keySelector = keySelector;
      this.comparer = comparer;
      this.descending = descending;
      this.sortType = sortType;
    }

    internal int CompareKeys(int index1, int index2)
    {
      int res = comparer.Compare(map[index1].Key, map[index2].Key);
      return descending ? -res : res;
    }

    internal int CompareKeys(TKey key1, TKey key2)
    {
      int res = comparer.Compare(key1, key2);
      return descending ? -res : res;
    }

    private void DoSort()
    {
      if (map.Length > 1)
      {
        ISort<TKey> sorter;
        switch (sortType)
        {
          case SortType.Quick:
            sorter = new QuickSort<TKey>(map, comparer, descending);
            break;
          case SortType.Merge:
            sorter = new MergeSort<TKey>(map, comparer, descending);
            break;
          case SortType.Insert:
            sorter = new InsertSort<TKey>(map, comparer, descending);
            break;
          case SortType.Heap:
            sorter = new HeapSort<TKey>(map, comparer, descending);
            break;
          case SortType.Bubble:
            sorter = new BubbleSort<TKey>(map, comparer, descending);
            break;
          case SortType.Shell:
            sorter = new ShellSort<TKey>(map, comparer, descending);
            break;
          case SortType.Select:
            sorter = new SelectSort<TKey>(map, comparer, descending);
            break;
          default:
#if DEBUG
            throw new NotImplementedException();
#else
            sorter = new QuickSort<TKey>(map, comparer, descending);
            break;
#endif
        }
        map = sorter.Sort();
      }
    }

    internal override IEnumerator<TElement> Sort(IEnumerable<TElement> source)
    {
      TElement[] sourceArr = source.ToArray();
      int itemsCount = sourceArr.Length;

      if (itemsCount == 0)
        yield break;

      if (itemsCount == 1)
        yield return sourceArr[0];
      else
      {
        map = new MapItem<TKey>[itemsCount];
        for (int i = 0; i < itemsCount; i++)
          map[i] = new MapItem<TKey>(i, keySelector(sourceArr[i]));

        DoSort();
        if (Next == null)
        {
          for (int i = 0; i < itemsCount; i++)
            yield return sourceArr[map[i].Index];
        }
        else
        {
          int last = itemsCount - 1;
          int current = 0;
          int start = -1;

          while (current < last)
          {
            int nextindex = current + 1;
            int cmpRes = CompareKeys(current, nextindex);
            if (cmpRes == 0)  // Equal
            {
              if (start == -1)
                start = current;
            }
            else              // Diff
            {
              if (start != -1)
              {
                int elements = nextindex - start;
                TElement[] subSource = new TElement[elements];
                for (int i = 0; i < elements; i++)
                  subSource[i] = sourceArr[map[i + start].Index];

                var sa = Next.Sort(subSource);
                while (sa.MoveNext())
                  yield return sa.Current;
                start = -1;
              }
              else
                yield return sourceArr[map[current].Index];
            }
            current++;
          }
          if (start != -1)
          {
            int elements = (current + 1) - start;
            TElement[] subSource = new TElement[elements];
            for (int i = 0; i < elements; i++)
              subSource[i] = sourceArr[map[i + start].Index];

            var sa = Next.Sort(subSource);
            while (sa.MoveNext())
              yield return sa.Current;

          }
          else
            if (current == last)
              yield return sourceArr[map[last].Index];

        }
      }
    }
  }
}

