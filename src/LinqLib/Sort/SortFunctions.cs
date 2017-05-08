using System;
using System.Collections.Generic;

namespace LinqLib.Sort
{
  /// <summary>
  /// Documentation
  /// </summary>
  public static class SortFunctions
  {
    /// <summary>
    /// Documentation
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="source"></param>
    /// <param name="keySelector"></param>
    /// <param name="sortType"></param>
    /// <returns></returns>
    public static IComposableSortEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, SortType sortType)
    {
      return new ComposableSortEnumerable<TSource, TKey>(source, keySelector, sortType, null, false);
    }

    /// <summary>
    /// Documentation
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="source"></param>
    /// <param name="keySelector"></param>
    /// <param name="sortType"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static IComposableSortEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, SortType sortType, IComparer<TKey> comparer)
    {
      return new ComposableSortEnumerable<TSource, TKey>(source, keySelector, sortType, comparer, false);
    }

    /// <summary>
    /// Documentation
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="source"></param>
    /// <param name="keySelector"></param>
    /// <param name="sortType"></param>
    /// <returns></returns>
    public static IComposableSortEnumerable<TSource> OrderByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, SortType sortType)
    {
      return new ComposableSortEnumerable<TSource, TKey>(source, keySelector, sortType, null, true);
    }

    /// <summary>
    /// Documentation
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="source"></param>
    /// <param name="keySelector"></param>
    /// <param name="sortType"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static IComposableSortEnumerable<TSource> OrderByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, SortType sortType, IComparer<TKey> comparer)
    {
      return new ComposableSortEnumerable<TSource, TKey>(source, keySelector, sortType, comparer, true);
    }

    /// <summary>
    /// Documentation
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="source"></param>
    /// <param name="keySelector"></param>
    /// <param name="sortType"></param>
    /// <returns></returns>
    public static IComposableSortEnumerable<TSource> ThenBy<TSource, TKey>(this IComposableSortEnumerable<TSource> source, Func<TSource, TKey> keySelector, SortType sortType)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      source.AppendSorter(keySelector, sortType, null, false);
      return source;
    }

    /// <summary>
    /// Documentation
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="source"></param>
    /// <param name="keySelector"></param>
    /// <param name="sortType"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static IComposableSortEnumerable<TSource> ThenBy<TSource, TKey>(this IComposableSortEnumerable<TSource> source, Func<TSource, TKey> keySelector, SortType sortType, IComparer<TKey> comparer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      source.AppendSorter(keySelector, sortType, comparer, false);
      return source;
    }

    /// <summary>
    /// Documentation
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="source"></param>
    /// <param name="keySelector"></param>
    /// <param name="sortType"></param>
    /// <returns></returns>
    public static IComposableSortEnumerable<TSource> ThenByDescending<TSource, TKey>(this IComposableSortEnumerable<TSource> source, Func<TSource, TKey> keySelector, SortType sortType)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      source.AppendSorter(keySelector, sortType, null, true);
      return source;
    }

    /// <summary>
    /// Documentation
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="source"></param>
    /// <param name="keySelector"></param>
    /// <param name="sortType"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static IComposableSortEnumerable<TSource> ThenByDescending<TSource, TKey>(this IComposableSortEnumerable<TSource> source, Func<TSource, TKey> keySelector, SortType sortType, IComparer<TKey> comparer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      source.AppendSorter(keySelector, sortType, comparer, true);
      return source;
    }
  }
}







