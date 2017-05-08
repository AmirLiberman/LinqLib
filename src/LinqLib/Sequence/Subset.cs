using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqLib.Sequence
{
  /// <summary>
  /// Provides methods that return a subset of provided sequences.
  /// </summary>
  public static class Subset
  {
    #region Take Pattern

    /// <summary>
    /// Takes all elements that are in odd index position in the sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">The sequence to return elements from.</param>
    /// <returns>A  sequence of all elements in odd index position.</returns>
    /// <exception cref="System.ArgumentNullException">source is null</exception>
    public static IEnumerable<TSource> TakeOdd<TSource>(this IEnumerable<TSource> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      using (IEnumerator<TSource> iter = source.GetEnumerator())
        while (iter.MoveNext())
        {
          yield return iter.Current;
          if (!iter.MoveNext())
            yield break;
        }
    }

    /// <summary>
    /// Takes all elements that are in even index position in the sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">The sequence to return elements from.</param>
    /// <returns>A  sequence of all elements in even index position.</returns>
    /// <exception cref="System.ArgumentNullException">source is null</exception>
    public static IEnumerable<TSource> TakeEven<TSource>(this IEnumerable<TSource> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      return source.Skip(1).TakeOdd();
    }

    /// <summary>
    /// Takes all elements that are within the take/skip pattern provided.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">The sequence to return elements from.</param>
    /// <param name="take">The number of elements to take.</param>
    /// <param name="skip">The number of elements to skip.</param>
    /// <returns>A  sequence of all elements in matching the take/skip pattern provided.</returns>
    /// <exception cref="System.ArgumentNullException">source is null</exception>
    /// <exception cref="System.ArgumentException">take must and skip be larger than 0</exception>
    public static IEnumerable<TSource> TakePattern<TSource>(this IEnumerable<TSource> source, int take, int skip)
    {
      return TakePattern(source, 0, take, skip);
    }

    /// <summary>
    /// Takes all elements that are within the take/skip pattern provided.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">The sequence to return elements from.</param>
    /// <param name="initialSkip">The number of elements to skip from the start of the sequence.</param>
    /// <param name="take">The number of elements to take.</param>
    /// <param name="skip">The number of elements to skip.</param>
    /// <returns>A  sequence of all elements in matching the take/skip pattern provided.</returns>
    /// <exception cref="System.ArgumentNullException">source is null</exception>
    /// <exception cref="System.ArgumentException">take must and skip be larger than 0</exception>    
    public static IEnumerable<TSource> TakePattern<TSource>(this IEnumerable<TSource> source, int initialSkip, int take, int skip)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (take < 1)
        throw Error.InvalidTakeCount("take");
      if (skip < 1)
        throw Error.InvalidSkipCount("skip");

      using (IEnumerator<TSource> iter = source.GetEnumerator())
      {
        for (int s = 0; s <= initialSkip; s++)
          if (!iter.MoveNext())
            yield break;

        while (true)
        {
          for (int t = 0; t < take; t++)
          {
            yield return iter.Current;
            if (!iter.MoveNext())
              yield break;
          }
          for (int s = 0; s < skip; s++)
            if (!iter.MoveNext())
              yield break;
        }
      }
    }

    #endregion

    #region Take Before

    #region Plain

    /// <summary>
    /// Returns a all elements from the start of a sequence up to the specified item.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">The sequence to return elements from.</param>
    /// <param name="item">The element where operation stops.</param>
    /// <returns>All elements from the start of a sequence up to the specified item.</returns>
    /// <exception cref="System.ArgumentNullException">source is null</exception>
    public static IEnumerable<TSource> TakeBefore<TSource>(this IEnumerable<TSource> source, TSource item)
    {
      return TakeBefore(source, item, new SimpleEqualityComparer<TSource>());
    }

    /// <summary>
    /// Returns the Specified number of elements prior to the specified item.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">The sequence to return elements from.</param>
    /// <param name="item">The element where operation stops.</param>
    /// <param name="count">Number of elements to take.</param>
    /// <returns>The Specified number of elements prior to the specified item.</returns>
    /// <exception cref="System.ArgumentNullException">source is null</exception>
    public static IEnumerable<TSource> TakeBefore<TSource>(this IEnumerable<TSource> source, TSource item, int count)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      return source.Reverse().TakeAfter(item, count).Reverse();
    }

    /// <summary>
    /// Returns a all elements from the start of a sequence up to and including the specified item.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">The sequence to return elements from.</param>
    /// <param name="item">The last element to take.</param>
    /// <returns>All elements from the start of a sequence up to and including the specified item.</returns>
    /// <exception cref="System.ArgumentNullException">source is null</exception>
    public static IEnumerable<TSource> TakeSelfAndBefore<TSource>(this IEnumerable<TSource> source, TSource item)
    {
      return TakeSelfAndBefore(source, item, new SimpleEqualityComparer<TSource>());
    }

    /// <summary>
    /// Returns the Specified number of elements prior to and including the specified item.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">The sequence to return elements from.</param>
    /// <param name="item">The element where operation stops.</param>
    /// <param name="count">Number of elements to take.</param>
    /// <returns>The Specified number of elements prior to and including the specified item.</returns>
    /// <exception cref="System.ArgumentNullException">source is null</exception>
    public static IEnumerable<TSource> TakeSelfAndBefore<TSource>(this IEnumerable<TSource> source, TSource item, int count)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      return source.Reverse().TakeSelfAndAfter(item, count).Reverse();
    }

    #endregion

    #region Custom Compare

    /// <summary>
    /// Returns all elements from the start of a sequence up to the specified item using a custom comparer.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">The sequence to return elements from.</param>
    /// <param name="item">The element where operation stops.</param>
    /// <param name="comparer">A IEqualityComparer comparer to use when evaluating items</param>
    /// <returns>All elements from the start of a sequence up to the specified item.</returns>
    /// <exception cref="System.ArgumentNullException">source is null</exception>
    public static IEnumerable<TSource> TakeBefore<TSource>(this IEnumerable<TSource> source, TSource item, IEqualityComparer<TSource> comparer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (comparer == null)
        throw Error.ArgumentNull("comparer");

      foreach (TSource current in source)
      {
        if (comparer.Equals(current, item))
          break;
        yield return current;
      }
    }

    /// <summary>
    /// Returns the Specified number of elements prior to the specified item using a custom comparer.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">The sequence to return elements from.</param>
    /// <param name="item">The element where operation stops.</param>
    /// <param name="count">Number of elements to take.</param>
    /// <param name="comparer">A IEqualityComparer comparer to use when evaluating items</param>
    /// <returns>The Specified number of elements prior to the specified item.</returns>
    /// <exception cref="System.ArgumentNullException">source is null</exception>
    public static IEnumerable<TSource> TakeBefore<TSource>(this IEnumerable<TSource> source, TSource item, int count, IEqualityComparer<TSource> comparer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      return source.Reverse().TakeAfter(item, count, comparer).Reverse();
    }

    /// <summary>
    /// Returns a all elements from the start of a sequence up to and including the specified item using a custom comparer.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">The sequence to return elements from.</param>
    /// <param name="item">The last element to take.</param>
    /// <param name="comparer">A IEqualityComparer comparer to use when evaluating items</param>
    /// <returns>All elements from the start of a sequence up to and including the specified item.</returns>
    /// <exception cref="System.ArgumentNullException">source is null</exception>
    public static IEnumerable<TSource> TakeSelfAndBefore<TSource>(this IEnumerable<TSource> source, TSource item, IEqualityComparer<TSource> comparer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (comparer == null)
        throw Error.ArgumentNull("comparer");

      foreach (TSource current in source)
      {
        yield return current;
        if (comparer.Equals(current, item))
          break;
      }
    }

    /// <summary>
    /// Returns the Specified number of elements prior to and including the specified item using a custom comparer.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">The sequence to return elements from.</param>
    /// <param name="item">The element where operation stops.</param>
    /// <param name="count">Number of elements to take.</param>
    /// <param name="comparer">A IEqualityComparer comparer to use when evaluating items</param>
    /// <returns>The Specified number of elements prior to and including the specified item.</returns>
    /// <exception cref="System.ArgumentNullException">source is null</exception>
    public static IEnumerable<TSource> TakeSelfAndBefore<TSource>(this IEnumerable<TSource> source, TSource item, int count, IEqualityComparer<TSource> comparer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      return source.Reverse().TakeSelfAndAfter(item, count, comparer).Reverse();
    }

    #endregion

    #endregion

    #region Take After

    #region Plain

    /// <summary>
    /// Returns all elements after the specified item to end of sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">The sequence to return elements from.</param>
    /// <param name="item">The element where operation starts after.</param>
    /// <returns>All elements after the specified item to end of sequence.</returns>
    /// <exception cref="System.ArgumentNullException">source is null</exception>
    public static IEnumerable<TSource> TakeAfter<TSource>(this IEnumerable<TSource> source, TSource item)
    {
      return TakeAfter(source, item, new SimpleEqualityComparer<TSource>());
    }

    /// <summary>
    /// Returns the Specified number elements after the specified item.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">The sequence to return elements from.</param>
    /// <param name="item">The element where operation starts after.</param>
    /// <param name="count">Number of elements to take.</param>
    /// <returns>The Specified number elements after the specified item.</returns>
    /// <exception cref="System.ArgumentNullException">source is null</exception>
    public static IEnumerable<TSource> TakeAfter<TSource>(this IEnumerable<TSource> source, TSource item, int count)
    {
      return TakeAfter(source, item, count, new SimpleEqualityComparer<TSource>());
    }

    /// <summary>
    /// Returns all elements after and including the specified item to end of sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">The sequence to return elements from.</param>
    /// <param name="item">The element where operation starts.</param>
    /// <returns>All elements after and including the specified item to end of sequence.</returns>
    /// <exception cref="System.ArgumentNullException">source is null</exception>
    public static IEnumerable<TSource> TakeSelfAndAfter<TSource>(this IEnumerable<TSource> source, TSource item)
    {
      return TakeSelfAndAfter(source, item, new SimpleEqualityComparer<TSource>());
    }

    /// <summary>
    /// Returns the Specified number elements after and including the specified item.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">The sequence to return elements from.</param>
    /// <param name="item">The element where operation starts.</param>
    /// <param name="count">Number of elements to take.</param>
    /// <returns>The Specified number elements after and including the specified item.</returns>
    /// <exception cref="System.ArgumentNullException">source is null</exception>
    public static IEnumerable<TSource> TakeSelfAndAfter<TSource>(this IEnumerable<TSource> source, TSource item, int count)
    {
      return TakeSelfAndAfter(source, item, count, new SimpleEqualityComparer<TSource>());
    }

    #endregion

    #region Custom Compare

    /// <summary>
    /// Returns all elements after the specified item to end of sequence using a custom comparer.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">The sequence to return elements from.</param>
    /// <param name="item">The element where operation starts after.</param>
    /// <param name="comparer">A IEqualityComparer comparer to use when evaluating items</param>
    /// <returns>All elements after the specified item to end of sequence.</returns>
    /// <exception cref="System.ArgumentNullException">source is null</exception>
    public static IEnumerable<TSource> TakeAfter<TSource>(this IEnumerable<TSource> source, TSource item, IEqualityComparer<TSource> comparer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (comparer == null)
        throw Error.ArgumentNull("comparer");

      bool found = false;
      foreach (TSource current in source)
        if (!found)
          found = comparer.Equals(current, item);
        else
          yield return current;
    }

    /// <summary>
    /// Returns the Specified number elements after the specified item using a custom comparer.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">The sequence to return elements from.</param>
    /// <param name="item">The element where operation starts after.</param>
    /// <param name="count">Number of elements to take.</param>
    /// <param name="comparer">A IEqualityComparer comparer to use when evaluating items</param>
    /// <returns>The Specified number elements after the specified item.</returns>
    /// <exception cref="System.ArgumentNullException">source is null</exception>
    public static IEnumerable<TSource> TakeAfter<TSource>(this IEnumerable<TSource> source, TSource item, int count, IEqualityComparer<TSource> comparer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (comparer == null)
        throw Error.ArgumentNull("comparer");

      bool found = false;
      foreach (TSource current in source)
        if (!found)
          found = comparer.Equals(current, item);
        else
        {
          yield return current;
          count--;
          if (count == 0)
            break;
        }
    }

    /// <summary>
    /// Returns all elements after and including the specified item to end of sequence using a custom comparer. 
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">The sequence to return elements from.</param>
    /// <param name="item">The element where operation starts.</param>
    /// <param name="comparer">A IEqualityComparer comparer to use when evaluating items</param>
    /// <returns>All elements after and including the specified item to end of sequence.</returns>
    /// <exception cref="System.ArgumentNullException">source is null</exception>
    public static IEnumerable<TSource> TakeSelfAndAfter<TSource>(this IEnumerable<TSource> source, TSource item, IEqualityComparer<TSource> comparer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (comparer == null)
        throw Error.ArgumentNull("comparer");

      bool found = false;
      foreach (TSource current in source)
      {
        if (!found)
          found = comparer.Equals(current, item);
        if (found)
          yield return current;
      }
    }

    /// <summary>
    /// Returns the Specified number elements after and including the specified item using a custom comparer.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">The sequence to return elements from.</param>
    /// <param name="item">The element where operation starts.</param>
    /// <param name="count">Number of elements to take.</param>
    /// <param name="comparer">A IEqualityComparer comparer to use when evaluating items</param>
    /// <returns>The Specified number elements after and including the specified item.</returns>
    /// <exception cref="System.ArgumentNullException">source is null</exception>
    public static IEnumerable<TSource> TakeSelfAndAfter<TSource>(this IEnumerable<TSource> source, TSource item, int count, IEqualityComparer<TSource> comparer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (comparer == null)
        throw Error.ArgumentNull("comparer");

      bool found = false;
      foreach (TSource current in source)
      {
        if (!found)
        {
          found = comparer.Equals(current, item);
          if (!found)
            continue;
        }

        yield return current;
        count--;
        if (count < 0)
          break;
      }
    }

    #endregion

    #endregion

    #region Take Around

    #region Plain

    /// <summary>
    /// Takes the specified number of items before and after the the specified item.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">The sequence to return elements from.</param>
    /// <param name="item">The element where operation occurs.</param>
    /// <param name="count">number of items to take (total items will be up to count*2 + 1).</param>
    /// <returns>The specified number of items before and after the the specified item.</returns>
    public static IEnumerable<TSource> TakeAround<TSource>(this IEnumerable<TSource> source, TSource item, int count)
    {
      return TakeAround(source, item, count, count);
    }

    /// <summary>
    /// Takes the specified number of items before and after the the specified item.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">The sequence to return elements from.</param>
    /// <param name="item">The element where operation occurs.</param>
    /// <param name="before">number of items to take before the instance of item.</param>
    /// <param name="after">number of items to take after the instance of item.</param>
    /// <returns>The specified number of items before and after the the specified item.</returns>
    public static IEnumerable<TSource> TakeAround<TSource>(this IEnumerable<TSource> source, TSource item, int before, int after)
    {
      TSource[] sourceArr = source.ToArray();

      foreach (TSource current in TakeSelfAndBefore(sourceArr, item, before))
        yield return current;
      foreach (TSource current in TakeAfter(sourceArr, item, after))
        yield return current;
    }

    #endregion

    #region Custom Compare

    /// <summary>
    /// Takes the specified number of items before and after the the specified item using a custom comparer.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">The sequence to return elements from.</param>
    /// <param name="item">The element where operation occurs.</param>
    /// <param name="count">number of items to take (total items will be up to count*2 + 1).</param>
    /// <param name="comparer">A IEqualityComparer comparer to use when evaluating items</param>
    /// <returns>The specified number of items before and after the the specified item.</returns>
    public static IEnumerable<TSource> TakeAround<TSource>(this IEnumerable<TSource> source, TSource item, int count, IEqualityComparer<TSource> comparer)
    {
      return TakeAround(source, item, count, count, comparer);
    }

    /// <summary>
    /// Takes the specified number of items before and after the the specified item using a custom comparer.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">The sequence to return elements from.</param>
    /// <param name="item">The element where operation occurs.</param>
    /// <param name="before">number of items to take before the instance of item.</param>
    /// <param name="after">number of items to take after the instance of item.</param>
    /// <param name="comparer">A IEqualityComparer comparer to use when evaluating items</param>
    /// <returns>The specified number of items before and after the the specified item.</returns>
    public static IEnumerable<TSource> TakeAround<TSource>(this IEnumerable<TSource> source, TSource item, int before, int after, IEqualityComparer<TSource> comparer)
    {
      TSource[] sourceArr = source.ToArray();
      foreach (TSource current in TakeSelfAndBefore(sourceArr, item, before, comparer))
        yield return current;
      foreach (TSource current in TakeAfter(sourceArr, item, after, comparer))
        yield return current;
    }

    #endregion

    #endregion

    #region Take Top

    /// <summary>
    /// Returns a specified number of elements from the start of a sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">The sequence to return elements from.</param>
    /// <param name="count">The number of elements to return.</param>
    /// <returns>An System.Collections.Generic.IEnumerable&lt;T&gt; that contains the specified number of elements from the top of the input sequence.</returns>
    /// <exception cref="System.ArgumentNullException">source is null</exception>
    public static IEnumerable<TSource> TakeTop<TSource>(this IEnumerable<TSource> source, int count)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      foreach (TSource current in source)
      {
        if (count == 0)
          break;

        yield return current;
        count--;
      }
    }

    /// <summary>
    /// Returns a specified number of elements from the start of a sequence based on a predicate.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">The sequence to return elements from.</param>
    /// <param name="count">The number of elements to return.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>An System.Collections.Generic.IEnumerable&lt;T&gt; that contains the specified number of elements from the top of the input sequence based on a predicate.</returns>
    /// <exception cref="System.ArgumentNullException">source is null</exception>
    public static IEnumerable<TSource> TakeTop<TSource>(this IEnumerable<TSource> source, int count, Func<TSource, bool> predicate)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (predicate == null)
        throw Error.ArgumentNull("predicate");

      return source.Where(predicate).TakeTop(count);
    }

    /// <summary>
    /// Returns a specified percent of elements from the start of a sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">The sequence to return elements from.</param>
    /// <param name="percent">The percent of elements to return. user 1.00 to indicate 100%.</param>
    /// <returns>An System.Collections.Generic.IEnumerable&lt;T&gt; that contains the specified percent of elements from the top of the input sequence.</returns>
    /// <exception cref="System.ArgumentNullException">source is null</exception>
    public static IEnumerable<TSource> TakeTopPercent<TSource>(this IEnumerable<TSource> source, double percent) // 1 == 100%
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      TSource[] sourceArr = source.ToArray();
      int count = (int)(sourceArr.Count() * percent);
      return sourceArr.TakeTop(count);
    }

    /// <summary>
    /// Returns a specified percent of elements from the start of a sequence based on a predicate.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">The sequence to return elements from.</param>
    /// <param name="percent">The percent of elements to return. user 1.00 to indicate 100%.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>An System.Collections.Generic.IEnumerable&lt;T&gt; that contains the specified percent of elements from the top of the input sequence based on a predicate.</returns>
    /// <exception cref="System.ArgumentNullException">source is null</exception>
    public static IEnumerable<TSource> TakeTopPercent<TSource>(this IEnumerable<TSource> source, double percent, Func<TSource, bool> predicate) // 1 == 100%
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (predicate == null)
        throw Error.ArgumentNull("predicate");

        TSource[] sourceArr = source.ToArray();
        int count = (int)(sourceArr.Length * percent);
        return sourceArr.TakeTop(count, predicate);
    }

    #endregion

    #region Take Bottom

    /// <summary>
    /// Returns a specified number of elements from the end of a sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">The sequence to return elements from.</param>
    /// <param name="count">The number of elements to return.</param>
    /// <returns>An System.Collections.Generic.IEnumerable&lt;T&gt; that contains the specified number of elements from the bottom of the input sequence.</returns>
    /// <exception cref="System.ArgumentNullException">source is null</exception>
    public static IEnumerable<TSource> TakeBottom<TSource>(this IEnumerable<TSource> source, int count)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      return source.Reverse().TakeTop(count).Reverse();
    }

    /// <summary>
    /// Returns a specified number of elements from the end of a sequence based on a predicate.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">The sequence to return elements from.</param>
    /// <param name="count">The number of elements to return.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>An System.Collections.Generic.IEnumerable&lt;T&gt; that contains the specified number of elements from the bottom of the input sequence based on a predicate.</returns>
    /// <exception cref="System.ArgumentNullException">source is null</exception>
    public static IEnumerable<TSource> TakeBottom<TSource>(this IEnumerable<TSource> source, int count, Func<TSource, bool> predicate)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (predicate == null)
        throw Error.ArgumentNull("predicate");

      return source.Where(predicate).Reverse().TakeTop(count).Reverse();
    }

    /// <summary>
    /// Returns a specified percent of elements from the end of a sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">The sequence to return elements from.</param>
    /// <param name="percent">The percent of elements to return. user 1.00 to indicate 100%.</param>
    /// <returns>An System.Collections.Generic.IEnumerable&lt;T&gt; that contains the specified percent of elements from the bottom of the input sequence.</returns>
    /// <exception cref="System.ArgumentNullException">source is null</exception>
    public static IEnumerable<TSource> TakeBottomPercent<TSource>(this IEnumerable<TSource> source, double percent) // 1 == 100%
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      TSource[] sourceArr = source.ToArray();
      int count = (int)(sourceArr.Count() * percent);
      return sourceArr.Reverse().TakeTop(count).Reverse();
    }

    /// <summary>
    /// Returns a specified percent of elements from the end of a sequence based on a predicate.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">The sequence to return elements from.</param>
    /// <param name="percent">The percent of elements to return. user 1.00 to indicate 100%.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>An System.Collections.Generic.IEnumerable&lt;T&gt; that contains the specified percent of elements from the bottom of the input sequence based on a predicate.</returns>
    /// <exception cref="System.ArgumentNullException">source is null</exception>
    public static IEnumerable<TSource> TakeBottomPercent<TSource>(this IEnumerable<TSource> source, double percent, Func<TSource, bool> predicate) // 1 == 100%
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (predicate == null)
        throw Error.ArgumentNull("predicate");

      TSource[] sourceArr = source.ToArray();
      int count = (int)(sourceArr.Count() * percent);
      return sourceArr.Where(predicate).Reverse().TakeTop(count).Reverse();
    }

    #endregion
  }
}
