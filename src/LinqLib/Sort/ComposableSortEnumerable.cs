using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;

namespace LinqLib.Sort
{
  /// <summary>
  /// Documentation
  /// </summary>
  /// <typeparam name="TElement"></typeparam>
  public abstract class ComposableSortEnumerable<TElement> : IComposableSortEnumerable<TElement>
  {
    internal IEnumerable<TElement> Source;
    internal ComposableSorter<TElement> ComposableSorter;

    /// <summary>
    /// Documentation
    /// </summary>
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    protected ComposableSortEnumerable()
    {    }

    public void AppendSorter<TKey>(Func<TElement, TKey> keySelector, SortType sortType, IComparer<TKey> comparer, bool descending)
    {
      ComposableSortEnumerable<TElement, TKey> composableSortEnumerable = new ComposableSortEnumerable<TElement, TKey>(this, keySelector, sortType, comparer, descending);
      SetNext(ComposableSorter, composableSortEnumerable.ComposableSorter);
    }

    private static void SetNext(ComposableSorter<TElement> firstSorter, ComposableSorter<TElement> secondSorter)
    {
      if (firstSorter.Next == null)
        firstSorter.Next = secondSorter;
      else
        SetNext(firstSorter.Next, secondSorter);
    }

    /// <summary>
    /// Documentation
    /// </summary>
    /// <returns></returns>
    public abstract IEnumerator<TElement> GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }

  internal class ComposableSortEnumerable<TElement, TKey> : ComposableSortEnumerable<TElement>
  {
    internal Func<TElement, TKey> KeySelector;
    internal IComparer<TKey> Comparer;
    internal bool Descending;
    internal SortType SortType;

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal ComposableSortEnumerable(IEnumerable<TElement> source, Func<TElement, TKey> keySelector, SortType sortType, IComparer<TKey> comparer, bool descending)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (keySelector == null)
        throw Error.ArgumentNull("keySelector");

      if (!Enum.IsDefined(typeof(SortType), sortType))
        sortType = SortType.Quick;

      Source = source;
      KeySelector = keySelector;
      SortType = sortType;

      IComparer<TKey> defaultComparer = comparer ?? Comparer<TKey>.Default;

      Comparer = defaultComparer;
      Descending = descending;

      ComposableSorter = new ComposableSorter<TElement, TKey>(KeySelector, SortType, Comparer, Descending);
    }

    public override IEnumerator<TElement> GetEnumerator()
    {
      return ComposableSorter.Sort(Source);
    }
  }
}
