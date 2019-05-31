using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqLib.Sequence
{
  /// <summary>
  /// Provides sequence relations analysis and pattern detection methods.
  /// </summary>
  public static class Analysis
  {
    #region Sequence Relations

    /// <summary>
    /// Evaluated the relations between two sequences.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of the evaluated sequences.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="other">The sequence to use when evaluating the relation with the source.</param>
    /// <returns>A SequenceRelationType indicating the type of the relation between the sequences.</returns>
    /// <exception cref="System.ArgumentNullException">source or other is null.</exception>
    public static SequenceRelationType SequenceRelation<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> other)
    {
      return SequenceRelation(source, other, new SimpleEqualityComparer<TSource>());
    }

    /// <summary>
    /// Evaluated the relations between two sequences using a user supplied comparer.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of the evaluated sequences.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="other">The sequence to use when evaluating the relation with the source.</param>
    /// <param name="comparer">The IEqualityComparer&lt;TSource&gt; to use when comparing elements.</param>
    /// <returns>A SequenceRelationType indicating the type of the relation between the sequences.</returns>
    /// <exception cref="System.ArgumentNullException">source or other is null.</exception>
    public static SequenceRelationType SequenceRelation<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> other, IEqualityComparer<TSource> comparer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (other == null)
        throw Error.ArgumentNull("other");

      TSource[] sourceArr = source.ToArray();
      TSource[] otherArr = other.ToArray();

      int inputCount = sourceArr.Count();
      int otherCount = otherArr.Count();

      if (inputCount == otherCount) // Check for Same/Similar
        return IsSameOrSimilar(sourceArr, otherArr, comparer);

      if (inputCount > otherCount) // Contains/Intersects/None 
        return IsContainsOrIntersectesOrNone(sourceArr, otherArr, comparer);

      // Contained/Intersects/None       
      return IsContainedOrIntersectesOrNone(sourceArr, otherArr, comparer);
    }

    /// <summary>
    /// Compares two sequences creating a new sequence of results. If sequences are not equal in length, the returned sequence will be as long as the shorter sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of the compared sequences. TSource must implement IComparable.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="other">The sequence to compare with the source.</param>
    /// <returns>A sequence of the result returned by comparing items from the source and other sequences.</returns>
    /// <exception cref="System.ArgumentNullException">source or other is null.</exception>
    public static IEnumerable<int> CompareTo<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> other) where TSource : IComparable<TSource>
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (other == null)
        throw Error.ArgumentNull("other");

      using (IEnumerator<TSource> e1 = source.GetEnumerator())
      using (IEnumerator<TSource> e2 = other.GetEnumerator())
        while (e1.MoveNext() && e2.MoveNext())
          yield return e1.Current.CompareTo(e2.Current);
    }

    /// <summary>
    /// Compares two sequences creating a new sequence of results using a user supplied comparer. If sequences are not equal in length, the returned sequence will be as long as the shorter sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of the compared sequences.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="other">The sequence to compare with the source.</param>
    /// <param name="comparer">The IEqualityComparer&lt;TSource&gt; to use when comparing elements.</param>
    /// <returns>A sequence of the result returned by comparing items from the source and other sequences.</returns>
    /// <exception cref="System.ArgumentNullException">source or other is null.</exception>
    /// <exception cref="System.ArgumentNullException">comparer is null.</exception>
    public static IEnumerable<int> CompareTo<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> other, IComparer<TSource> comparer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (other == null)
        throw Error.ArgumentNull("other");
      if (comparer == null)
        throw Error.ArgumentNull("comparer");

      using (IEnumerator<TSource> e1 = source.GetEnumerator())
      using (IEnumerator<TSource> e2 = other.GetEnumerator())
        while (e1.MoveNext() && e2.MoveNext())
          yield return comparer.Compare(e1.Current, e2.Current);
    }

    #endregion

    #region Match and Align

    /// <summary>
    /// matches two sequences positioning matching elements together while omitting mismatches.
    /// </summary>
    /// <typeparam name="T">Type of elements in the left and right sequences.</typeparam>    
    /// <param name="leftSequence">A System.IEnumerable&lt;T&gt; of elements to match to.</param>
    /// <param name="rightSequence">A System.IEnumerable&lt;T&gt; of elements to match with.</param>
    /// <returns>A sequence of MatchResult elements matching left and right elements while omitting elements when the left or right element is missing.</returns>
    /// <exception cref="System.ArgumentNullException">leftSequence or rightSequence is null.</exception>
    public static IEnumerable<MatchResult<T, T>> Match<T>(this IEnumerable<T> leftSequence, IEnumerable<T> rightSequence)
    {
      return Match(leftSequence, rightSequence, (X1, X2) => Equals(X1, X2));
    }

    /// <summary>
    /// matches two sequences positioning matching elements together while omitting mismatches.
    /// </summary>
    /// <typeparam name="TLeft">Type of elements in the left sequence.</typeparam>    
    /// <typeparam name="TRight">Type of elements in the right sequence.</typeparam>
    /// <param name="leftSequence">A System.IEnumerable&lt;TLeft&gt; of elements to match to.</param>
    /// <param name="rightSequence">A System.IEnumerable&lt;TRight&gt; of elements to match with.</param>
    /// <param name="comparer">A custom comparer function that matches the left and right items.</param>
    /// <returns>A sequence of MatchResult elements matching left and right elements while omitting elements when the left or right element is missing.</returns>
    /// <exception cref="System.ArgumentNullException">leftSequence or rightSequence is null.</exception>
    public static IEnumerable<MatchResult<TLeft, TRight>> Match<TLeft, TRight>(this IEnumerable<TLeft> leftSequence, IEnumerable<TRight> rightSequence, Func<TLeft, TRight, bool> comparer)
    {
      if (leftSequence == null)
        throw Error.ArgumentNull("leftSequence");
      if (rightSequence == null)
        throw Error.ArgumentNull("rightSequence");
      if (comparer == null)
        throw Error.ArgumentNull("comparer");

      Queue<TLeft> q1 = new Queue<TLeft>();
      Queue<TRight> q2 = new Queue<TRight>();

      using (IEnumerator<TLeft> e1 = leftSequence.GetEnumerator())
      using (IEnumerator<TRight> e2 = rightSequence.GetEnumerator())
        while (e1.MoveNext() && e2.MoveNext())
        {
          q1.Enqueue(e1.Current);
          q2.Enqueue(e2.Current);
          var intersection = (from t1 in q1
                              from t2 in q2
                              where comparer(t1, t2)
                              select new { t1, t2 }).FirstOrDefault();
          if (intersection == null)
            continue;

          TLeft leftItem = default(TLeft);
          TRight rightItem = default(TRight);
          while (q1.Any())
          {
            leftItem = q1.Dequeue();
            if (Equals(leftItem, intersection.t1))
              break;
          }

          while (q2.Any())
          {
            rightItem = q2.Dequeue();
            if (Equals(rightItem, intersection.t2))
              break;
          }

          yield return new MatchResult<TLeft, TRight>(leftItem, rightItem);
        }
    }

    /// <summary>
    /// Aligns two sequences positioning matching elements together and indicating mismatches.
    /// </summary>
    /// <typeparam name="T">Type of elements in the left and right sequences.</typeparam>    
    /// <param name="leftSequence">A System.IEnumerable&lt;T&gt; of elements to align to.</param>
    /// <param name="rightSequence">A System.IEnumerable&lt;T&gt; of elements to align with.</param>
    /// <returns>A sequence of AlignResult elements matching left and right elements or returning a single element indicating whether the left or right element is missing.</returns>
    /// <exception cref="System.ArgumentNullException">leftSequence or rightSequence is null.</exception>
    public static IEnumerable<AlignResult<T, T>> Align<T>(this IEnumerable<T> leftSequence, IEnumerable<T> rightSequence)
    {
      return Align(leftSequence, rightSequence, default(T));
    }

    /// <summary>
    /// Aligns two sequences positioning matching elements together and indicating mismatches.
    /// </summary>
    /// <typeparam name="T">Type of elements in the left and right sequences.</typeparam>    
    /// <param name="leftSequence">A System.IEnumerable&lt;T&gt; of elements to align to.</param>
    /// <param name="rightSequence">A System.IEnumerable&lt;T&gt; of elements to align with.</param>
    /// <param name="replacer">A default value to place when the right or left elements are missing.</param>
    /// <returns>A sequence of AlignResult elements matching left and right elements or returning a single element indicating whether the left or right element is missing.</returns>
    /// <exception cref="System.ArgumentNullException">leftSequence or rightSequence is null.</exception>
    public static IEnumerable<AlignResult<T, T>> Align<T>(this IEnumerable<T> leftSequence, IEnumerable<T> rightSequence, T replacer)
    {
      return Align(leftSequence, rightSequence, (X1, X2) => Equals(X1, X2), replacer);
    }

    /// <summary>
    /// Aligns two sequences positioning matching elements together and indicating mismatches.
    /// </summary>
    /// <typeparam name="T">Type of elements in the left and right sequences.</typeparam>    
    /// <param name="leftSequence">A System.IEnumerable&lt;T&gt; of elements to align to.</param>
    /// <param name="rightSequence">A System.IEnumerable&lt;T&gt; of elements to align with.</param>
    /// <param name="comparer">A custom comparer function that matches the left and right items.</param>
    /// <param name="replacer">A default value to place when the right or left elements are missing.</param>
    /// <returns>A sequence of AlignResult elements matching left and right elements or returning a single element indicating whether the left or right element is missing.</returns>
    /// <exception cref="System.ArgumentNullException">leftSequence or rightSequence is null.</exception>
    public static IEnumerable<AlignResult<T, T>> Align<T>(this IEnumerable<T> leftSequence, IEnumerable<T> rightSequence, Func<T, T, bool> comparer, T replacer)
    {
      return Align(leftSequence, rightSequence, comparer, replacer, replacer);
    }

    /// <summary>
    /// Aligns two sequences positioning matching elements together and indicating mismatches.
    /// </summary>
    /// <typeparam name="TLeft">Type of elements in the left sequence.</typeparam>    
    /// <typeparam name="TRight">Type of elements in the right sequence.</typeparam>
    /// <param name="leftSequence">A System.IEnumerable&lt;TLeft&gt; of elements to align to.</param>
    /// <param name="rightSequence">A System.IEnumerable&lt;TRight&gt; of elements to align with.</param>
    /// <param name="comparer">A custom comparer function that matches the left and right items.</param>
    /// <returns>A sequence of AlignResult elements matching left and right elements or returning a single element indicating whether the left or right element is missing.</returns>
    /// <exception cref="System.ArgumentNullException">leftSequence or rightSequence is null.</exception>
    public static IEnumerable<AlignResult<TLeft, TRight>> Align<TLeft, TRight>(this IEnumerable<TLeft> leftSequence, IEnumerable<TRight> rightSequence, Func<TLeft, TRight, bool> comparer)
    {
      return Align(leftSequence, rightSequence, comparer, default(TLeft), default(TRight));
    }

    /// <summary>
    /// Aligns two sequences positioning matching elements together and indicating mismatches.
    /// </summary>
    /// <typeparam name="TLeft">Type of elements in the left sequence.</typeparam>    
    /// <typeparam name="TRight">Type of elements in the right sequence.</typeparam>
    /// <param name="leftSequence">A System.IEnumerable&lt;TLeft&gt; of elements to align to.</param>
    /// <param name="rightSequence">A System.IEnumerable&lt;TRight&gt; of elements to align with.</param>
    /// <param name="comparer">A custom comparer function that matches the left and right items.</param>
    /// <param name="leftReplacer">A default value to place when the left element is missing.</param>
    /// <param name="rightReplacer">A default value to place when the right element is missing.</param>
    /// <returns>A sequence of AlignResult elements matching left and right elements or returning a single element indicating whether the left or right element is missing.</returns>
    /// <exception cref="System.ArgumentNullException">leftSequence or rightSequence is null.</exception>
    public static IEnumerable<AlignResult<TLeft, TRight>> Align<TLeft, TRight>(this IEnumerable<TLeft> leftSequence, IEnumerable<TRight> rightSequence, Func<TLeft, TRight, bool> comparer, TLeft leftReplacer, TRight rightReplacer)
    {
      if (leftSequence == null)
        throw Error.ArgumentNull("leftSequence");
      if (rightSequence == null)
        throw Error.ArgumentNull("rightSequence");

      Queue<TLeft> leftQueue = new Queue<TLeft>();
      Queue<TRight> rightQueue = new Queue<TRight>();

      using (IEnumerator<TLeft> left = leftSequence.GetEnumerator())
      using (IEnumerator<TRight> right = rightSequence.GetEnumerator())
      {
        while (true)
        {
          bool movedLeft = left.MoveNext();
          bool movedRight = right.MoveNext();
          if (movedLeft)
            leftQueue.Enqueue(left.Current);
          if (movedRight)
            rightQueue.Enqueue(right.Current);

          if (!(movedLeft && movedRight))
            break;

          var intersection = (from t1 in leftQueue
                              from t2 in rightQueue
                              where comparer(t1, t2)
                              select new { t1, t2 }).FirstOrDefault();

          if (intersection == null)
            continue;

          TLeft leftItem = leftReplacer;
          TRight rightItem = rightReplacer;
          while (leftQueue.Any())
          {
            leftItem = leftQueue.Dequeue();
            if (Equals(leftItem, intersection.t1))
              break;

            yield return new AlignResult<TLeft, TRight>(leftItem, rightReplacer, AlignType.RightMissing);
          }

          while (rightQueue.Any())
          {
            rightItem = rightQueue.Dequeue();
            if (Equals(rightItem, intersection.t2))
              break;

            yield return new AlignResult<TLeft, TRight>(leftReplacer, rightItem, AlignType.LeftMissing);
          }

          yield return new AlignResult<TLeft, TRight>(leftItem, rightItem);
        }

        while (left.MoveNext())
          leftQueue.Enqueue(left.Current);
        while (right.MoveNext())
          rightQueue.Enqueue(right.Current);
      }

      while (leftQueue.Any())
        yield return new AlignResult<TLeft, TRight>(leftQueue.Dequeue(), rightReplacer, AlignType.RightMissing);

      while (rightQueue.Any())
        yield return new AlignResult<TLeft, TRight>(leftReplacer, rightQueue.Dequeue(), AlignType.LeftMissing);
    }

    #endregion

    #region Pattern Detection

    /// <summary>
    /// Attempts to discover a repetitive pattern of elements in the supplied sequence.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A System.IEnumerable&lt;T&gt; of elements to scan for pattern.</param>
    /// <returns>A System.IEnumerable&lt;T&gt; with the found pattern, null if pattern was not found.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <remarks>A sequence of 1,2,3,1,2,3,1,2,3,1 will return 1,2,3. the returned pattern fits three time in source and the last element matched the start of the discovered pattern.</remarks>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    public static IEnumerable<T> GetPattern<T>(this IEnumerable<T> source)
    {
      return source.GetPattern(new SimpleEqualityComparer<T>(), 1, int.MaxValue, false);
    }

    /// <summary>
    /// Attempts to discover a repetitive pattern of elements in the supplied sequence.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A System.IEnumerable&lt;T&gt; of elements to scan for pattern.</param>
    /// <param name="minSize">The minimal length of the pattern.</param>
    /// <param name="maxSize">The maximal length of the pattern.</param>
    /// <param name="exactFit">A Boolean indicating if the discovered pattern must fit exactly into the source.</param>
    /// <returns>A System.IEnumerable&lt;T&gt; with the found pattern, null if pattern was not found.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    public static IEnumerable<T> GetPattern<T>(this IEnumerable<T> source, int minSize, int maxSize, bool exactFit)
    {
      return source.GetPattern(new SimpleEqualityComparer<T>(), minSize, maxSize, exactFit);
    }

    /// <summary>
    /// Attempts to discover a repetitive pattern of elements in the supplied sequence using a user supplied comparer.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A System.IEnumerable&lt;T&gt; of elements to scan for pattern.</param>
    /// <param name="comparer">The IEqualityComparer&lt;TSource&gt; to use when processing elements.</param>
    /// <param name="minSize">The minimal length of the pattern.</param>
    /// <param name="maxSize">The maximal length of the pattern.</param>
    /// <param name="exactFit">A Boolean indicating if the discovered pattern must fit exactly into the source.</param>
    /// <returns>A System.IEnumerable&lt;T&gt; with the found pattern, null if pattern was not found.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    public static IEnumerable<T> GetPattern<T>(this IEnumerable<T> source, IEqualityComparer<T> comparer, int minSize, int maxSize, bool exactFit)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      bool found = false;

      if (minSize < 1)
        minSize = 1;

      T[] sourceArr = source.ToArray();
      int len = sourceArr.Length;
      T[] pat = null;
      maxSize = maxSize > len / 2 ? len / 2 : maxSize;

      for (int patSize = minSize; patSize <= maxSize; patSize++)
      {
        if (exactFit && len % patSize != 0)
          continue;

        pat = sourceArr.Take(patSize).ToArray();

        for (int skip = 1; skip < len / patSize; skip++)
        {
          found = sourceArr.Skip(skip * patSize).Take(patSize).SequenceEqual(pat, comparer);
          if (!found)
            break;
        }

        if (found)
          if (!exactFit)
          {
            int rem = len % pat.Length;
            if (rem == 0)
              break;

            if (pat.Take(rem).SequenceEqual(sourceArr.Skip(len - rem)))
              break;
          }
          else
            break;
      }
      return found ? pat : null;
    }

    #endregion

    #region Index Of

    /// <summary>
    /// Reports the index of the first occurrence of the specified item in the source sequence.
    /// </summary>
    /// <typeparam name="T">Type of elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="item">The item to seek.</param>
    /// <returns>The zero-based index position of item if that element is found, or -1 if it is not.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    public static int IndexOf<T>(this IEnumerable<T> source, T item)
    {
      return IndexOf(source, item, new SimpleEqualityComparer<T>());
    }

    /// <summary>
    /// Reports the index of the first occurrence of the specified item in the source sequence.
    /// </summary>
    /// <typeparam name="T">Type of elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="item">The item to seek.</param>
    /// <param name="startIndex">The search starting position.</param>
    /// <returns>The zero-based index position of item if that element is found, or -1 if it is not.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    public static int IndexOf<T>(this IEnumerable<T> source, T item, int startIndex)
    {
      return IndexOf(source, item, new SimpleEqualityComparer<T>(), startIndex);
    }

    /// <summary>
    /// Reports the index of the first occurrence of the specified item in the source sequence.
    /// </summary>
    /// <typeparam name="T">Type of elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="item">The item to seek.</param>
    /// <param name="comparer">A IEqualityComparer comparer to use when evaluating items</param>
    /// <returns>The zero-based index position of item if that element is found, or -1 if it is not.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentNullException">comparer is null.</exception>
    public static int IndexOf<T>(this IEnumerable<T> source, T item, IEqualityComparer<T> comparer)
    {
      return IndexOf(source, item, comparer, 0);
    }

    /// <summary>
    /// Reports the index of the first occurrence of the specified item in the source sequence.
    /// </summary>
    /// <typeparam name="T">Type of elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="item">The item to seek.</param>
    /// <param name="comparer">A IEqualityComparer comparer to use when evaluating items</param>
    /// <param name="startIndex">The search starting position.</param>
    /// <returns>The zero-based index position of item if that element is found, or -1 if it is not.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentNullException">comparer is null.</exception>
    public static int IndexOf<T>(this IEnumerable<T> source, T item, IEqualityComparer<T> comparer, int startIndex)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (comparer == null)
        throw Error.ArgumentNull("comparer");

      int idx = startIndex;

      foreach (T sourceItem in source.Skip(startIndex))
      {
        if (comparer.Equals(sourceItem, item))
          return idx;
        idx++;
      }
      return -1;
    }

    /// <summary>
    /// Returns a sequence of indexes of occurrences of the specified item in the source sequence.
    /// </summary>
    /// <typeparam name="T">Type of elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="item">The item to seek.</param>
    /// <returns>A sequence of indexes of occurrences of the specified item in the source sequence</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    public static IEnumerable<int> IndexesOf<T>(this IEnumerable<T> source, T item)
    {
      return IndexesOf(source, item, new SimpleEqualityComparer<T>());
    }

    /// <summary>
    /// Returns a sequence of indexes of occurrences of the specified item in the source sequence.
    /// </summary>
    /// <typeparam name="T">Type of elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="item">The item to seek.</param>
    /// <param name="comparer">A IEqualityComparer comparer to use when evaluating items</param>
    /// <returns>A sequence of indexes of occurrences of the specified item in the source sequence</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentNullException">comparer is null.</exception>
    public static IEnumerable<int> IndexesOf<T>(this IEnumerable<T> source, T item, IEqualityComparer<T> comparer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (comparer == null)
        throw Error.ArgumentNull("comparer");

      int idx = 0;
      foreach (T sourceItem in source)
      {
        if (comparer.Equals(sourceItem, item))
          yield return idx;
        idx++;
      }
    }

    /// <summary>
    /// Reports the index of the first occurrence of the matching item in the source sequence.
    /// </summary>
    /// <typeparam name="T">Type of elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>The zero-based index position of item if that element is found, or -1 if it is not.</returns>
    /// <exception cref="System.ArgumentNullException">source or is null.</exception> 
    /// <exception cref="System.ArgumentNullException">predicate or is null.</exception> 
    public static int IndexOf<T>(this IEnumerable<T> source, Predicate<T> predicate)
    {
      return IndexOf(source, predicate, 0);
    }

    /// <summary>
    /// Reports the index of the first occurrence of the matching item in the source sequence.
    /// </summary>
    /// <typeparam name="T">Type of elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="startIndex">The search starting position.</param>
    /// <returns>The zero-based index position of item if that element is found, or -1 if it is not.</returns>
    /// <exception cref="System.ArgumentNullException">source or is null.</exception> 
    /// <exception cref="System.ArgumentNullException">predicate or is null.</exception> 
    public static int IndexOf<T>(this IEnumerable<T> source, Predicate<T> predicate, int startIndex)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (predicate == null)
        throw Error.ArgumentNull("predicate");

      int idx = startIndex;

      foreach (T sourceItem in source.Skip(startIndex))
      {
        if (predicate(sourceItem))
          return idx;
        idx++;
      }
      return -1;
    }

    #endregion

    #region Private Helpers

    /// <summary>
    /// Used internally by SequenceRelation method
    /// </summary>
    private static SequenceRelationType IsSameOrSimilar<TSource>(IEnumerable<TSource> source, IEnumerable<TSource> other, IEqualityComparer<TSource> comparer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (comparer == null)
        throw Error.ArgumentNull("comparer");

      TSource[] sourceArr = source.ToArray();
      TSource[] otherArr = other.ToArray();
      if (sourceArr.SequenceEqual(otherArr, comparer))
        return SequenceRelationType.Equal;

      if (sourceArr.OrderBy(X => comparer.GetHashCode(X)).SequenceEqual(otherArr.OrderBy(X => comparer.GetHashCode(X))))
        return SequenceRelationType.Similar;

      foreach (TSource item in otherArr)
        if (sourceArr.Contains(item, comparer))
          return SequenceRelationType.Intersects;
      return SequenceRelationType.None;
    }

    /// <summary>
    /// Used internally by SequenceRelation method
    /// </summary>
    private static SequenceRelationType IsContainsOrIntersectesOrNone<TSource>(IEnumerable<TSource> source, IEnumerable<TSource> other, IEqualityComparer<TSource> comparer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (comparer == null)
        throw Error.ArgumentNull("comparer");

      bool any = false;
      bool all = true;

      TSource[] sourceArr = source.ToArray();

      foreach (TSource item in other)
      {
        if (sourceArr.Contains(item, comparer))
          any = true;
        else
          all = false;
        if (any && !all)
          break;
      }
      if (all)
        return SequenceRelationType.Contains;

      if (any)
        return SequenceRelationType.Intersects;

      return SequenceRelationType.None;
    }

    /// <summary>
    /// Used internally by SequenceRelation method
    /// </summary>
    private static SequenceRelationType IsContainedOrIntersectesOrNone<TSource>(IEnumerable<TSource> source, IEnumerable<TSource> other, IEqualityComparer<TSource> comparer)
    {
      SequenceRelationType sequenceRelationType = IsContainsOrIntersectesOrNone(other, source, comparer);
      if (sequenceRelationType == SequenceRelationType.Contains)
        return SequenceRelationType.Contained;

      return sequenceRelationType;
    }

    #endregion
  }
}
