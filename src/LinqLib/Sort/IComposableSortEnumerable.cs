using System;
using System.Collections.Generic;

namespace LinqLib.Sort
{
  /// <summary>
  /// Documentation
  /// </summary>
  /// <typeparam name="TElement"></typeparam>
  public interface IComposableSortEnumerable<out TElement> : IEnumerable<TElement>
  {
    /// <summary>
    /// Documentation
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="keySelector"></param>
    /// <param name="sortType"></param>
    /// <param name="comparer"></param>
    /// <param name="descending"></param>
    void AppendSorter<TKey>(Func<TElement, TKey> keySelector, SortType sortType, IComparer<TKey> comparer, bool descending);
  }
}
