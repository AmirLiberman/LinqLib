using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqLib.Operators
{
  /// <summary>
  /// Provides extension methods for calculating Frequency, probability, Average, variance and standard deviation on sequences
  /// </summary>
  public static class Statistical
  {
    #region Sequence Frequency and probability

    /// <summary>
    /// Calculates the frequency of each element in a sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of the evaluated sequences.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <returns>A sequence of Key-Value pairs with an entry for each element in original sequence and its frequency.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    public static IEnumerable<KeyValuePair<TSource, int>> Frequency<TSource>(this IEnumerable<TSource> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      return source.GroupBy(I => I).Select(K => new KeyValuePair<TSource, int>(K.Key, K.Count()));
    }

    /// <summary>
    /// Calculates the frequency of each element in a sequence based on a custom list of buckets.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of the evaluated sequences.</typeparam>
    /// <typeparam name="TBucket">The type of the bucket elements.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="buckets">A list of buckets to which each element in the source sequence may fit.</param>
    /// <param name="bucketSelector">A function that take a source element and the list of buckets and returns the bucket where the source element fits.</param>
    /// <returns>A sequence of Key-Value pairs with an entry for each bucket element and its frequency.</returns>
    /// <exception cref="System.ArgumentNullException">source or buckets or bucketSelector is null.</exception>
    public static IEnumerable<KeyValuePair<TBucket, int>> Frequency<TSource, TBucket>(this IEnumerable<TSource> source, IEnumerable<TBucket> buckets, Func<TSource, IEnumerable<TBucket>, TBucket> bucketSelector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (buckets == null)
        throw Error.ArgumentNull("buckets");
      if (bucketSelector == null)
        throw Error.ArgumentNull("bucketSelector");

      IEnumerable<KeyValuePair<TBucket, int>> rawFreq =
        source.GroupBy(I => bucketSelector(I, buckets)).Select(K => new KeyValuePair<TBucket, int>(K.Key, K.Count()));
      IEnumerable<KeyValuePair<TBucket, int>> freq = from bucket in buckets
                                                     join freqItem in rawFreq on bucket equals freqItem.Key into
                                                       freqBucketList
                                                     from item in
                                                       freqBucketList.DefaultIfEmpty(
                                                         new KeyValuePair<TBucket, int>(bucket, 0))
                                                     select item;
      return freq;
    }

    /// <summary>
    /// Calculates the frequency of each element based on a custom transformation.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of the evaluated sequences.</typeparam>
    /// <typeparam name="TBucket">The type of the bucket elements.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="bucketSelector">A function that take a source element and returns the bucket where the source element fits.</param>
    /// <returns>A sequence of Key-Value pairs with an entry for each bucket element and its frequency.</returns>
    /// <exception cref="System.ArgumentNullException">source or bucketSelector is null.</exception>
    public static IEnumerable<KeyValuePair<TBucket, int>> Frequency<TSource, TBucket>(this IEnumerable<TSource> source, Func<TSource, TBucket> bucketSelector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (bucketSelector == null)
        throw Error.ArgumentNull("bucketSelector");

      return source.GroupBy(I => bucketSelector(I)).Select(K => new KeyValuePair<TBucket, int>(K.Key, K.Count()));
    }

    /// <summary>
    /// Calculates the probability of randomly matching an item in a sequence. 
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of the evaluated sequences.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="item">The item to match.</param>
    /// <returns>A double-precision floating-point value between 0 and 1.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    public static double Probability<TSource>(this IEnumerable<TSource> source, TSource item)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      TSource[] sourceArr = source.ToArray();
      double items = sourceArr.Count();
      double matches = sourceArr.Count(I => item.Equals(I));

      return matches / items;
    }

    /// <summary>
    /// Calculates the probability of randomly matching an item in a sequence using a custom comparer. 
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of the evaluated sequences.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="item">The item to match.</param>
    /// <param name="comparer">An IEqualityComparer instance to use when matching elements.</param>
    /// <returns>A double-precision floating-point value between 0 and 1.</returns>    
    /// <exception cref="System.ArgumentNullException">source or comparer is null.</exception>
    public static double Probability<TSource>(this IEnumerable<TSource> source, TSource item, IEqualityComparer<TSource> comparer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (comparer == null)
        throw Error.ArgumentNull("comparer");

      TSource[] sourceArr = source.ToArray();

      double items = sourceArr.Count();
      double matches = sourceArr.Count(I => comparer.Equals(item, I));

      return matches / items;
    }

    #endregion

    #region Moving Sum

    /// <summary>
    /// Computes the moving sum of a sequence of System.Double values.
    /// </summary>
    /// <param name="source">A sequence of System.Double values to calculate the moving sum on.</param>
    /// <param name="blockSize">The number of elements in the moving sum block. Block size must be two or larger.</param>
    /// <returns>A sequence of System.Double values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    public static IEnumerable<double> MovingSum(this IEnumerable<double> source, int blockSize)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (blockSize < 2)
        throw Error.ValueMinTwo("blockSize");

      double sum = 0;
      int block = blockSize;
      int nans = -1;

      using (IEnumerator<double> left = source.GetEnumerator())
      using (IEnumerator<double> right = source.GetEnumerator())
      {
        // Add the first set of blockSize elements
        double value;
        while (block > 1)
        {
          block--;
          if (!right.MoveNext())
          {
            // If any of the elements in the block were nan, yield a nan, otherwise yield the sum.
            if (nans > 0)
              yield return double.NaN;
            else
              yield return sum;
            yield break;
          }
          value = right.Current;
          if (double.IsNaN(value))
            nans = blockSize;
          else
          {
            sum += value;
            nans--;
          }
        }

        // Add a value from the left and subtract from the right.
        // If a nan is encountered, set the nans to the block size and yield nanas until it clears.
        while (right.MoveNext())
        {
          value = right.Current;
          if (double.IsNaN(value))
            nans = blockSize;
          else
          {
            sum += value;
            nans--;
          }
          if (nans > 0)
            yield return double.NaN;
          else
            yield return sum;

          left.MoveNext();
          value = left.Current;
          if (!double.IsNaN(value))
            sum -= value;
        }
      }
    }

    /// <summary>
    /// Computes the moving variance in a sequence of a sequence of Nullable&lt;System.Double&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Double&gt; values to calculate the moving variance in a sequence on.</param>
    /// <param name="blockSize">The number of elements in the moving variance in a sequence block. Block size must be two or larger.</param>
    /// <returns>A sequence of Nullable&lt;System.Double&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    public static IEnumerable<double?> MovingSum(this IEnumerable<double?> source, int blockSize)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      return source.Select(S => S.GetValueOrDefault()).MovingSum(blockSize).Cast<double?>();
    }

    /// <summary>
    /// Computes the moving variance in a sequence of a sequence of System.Singel values.
    /// </summary>
    /// <param name="source">A sequence of System.Singel values to calculate the moving variance in a sequence on.</param>
    /// <param name="blockSize">The number of elements in the moving variance in a sequence block. Block size must be two or larger.</param>
    /// <returns>A sequence of System.Singel values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    public static IEnumerable<float> MovingSum(this IEnumerable<float> source, int blockSize)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (blockSize < 2)
        throw Error.ValueMinTwo("blockSize");

      float sum = 0;
      int block = blockSize;
      int nans = -1;

      using (IEnumerator<float> left = source.GetEnumerator())
      using (IEnumerator<float> right = source.GetEnumerator())
      {
        float value;
        while (block > 1)
        {
          block--;
          if (!right.MoveNext())
          {
            if (nans > 0)
              yield return float.NaN;
            else
              yield return sum;
            yield break;
          }
          value = right.Current;
          if (float.IsNaN(value))
            nans = blockSize;
          else
          {
            sum += value;
            nans--;
          }
        }

        while (right.MoveNext())
        {
          value = right.Current;
          if (float.IsNaN(value))
            nans = blockSize;
          else
          {
            sum += value;
            nans--;
          }
          if (nans > 0)
            yield return float.NaN;
          else
            yield return sum;

          left.MoveNext();
          value = left.Current;
          if (!float.IsNaN(value))
            sum -= value;
        }
      }
    }

    /// <summary>
    /// Computes the moving variance in a sequence of a sequence of Nullable&lt;System.Singel&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Singel&gt; values to calculate the moving variance in a sequence on.</param>
    /// <param name="blockSize">The number of elements in the moving variance in a sequence block. Block size must be two or larger.</param>
    /// <returns>A sequence of Nullable&lt;System.Singel&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    public static IEnumerable<float?> MovingSum(this IEnumerable<float?> source, int blockSize)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      return source.Select(S => S.GetValueOrDefault()).MovingSum(blockSize).Cast<float?>();
    }

    /// <summary>
    /// Computes the moving variance in a sequence of a sequence of System.Decimal values.
    /// </summary>
    /// <param name="source">A sequence of System.Decimal values to calculate the moving variance in a sequence on.</param>
    /// <param name="blockSize">The number of elements in the moving variance in a sequence block. Block size must be two or larger.</param>
    /// <returns>A sequence of System.Decimal values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    public static IEnumerable<decimal> MovingSum(this IEnumerable<decimal> source, int blockSize)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (blockSize < 2)
        throw Error.ValueMinTwo("blockSize");

      decimal sum = 0;
      int block = blockSize;
      using (IEnumerator<decimal> left = source.GetEnumerator())
      using (IEnumerator<decimal> right = source.GetEnumerator())
      {
        while (block > 1)
        {
          block--;
          if (!right.MoveNext())
          {
            yield return sum;
            yield break;
          }
          sum += right.Current;
        }

        while (right.MoveNext())
        {
          sum += right.Current;
          yield return sum;
          left.MoveNext();
          sum -= left.Current;
        }
      }
    }

    /// <summary>
    /// Computes the moving variance in a sequence of a sequence of Nullable&lt;System.Decimal&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Decimal&gt; values to calculate the moving variance in a sequence on.</param>
    /// <param name="blockSize">The number of elements in the moving variance in a sequence block. Block size must be two or larger.</param>
    /// <returns>A sequence of Nullable&lt;System.Decimal&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    public static IEnumerable<decimal?> MovingSum(this IEnumerable<decimal?> source, int blockSize)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      return source.Select(S => S.GetValueOrDefault()).MovingSum(blockSize).Cast<decimal?>();
    }

    /// <summary>
    /// Computes the moving variance in a sequence of a sequence of System.Int64 values.
    /// </summary>
    /// <param name="source">A sequence of System.Int64 values to calculate the moving variance in a sequence on.</param>
    /// <param name="blockSize">The number of elements in the moving variance in a sequence block. Block size must be two or larger.</param>
    /// <returns>A sequence of System.Double values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    public static IEnumerable<long> MovingSum(this IEnumerable<long> source, int blockSize)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (blockSize < 2)
        throw Error.ValueMinTwo("blockSize");

      long sum = 0;
      int block = blockSize;
      using (IEnumerator<long> left = source.GetEnumerator())
      using (IEnumerator<long> right = source.GetEnumerator())
      {
        while (block > 1)
        {
          block--;
          if (!right.MoveNext())
          {
            yield return sum;
            yield break;
          }
          sum += right.Current;
        }

        while (right.MoveNext())
        {
          sum += right.Current;
          yield return sum;
          left.MoveNext();
          sum -= left.Current;
        }
      }
    }

    /// <summary>
    /// Computes the moving variance in a sequence of a sequence of Nullable&lt;System.Int64&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Int64&gt; values to calculate the moving variance in a sequence on.</param>
    /// <param name="blockSize">The number of elements in the moving variance in a sequence block. Block size must be two or larger.</param>
    /// <returns>A sequence of Nullable&lt;System.Double&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    public static IEnumerable<long?> MovingSum(this IEnumerable<long?> source, int blockSize)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      return source.Select(S => S.GetValueOrDefault()).MovingSum(blockSize).Cast<long?>();
    }

    /// <summary>
    /// Computes the moving variance in a sequence of a sequence of System.Int32 values.
    /// </summary>
    /// <param name="source">A sequence of System.Int32 values to calculate the moving variance in a sequence on.</param>
    /// <param name="blockSize">The number of elements in the moving variance in a sequence block. Block size must be two or larger.</param>
    /// <returns>A sequence of System.Double values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    public static IEnumerable<int> MovingSum(this IEnumerable<int> source, int blockSize)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (blockSize < 2)
        throw Error.ValueMinTwo("blockSize");

      int sum = 0;
      int block = blockSize;
      using (IEnumerator<int> left = source.GetEnumerator())
      using (IEnumerator<int> right = source.GetEnumerator())
      {
        while (block > 1)
        {
          block--;
          if (!right.MoveNext())
          {
            yield return sum;
            yield break;
          }
          sum += right.Current;
        }

        while (right.MoveNext())
        {
          sum += right.Current;
          yield return sum;
          left.MoveNext();
          sum -= left.Current;
        }
      }
    }

    /// <summary>
    /// Computes the moving variance in a sequence of a sequence of Nullable&lt;System.Int32&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Int32&gt; values to calculate the moving variance in a sequence on.</param>
    /// <param name="blockSize">The number of elements in the moving variance in a sequence block. Block size must be two or larger.</param>
    /// <returns>A sequence of Nullable&lt;System.Double&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    public static IEnumerable<int?> MovingSum(this IEnumerable<int?> source, int blockSize)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      return source.Select(S => S.GetValueOrDefault()).MovingSum(blockSize).Cast<int?>();
    }

    /// <summary>
    /// Computes the moving variance in a sequence of a sequence of System.Double values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">A sequence of System.Double values to calculate the moving variance in a sequence on.</param>
    /// <param name="blockSize">The number of elements in the moving variance in a sequence block. Block size must be two or larger.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of System.Double values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    public static IEnumerable<double> MovingSum<TSource>(this IEnumerable<TSource> source, int blockSize, Func<TSource, double> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return MovingSum(source.Select(S => selector(S)), blockSize);
    }

    /// <summary>
    /// Computes the moving variance in a sequence of a sequence of Nullable&lt;System.Double&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">A sequence of Nullable&lt;System.Double&gt; values to calculate the moving variance in a sequence on.</param>
    /// <param name="blockSize">The number of elements in the moving variance in a sequence block. Block size must be two or larger.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of Nullable&lt;System.Double&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    public static IEnumerable<double?> MovingSum<TSource>(this IEnumerable<TSource> source, int blockSize, Func<TSource, double?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return MovingSum(source.Select(S => selector(S)), blockSize);
    }

    /// <summary>
    /// Computes the moving variance in a sequence of a sequence of System.Single values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">A sequence of System.Single values to calculate the moving variance in a sequence on.</param>
    /// <param name="blockSize">The number of elements in the moving variance in a sequence block. Block size must be two or larger.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of System.Single values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    public static IEnumerable<float> MovingSum<TSource>(this IEnumerable<TSource> source, int blockSize, Func<TSource, float> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return MovingSum(source.Select(S => selector(S)), blockSize);
    }

    /// <summary>
    /// Computes the moving variance in a sequence of a sequence of Nullable&lt;System.Single&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">A sequence of Nullable&lt;System.Single&gt; values to calculate the moving variance in a sequence on.</param>
    /// <param name="blockSize">The number of elements in the moving variance in a sequence block. Block size must be two or larger.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of Nullable&lt;System.Single&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    public static IEnumerable<float?> MovingSum<TSource>(this IEnumerable<TSource> source, int blockSize, Func<TSource, float?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return MovingSum(source.Select(S => selector(S)), blockSize);
    }

    /// <summary>
    /// Computes the moving variance in a sequence of a sequence of System.Decimal values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">A sequence of System.Decimal values to calculate the moving variance in a sequence on.</param>
    /// <param name="blockSize">The number of elements in the moving variance in a sequence block. Block size must be two or larger.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of System.Decimal values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    public static IEnumerable<decimal> MovingSum<TSource>(this IEnumerable<TSource> source, int blockSize, Func<TSource, decimal> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return MovingSum(source.Select(S => selector(S)), blockSize);
    }

    /// <summary>
    /// Computes the moving variance in a sequence of a sequence of Nullable&lt;System.Decimal&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">A sequence of Nullable&lt;System.Decimal&gt; values to calculate the moving variance in a sequence on.</param>
    /// <param name="blockSize">The number of elements in the moving variance in a sequence block. Block size must be two or larger.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of Nullable&lt;System.Decimal&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    public static IEnumerable<decimal?> MovingSum<TSource>(this IEnumerable<TSource> source, int blockSize, Func<TSource, decimal?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return MovingSum(source.Select(S => selector(S)), blockSize);
    }

    /// <summary>
    /// Computes the moving variance in a sequence of a sequence of System.Int64 values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">A sequence of System.Int64 values to calculate the moving variance in a sequence on.</param>
    /// <param name="blockSize">The number of elements in the moving variance in a sequence block. Block size must be two or larger.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of System.Double values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    public static IEnumerable<long> MovingSum<TSource>(this IEnumerable<TSource> source, int blockSize, Func<TSource, long> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return MovingSum(source.Select(S => selector(S)), blockSize);
    }

    /// <summary>
    /// Computes the moving variance in a sequence of a sequence of Nullable&lt;System.Int64&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">A sequence of Nullable&lt;System.Int64&gt; values to calculate the moving variance in a sequence on.</param>
    /// <param name="blockSize">The number of elements in the moving variance in a sequence block. Block size must be two or larger.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of Nullable&lt;System.Double&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    public static IEnumerable<long?> MovingSum<TSource>(this IEnumerable<TSource> source, int blockSize, Func<TSource, long?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return MovingSum(source.Select(S => selector(S)), blockSize);
    }

    /// <summary>
    /// Computes the moving variance in a sequence of a sequence of System.Int32 values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">A sequence of System.Int32 values to calculate the moving variance in a sequence on.</param>
    /// <param name="blockSize">The number of elements in the moving variance in a sequence block. Block size must be two or larger.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of System.Double values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    public static IEnumerable<int> MovingSum<TSource>(this IEnumerable<TSource> source, int blockSize, Func<TSource, int> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return MovingSum(source.Select(S => selector(S)), blockSize);
    }

    /// <summary>
    /// Computes the moving variance in a sequence of a sequence of Nullable&lt;System.Int32&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">A sequence of Nullable&lt;System.Int32&gt; values to calculate the moving variance in a sequence on.</param>
    /// <param name="blockSize">The number of elements in the moving variance in a sequence block. Block size must be two or larger.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of Nullable&lt;System.Double&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    public static IEnumerable<int?> MovingSum<TSource>(this IEnumerable<TSource> source, int blockSize, Func<TSource, int?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return MovingSum(source.Select(S => selector(S)), blockSize);
    }

    #endregion

    #region Standard Moving Average

    /// <summary>
    /// Computes the moving average of a sequence of System.Double values.
    /// </summary>
    /// <param name="source">A sequence of System.Double values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>
    /// <returns>A sequence of System.Double values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    public static IEnumerable<double> StandardMovingAverage(this IEnumerable<double> source, int blockSize)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (blockSize < 2)
        throw Error.ValueMinTwo("blockSize");

      double sum = 0;
      int block = blockSize;
      int nans = -1;

      using (IEnumerator<double> left = source.GetEnumerator())
      using (IEnumerator<double> right = source.GetEnumerator())
      {
        double value;
        while (block > 1)
        {
          block--;
          if (!right.MoveNext())
          {
            if (nans > 0)
              yield return double.NaN;
            else
              yield return sum / (blockSize - block - 1);
            yield break;
          }
          value = right.Current;
          if (double.IsNaN(value))
            nans = blockSize;
          else
          {
            sum += value;
            nans--;
          }
        }

        while (right.MoveNext())
        {
          value = right.Current;
          if (double.IsNaN(value))
            nans = blockSize;
          else
          {
            sum += value;
            nans--;
          }
          if (nans > 0)
            yield return double.NaN;
          else
            yield return sum / blockSize;

          left.MoveNext();
          value = left.Current;
          if (!double.IsNaN(value))
            sum -= value;
        }
      }
    }

    /// <summary>
    /// Computes the moving average of a sequence of Nullable&lt;System.Double&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Double&gt; values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>
    /// <returns>A sequence of Nullable&lt;System.Double&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    public static IEnumerable<double?> StandardMovingAverage(this IEnumerable<double?> source, int blockSize)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (blockSize < 2)
        throw Error.ValueMinTwo("blockSize");

      int block = blockSize;
      int elements = 0;
      int nans = -1;
      double sum = 0;
      using (IEnumerator<double?> left = source.GetEnumerator())
      using (IEnumerator<double?> right = source.GetEnumerator())
      {
        double? curr;
        while (block > 1)
        {
          block--;
          if (!right.MoveNext())
          {
            if (nans > 0)
              yield return double.NaN;
            else if (elements > 0)
              yield return sum / elements;
            else
              yield return null;
            yield break;
          }
          curr = right.Current;

          if (!curr.HasValue)
            continue;

          if (double.IsNaN(curr.Value))
            nans = blockSize;
          else
          {
            sum += curr.Value;
            nans--;
          }
          elements++;
        }

        while (right.MoveNext())
        {
          curr = right.Current;
          if (curr.HasValue)
          {
            if (double.IsNaN(curr.Value))
              nans = blockSize;
            else
            {
              sum += curr.Value;
              nans--;
            }
            elements++;
          }

          if (nans > 0)
            yield return double.NaN;
          else if (elements > 0)
            yield return sum / elements;
          else
            yield return null;
          left.MoveNext();
          curr = left.Current;
          if (!curr.HasValue)
            continue;

          if (!double.IsNaN(curr.Value))
            sum -= curr.Value;
          elements--;
        }
      }
    }

    /// <summary>
    /// Computes the moving average of a sequence of System.Singel values.
    /// </summary>
    /// <param name="source">A sequence of System.Singel values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>
    /// <returns>A sequence of System.Singel values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    public static IEnumerable<float> StandardMovingAverage(this IEnumerable<float> source, int blockSize)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (blockSize < 2)
        throw Error.ValueMinTwo("blockSize");

      float sum = 0;
      int block = blockSize;
      int nans = -1;

      using (IEnumerator<float> left = source.GetEnumerator())
      using (IEnumerator<float> right = source.GetEnumerator())
      {
        float value;
        while (block > 1)
        {
          block--;
          if (!right.MoveNext())
          {
            if (nans > 0)
              yield return float.NaN;
            else
              yield return sum / (blockSize - block - 1);
            yield break;
          }
          value = right.Current;
          if (float.IsNaN(value))
            nans = blockSize;
          else
          {
            sum += value;
            nans--;
          }
        }

        while (right.MoveNext())
        {
          value = right.Current;
          if (float.IsNaN(value))
            nans = blockSize;
          else
          {
            sum += value;
            nans--;
          }
          if (nans > 0)
            yield return float.NaN;
          else
            yield return sum / blockSize;

          left.MoveNext();
          value = left.Current;
          if (!float.IsNaN(value))
            sum -= value;
        }
      }
    }

    /// <summary>
    /// Computes the moving average of a sequence of Nullable&lt;System.Singel&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Singel&gt; values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>
    /// <returns>A sequence of Nullable&lt;System.Singel&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    public static IEnumerable<float?> StandardMovingAverage(this IEnumerable<float?> source, int blockSize)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (blockSize < 2)
        throw Error.ValueMinTwo("blockSize");

      int block = blockSize;
      int elements = 0;
      int nans = -1;
      float sum = 0;
      using (IEnumerator<float?> left = source.GetEnumerator())
      using (IEnumerator<float?> right = source.GetEnumerator())
      {
        float? curr;
        while (block > 1)
        {
          block--;
          if (!right.MoveNext())
          {
            if (nans > 0)
              yield return float.NaN;
            else if (elements > 0)
              yield return sum / elements;
            else
              yield return null;
            yield break;
          }
          curr = right.Current;

          if (!curr.HasValue)
            continue;

          if (float.IsNaN(curr.Value))
            nans = blockSize;
          else
          {
            sum += curr.Value;
            nans--;
          }
          elements++;
        }

        while (right.MoveNext())
        {
          curr = right.Current;
          if (curr.HasValue)
          {
            if (float.IsNaN(curr.Value))
              nans = blockSize;
            else
            {
              sum += curr.Value;
              nans--;
            }
            elements++;
          }

          if (nans > 0)
            yield return float.NaN;
          else if (elements > 0)
            yield return sum / elements;
          else
            yield return null;
          left.MoveNext();
          curr = left.Current;

          if (!curr.HasValue)
            continue;

          if (!float.IsNaN(curr.Value))
            sum -= curr.Value;
          elements--;
        }
      }
    }

    /// <summary>
    /// Computes the moving average of a sequence of System.Decimal values.
    /// </summary>
    /// <param name="source">A sequence of System.Decimal values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>
    /// <returns>A sequence of System.Decimal values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    public static IEnumerable<decimal> StandardMovingAverage(this IEnumerable<decimal> source, int blockSize)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (blockSize < 2)
        throw Error.ValueMinTwo("blockSize");

      int block = blockSize;
      decimal sum = 0;
      using (IEnumerator<decimal> left = source.GetEnumerator())
      using (IEnumerator<decimal> right = source.GetEnumerator())
      {
        while (block > 1)
        {
          block--;
          if (!right.MoveNext())
          {
            yield return sum / (blockSize - block - 1);
            yield break;
          }
          sum += right.Current;
        }

        while (right.MoveNext())
        {
          sum += right.Current;
          yield return sum / blockSize;
          left.MoveNext();
          sum -= left.Current;
        }
      }
    }

    /// <summary>
    /// Computes the moving average of a sequence of Nullable&lt;System.Decimal&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Decimal&gt; values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>
    /// <returns>A sequence of Nullable&lt;System.Decimal&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    public static IEnumerable<decimal?> StandardMovingAverage(this IEnumerable<decimal?> source, int blockSize)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (blockSize < 2)
        throw Error.ValueMinTwo("blockSize");

      int block = blockSize;
      int elements = 0;
      decimal sum = 0;
      using (IEnumerator<decimal?> left = source.GetEnumerator())
      using (IEnumerator<decimal?> right = source.GetEnumerator())
      {
        decimal? curr;
        while (block > 1)
        {
          block--;
          if (!right.MoveNext())
          {
            if (elements > 0)
              yield return sum / elements;
            else
              yield return null;
            yield break;
          }
          curr = right.Current;

          if (!curr.HasValue)
            continue;

          sum += curr.Value;
          elements++;
        }

        while (right.MoveNext())
        {
          curr = right.Current;
          if (curr.HasValue)
          {
            sum += curr.Value;
            elements++;
          }

          if (elements > 0)
            yield return sum / elements;
          else
            yield return null;
          left.MoveNext();
          curr = left.Current;

          if (!curr.HasValue)
            continue;

          sum -= curr.Value;
          elements--;
        }
      }
    }

    /// <summary>
    /// Computes the moving average of a sequence of System.Int64 values.
    /// </summary>
    /// <param name="source">A sequence of System.Int64 values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>
    /// <returns>A sequence of System.Double values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    public static IEnumerable<double> StandardMovingAverage(this IEnumerable<long> source, int blockSize)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (blockSize < 2)
        throw Error.ValueMinTwo("blockSize");

      int block = blockSize;
      long sum = 0;
      using (IEnumerator<long> left = source.GetEnumerator())
      using (IEnumerator<long> right = source.GetEnumerator())
      {
        while (block > 1)
        {
          block--;
          if (!right.MoveNext())
          {
            yield return (double)sum / (blockSize - block - 1);
            yield break;
          }
          sum += right.Current;
        }

        while (right.MoveNext())
        {
          sum += right.Current;
          yield return (double)sum / blockSize;
          left.MoveNext();
          sum -= left.Current;
        }
      }
    }

    /// <summary>
    /// Computes the moving average of a sequence of Nullable&lt;System.Int64&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Int64&gt; values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>
    /// <returns>A sequence of Nullable&lt;System.Double&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    public static IEnumerable<double?> StandardMovingAverage(this IEnumerable<long?> source, int blockSize)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (blockSize < 2)
        throw Error.ValueMinTwo("blockSize");

      int block = blockSize;
      int elements = 0;
      long sum = 0;
      using (IEnumerator<long?> left = source.GetEnumerator())
      using (IEnumerator<long?> right = source.GetEnumerator())
      {
        long? curr;
        while (block > 1)
        {
          block--;
          if (!right.MoveNext())
          {
            if (elements > 0)
              yield return (double)sum / elements;
            else
              yield return null;
            yield break;
          }
          curr = right.Current;

          if (!curr.HasValue)
            continue;

          sum += curr.Value;
          elements++;
        }

        while (right.MoveNext())
        {
          curr = right.Current;
          if (curr.HasValue)
          {
            sum += curr.Value;
            elements++;
          }

          if (elements > 0)
            yield return (double)sum / elements;
          else
            yield return null;
          left.MoveNext();
          curr = left.Current;

          if (!curr.HasValue)
            continue;

          sum -= curr.Value;
          elements--;
        }
      }
    }

    /// <summary>
    /// Computes the moving average of a sequence of System.Int32 values.
    /// </summary>
    /// <param name="source">A sequence of System.Int32 values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>
    /// <returns>A sequence of System.Double values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    public static IEnumerable<double> StandardMovingAverage(this IEnumerable<int> source, int blockSize)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (blockSize < 2)
        throw Error.ValueMinTwo("blockSize");

      int block = blockSize;
      int sum = 0;
      using (IEnumerator<int> left = source.GetEnumerator())
      using (IEnumerator<int> right = source.GetEnumerator())
      {
        while (block > 1)
        {
          block--;
          if (!right.MoveNext())
          {
            yield return (double)sum / (blockSize - block - 1);
            yield break;
          }
          sum += right.Current;
        }

        while (right.MoveNext())
        {
          sum += right.Current;
          yield return (double)sum / blockSize;
          left.MoveNext();
          sum -= left.Current;
        }
      }
    }

    /// <summary>
    /// Computes the moving average of a sequence of Nullable&lt;System.Int32&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Int32&gt; values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>
    /// <returns>A sequence of Nullable&lt;System.Double&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    public static IEnumerable<double?> StandardMovingAverage(this IEnumerable<int?> source, int blockSize)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (blockSize < 2)
        throw Error.ValueMinTwo("blockSize");

      int block = blockSize;
      int elements = 0;
      int sum = 0;
      using (IEnumerator<int?> left = source.GetEnumerator())
      using (IEnumerator<int?> right = source.GetEnumerator())
      {
        int? curr;
        while (block > 1)
        {
          block--;
          if (!right.MoveNext())
          {
            if (elements > 0)
              yield return (double)sum / elements;
            else
              yield return null;
            yield break;
          }
          curr = right.Current;

          if (!curr.HasValue)
            continue;

          sum += curr.Value;
          elements++;
        }

        while (right.MoveNext())
        {
          curr = right.Current;
          if (curr.HasValue)
          {
            sum += curr.Value;
            elements++;
          }

          if (elements > 0)
            yield return (double)sum / elements;
          else
            yield return null;
          left.MoveNext();
          curr = left.Current;

          if (!curr.HasValue)
            continue;

          sum -= curr.Value;
          elements--;
        }
      }
    }

    /// <summary>
    /// Computes the moving average of a sequence of System.Double values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">A sequence of System.Double values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of System.Double values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    public static IEnumerable<double> StandardMovingAverage<TSource>(this IEnumerable<TSource> source, int blockSize, Func<TSource, double> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return StandardMovingAverage(source.Select(S => selector(S)), blockSize);
    }

    /// <summary>
    /// Computes the moving average of a sequence of Nullable&lt;System.Double&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">A sequence of Nullable&lt;System.Double&gt; values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of Nullable&lt;System.Double&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    public static IEnumerable<double?> StandardMovingAverage<TSource>(this IEnumerable<TSource> source, int blockSize, Func<TSource, double?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return StandardMovingAverage(source.Select(S => selector(S)), blockSize);
    }

    /// <summary>
    /// Computes the moving average of a sequence of System.Single values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">A sequence of System.Single values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of System.Single values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    public static IEnumerable<float> StandardMovingAverage<TSource>(this IEnumerable<TSource> source, int blockSize, Func<TSource, float> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return StandardMovingAverage(source.Select(S => selector(S)), blockSize);
    }

    /// <summary>
    /// Computes the moving average of a sequence of Nullable&lt;System.Single&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">A sequence of Nullable&lt;System.Single&gt; values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of Nullable&lt;System.Single&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    public static IEnumerable<float?> StandardMovingAverage<TSource>(this IEnumerable<TSource> source, int blockSize, Func<TSource, float?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return StandardMovingAverage(source.Select(S => selector(S)), blockSize);
    }

    /// <summary>
    /// Computes the moving average of a sequence of System.Decimal values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">A sequence of System.Decimal values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of System.Decimal values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    public static IEnumerable<decimal> StandardMovingAverage<TSource>(this IEnumerable<TSource> source, int blockSize, Func<TSource, decimal> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return StandardMovingAverage(source.Select(S => selector(S)), blockSize);
    }

    /// <summary>
    /// Computes the moving average of a sequence of Nullable&lt;System.Decimal&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">A sequence of Nullable&lt;System.Decimal&gt; values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of Nullable&lt;System.Decimal&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    public static IEnumerable<decimal?> StandardMovingAverage<TSource>(this IEnumerable<TSource> source, int blockSize, Func<TSource, decimal?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return StandardMovingAverage(source.Select(S => selector(S)), blockSize);
    }

    /// <summary>
    /// Computes the moving average of a sequence of System.Int64 values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">A sequence of System.Int64 values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of System.Double values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    public static IEnumerable<double> StandardMovingAverage<TSource>(this IEnumerable<TSource> source, int blockSize, Func<TSource, long> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return StandardMovingAverage(source.Select(S => selector(S)), blockSize);
    }

    /// <summary>
    /// Computes the moving average of a sequence of Nullable&lt;System.Int64&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">A sequence of Nullable&lt;System.Int64&gt; values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of Nullable&lt;System.Double&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    public static IEnumerable<double?> StandardMovingAverage<TSource>(this IEnumerable<TSource> source, int blockSize, Func<TSource, long?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return StandardMovingAverage(source.Select(S => selector(S)), blockSize);
    }

    /// <summary>
    /// Computes the moving average of a sequence of System.Int32 values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">A sequence of System.Int32 values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of System.Double values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    public static IEnumerable<double> StandardMovingAverage<TSource>(this IEnumerable<TSource> source, int blockSize, Func<TSource, int> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return StandardMovingAverage(source.Select(S => selector(S)), blockSize);
    }

    /// <summary>
    /// Computes the moving average of a sequence of Nullable&lt;System.Int32&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">A sequence of Nullable&lt;System.Int32&gt; values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of Nullable&lt;System.Double&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    public static IEnumerable<double?> StandardMovingAverage<TSource>(this IEnumerable<TSource> source, int blockSize, Func<TSource, int?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return StandardMovingAverage(source.Select(S => selector(S)), blockSize);
    }

    #endregion

    #region Cumulative Moving Average

    /// <summary>
    /// Computes the cumulative moving average of a sequence of System.Double values.
    /// </summary>
    /// <param name="source">A sequence of System.Double values to calculate the moving average on.</param>
    /// <returns>A sequence of System.Double values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    public static IEnumerable<double> CumulativeMovingAverage(this IEnumerable<double> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      double sum = 0;
      int idx = 0;

      return source.Select(S => (sum += S) / ++idx);
    }

    /// <summary>
    /// Computes the cumulative moving average of a sequence of Nullable&lt;System.Double&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Double&gt; values to calculate the moving average on.</param>
    /// <returns>A sequence of Nullable&lt;System.Double&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    public static IEnumerable<double?> CumulativeMovingAverage(this IEnumerable<double?> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      double sum = 0;
      int idx = 0;

      return source.Where(S => S.HasValue).Select(S => (sum += S.Value) / ++idx).Cast<double?>();
    }

    /// <summary>
    /// Computes the cumulative moving average of a sequence of System.Singel values.
    /// </summary>
    /// <param name="source">A sequence of System.Singel values to calculate the moving average on.</param>
    /// <returns>A sequence of System.Singel values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    public static IEnumerable<float> CumulativeMovingAverage(this IEnumerable<float> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      float sum = 0;
      int idx = 0;

      return source.Select(S => (sum += S) / ++idx);
    }

    /// <summary>
    /// Computes the cumulative moving average of a sequence of Nullable&lt;System.Singel&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Singel&gt; values to calculate the moving average on.</param>
    /// <returns>A sequence of Nullable&lt;System.Singel&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    public static IEnumerable<float?> CumulativeMovingAverage(this IEnumerable<float?> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      float sum = 0;
      int idx = 0;

      return source.Where(S => S.HasValue).Select(S => (sum += S.Value) / ++idx).Cast<float?>();
    }

    /// <summary>
    /// Computes the cumulative moving average of a sequence of System.Decimal values.
    /// </summary>
    /// <param name="source">A sequence of System.Decimal values to calculate the moving average on.</param>    
    /// <returns>A sequence of System.Decimal values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    public static IEnumerable<decimal> CumulativeMovingAverage(this IEnumerable<decimal> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      decimal sum = 0;
      int idx = 0;

      return source.Select(S => (sum += S) / ++idx);
    }

    /// <summary>
    /// Computes the cumulative moving average of a sequence of Nullable&lt;System.Decimal&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Decimal&gt; values to calculate the moving average on.</param>
    /// <returns>A sequence of Nullable&lt;System.Decimal&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    public static IEnumerable<decimal?> CumulativeMovingAverage(this IEnumerable<decimal?> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      decimal sum = 0;
      int idx = 0;

      return source.Where(S => S.HasValue).Select(S => (sum += S.Value) / ++idx).Cast<decimal?>();
    }

    /// <summary>
    /// Computes the cumulative moving average of a sequence of System.Int64 values.
    /// </summary>
    /// <param name="source">A sequence of System.Int64 values to calculate the moving average on.</param>
    /// <returns>A sequence of System.Double values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    public static IEnumerable<double> CumulativeMovingAverage(this IEnumerable<long> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      double sum = 0;
      int idx = 0;

      return source.Select(ITEM => (sum += ITEM) / ++idx);
    }

    /// <summary>
    /// Computes the cumulative moving average of a sequence of Nullable&lt;System.Int64&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Int64&gt; values to calculate the moving average on.</param>
    /// <returns>A sequence of Nullable&lt;System.Double&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    public static IEnumerable<double?> CumulativeMovingAverage(this IEnumerable<long?> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      double sum = 0;
      int idx = 0;

      return source.Where(S => S.HasValue).Select(S => (sum += S.Value) / ++idx).Cast<double?>();
    }

    /// <summary>
    /// Computes the cumulative moving average of a sequence of System.Int32 values.
    /// </summary>
    /// <param name="source">A sequence of System.Int32 values to calculate the moving average on.</param>
    /// <returns>A sequence of System.Double values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    public static IEnumerable<double> CumulativeMovingAverage(this IEnumerable<int> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      double sum = 0;
      int idx = 0;

      return source.Select(S => (sum += S) / ++idx);
    }

    /// <summary>
    /// Computes the cumulative moving average of a sequence of Nullable&lt;System.Int32&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Int32&gt; values to calculate the moving average on.</param>
    /// <returns>A sequence of Nullable&lt;System.Double&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    public static IEnumerable<double?> CumulativeMovingAverage(this IEnumerable<int?> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      double sum = 0;
      int idx = 0;

      return source.Where(S => S.HasValue).Select(S => (sum += S.Value) / ++idx).Cast<double?>();

    }

    /// <summary>
    /// Computes the cumulative moving average of a sequence of System.Double values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">A sequence of System.Double values to calculate the moving average on.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of System.Double values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    public static IEnumerable<double> CumulativeMovingAverage<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return CumulativeMovingAverage(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the cumulative moving average of a sequence of Nullable&lt;System.Double&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">A sequence of Nullable&lt;System.Double&gt; values to calculate the moving average on.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of Nullable&lt;System.Double&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    public static IEnumerable<double?> CumulativeMovingAverage<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return CumulativeMovingAverage(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the cumulative moving average of a sequence of System.Single values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">A sequence of System.Single values to calculate the moving average on.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of System.Single values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    public static IEnumerable<float> CumulativeMovingAverage<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return CumulativeMovingAverage(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the cumulative moving average of a sequence of Nullable&lt;System.Single&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">A sequence of Nullable&lt;System.Single&gt; values to calculate the moving average on.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of Nullable&lt;System.Single&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    public static IEnumerable<float?> CumulativeMovingAverage<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return CumulativeMovingAverage(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the cumulative moving average of a sequence of System.Decimal values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">A sequence of System.Decimal values to calculate the moving average on.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of System.Decimal values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    public static IEnumerable<decimal> CumulativeMovingAverage<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return CumulativeMovingAverage(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the cumulative moving average of a sequence of Nullable&lt;System.Decimal&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">A sequence of Nullable&lt;System.Decimal&gt; values to calculate the moving average on.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of Nullable&lt;System.Decimal&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    public static IEnumerable<decimal?> CumulativeMovingAverage<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return CumulativeMovingAverage(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the cumulative moving average of a sequence of System.Int64 values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">A sequence of System.Int64 values to calculate the moving average on.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of System.Double values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    public static IEnumerable<double> CumulativeMovingAverage<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return CumulativeMovingAverage(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the cumulative moving average of a sequence of Nullable&lt;System.Int64&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">A sequence of Nullable&lt;System.Int64&gt; values to calculate the moving average on.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of Nullable&lt;System.Double&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    public static IEnumerable<double?> CumulativeMovingAverage<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return CumulativeMovingAverage(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the cumulative moving average of a sequence of System.Int32 values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">A sequence of System.Int32 values to calculate the moving average on.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of System.Double values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    public static IEnumerable<double> CumulativeMovingAverage<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return CumulativeMovingAverage(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the cumulative moving average of a sequence of Nullable&lt;System.Int32&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">A sequence of Nullable&lt;System.Int32&gt; values to calculate the moving average on.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of Nullable&lt;System.Double&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    public static IEnumerable<double?> CumulativeMovingAverage<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return CumulativeMovingAverage(source.Select(S => selector(S)));
    }

    #endregion

    #region  Weighted Moving Average

    /// <summary>
    /// Computes the weighted moving average of a sequence of System.Double values using lower weight for 'older' items. 
    /// </summary>
    /// <param name="source">A sequence of System.Double values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>
    /// <param name="weight">A function that compute the weight of an element based on its index in the block.</param>
    /// <returns>A sequence of System.Double values.</returns>
    /// <exception cref="System.ArgumentNullException">source or weight is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">source must be larger than block size.</exception>    
    public static IEnumerable<double> WeightedMovingAverage(this IEnumerable<double> source, int blockSize, Func<int, double> weight)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (weight == null)
        throw Error.ArgumentNull("weight");
      if (blockSize < 2)
        throw Error.ValueMinTwo("blockSize");

      Queue<double> current = new Queue<double>();
      double[] factor = new double[blockSize];

      using (IEnumerator<double> se = source.GetEnumerator())
      {
        for (int i = 0; i < blockSize - 1; i++)
        {
          if (!se.MoveNext())
            throw Error.ValueMinBlockSize("source");
          current.Enqueue(se.Current);
          factor[i] = weight(i + 1);
        }
        factor[blockSize - 1] = weight(blockSize);
        double factorSum = factor.Sum();

        while (se.MoveNext())
        {
          current.Enqueue(se.Current);
          yield return current.Multiply(factor).Sum() / factorSum;
          current.Dequeue();
        }
      }
    }

    /// <summary>
    /// Computes the weighted moving average of a sequence of Nullable&lt;System.Double&gt; values using lower weight for 'older' items.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Double&gt; values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>
    /// <param name="weight">A function that compute the weight of an element based on its index in the block.</param>
    /// <returns>A sequence of Nullable&lt;System.Double&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source or weight is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">source must be larger than block size.</exception>
    public static IEnumerable<double?> WeightedMovingAverage(this IEnumerable<double?> source, int blockSize, Func<int, double> weight)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (weight == null)
        throw Error.ArgumentNull("weight");
      if (blockSize < 2)
        throw Error.ValueMinTwo("blockSize");

      Queue<double?> current = new Queue<double?>();
      double?[] factor = new double?[blockSize];

      using (IEnumerator<double?> se = source.GetEnumerator())
      {
        for (int i = 0; i < blockSize - 1; i++)
        {
          if (!se.MoveNext())
            throw Error.ValueMinBlockSize("source");
          current.Enqueue(se.Current);
          factor[i] = weight(i + 1);
        }
        factor[blockSize - 1] = weight(blockSize);
        double? factorFullSum = factor.Sum();

        while (se.MoveNext())
        {
          current.Enqueue(se.Current);
          double? tempSum;
          if (current.All(V => !V.HasValue))
            tempSum = null;
          else if (current.Any(V => !V.HasValue))
          {
            IEnumerable<double?> map = current.Divide(current);
            tempSum = map.Multiply(factor).Sum();
          }
          else
            tempSum = factorFullSum;

          yield return current.Multiply(factor).Sum() / tempSum;
          current.Dequeue();
        }
      }
    }

    /// <summary>
    /// Computes the weighted moving average of a sequence of System.Single values using lower weight for 'older' items. 
    /// </summary>
    /// <param name="source">A sequence of System.Single values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>
    /// <param name="weight">A function that compute the weight of an element based on its index in the block.</param>
    /// <returns>A sequence of System.Single values.</returns>
    /// <exception cref="System.ArgumentNullException">source or weight is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">source must be larger than block size.</exception>
    public static IEnumerable<float> WeightedMovingAverage(this IEnumerable<float> source, int blockSize, Func<int, float> weight)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (weight == null)
        throw Error.ArgumentNull("weight");
      if (blockSize < 2)
        throw Error.ValueMinTwo("blockSize");

      Queue<float> current = new Queue<float>();
      float[] factor = new float[blockSize];

      using (IEnumerator<float> se = source.GetEnumerator())
      {
        for (int i = 0; i < blockSize - 1; i++)
        {
          if (!se.MoveNext())
            throw Error.ValueMinBlockSize("source");
          current.Enqueue(se.Current);
          factor[i] = weight(i + 1);
        }
        factor[blockSize - 1] = weight(blockSize);
        float factorSum = factor.Sum();

        while (se.MoveNext())
        {
          current.Enqueue(se.Current);
          yield return current.Multiply(factor).Sum() / factorSum;
          current.Dequeue();
        }
      }
    }

    /// <summary>
    /// Computes the weighted moving average of a sequence of Nullable&lt;System.Single&gt; values using lower weight for 'older' items.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Single&gt; values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>
    /// <param name="weight">A function that compute the weight of an element based on its index in the block.</param>
    /// <returns>A sequence of Nullable&lt;System.Single&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source or weight is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">source must be larger than block size.</exception>
    public static IEnumerable<float?> WeightedMovingAverage(this IEnumerable<float?> source, int blockSize, Func<int, float> weight)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (weight == null)
        throw Error.ArgumentNull("weight");
      if (blockSize < 2)
        throw Error.ValueMinTwo("blockSize");

      Queue<float?> current = new Queue<float?>();
      float?[] factor = new float?[blockSize];

      using (IEnumerator<float?> se = source.GetEnumerator())
      {
        for (int i = 0; i < blockSize - 1; i++)
        {
          if (!se.MoveNext())
            throw Error.ValueMinBlockSize("source");
          current.Enqueue(se.Current);
          factor[i] = weight(i + 1);
        }
        factor[blockSize - 1] = weight(blockSize);
        float? factorFullSum = factor.Sum();

        while (se.MoveNext())
        {
          current.Enqueue(se.Current);
          float? tempSum;
          if (current.All(V => !V.HasValue))
            tempSum = null;
          else if (current.Any(V => !V.HasValue))
          {
            IEnumerable<float?> map = current.Divide(current);
            tempSum = map.Multiply(factor).Sum();
          }
          else
            tempSum = factorFullSum;

          yield return current.Multiply(factor).Sum() / tempSum;
          current.Dequeue();
        }
      }
    }

    /// <summary>
    /// Computes the weighted moving average of a sequence of System.Decimal values using lower weight for 'older' items. 
    /// </summary>
    /// <param name="source">A sequence of System.Decimal values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>
    /// <param name="weight">A function that compute the weight of an element based on its index in the block.</param>
    /// <returns>A sequence of System.Decimal values.</returns>
    /// <exception cref="System.ArgumentNullException">source or weight is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">source must be larger than block size.</exception>
    public static IEnumerable<decimal> WeightedMovingAverage(this IEnumerable<decimal> source, int blockSize, Func<int, decimal> weight)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (weight == null)
        throw Error.ArgumentNull("weight");
      if (blockSize < 2)
        throw Error.ValueMinTwo("blockSize");

      Queue<decimal> current = new Queue<decimal>();
      decimal[] factor = new decimal[blockSize];

      using (IEnumerator<decimal> se = source.GetEnumerator())
      {
        for (int i = 0; i < blockSize - 1; i++)
        {
          if (!se.MoveNext())
            throw Error.ValueMinBlockSize("source");
          current.Enqueue(se.Current);
          factor[i] = weight(i + 1);
        }
        factor[blockSize - 1] = weight(blockSize);
        decimal factorSum = factor.Sum();

        while (se.MoveNext())
        {
          current.Enqueue(se.Current);
          yield return current.Multiply(factor).Sum() / factorSum;
          current.Dequeue();
        }
      }
    }

    /// <summary>
    /// Computes the weighted moving average of a sequence of Nullable&lt;System.Decimal&gt; values using lower weight for 'older' items.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Decimal&gt; values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>
    /// <param name="weight">A function that compute the weight of an element based on its index in the block.</param>
    /// <returns>A sequence of Nullable&lt;System.Decimal&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source or weight is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">source must be larger than block size.</exception>
    public static IEnumerable<decimal?> WeightedMovingAverage(this IEnumerable<decimal?> source, int blockSize, Func<int, decimal> weight)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (weight == null)
        throw Error.ArgumentNull("weight");
      if (blockSize < 2)
        throw Error.ValueMinTwo("blockSize");

      Queue<decimal?> current = new Queue<decimal?>();
      decimal?[] factor = new decimal?[blockSize];

      using (IEnumerator<decimal?> se = source.GetEnumerator())
      {
        for (int i = 0; i < blockSize - 1; i++)
        {
          if (!se.MoveNext())
            throw Error.ValueMinBlockSize("source");
          current.Enqueue(se.Current);
          factor[i] = weight(i + 1);
        }
        factor[blockSize - 1] = weight(blockSize);
        decimal? factorFullSum = factor.Sum();

        while (se.MoveNext())
        {
          current.Enqueue(se.Current);
          decimal? tempSum;
          if (current.All(V => !V.HasValue))
            tempSum = null;
          else if (current.Any(V => !V.HasValue))
          {
            IEnumerable<decimal?> map = current.Divide(current);
            tempSum = map.Multiply(factor).Sum();
          }
          else
            tempSum = factorFullSum;

          yield return current.Multiply(factor).Sum() / tempSum;
          current.Dequeue();
        }
      }
    }

    /// <summary>
    /// Computes the weighted moving average of a sequence of System.Int64 values using lower weight for 'older' items. 
    /// </summary>
    /// <param name="source">A sequence of System.Int64 values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>
    /// <param name="weight">A function that compute the weight of an element based on its index in the block.</param>
    /// <returns>A sequence of System.Double values.</returns>
    /// <exception cref="System.ArgumentNullException">source or weight is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">source must be larger than block size.</exception>
    public static IEnumerable<double> WeightedMovingAverage(this IEnumerable<long> source, int blockSize, Func<int, double> weight)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (weight == null)
        throw Error.ArgumentNull("weight");
      if (blockSize < 2)
        throw Error.ValueMinTwo("blockSize");

      Queue<double> current = new Queue<double>();
      double[] factor = new double[blockSize];

      using (IEnumerator<long> se = source.GetEnumerator())
      {
        for (int i = 0; i < blockSize - 1; i++)
        {
          if (!se.MoveNext())
            throw Error.ValueMinBlockSize("source");
          current.Enqueue(se.Current);
          factor[i] = weight(i + 1);
        }
        factor[blockSize - 1] = weight(blockSize);
        double factorSum = factor.Sum();

        while (se.MoveNext())
        {
          current.Enqueue(se.Current);
          yield return current.Multiply(factor).Sum() / factorSum;
          current.Dequeue();
        }
      }
    }

    /// <summary>
    /// Computes the weighted moving average of a sequence of Nullable&lt;System.Int64&gt; values using lower weight for 'older' items.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Int64&gt; values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>
    /// <param name="weight">A function that compute the weight of an element based on its index in the block.</param>
    /// <returns>A sequence of Nullable&lt;System.Double&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source or weight is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">source must be larger than block size.</exception>
    public static IEnumerable<double?> WeightedMovingAverage(this IEnumerable<long?> source, int blockSize, Func<int, double> weight)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (weight == null)
        throw Error.ArgumentNull("weight");
      if (blockSize < 2)
        throw Error.ValueMinTwo("blockSize");

      Queue<double?> current = new Queue<double?>();
      double?[] factor = new double?[blockSize];

      using (IEnumerator<long?> se = source.GetEnumerator())
      {
        for (int i = 0; i < blockSize - 1; i++)
        {
          if (!se.MoveNext())
            throw Error.ValueMinBlockSize("source");
          current.Enqueue(se.Current);
          factor[i] = weight(i + 1);
        }
        factor[blockSize - 1] = weight(blockSize);
        double? factorFullSum = factor.Sum();

        while (se.MoveNext())
        {
          current.Enqueue(se.Current);
          double? tempSum;
          if (current.All(V => !V.HasValue))
            tempSum = null;
          else if (current.Any(V => !V.HasValue))
          {
            IEnumerable<double?> map = current.Divide(current);
            tempSum = map.Multiply(factor).Sum();
          }
          else
            tempSum = factorFullSum;

          yield return current.Multiply(factor).Sum() / tempSum;
          current.Dequeue();
        }
      }
    }

    /// <summary>
    /// Computes the weighted moving average of a sequence of System.Int32 values using lower weight for 'older' items. 
    /// </summary>
    /// <param name="source">A sequence of System.Int32 values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>
    /// <param name="weight">A function that compute the weight of an element based on its index in the block.</param>
    /// <returns>A sequence of System.Double values.</returns>
    /// <exception cref="System.ArgumentNullException">source or weight is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">source must be larger than block size.</exception>
    public static IEnumerable<double> WeightedMovingAverage(this IEnumerable<int> source, int blockSize, Func<int, double> weight)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (weight == null)
        throw Error.ArgumentNull("weight");
      if (blockSize < 2)
        throw Error.ValueMinTwo("blockSize");

      Queue<double> current = new Queue<double>();
      double[] factor = new double[blockSize];

      using (IEnumerator<int> se = source.GetEnumerator())
      {
        for (int i = 0; i < blockSize - 1; i++)
        {
          if (!se.MoveNext())
            throw Error.ValueMinBlockSize("source");
          current.Enqueue(se.Current);
          factor[i] = weight(i + 1);
        }
        factor[blockSize - 1] = weight(blockSize);
        double factorSum = factor.Sum();

        while (se.MoveNext())
        {
          current.Enqueue(se.Current);
          yield return current.Multiply(factor).Sum() / factorSum;
          current.Dequeue();
        }
      }
    }

    /// <summary>
    /// Computes the weighted moving average of a sequence of Nullable&lt;System.Int32&gt; values using lower weight for 'older' items.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Int32&gt; values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>
    /// <param name="weight">A function that compute the weight of an element based on its index in the block.</param>
    /// <returns>A sequence of Nullable&lt;System.Double&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source or weight is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">source must be larger than block size.</exception>
    public static IEnumerable<double?> WeightedMovingAverage(this IEnumerable<int?> source, int blockSize, Func<int, double> weight)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (weight == null)
        throw Error.ArgumentNull("weight");
      if (blockSize < 2)
        throw Error.ValueMinTwo("blockSize");

      Queue<double?> current = new Queue<double?>();
      double?[] factor = new double?[blockSize];

      using (IEnumerator<int?> se = source.GetEnumerator())
      {
        for (int i = 0; i < blockSize - 1; i++)
        {
          if (!se.MoveNext())
            throw Error.ValueMinBlockSize("source");
          current.Enqueue(se.Current);
          factor[i] = weight(i + 1);
        }
        factor[blockSize - 1] = weight(blockSize);
        double? factorFullSum = factor.Sum();

        while (se.MoveNext())
        {
          current.Enqueue(se.Current);
          double? tempSum;
          if (current.All(V => !V.HasValue))
            tempSum = null;
          else if (current.Any(V => !V.HasValue))
          {
            IEnumerable<double?> map = current.Divide(current);
            tempSum = map.Multiply(factor).Sum();
          }
          else
            tempSum = factorFullSum;

          yield return current.Multiply(factor).Sum() / tempSum;
          current.Dequeue();
        }
      }
    }

    /// <summary>
    /// Computes the weighted moving average of a sequence of System.Double values using lower weight for 'older' items. 
    /// </summary>
    /// <param name="source">A sequence of System.Double values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>    
    /// <returns>A sequence of System.Double values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">source must be larger than block size.</exception>
    /// <remarks>Elements position within the block is used as weight. In a block of for items, the first (oldest) element will have the 1/4 of the weight of the last element.</remarks>
    public static IEnumerable<double> WeightedMovingAverage(this IEnumerable<double> source, int blockSize)
    {
      return WeightedMovingAverage(source, blockSize, X => X);
    }

    /// <summary>
    /// Computes the weighted moving average of a sequence of Nullable&lt;System.Double&gt; values using lower weight for 'older' items.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Double&gt; values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>    
    /// <returns>A sequence of Nullable&lt;System.Double&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">source must be larger than block size.</exception>
    /// <remarks>Elements position within the block is used as weight. In a block of for items, the first (oldest) element will have the 1/4 of the weight of the last element.</remarks>
    public static IEnumerable<double?> WeightedMovingAverage(this IEnumerable<double?> source, int blockSize)
    {
      return WeightedMovingAverage(source, blockSize, X => X);
    }

    /// <summary>
    /// Computes the weighted moving average of a sequence of System.Single values using lower weight for 'older' items. 
    /// </summary>
    /// <param name="source">A sequence of System.Single values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>    
    /// <returns>A sequence of System.Single values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">source must be larger than block size.</exception>
    /// <remarks>Elements position within the block is used as weight. In a block of for items, the first (oldest) element will have the 1/4 of the weight of the last element.</remarks>
    public static IEnumerable<float> WeightedMovingAverage(this IEnumerable<float> source, int blockSize)
    {
      return WeightedMovingAverage(source, blockSize, X => X);
    }

    /// <summary>
    /// Computes the weighted moving average of a sequence of Nullable&lt;System.Single&gt; values using lower weight for 'older' items.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Single&gt; values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>    
    /// <returns>A sequence of Nullable&lt;System.Single&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">source must be larger than block size.</exception>
    /// <remarks>Elements position within the block is used as weight. In a block of for items, the first (oldest) element will have the 1/4 of the weight of the last element.</remarks>
    public static IEnumerable<float?> WeightedMovingAverage(this IEnumerable<float?> source, int blockSize)
    {
      return WeightedMovingAverage(source, blockSize, X => X);
    }

    /// <summary>
    /// Computes the weighted moving average of a sequence of System.Decimal values using lower weight for 'older' items. 
    /// </summary>
    /// <param name="source">A sequence of System.Decimal values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>    
    /// <returns>A sequence of System.Decimal values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">source must be larger than block size.</exception>
    /// <remarks>Elements position within the block is used as weight. In a block of for items, the first (oldest) element will have the 1/4 of the weight of the last element.</remarks>
    public static IEnumerable<decimal> WeightedMovingAverage(this IEnumerable<decimal> source, int blockSize)
    {
      return WeightedMovingAverage(source, blockSize, X => X);
    }

    /// <summary>
    /// Computes the weighted moving average of a sequence of Nullable&lt;System.Decimal&gt; values using lower weight for 'older' items.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Decimal&gt; values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>    
    /// <returns>A sequence of Nullable&lt;System.Decimal&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">source must be larger than block size.</exception>
    /// <remarks>Elements position within the block is used as weight. In a block of for items, the first (oldest) element will have the 1/4 of the weight of the last element.</remarks>
    public static IEnumerable<decimal?> WeightedMovingAverage(this IEnumerable<decimal?> source, int blockSize)
    {
      return WeightedMovingAverage(source, blockSize, X => X);
    }

    /// <summary>
    /// Computes the weighted moving average of a sequence of System.Int64 values using lower weight for 'older' items. 
    /// </summary>
    /// <param name="source">A sequence of System.Int64 values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>    
    /// <returns>A sequence of System.Double values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">source must be larger than block size.</exception>
    /// <remarks>Elements position within the block is used as weight. In a block of for items, the first (oldest) element will have the 1/4 of the weight of the last element.</remarks>
    public static IEnumerable<double> WeightedMovingAverage(this IEnumerable<long> source, int blockSize)
    {
      return WeightedMovingAverage(source, blockSize, X => X);
    }

    /// <summary>
    /// Computes the weighted moving average of a sequence of Nullable&lt;System.Int64&gt; values using lower weight for 'older' items.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Int64&gt; values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>    
    /// <returns>A sequence of Nullable&lt;System.Double&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">source must be larger than block size.</exception>
    /// <remarks>Elements position within the block is used as weight. In a block of for items, the first (oldest) element will have the 1/4 of the weight of the last element.</remarks>
    public static IEnumerable<double?> WeightedMovingAverage(this IEnumerable<long?> source, int blockSize)
    {
      return WeightedMovingAverage(source, blockSize, X => X);
    }

    /// <summary>
    /// Computes the weighted moving average of a sequence of System.Int32 values using lower weight for 'older' items. 
    /// </summary>
    /// <param name="source">A sequence of System.Int32 values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>    
    /// <returns>A sequence of System.Double values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">source must be larger than block size.</exception>
    /// <remarks>Elements position within the block is used as weight. In a block of for items, the first (oldest) element will have the 1/4 of the weight of the last element.</remarks>
    public static IEnumerable<double> WeightedMovingAverage(this IEnumerable<int> source, int blockSize)
    {
      return WeightedMovingAverage(source, blockSize, X => X);
    }

    /// <summary>
    /// Computes the weighted moving average of a sequence of Nullable&lt;System.Int32&gt; values using lower weight for 'older' items.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Int32&gt; values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>    
    /// <returns>A sequence of Nullable&lt;System.Double&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">source must be larger than block size.</exception>
    /// <remarks>Elements position within the block is used as weight. In a block of for items, the first (oldest) element will have the 1/4 of the weight of the last element.</remarks>
    public static IEnumerable<double?> WeightedMovingAverage(this IEnumerable<int?> source, int blockSize)
    {
      return WeightedMovingAverage(source, blockSize, X => X);
    }

    /// <summary> 
    /// Computes the weighted moving average of a sequence of System.Double values that are obtained by invoking a transform function on each element of the input sequence using lower weight for 'older' items. 
    /// </summary>
    /// <param name="source">A sequence of System.Double values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>
    /// <param name="weight">A function that compute the weight of an element based on its index in the block.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of System.Double values.</returns>
    /// <exception cref="System.ArgumentNullException">source or weight or selector is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">source must be larger than block size.</exception>
    public static IEnumerable<double> WeightedMovingAverage<TSource>(this IEnumerable<TSource> source, int blockSize, Func<int, double> weight, Func<TSource, double> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return source.Select(S => selector(S)).WeightedMovingAverage(blockSize, weight);
    }

    /// <summary>
    /// Computes the weighted moving average of a sequence of Nullable&lt;System.Double&gt; values that are obtained by invoking a transform function on each element of the input sequence using lower weight for 'older' items.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Double&gt; values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>
    /// <param name="weight">A function that compute the weight of an element based on its index in the block.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of Nullable&lt;System.Double&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source or weight or selector is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>    
    /// <exception cref="System.ArgumentException">source must be larger than block size.</exception>
    public static IEnumerable<double?> WeightedMovingAverage<TSource>(this IEnumerable<TSource> source, int blockSize, Func<int, double> weight, Func<TSource, double?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return source.Select(S => selector(S)).WeightedMovingAverage(blockSize, weight);
    }

    /// <summary> 
    /// Computes the weighted moving average of a sequence of System.Single values that are obtained by invoking a transform function on each element of the input sequence using lower weight for 'older' items. 
    /// </summary>
    /// <param name="source">A sequence of System.Single values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>
    /// <param name="weight">A function that compute the weight of an element based on its index in the block.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of System.Single values.</returns>
    /// <exception cref="System.ArgumentNullException">source or weight or selector is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">source must be larger than block size.</exception>
    public static IEnumerable<float> WeightedMovingAverage<TSource>(this IEnumerable<TSource> source, int blockSize, Func<int, float> weight, Func<TSource, float> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return source.Select(S => selector(S)).WeightedMovingAverage(blockSize, weight);
    }

    /// <summary>
    /// Computes the weighted moving average of a sequence of Nullable&lt;System.Single&gt; values that are obtained by invoking a transform function on each element of the input sequence using lower weight for 'older' items.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Single&gt; values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>
    /// <param name="weight">A function that compute the weight of an element based on its index in the block.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of Nullable&lt;System.Single&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source or weight or selector is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>    
    /// <exception cref="System.ArgumentException">source must be larger than block size.</exception>
    public static IEnumerable<float?> WeightedMovingAverage<TSource>(this IEnumerable<TSource> source, int blockSize, Func<int, float> weight, Func<TSource, float?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return source.Select(S => selector(S)).WeightedMovingAverage(blockSize, weight);
    }

    /// <summary> 
    /// Computes the weighted moving average of a sequence of System.Decimal values that are obtained by invoking a transform function on each element of the input sequence using lower weight for 'older' items. 
    /// </summary>
    /// <param name="source">A sequence of System.Decimal values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>
    /// <param name="weight">A function that compute the weight of an element based on its index in the block.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of System.Decimal values.</returns>
    /// <exception cref="System.ArgumentNullException">source or weight or selector is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">source must be larger than block size.</exception>
    public static IEnumerable<decimal> WeightedMovingAverage<TSource>(this IEnumerable<TSource> source, int blockSize, Func<int, decimal> weight, Func<TSource, decimal> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return source.Select(S => selector(S)).WeightedMovingAverage(blockSize, weight);
    }

    /// <summary>
    /// Computes the weighted moving average of a sequence of Nullable&lt;System.Decimal&gt; values that are obtained by invoking a transform function on each element of the input sequence using lower weight for 'older' items.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Decimal&gt; values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>
    /// <param name="weight">A function that compute the weight of an element based on its index in the block.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of Nullable&lt;System.Decimal&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source or weight or selector is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>    
    /// <exception cref="System.ArgumentException">source must be larger than block size.</exception>
    public static IEnumerable<decimal?> WeightedMovingAverage<TSource>(this IEnumerable<TSource> source, int blockSize, Func<int, decimal> weight, Func<TSource, decimal?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return source.Select(S => selector(S)).WeightedMovingAverage(blockSize, weight);
    }

    /// <summary> 
    /// Computes the weighted moving average of a sequence of System.Int64 values that are obtained by invoking a transform function on each element of the input sequence using lower weight for 'older' items. 
    /// </summary>
    /// <param name="source">A sequence of System.Int64 values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>
    /// <param name="weight">A function that compute the weight of an element based on its index in the block.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of System.Double values.</returns>
    /// <exception cref="System.ArgumentNullException">source or weight or selector is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">source must be larger than block size.</exception>
    public static IEnumerable<double> WeightedMovingAverage<TSource>(this IEnumerable<TSource> source, int blockSize, Func<int, double> weight, Func<TSource, long> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return source.Select(S => selector(S)).WeightedMovingAverage(blockSize, weight);
    }

    /// <summary>
    /// Computes the weighted moving average of a sequence of Nullable&lt;System.Int64&gt; values that are obtained by invoking a transform function on each element of the input sequence using lower weight for 'older' items.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Int64&gt; values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>
    /// <param name="weight">A function that compute the weight of an element based on its index in the block.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of Nullable&lt;System.Double&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source or weight or selector is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>    
    /// <exception cref="System.ArgumentException">source must be larger than block size.</exception>
    public static IEnumerable<double?> WeightedMovingAverage<TSource>(this IEnumerable<TSource> source, int blockSize, Func<int, double> weight, Func<TSource, long?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return source.Select(S => selector(S)).WeightedMovingAverage(blockSize, weight);
    }

    /// <summary> 
    /// Computes the weighted moving average of a sequence of System.Int32 values that are obtained by invoking a transform function on each element of the input sequence using lower weight for 'older' items. 
    /// </summary>
    /// <param name="source">A sequence of System.Int32 values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>
    /// <param name="weight">A function that compute the weight of an element based on its index in the block.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of System.Double values.</returns>
    /// <exception cref="System.ArgumentNullException">source or weight or selector is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">source must be larger than block size.</exception>
    public static IEnumerable<double> WeightedMovingAverage<TSource>(this IEnumerable<TSource> source, int blockSize, Func<int, double> weight, Func<TSource, int> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return source.Select(S => selector(S)).WeightedMovingAverage(blockSize, weight);
    }

    /// <summary>
    /// Computes the weighted moving average of a sequence of Nullable&lt;System.Int32&gt; values that are obtained by invoking a transform function on each element of the input sequence using lower weight for 'older' items.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Int32&gt; values to calculate the moving average on.</param>
    /// <param name="blockSize">The number of elements in the moving average block. Block size must be two or larger.</param>
    /// <param name="weight">A function that compute the weight of an element based on its index in the block.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of Nullable&lt;System.Double&gt; values.</returns>
    /// <exception cref="System.ArgumentNullException">source or weight or selector is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>    
    /// <exception cref="System.ArgumentException">source must be larger than block size.</exception>
    public static IEnumerable<double?> WeightedMovingAverage<TSource>(this IEnumerable<TSource> source, int blockSize, Func<int, double> weight, Func<TSource, int?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return source.Select(S => selector(S)).WeightedMovingAverage(blockSize, weight);
    }

    #endregion

    #region Variance

    /// <summary>
    /// Computes the variance in a sequence of System.Double values.
    /// </summary>
    /// <param name="source">A sequence of System.Double values to calculate the variance of.</param>
    /// <returns>The variance in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static double Variance(this IEnumerable<double> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      double[] sourceArr = source.ToArray();
      int itemsCount = sourceArr.Length;

      if (itemsCount < 2)
        throw Error.SequenceMinTwo("source");

      double sum = 0;
      double sumSqr = 0;


      for (int i = 0; i < itemsCount; i++)
      {
        sum += sourceArr[i];
        sumSqr += sourceArr[i] * sourceArr[i];
      }
      return (sumSqr - sum * (sum / itemsCount)) / (itemsCount - 1);
    }

    /// <summary>
    /// Computes the variance in a sequence of Nullable&lt;System.Double&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Double&gt; values to calculate the variance of.</param>
    /// <returns>The variance in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static double? Variance(this IEnumerable<double?> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      return source.Where(V => V.HasValue).Select(V => V.Value).Variance();
    }

    /// <summary>
    /// Computes the variance in a sequence of System.Single values.
    /// </summary>
    /// <param name="source">A sequence of System.Single values to calculate the variance of.</param>
    /// <returns>The variance in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static float Variance(this IEnumerable<float> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      float[] sourceArr = source.ToArray();
      int itemsCount = sourceArr.Length;

      if (itemsCount < 2)
        throw Error.SequenceMinTwo("source");

      float sum = 0;
      float sumSqr = 0;


      for (int i = 0; i < itemsCount; i++)
      {
        sum += sourceArr[i];
        sumSqr += sourceArr[i] * sourceArr[i];
      }
      return (sumSqr - sum * (sum / itemsCount)) / (itemsCount - 1);
    }

    /// <summary>
    /// Computes the variance in a sequence of Nullable&lt;System.Single&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Single&gt; values to calculate the variance of.</param>
    /// <returns>The variance in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static float? Variance(this IEnumerable<float?> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      return source.Where(V => V.HasValue).Select(V => V.Value).Variance();
    }

    /// <summary>
    /// Computes the variance in a sequence of System.Decimal values.
    /// </summary>
    /// <param name="source">A sequence of System.Decimal values to calculate the variance of.</param>
    /// <returns>The variance in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static decimal Variance(this IEnumerable<decimal> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      decimal[] sourceArr = source.ToArray();
      int itemsCount = sourceArr.Length;

      if (itemsCount < 2)
        throw Error.SequenceMinTwo("source");

      decimal sum = 0;
      decimal sumSqr = 0;


      for (int i = 0; i < itemsCount; i++)
      {
        sum += sourceArr[i];
        sumSqr += sourceArr[i] * sourceArr[i];
      }
      return (sumSqr - sum * (sum / itemsCount)) / (itemsCount - 1);
    }

    /// <summary>
    /// Computes the variance in a sequence of Nullable&lt;System.Decimal&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Decimal&gt; values to calculate the variance of.</param>
    /// <returns>The variance in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static decimal? Variance(this IEnumerable<decimal?> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      return source.Where(V => V.HasValue).Select(V => V.Value).Variance();
    }

    /// <summary>
    /// Computes the variance in a sequence of System.Int64 values.
    /// </summary>
    /// <param name="source">A sequence of System.Int64 values to calculate the variance of.</param>
    /// <returns>The variance in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static double Variance(this IEnumerable<long> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      long[] sourceArr = source.ToArray();
      int itemsCount = sourceArr.Length;

      if (itemsCount < 2)
        throw Error.SequenceMinTwo("source");

      double sum = 0;
      double sumSqr = 0;


      for (int i = 0; i < itemsCount; i++)
      {
        sum += sourceArr[i];
        sumSqr += sourceArr[i] * sourceArr[i];
      }
      return (sumSqr - sum * (sum / itemsCount)) / (itemsCount - 1);
    }

    /// <summary>
    /// Computes the variance in a sequence of Nullable&lt;System.Int64&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Int64&gt; values to calculate the variance of.</param>
    /// <returns>The variance in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static double? Variance(this IEnumerable<long?> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      return source.Where(V => V.HasValue).Select(V => V.Value).Variance();
    }

    /// <summary>
    /// Computes the variance in a sequence of System.Int32 values.
    /// </summary>
    /// <param name="source">A sequence of System.Int32 values to calculate the variance of.</param>
    /// <returns>The variance in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static double Variance(this IEnumerable<int> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      int[] sourceArr = source.ToArray();
      int itemsCount = sourceArr.Length;

      if (itemsCount < 2)
        throw Error.SequenceMinTwo("source");

      double sum = 0;
      double sumSqr = 0;

      for (int i = 0; i < itemsCount; i++)
      {
        sum += sourceArr[i];
        sumSqr += sourceArr[i] * sourceArr[i];
      }
      return (sumSqr - sum * (sum / itemsCount)) / (itemsCount - 1);
    }

    /// <summary>
    /// Computes the variance in a sequence of Nullable&lt;System.Int32&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Int32&gt; values to calculate the variance of.</param>
    /// <returns>The variance in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static double? Variance(this IEnumerable<int?> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      return source.Where(V => V.HasValue).Select(V => V.Value).Variance();
    }

    /// <summary>
    /// Computes the variance in a sequence of System.Double values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of System.Double values to calculate the variance of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>The variance in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static double Variance<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return Variance(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the variance in a sequence of Nullable&lt;System.Double&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Double&gt; values to calculate the variance of.</param>
    /// <returns>The variance in the sequence of values.</returns>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static double? Variance<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return Variance(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the variance in a sequence of System.Single values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of System.Single values to calculate the variance of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>The variance in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static float Variance<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return Variance(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the variance in a sequence of Nullable&lt;System.Single&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Single&gt; values to calculate the variance of.</param>
    /// <returns>The variance in the sequence of values.</returns>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static float? Variance<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return Variance(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the variance in a sequence of System.Decimal values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of System.Decimal values to calculate the variance of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>The variance in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static decimal Variance<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return Variance(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the variance in a sequence of Nullable&lt;System.Decimal&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Decimal&gt; values to calculate the variance of.</param>
    /// <returns>The variance in the sequence of values.</returns>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static decimal? Variance<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return Variance(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the variance in a sequence of System.Int64 values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of System.Int64 values to calculate the variance of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>The variance in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static double Variance<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return Variance(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the variance in a sequence of Nullable&lt;System.Int64&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Int64&gt; values to calculate the variance of.</param>
    /// <returns>The variance in the sequence of values.</returns>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static double? Variance<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return Variance(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the variance in a sequence of System.Int32 values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of System.Int32 values to calculate the variance of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>The variance in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static double Variance<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return Variance(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the variance in a sequence of Nullable&lt;System.Int32&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Int32&gt; values to calculate the variance of.</param>
    /// <returns>The variance in the sequence of values.</returns>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static double? Variance<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return Variance(source.Select(S => selector(S)));
    }

    #endregion

    #region Variance (Cumulative)

    /// <summary>
    /// Computes the cumulative variance in a sequence of System.Double values.
    /// </summary>
    /// <param name="source">A sequence of System.Double values to calculate the variance of.</param>
    /// <returns>A sequence of values that are the computed variance for the first 2,3,4,5....N elements.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double> CumulativeVariance(this IEnumerable<double> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      double[] sourceArr = source.ToArray();
      int arrLen = sourceArr.Length;
      if (arrLen < 2)
        throw Error.SequenceMinTwo("source");

      double sum = sourceArr[0];
      double sumSqr = sum * sum;

      for (int i = 1; i < arrLen; i++)
      {
        sum += sourceArr[i];
        sumSqr += sourceArr[i] * sourceArr[i];
        yield return (sumSqr - sum * (sum / (i + 1))) / i;
      }
    }

    /// <summary>
    /// Computes the cumulative variance in a sequence of Nullable&lt;System.Double&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Double&gt; values to calculate the variance of.</param>
    /// <returns>A sequence of values that are the computed variance for the first 2,3,4,5....N elements.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double?> CumulativeVariance(this IEnumerable<double?> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      IEnumerable<double?> sourceArr = source.ToArray();
      if (sourceArr.Where(V => V.HasValue).Take(2).Count() < 2)
        throw Error.SequenceMinTwo("source");

      using (IEnumerator<double?> src = sourceArr.GetEnumerator())
        if (src.MoveNext())
        {
          double sum = src.Current.GetValueOrDefault();
          double sumSqr = sum * sum;
          int i = src.Current.HasValue ? 1 : 0;
          while (src.MoveNext())
          {
            if (src.Current.HasValue)
            {
              double curr = src.Current.Value;
              i++;
              sum += curr;
              sumSqr += curr * curr;
            }
            yield return (sumSqr - sum * (sum / i)) / (i - 1);
          }
        }
    }

    /// <summary>
    /// Computes the cumulative variance in a sequence of System.Single values.
    /// </summary>
    /// <param name="source">A sequence of System.Single values to calculate the variance of.</param>
    /// <returns>A sequence of values that are the computed variance for the first 2,3,4,5....N elements.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<float> CumulativeVariance(this IEnumerable<float> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      float[] sourceArr = source.ToArray();
      int arrLen = sourceArr.Length;
      if (arrLen < 2)
        throw Error.SequenceMinTwo("source");

      float sum = sourceArr[0];
      float sumSqr = sum * sum;

      for (int i = 1; i < arrLen; i++)
      {
        sum += sourceArr[i];
        sumSqr += sourceArr[i] * sourceArr[i];
        yield return (sumSqr - sum * (sum / (i + 1))) / i;
      }
    }

    /// <summary>
    /// Computes the cumulative variance in a sequence of Nullable&lt;System.Single&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Single&gt; values to calculate the variance of.</param>
    /// <returns>A sequence of values that are the computed variance for the first 2,3,4,5....N elements.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<float?> CumulativeVariance(this IEnumerable<float?> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      IEnumerable<float?> sourceArr = source.ToArray();
      if (sourceArr.Where(V => V.HasValue).Take(2).Count() < 2)
        throw Error.SequenceMinTwo("source");

      using (IEnumerator<float?> src = sourceArr.GetEnumerator())
        if (src.MoveNext())
        {
          float sum = src.Current.GetValueOrDefault();
          float sumSqr = sum * sum;
          int i = src.Current.HasValue ? 1 : 0;
          while (src.MoveNext())
          {
            if (src.Current.HasValue)
            {
              float curr = src.Current.Value;
              i++;
              sum += curr;
              sumSqr += curr * curr;
            }
            yield return (sumSqr - sum * (sum / i)) / (i - 1);
          }
        }
    }

    /// <summary>
    /// Computes the cumulative variance in a sequence of System.Decimal values.
    /// </summary>
    /// <param name="source">A sequence of System.Decimal values to calculate the variance of.</param>
    /// <returns>A sequence of values that are the computed variance for the first 2,3,4,5....N elements.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<decimal> CumulativeVariance(this IEnumerable<decimal> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      decimal[] sourceArr = source.ToArray();
      int arrLen = sourceArr.Length;
      if (arrLen < 2)
        throw Error.SequenceMinTwo("source");

      decimal sum = sourceArr[0];
      decimal sumSqr = sum * sum;

      for (int i = 1; i < arrLen; i++)
      {
        sum += sourceArr[i];
        sumSqr += sourceArr[i] * sourceArr[i];
        yield return (sumSqr - sum * (sum / (i + 1))) / i;
      }
    }

    /// <summary>
    /// Computes the cumulative variance in a sequence of Nullable&lt;System.Decimal&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Decimal&gt; values to calculate the variance of.</param>
    /// <returns>A sequence of values that are the computed variance for the first 2,3,4,5....N elements.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<decimal?> CumulativeVariance(this IEnumerable<decimal?> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      IEnumerable<decimal?> sourceArr = source.ToArray();
      if (sourceArr.Where(V => V.HasValue).Take(2).Count() < 2)
        throw Error.SequenceMinTwo("source");

      using (IEnumerator<decimal?> src = sourceArr.GetEnumerator())
        if (src.MoveNext())
        {
          decimal sum = src.Current.GetValueOrDefault();
          decimal sumSqr = sum * sum;
          int i = src.Current.HasValue ? 1 : 0;
          while (src.MoveNext())
          {

            if (src.Current.HasValue)
            {
              decimal curr = src.Current.Value;
              i++;
              sum += curr;
              sumSqr += curr * curr;
            }
            if (i <= 1)
              throw Error.CumulativeVarianceZeroMembers();

            yield return (sumSqr - sum * (sum / i)) / (i - 1);
          }
        }
    }

    /// <summary>
    /// Computes the cumulative variance in a sequence of System.Int64 values.
    /// </summary>
    /// <param name="source">A sequence of System.Int64 values to calculate the variance of.</param>
    /// <returns>A sequence of values that are the computed variance for the first 2,3,4,5....N elements.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double> CumulativeVariance(this IEnumerable<long> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      long[] sourceArr = source.ToArray();
      int arrLen = sourceArr.Length;
      if (arrLen < 2)
        throw Error.SequenceMinTwo("source");

      double sum = sourceArr[0];
      double sumSqr = sum * sum;

      for (int i = 1; i < arrLen; i++)
      {
        sum += sourceArr[i];
        sumSqr += sourceArr[i] * sourceArr[i];
        yield return (sumSqr - sum * (sum / (i + 1))) / i;
      }
    }

    /// <summary>
    /// Computes the cumulative variance in a sequence of Nullable&lt;System.Int64&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Int64&gt; values to calculate the variance of.</param>
    /// <returns>A sequence of values that are the computed variance for the first 2,3,4,5....N elements.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double?> CumulativeVariance(this IEnumerable<long?> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      IEnumerable<long?> sourceArr = source.ToArray();
      if (sourceArr.Where(V => V.HasValue).Take(2).Count() < 2)
        throw Error.SequenceMinTwo("source");

      using (IEnumerator<long?> src = sourceArr.GetEnumerator())
        if (src.MoveNext())
        {
          double sum = src.Current.GetValueOrDefault();
          double sumSqr = sum * sum;
          int i = src.Current.HasValue ? 1 : 0;
          while (src.MoveNext())
          {
            if (src.Current.HasValue)
            {
              long curr = src.Current.Value;
              i++;
              sum += curr;
              sumSqr += curr * curr;
            }
            if (i <= 1)
              throw Error.CumulativeVarianceZeroMembers();

            yield return (sumSqr - sum * (sum / i)) / (i - 1);
          }
        }
    }

    /// <summary>
    /// Computes the cumulative variance in a sequence of System.Int32 values.
    /// </summary>
    /// <param name="source">A sequence of System.Int32 values to calculate the variance of.</param>
    /// <returns>A sequence of values that are the computed variance for the first 2,3,4,5....N elements.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double> CumulativeVariance(this IEnumerable<int> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      int[] sourceArr = source.ToArray();
      int arrLen = sourceArr.Length;
      if (arrLen < 2)
        throw Error.SequenceMinTwo("source");

      double sum = sourceArr[0];
      double sumSqr = sum * sum;

      for (int i = 1; i < arrLen; i++)
      {
        sum += sourceArr[i];
        sumSqr += sourceArr[i] * sourceArr[i];
        yield return (sumSqr - sum * (sum / (i + 1))) / i;
      }
    }

    /// <summary>
    /// Computes the cumulative variance in a sequence of Nullable&lt;System.Int32&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Int32&gt; values to calculate the variance of.</param>
    /// <returns>A sequence of values that are the computed variance for the first 2,3,4,5....N elements.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double?> CumulativeVariance(this IEnumerable<int?> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      IEnumerable<int?> sourceArr = source.ToArray();
      if (sourceArr.Where(V => V.HasValue).Take(2).Count() < 2)
        throw Error.SequenceMinTwo("source");

      using (IEnumerator<int?> src = sourceArr.GetEnumerator())
        if (src.MoveNext())
        {
          double sum = src.Current.GetValueOrDefault();
          double sumSqr = sum * sum;
          int i = src.Current.HasValue ? 1 : 0;
          while (src.MoveNext())
          {
            if (src.Current.HasValue)
            {
              double curr = src.Current.Value;
              i++;
              sum += curr;
              sumSqr += curr * curr;
            }
            if (i <= 1)
              throw Error.CumulativeVarianceZeroMembers();

            yield return (sumSqr - sum * (sum / i)) / (i - 1);
          }
        }
    }

    /// <summary>
    /// Computes the cumulative variance in a sequence of System.Double values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of System.Double values to calculate the variance of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of values that are the computed variance for the first 2,3,4,5....N elements.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double> CumulativeVariance<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return CumulativeVariance(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the cumulative variance in a sequence of Nullable&lt;System.Double&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Double&gt; values to calculate the variance of.</param>
    /// <returns>A sequence of values that are the computed variance for the first 2,3,4,5....N elements.</returns>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double?> CumulativeVariance<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return CumulativeVariance(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the cumulative variance in a sequence of System.Single values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of System.Single values to calculate the variance of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of values that are the computed variance for the first 2,3,4,5....N elements.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<float> CumulativeVariance<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return CumulativeVariance(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the cumulative variance in a sequence of Nullable&lt;System.Single&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Single&gt; values to calculate the variance of.</param>
    /// <returns>A sequence of values that are the computed variance for the first 2,3,4,5....N elements.</returns>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<float?> CumulativeVariance<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return CumulativeVariance(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the cumulative variance in a sequence of System.Decimal values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of System.Decimal values to calculate the variance of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of values that are the computed variance for the first 2,3,4,5....N elements.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<decimal> CumulativeVariance<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return CumulativeVariance(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the cumulative variance in a sequence of Nullable&lt;System.Decimal&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Decimal&gt; values to calculate the variance of.</param>
    /// <returns>A sequence of values that are the computed variance for the first 2,3,4,5....N elements.</returns>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<decimal?> CumulativeVariance<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return CumulativeVariance(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the cumulative variance in a sequence of System.Int64 values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of System.Int64 values to calculate the variance of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of values that are the computed variance for the first 2,3,4,5....N elements.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double> CumulativeVariance<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return CumulativeVariance(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the cumulative variance in a sequence of Nullable&lt;System.Int64&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Int64&gt; values to calculate the variance of.</param>
    /// <returns>A sequence of values that are the computed variance for the first 2,3,4,5....N elements.</returns>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double?> CumulativeVariance<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return CumulativeVariance(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the cumulative variance in a sequence of System.Int32 values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of System.Int32 values to calculate the variance of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of values that are the computed variance for the first 2,3,4,5....N elements.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double> CumulativeVariance<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return CumulativeVariance(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the cumulative variance in a sequence of Nullable&lt;System.Int32&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Int32&gt; values to calculate the variance of.</param>
    /// <returns>A sequence of values that are the computed variance for the first 2,3,4,5....N elements.</returns>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double?> CumulativeVariance<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return CumulativeVariance(source.Select(S => selector(S)));
    }

    #endregion

    #region Variance (Block)

    /// <summary>
    /// Computes the variance of blocks of elements in a sequence of System.Double values.
    /// </summary>
    /// <param name="source">A sequence of System.Double values to calculate the variance of.</param>
    /// <param name="blockSize">The number of elements in the variance block. Block size must be two or larger.</param>
    /// <returns>The variance in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double> Variance(this IEnumerable<double> source, int blockSize)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (blockSize < 2)
        throw Error.ValueMinTwo("blockSize");

      int block = blockSize;
      int blockSize2 = blockSize - 1;
      double sum = 0;
      double sumSqr = 0;
      int nans = -1;
      using (IEnumerator<double> left = source.GetEnumerator())
      using (IEnumerator<double> right = source.GetEnumerator())
      {
        int elements = 0;
        double curr;
        while (block > 1)
        {
          block--;
          if (!right.MoveNext())
          {
            if (elements < 2)
              throw Error.SequenceMinTwo("source");

            if (nans > 0)
              yield return double.NaN;
            else
              yield return (sumSqr - sum * (sum / elements)) / (elements - 1);
            yield break;
          }
          curr = right.Current;
          if (double.IsNaN(curr))
            nans = blockSize;
          else
          {
            sum += curr;
            sumSqr += curr * curr;
            nans--;
          }
          elements++;
        }

        while (right.MoveNext())
        {
          curr = right.Current;
          if (double.IsNaN(curr))
            nans = blockSize;
          else
          {
            sum += curr;
            sumSqr += curr * curr;
            nans--;
          }
          if (nans > 0)
            yield return double.NaN;
          else
            yield return (sumSqr - sum * (sum / blockSize)) / blockSize2;
          left.MoveNext();
          curr = left.Current;
          if (double.IsNaN(curr))
            continue;

          sum -= curr;
          sumSqr -= curr * curr;
        }
      }
    }

    /// <summary>
    /// Computes the variance of blocks of elements in a sequence of Nullable&lt;System.Double&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Double&gt; values to calculate the variance of.</param>
    /// <param name="blockSize">The number of elements in the variance block. Block size must be two or larger.</param>
    /// <returns>The variance in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double?> Variance(this IEnumerable<double?> source, int blockSize)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (blockSize < 2)
        throw Error.ValueMinTwo("blockSize");

      int block = blockSize;
      double sum = 0;
      double sumSqr = 0;
      int nans = -1;
      using (IEnumerator<double?> left = source.GetEnumerator())
      using (IEnumerator<double?> right = source.GetEnumerator())
      {
        int elements = 0;
        double curr;
        while (block > 1)
        {
          block--;
          if (!right.MoveNext())
          {
            if (elements < 2)
              throw Error.SequenceMinTwoNotNull("source");

            if (nans > 0)
              yield return double.NaN;
            else
              yield return (sumSqr - sum * (sum / elements)) / (elements - 1);
            yield break;
          }
          curr = right.Current.GetValueOrDefault();
          nans--;
          if (!right.Current.HasValue)
            continue;

          if (double.IsNaN(curr))
            nans = blockSize;
          else
          {
            sum += curr;
            sumSqr += curr * curr;
          }
          elements++;
        }

        while (right.MoveNext())
        {
          curr = right.Current.GetValueOrDefault();
          nans--;
          if (right.Current.HasValue)
          {
            if (double.IsNaN(curr))
              nans = blockSize;
            else
            {
              sum += curr;
              sumSqr += curr * curr;
            }
            elements++;
          }
          if (elements <= 1)
            yield return null;
          else if (nans > 0)
            yield return double.NaN;
          else
            yield return (sumSqr - sum * (sum / elements)) / (elements - 1);

          left.MoveNext();
          curr = left.Current.GetValueOrDefault();

          if (!left.Current.HasValue)
            continue;

          if (!double.IsNaN(curr))
          {
            sum -= curr;
            sumSqr -= curr * curr;
          }
          elements--;
        }
      }
    }

    /// <summary>
    /// Computes the variance of blocks of elements in a sequence of System.Single values.
    /// </summary>
    /// <param name="source">A sequence of System.Single values to calculate the variance of.</param>
    /// <param name="blockSize">The number of elements in the variance block. Block size must be two or larger.</param>
    /// <returns>The variance in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<float> Variance(this IEnumerable<float> source, int blockSize)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (blockSize < 2)
        throw Error.ValueMinTwo("blockSize");

      int block = blockSize;
      int blockSize2 = blockSize - 1;
      float sum = 0;
      float sumSqr = 0;
      int nans = -1;
      using (IEnumerator<float> left = source.GetEnumerator())
      using (IEnumerator<float> right = source.GetEnumerator())
      {
        int elements = 0;
        float curr;
        while (block > 1)
        {
          block--;
          if (!right.MoveNext())
          {
            if (elements < 2)
              throw Error.SequenceMinTwo("source");

            if (nans > 0)
              yield return float.NaN;
            else
              yield return (sumSqr - sum * (sum / elements)) / (elements - 1);
            yield break;
          }
          curr = right.Current;
          if (float.IsNaN(curr))
            nans = blockSize;
          else
          {
            sum += curr;
            sumSqr += curr * curr;
            nans--;
          }
          elements++;
        }

        while (right.MoveNext())
        {
          curr = right.Current;
          if (float.IsNaN(curr))
            nans = blockSize;
          else
          {
            sum += curr;
            sumSqr += curr * curr;
            nans--;
          }
          if (nans > 0)
            yield return float.NaN;
          else
            yield return (sumSqr - sum * (sum / blockSize)) / blockSize2;
          left.MoveNext();
          curr = left.Current;

          if (float.IsNaN(curr))
            continue;

          sum -= curr;
          sumSqr -= curr * curr;
        }
      }
    }

    /// <summary>
    /// Computes the variance of blocks of elements in a sequence of Nullable&lt;System.Single&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Single&gt; values to calculate the variance of.</param>
    /// <param name="blockSize">The number of elements in the variance block. Block size must be two or larger.</param>
    /// <returns>The variance in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<float?> Variance(this IEnumerable<float?> source, int blockSize)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (blockSize < 2)
        throw Error.ValueMinTwo("blockSize");

      int block = blockSize;
      float sum = 0;
      float sumSqr = 0;
      int nans = -1;
      using (IEnumerator<float?> left = source.GetEnumerator())
      using (IEnumerator<float?> right = source.GetEnumerator())
      {
        int elements = 0;
        float curr;
        while (block > 1)
        {
          block--;
          if (!right.MoveNext())
          {
            if (elements < 2)
              throw Error.SequenceMinTwoNotNull("source");

            if (nans > 0)
              yield return float.NaN;
            else
              yield return (sumSqr - sum * (sum / elements)) / (elements - 1);
            yield break;
          }
          curr = right.Current.GetValueOrDefault();
          nans--;

          if (!right.Current.HasValue)
            continue;

          if (float.IsNaN(curr))
            nans = blockSize;
          else
          {
            sum += curr;
            sumSqr += curr * curr;
          }
          elements++;
        }

        while (right.MoveNext())
        {
          curr = right.Current.GetValueOrDefault();
          nans--;
          if (right.Current.HasValue)
          {
            if (float.IsNaN(curr))
              nans = blockSize;
            else
            {
              sum += curr;
              sumSqr += curr * curr;
            }
            elements++;
          }
          if (elements <= 1)
            yield return null;
          else if (nans > 0)
            yield return float.NaN;
          else
            yield return (sumSqr - sum * (sum / elements)) / (elements - 1);

          left.MoveNext();
          curr = left.Current.GetValueOrDefault();

          if (!left.Current.HasValue)
            continue;

          if (!float.IsNaN(curr))
          {
            sum -= curr;
            sumSqr -= curr * curr;
          }
          elements--;
        }
      }
    }

    /// <summary>
    /// Computes the variance of blocks of elements in a sequence of System.Decimal values.
    /// </summary>
    /// <param name="source">A sequence of System.Decimal values to calculate the variance of.</param>
    /// <param name="blockSize">The number of elements in the variance block. Block size must be two or larger.</param>
    /// <returns>The variance in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<decimal> Variance(this IEnumerable<decimal> source, int blockSize)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (blockSize < 2)
        throw Error.ValueMinTwo("blockSize");

      int block = blockSize;
      int blockSize2 = blockSize - 1;
      decimal sum = 0;
      decimal sumSqr = 0;
      using (IEnumerator<decimal> left = source.GetEnumerator())
      using (IEnumerator<decimal> right = source.GetEnumerator())
      {
        int elements = 0;
        decimal curr;
        while (block > 1)
        {
          block--;
          if (!right.MoveNext())
          {
            if (elements < 2)
              throw Error.SequenceMinTwo("source");

            yield return (sumSqr - sum * (sum / elements)) / (elements - 1);
            yield break;
          }
          curr = right.Current;
          sum += curr;
          sumSqr += curr * curr;
          elements++;
        }

        while (right.MoveNext())
        {
          curr = right.Current;
          sum += curr;
          sumSqr += curr * curr;
          yield return (sumSqr - sum * (sum / blockSize)) / blockSize2;
          left.MoveNext();
          curr = left.Current;
          sum -= curr;
          sumSqr -= curr * curr;
        }
      }
    }

    /// <summary>
    /// Computes the variance of blocks of elements in a sequence of Nullable&lt;System.Decimal&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Decimal&gt; values to calculate the variance of.</param>
    /// <param name="blockSize">The number of elements in the variance block. Block size must be two or larger.</param>
    /// <returns>The variance in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<decimal?> Variance(this IEnumerable<decimal?> source, int blockSize)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (blockSize < 2)
        throw Error.ValueMinTwo("blockSize");

      int block = blockSize;
      decimal sum = 0;
      decimal sumSqr = 0;
      using (IEnumerator<decimal?> left = source.GetEnumerator())
      using (IEnumerator<decimal?> right = source.GetEnumerator())
      {
        int elements = 0;
        decimal curr;
        while (block > 1)
        {
          block--;
          if (!right.MoveNext())
          {
            if (elements < 2)
              throw Error.SequenceMinTwoNotNull("source");

            yield return (sumSqr - sum * (sum / elements)) / (elements - 1);
            yield break;
          }
          curr = right.Current.GetValueOrDefault();

          if (!right.Current.HasValue)
            continue;

          sum += curr;
          sumSqr += curr * curr;
          elements++;
        }

        while (right.MoveNext())
        {
          curr = right.Current.GetValueOrDefault();
          if (right.Current.HasValue)
          {
            sum += curr;
            sumSqr += curr * curr;
            elements++;
          }
          if (elements <= 1)
            yield return null;
          else
            yield return (sumSqr - sum * (sum / elements)) / (elements - 1);

          left.MoveNext();
          curr = left.Current.GetValueOrDefault();

          if (!left.Current.HasValue)
            continue;

          sum -= curr;
          sumSqr -= curr * curr;
          elements--;
        }
      }
    }

    /// <summary>
    /// Computes the variance of blocks of elements in a sequence of System.Int64 values.
    /// </summary>
    /// <param name="source">A sequence of System.Int64 values to calculate the variance of.</param>
    /// <param name="blockSize">The number of elements in the variance block. Block size must be two or larger.</param>
    /// <returns>The variance in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double> Variance(this IEnumerable<long> source, int blockSize)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (blockSize < 2)
        throw Error.ValueMinTwo("blockSize");

      int block = blockSize;
      int blockSize2 = blockSize - 1;
      double sum = 0;
      double sumSqr = 0;
      using (IEnumerator<long> left = source.GetEnumerator())
      using (IEnumerator<long> right = source.GetEnumerator())
      {
        int elements = 0;
        long curr;
        while (block > 1)
        {
          block--;
          if (!right.MoveNext())
          {
            if (elements < 2)
              throw Error.SequenceMinTwo("source");

            yield return (sumSqr - sum * (sum / elements)) / (elements - 1);
            yield break;
          }
          curr = right.Current;
          sum += curr;
          sumSqr += curr * curr;
          elements++;
        }

        while (right.MoveNext())
        {
          curr = right.Current;
          sum += curr;
          sumSqr += curr * curr;
          yield return (sumSqr - sum * (sum / blockSize)) / blockSize2;
          left.MoveNext();
          curr = left.Current;
          sum -= curr;
          sumSqr -= curr * curr;
        }
      }
    }

    /// <summary>
    /// Computes the variance of blocks of elements in a sequence of Nullable&lt;System.Int64&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Int64&gt; values to calculate the variance of.</param>
    /// <param name="blockSize">The number of elements in the variance block. Block size must be two or larger.</param>
    /// <returns>The variance in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double?> Variance(this IEnumerable<long?> source, int blockSize)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (blockSize < 2)
        throw Error.ValueMinTwo("blockSize");

      int block = blockSize;
      double sum = 0;
      double sumSqr = 0;
      using (IEnumerator<long?> left = source.GetEnumerator())
      using (IEnumerator<long?> right = source.GetEnumerator())
      {
        int elements = 0;
        long curr;
        while (block > 1)
        {
          block--;
          if (!right.MoveNext())
          {
            if (elements < 2)
              throw Error.SequenceMinTwoNotNull("source");

            yield return (sumSqr - sum * (sum / elements)) / (elements - 1);
            yield break;
          }
          curr = right.Current.GetValueOrDefault();

          if (!right.Current.HasValue)
            continue;

          sum += curr;
          sumSqr += curr * curr;
          elements++;
        }

        while (right.MoveNext())
        {
          curr = right.Current.GetValueOrDefault();
          if (right.Current.HasValue)
          {
            sum += curr;
            sumSqr += curr * curr;
            elements++;
          }

          if (elements <= 1)
            yield return null;
          else
            yield return (sumSqr - sum * (sum / elements)) / (elements - 1);

          left.MoveNext();
          curr = left.Current.GetValueOrDefault();

          if (!left.Current.HasValue)
            continue;

          sum -= curr;
          sumSqr -= curr * curr;
          elements--;
        }
      }
    }

    /// <summary>
    /// Computes the variance of blocks of elements in a sequence of System.Int32 values.
    /// </summary>
    /// <param name="source">A sequence of System.Int32 values to calculate the variance of.</param>
    /// <param name="blockSize">The number of elements in the variance block. Block size must be two or larger.</param>
    /// <returns>The variance in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double> Variance(this IEnumerable<int> source, int blockSize)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (blockSize < 2)
        throw Error.ValueMinTwo("blockSize");

      int block = blockSize;
      int blockSize2 = blockSize - 1;
      double sum = 0;
      double sumSqr = 0;
      using (IEnumerator<int> left = source.GetEnumerator())
      using (IEnumerator<int> right = source.GetEnumerator())
      {
        int elements = 0;
        int curr;
        while (block > 1)
        {
          block--;
          if (!right.MoveNext())
          {
            if (elements < 2)
              throw Error.SequenceMinTwo("source");

            yield return (sumSqr - sum * (sum / elements)) / (elements - 1);
            yield break;
          }
          curr = right.Current;
          sum += curr;
          sumSqr += curr * curr;
          elements++;
        }

        while (right.MoveNext())
        {
          curr = right.Current;
          sum += curr;
          sumSqr += curr * curr;
          yield return (sumSqr - sum * (sum / blockSize)) / blockSize2;
          left.MoveNext();
          curr = left.Current;
          sum -= curr;
          sumSqr -= curr * curr;
        }
      }
    }

    /// <summary>
    /// Computes the variance of blocks of elements in a sequence of Nullable&lt;System.Int32&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Int32&gt; values to calculate the variance of.</param>
    /// <param name="blockSize">The number of elements in the variance block. Block size must be two or larger.</param>
    /// <returns>The variance in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double?> Variance(this IEnumerable<int?> source, int blockSize)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (blockSize < 2)
        throw Error.ValueMinTwo("blockSize");

      int block = blockSize;
      double sum = 0;
      double sumSqr = 0;
      using (IEnumerator<int?> left = source.GetEnumerator())
      using (IEnumerator<int?> right = source.GetEnumerator())
      {
        int elements = 0;
        int curr;
        while (block > 1)
        {
          block--;
          if (!right.MoveNext())
          {
            if (elements < 2)
              throw Error.SequenceMinTwoNotNull("source");

            yield return (sumSqr - sum * (sum / elements)) / (elements - 1);
            yield break;
          }
          curr = right.Current.GetValueOrDefault();

          if (!right.Current.HasValue)
            continue;

          sum += curr;
          sumSqr += curr * curr;
          elements++;
        }

        while (right.MoveNext())
        {
          curr = right.Current.GetValueOrDefault();
          if (right.Current.HasValue)
          {
            sum += curr;
            sumSqr += curr * curr;
            elements++;
          }

          if (elements <= 1)
            yield return null;
          else
            yield return (sumSqr - sum * (sum / elements)) / (elements - 1);

          left.MoveNext();
          curr = left.Current.GetValueOrDefault();

          if (!left.Current.HasValue)
            continue;

          sum -= curr;
          sumSqr -= curr * curr;
          elements--;
        }
      }
    }

    /// <summary>
    /// Computes the variance of blocks of elements in a sequence of System.Double values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of System.Double values to calculate the variance of.</param>
    /// <param name="blockSize">The number of elements in the variance block. Block size must be two or larger.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>The variance in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double> Variance<TSource>(this IEnumerable<TSource> source, int blockSize, Func<TSource, double> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return Variance(source.Select(S => selector(S)), blockSize);
    }

    /// <summary>
    /// Computes the variance of blocks of elements in a sequence of Nullable&lt;System.Double&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Double&gt; values to calculate the variance of.</param>
    /// <param name="blockSize">The number of elements in the variance block. Block size must be two or larger.</param>
    /// <returns>The variance in the sequence of values.</returns>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double?> Variance<TSource>(this IEnumerable<TSource> source, int blockSize, Func<TSource, double?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return Variance(source.Select(S => selector(S)), blockSize);
    }

    /// <summary>
    /// Computes the variance of blocks of elements in a sequence of System.Single values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of System.Single values to calculate the variance of.</param>
    /// <param name="blockSize">The number of elements in the variance block. Block size must be two or larger.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>The variance in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<float> Variance<TSource>(this IEnumerable<TSource> source, int blockSize, Func<TSource, float> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return Variance(source.Select(S => selector(S)), blockSize);
    }

    /// <summary>
    /// Computes the variance of blocks of elements in a sequence of Nullable&lt;System.Single&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Single&gt; values to calculate the variance of.</param>
    /// <param name="blockSize">The number of elements in the variance block. Block size must be two or larger.</param>
    /// <returns>The variance in the sequence of values.</returns>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<float?> Variance<TSource>(this IEnumerable<TSource> source, int blockSize, Func<TSource, float?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return Variance(source.Select(S => selector(S)), blockSize);
    }

    /// <summary>
    /// Computes the variance of blocks of elements in a sequence of System.Decimal values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of System.Decimal values to calculate the variance of.</param>
    /// <param name="blockSize">The number of elements in the variance block. Block size must be two or larger.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>The variance in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<decimal> Variance<TSource>(this IEnumerable<TSource> source, int blockSize, Func<TSource, decimal> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return Variance(source.Select(S => selector(S)), blockSize);
    }

    /// <summary>
    /// Computes the variance of blocks of elements in a sequence of Nullable&lt;System.Decimal&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Decimal&gt; values to calculate the variance of.</param>
    /// <param name="blockSize">The number of elements in the variance block. Block size must be two or larger.</param>
    /// <returns>The variance in the sequence of values.</returns>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<decimal?> Variance<TSource>(this IEnumerable<TSource> source, int blockSize, Func<TSource, decimal?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return Variance(source.Select(S => selector(S)), blockSize);
    }

    /// <summary>
    /// Computes the variance of blocks of elements in a sequence of System.Int64 values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of System.Int64 values to calculate the variance of.</param>
    /// <param name="blockSize">The number of elements in the variance block. Block size must be two or larger.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>The variance in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double> Variance<TSource>(this IEnumerable<TSource> source, int blockSize, Func<TSource, long> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return Variance(source.Select(S => selector(S)), blockSize);
    }

    /// <summary>
    /// Computes the variance of blocks of elements in a sequence of Nullable&lt;System.Int64&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Int64&gt; values to calculate the variance of.</param>
    /// <param name="blockSize">The number of elements in the variance block. Block size must be two or larger.</param>
    /// <returns>The variance in the sequence of values.</returns>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double?> Variance<TSource>(this IEnumerable<TSource> source, int blockSize, Func<TSource, long?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return Variance(source.Select(S => selector(S)), blockSize);
    }

    /// <summary>
    /// Computes the variance of blocks of elements in a sequence of System.Int32 values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of System.Int32 values to calculate the variance of.</param>
    /// <param name="blockSize">The number of elements in the variance block. Block size must be two or larger.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>The variance in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double> Variance<TSource>(this IEnumerable<TSource> source, int blockSize, Func<TSource, int> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return Variance(source.Select(S => selector(S)), blockSize);
    }

    /// <summary>
    /// Computes the variance of blocks of elements in a sequence of Nullable&lt;System.Int32&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Int32&gt; values to calculate the variance of.</param>
    /// <param name="blockSize">The number of elements in the variance block. Block size must be two or larger.</param>
    /// <returns>The variance in the sequence of values.</returns>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double?> Variance<TSource>(this IEnumerable<TSource> source, int blockSize, Func<TSource, int?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return Variance(source.Select(S => selector(S)), blockSize);
    }

    #endregion

    #region Standard Deviation

    /// <summary>
    /// Computes the standard deviation of a sequence of System.Double values.
    /// </summary>
    /// <param name="source">A sequence of System.Double values to calculate the standard deviation of.</param>
    /// <returns>The standard deviation of the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static double Stdev(this IEnumerable<double> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      return Math.Sqrt(source.Variance());
    }

    /// <summary>
    /// Computes the standard deviation of a sequence of Nullable&lt;System.Double&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Double&gt; values to calculate the standard deviation of.</param>
    /// <returns>The standard deviation of the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static double? Stdev(this IEnumerable<double?> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      double? variance = source.Variance();
      if (variance.HasValue)
        return Math.Sqrt(variance.Value);

      return null;
    }

    /// <summary>
    /// Computes the standard deviation of a sequence of System.Single values.
    /// </summary>
    /// <param name="source">A sequence of System.Single values to calculate the standard deviation of.</param>
    /// <returns>The standard deviation of the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static float Stdev(this IEnumerable<float> source)
    {
      return (float)Math.Sqrt(source.Variance());
    }

    /// <summary>
    /// Computes the standard deviation of a sequence of Nullable&lt;System.Single&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Single&gt; values to calculate the standard deviation of.</param>
    /// <returns>The standard deviation of the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static float? Stdev(this IEnumerable<float?> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      float? variance = source.Variance();
      if (variance.HasValue)
        return (float?)Math.Sqrt(variance.Value);

      return null;
    }

    /// <summary>
    /// Computes the standard deviation of a sequence of System.Decimal values.
    /// </summary>
    /// <param name="source">A sequence of System.Decimal values to calculate the standard deviation of.</param>
    /// <returns>The standard deviation of the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static decimal Stdev(this IEnumerable<decimal> source)
    {
      return (decimal)Math.Sqrt((double)source.Variance());
    }

    /// <summary>
    /// Computes the standard deviation of a sequence of Nullable&lt;System.Decimal&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Decimal&gt; values to calculate the standard deviation of.</param>
    /// <returns>The standard deviation of the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static decimal? Stdev(this IEnumerable<decimal?> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      decimal? variance = source.Variance();
      if (variance.HasValue)
        return (decimal?)Math.Sqrt((double)variance.Value);

      return null;
    }

    /// <summary>
    /// Computes the standard deviation of a sequence of System.Int64 values.
    /// </summary>
    /// <param name="source">A sequence of System.Int64 values to calculate the standard deviation of.</param>
    /// <returns>The standard deviation of the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static double Stdev(this IEnumerable<long> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      return Math.Sqrt(source.Variance());
    }

    /// <summary>
    /// Computes the standard deviation of a sequence of Nullable&lt;System.Int64&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Int64&gt; values to calculate the standard deviation of.</param>
    /// <returns>The standard deviation of the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static double? Stdev(this IEnumerable<long?> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      double? variance = source.Variance();
      if (variance.HasValue)
        return Math.Sqrt(variance.Value);

      return null;
    }

    /// <summary>
    /// Computes the standard deviation of a sequence of System.Int32 values.
    /// </summary>
    /// <param name="source">A sequence of System.Int32 values to calculate the standard deviation of.</param>
    /// <returns>The standard deviation of the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static double Stdev(this IEnumerable<int> source)
    {
      return Math.Sqrt(source.Variance());
    }

    /// <summary>
    /// Computes the standard deviation of a sequence of Nullable&lt;System.Int32&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Int32&gt; values to calculate the standard deviation of.</param>
    /// <returns>The standard deviation of the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static double? Stdev(this IEnumerable<int?> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      double? variance = source.Variance();
      if (variance.HasValue)
        return Math.Sqrt(variance.Value);

      return null;
    }

    /// <summary>
    /// Computes the standard deviation of a sequence of System.Double values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of System.Double values to calculate the standard deviation of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>The standard deviation of the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static double Stdev<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return Stdev(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the standard deviation of a sequence of Nullable&lt;System.Double&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Double&gt; values to calculate the standard deviation of.</param>
    /// <returns>The standard deviation of the sequence of values.</returns>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static double? Stdev<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return Stdev(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the standard deviation of a sequence of System.Single values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of System.Single values to calculate the standard deviation of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>The standard deviation of the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static float Stdev<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return Stdev(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the standard deviation of a sequence of Nullable&lt;System.Single&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Single&gt; values to calculate the standard deviation of.</param>
    /// <returns>The standard deviation of the sequence of values.</returns>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static float? Stdev<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return Stdev(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the standard deviation of a sequence of System.Decimal values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of System.Decimal values to calculate the standard deviation of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>The standard deviation of the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static decimal Stdev<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return Stdev(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the standard deviation of a sequence of Nullable&lt;System.Decimal&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Decimal&gt; values to calculate the standard deviation of.</param>
    /// <returns>The standard deviation of the sequence of values.</returns>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static decimal? Stdev<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return Stdev(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the standard deviation of a sequence of System.Int64 values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of System.Int64 values to calculate the standard deviation of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>The standard deviation of the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static double Stdev<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return Stdev(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the standard deviation of a sequence of Nullable&lt;System.Int64&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Int64&gt; values to calculate the standard deviation of.</param>
    /// <returns>The standard deviation of the sequence of values.</returns>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static double? Stdev<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return Stdev(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the standard deviation of a sequence of System.Int32 values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of System.Int32 values to calculate the standard deviation of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>The standard deviation of the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception> 
    public static double Stdev<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return Stdev(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the standard deviation of a sequence of Nullable&lt;System.Int32&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Int32&gt; values to calculate the standard deviation of.</param>
    /// <returns>The standard deviation of the sequence of values.</returns>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static double? Stdev<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return Stdev(source.Select(S => selector(S)));
    }

    #endregion

    #region Standard Deviation (Cumulative)

    /// <summary>
    /// Computes the cumulative standard deviation in a sequence of System.Double values.
    /// </summary>
    /// <param name="source">A sequence of System.Double values to calculate the standard deviation of.</param>
    /// <returns>A sequence of values that are the computed standard deviation for the first 2,3,4,5....N elements.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double> CumulativeStdev(this IEnumerable<double> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      return source.CumulativeVariance().Select(ITEM => Math.Sqrt(ITEM));
    }

    /// <summary>
    /// Computes the cumulative standard deviation in a sequence of Nullable&lt;System.Double&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Double&gt; values to calculate the standard deviation of.</param>
    /// <returns>A sequence of values that are the computed standard deviation for the first 2,3,4,5....N elements.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double?> CumulativeStdev(this IEnumerable<double?> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      return source.CumulativeVariance().Select(ITEM => Math.Sqrt(ITEM.GetValueOrDefault())).Select(DUMMY => (double?)DUMMY);
    }

    /// <summary>
    /// Computes the cumulative standard deviation in a sequence of System.Single values.
    /// </summary>
    /// <param name="source">A sequence of System.Single values to calculate the standard deviation of.</param>
    /// <returns>A sequence of values that are the computed standard deviation for the first 2,3,4,5....N elements.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<float> CumulativeStdev(this IEnumerable<float> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      return source.CumulativeVariance().Select(ITEM => (float)Math.Sqrt(ITEM));
    }

    /// <summary>
    /// Computes the cumulative standard deviation in a sequence of Nullable&lt;System.Single&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Single&gt; values to calculate the standard deviation of.</param>
    /// <returns>A sequence of values that are the computed standard deviation for the first 2,3,4,5....N elements.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<float?> CumulativeStdev(this IEnumerable<float?> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      return source.CumulativeVariance().Select(ITEM => (float?)Math.Sqrt(ITEM.GetValueOrDefault()));
    }

    /// <summary>
    /// Computes the cumulative standard deviation in a sequence of System.Decimal values.
    /// </summary>
    /// <param name="source">A sequence of System.Decimal values to calculate the standard deviation of.</param>
    /// <returns>A sequence of values that are the computed standard deviation for the first 2,3,4,5....N elements.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<decimal> CumulativeStdev(this IEnumerable<decimal> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      return source.CumulativeVariance().Select(ITEM => (decimal)Math.Sqrt((double)ITEM));
    }

    /// <summary>
    /// Computes the cumulative standard deviation in a sequence of Nullable&lt;System.Decimal&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Decimal&gt; values to calculate the standard deviation of.</param>
    /// <returns>A sequence of values that are the computed standard deviation for the first 2,3,4,5....N elements.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<decimal?> CumulativeStdev(this IEnumerable<decimal?> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      return source.CumulativeVariance().Select(ITEM => (decimal?)Math.Sqrt((double)ITEM.GetValueOrDefault()));
    }

    /// <summary>
    /// Computes the cumulative standard deviation in a sequence of System.Int64 values.
    /// </summary>
    /// <param name="source">A sequence of System.Int64 values to calculate the standard deviation of.</param>
    /// <returns>A sequence of values that are the computed standard deviation for the first 2,3,4,5....N elements.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double> CumulativeStdev(this IEnumerable<long> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      return source.CumulativeVariance().Select(ITEM => Math.Sqrt(ITEM));
    }

    /// <summary>
    /// Computes the cumulative standard deviation in a sequence of Nullable&lt;System.Int64&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Int64&gt; values to calculate the standard deviation of.</param>
    /// <returns>A sequence of values that are the computed standard deviation for the first 2,3,4,5....N elements.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double?> CumulativeStdev(this IEnumerable<long?> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      return source.CumulativeVariance().Select(ITEM => Math.Sqrt(ITEM.GetValueOrDefault())).Select(DUMMY => (double?)DUMMY);
    }

    /// <summary>
    /// Computes the cumulative standard deviation in a sequence of System.Int32 values.
    /// </summary>
    /// <param name="source">A sequence of System.Int32 values to calculate the standard deviation of.</param>
    /// <returns>A sequence of values that are the computed standard deviation for the first 2,3,4,5....N elements.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double> CumulativeStdev(this IEnumerable<int> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      return source.CumulativeVariance().Select(ITEM => Math.Sqrt(ITEM));
    }

    /// <summary>
    /// Computes the cumulative standard deviation in a sequence of Nullable&lt;System.Int32&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Int32&gt; values to calculate the standard deviation of.</param>
    /// <returns>A sequence of values that are the computed standard deviation for the first 2,3,4,5....N elements.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double?> CumulativeStdev(this IEnumerable<int?> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      return source.CumulativeVariance().Select(ITEM => Math.Sqrt(ITEM.GetValueOrDefault())).Select(DUMMY => (double?)DUMMY);
    }

    /// <summary>
    /// Computes the cumulative standard deviation in a sequence of System.Double values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of System.Double values to calculate the standard deviation of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of values that are the computed standard deviation for the first 2,3,4,5....N elements.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double> CumulativeStdev<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return CumulativeStdev(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the cumulative standard deviation in a sequence of Nullable&lt;System.Double&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Double&gt; values to calculate the standard deviation of.</param>
    /// <returns>A sequence of values that are the computed standard deviation for the first 2,3,4,5....N elements.</returns>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double?> CumulativeStdev<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return CumulativeStdev(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the cumulative standard deviation in a sequence of System.Single values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of System.Single values to calculate the standard deviation of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of values that are the computed standard deviation for the first 2,3,4,5....N elements.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<float> CumulativeStdev<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return CumulativeStdev(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the cumulative standard deviation in a sequence of Nullable&lt;System.Single&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Single&gt; values to calculate the standard deviation of.</param>
    /// <returns>A sequence of values that are the computed standard deviation for the first 2,3,4,5....N elements.</returns>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<float?> CumulativeStdev<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return CumulativeStdev(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the cumulative standard deviation in a sequence of System.Decimal values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of System.Decimal values to calculate the standard deviation of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of values that are the computed standard deviation for the first 2,3,4,5....N elements.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<decimal> CumulativeStdev<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return CumulativeStdev(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the cumulative standard deviation in a sequence of Nullable&lt;System.Decimal&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Decimal&gt; values to calculate the standard deviation of.</param>
    /// <returns>A sequence of values that are the computed standard deviation for the first 2,3,4,5....N elements.</returns>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<decimal?> CumulativeStdev<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return CumulativeStdev(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the cumulative standard deviation in a sequence of System.Int64 values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of System.Int64 values to calculate the standard deviation of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of values that are the computed standard deviation for the first 2,3,4,5....N elements.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double> CumulativeStdev<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return CumulativeStdev(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the cumulative standard deviation in a sequence of Nullable&lt;System.Int64&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Int64&gt; values to calculate the standard deviation of.</param>
    /// <returns>A sequence of values that are the computed standard deviation for the first 2,3,4,5....N elements.</returns>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double?> CumulativeStdev<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return CumulativeStdev(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the cumulative standard deviation in a sequence of System.Int32 values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of System.Int32 values to calculate the standard deviation of.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>A sequence of values that are the computed standard deviation for the first 2,3,4,5....N elements.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double> CumulativeStdev<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return CumulativeStdev(source.Select(S => selector(S)));
    }

    /// <summary>
    /// Computes the cumulative standard deviation in a sequence of Nullable&lt;System.Int32&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Int32&gt; values to calculate the standard deviation of.</param>
    /// <returns>A sequence of values that are the computed standard deviation for the first 2,3,4,5....N elements.</returns>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double?> CumulativeStdev<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return CumulativeStdev(source.Select(S => selector(S)));
    }

    #endregion

    #region Standard Deviation (Block)

    /// <summary>
    /// Computes the standard deviation of blocks of elements in a sequence of System.Double values.
    /// </summary>
    /// <param name="source">A sequence of System.Double values to calculate the standard deviation of.</param>
    /// <param name="blockSize">The number of elements in the standard deviation block. Block size must be two or larger.</param>
    /// <returns>The standard deviation in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double> Stdev(this IEnumerable<double> source, int blockSize)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      return source.Variance(blockSize).Select(ITEM => Math.Sqrt(ITEM));
    }

    /// <summary>
    /// Computes the standard deviation of blocks of elements in a sequence of Nullable&lt;System.Double&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Double&gt; values to calculate the standard deviation of.</param>
    /// <param name="blockSize">The number of elements in the standard deviation block. Block size must be two or larger.</param>
    /// <returns>The standard deviation in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double?> Stdev(this IEnumerable<double?> source, int blockSize)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      foreach (double? item in source.Variance(blockSize))
        if (item.HasValue)
          yield return Math.Sqrt(item.GetValueOrDefault());
        else
          yield return null;
    }

    /// <summary>
    /// Computes the standard deviation of blocks of elements in a sequence of System.Single values.
    /// </summary>
    /// <param name="source">A sequence of System.Single values to calculate the standard deviation of.</param>
    /// <param name="blockSize">The number of elements in the standard deviation block. Block size must be two or larger.</param>
    /// <returns>The standard deviation in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<float> Stdev(this IEnumerable<float> source, int blockSize)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      return source.Variance(blockSize).Select(ITEM => (float)Math.Sqrt(ITEM));
    }

    /// <summary>
    /// Computes the standard deviation of blocks of elements in a sequence of Nullable&lt;System.Single&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Single&gt; values to calculate the standard deviation of.</param>
    /// <param name="blockSize">The number of elements in the standard deviation block. Block size must be two or larger.</param>
    /// <returns>The standard deviation in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<float?> Stdev(this IEnumerable<float?> source, int blockSize)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      foreach (float? item in source.Variance(blockSize))
        if (item.HasValue)
          yield return (float?)Math.Sqrt(item.GetValueOrDefault());
        else
          yield return null;
    }

    /// <summary>
    /// Computes the standard deviation of blocks of elements in a sequence of System.Decimal values.
    /// </summary>
    /// <param name="source">A sequence of System.Decimal values to calculate the standard deviation of.</param>
    /// <param name="blockSize">The number of elements in the standard deviation block. Block size must be two or larger.</param>
    /// <returns>The standard deviation in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<decimal> Stdev(this IEnumerable<decimal> source, int blockSize)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      return source.Variance(blockSize).Select(ITEM => (decimal)Math.Sqrt((double)ITEM));
    }

    /// <summary>
    /// Computes the standard deviation of blocks of elements in a sequence of Nullable&lt;System.Decimal&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Decimal&gt; values to calculate the standard deviation of.</param>
    /// <param name="blockSize">The number of elements in the standard deviation block. Block size must be two or larger.</param>
    /// <returns>The standard deviation in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<decimal?> Stdev(this IEnumerable<decimal?> source, int blockSize)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      foreach (decimal? item in source.Variance(blockSize))
        if (item.HasValue)
          yield return (decimal?)Math.Sqrt((double)item.Value);
        else
          yield return null;
    }

    /// <summary>
    /// Computes the standard deviation of blocks of elements in a sequence of System.Int64 values.
    /// </summary>
    /// <param name="source">A sequence of System.Int64 values to calculate the standard deviation of.</param>
    /// <param name="blockSize">The number of elements in the standard deviation block. Block size must be two or larger.</param>
    /// <returns>The standard deviation in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double> Stdev(this IEnumerable<long> source, int blockSize)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      return source.Variance(blockSize).Select(ITEM => Math.Sqrt(ITEM));
    }

    /// <summary>
    /// Computes the standard deviation of blocks of elements in a sequence of Nullable&lt;System.Int64&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Int64&gt; values to calculate the standard deviation of.</param>
    /// <param name="blockSize">The number of elements in the standard deviation block. Block size must be two or larger.</param>
    /// <returns>The standard deviation in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double?> Stdev(this IEnumerable<long?> source, int blockSize)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      foreach (double? item in source.Variance(blockSize))
        if (item.HasValue)
          yield return Math.Sqrt(item.Value);
        else
          yield return null;
    }

    /// <summary>
    /// Computes the standard deviation of blocks of elements in a sequence of System.Int32 values.
    /// </summary>
    /// <param name="source">A sequence of System.Int32 values to calculate the standard deviation of.</param>
    /// <param name="blockSize">The number of elements in the standard deviation block. Block size must be two or larger.</param>
    /// <returns>The standard deviation in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double> Stdev(this IEnumerable<int> source, int blockSize)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      return source.Variance(blockSize).Select(ITEM => Math.Sqrt(ITEM));
    }

    /// <summary>
    /// Computes the standard deviation of blocks of elements in a sequence of Nullable&lt;System.Int32&gt; values.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Int32&gt; values to calculate the standard deviation of.</param>
    /// <param name="blockSize">The number of elements in the standard deviation block. Block size must be two or larger.</param>
    /// <returns>The standard deviation in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentException">blockSize must be two or larger.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double?> Stdev(this IEnumerable<int?> source, int blockSize)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      foreach (double? item in source.Variance(blockSize))
        if (item.HasValue)
          yield return Math.Sqrt(item.Value);
        else
          yield return null;
    }

    /// <summary>
    /// Computes the standard deviation of blocks of elements in a sequence of System.Double values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of System.Double values to calculate the standard deviation of.</param>
    /// <param name="blockSize">The number of elements in the standard deviation block. Block size must be two or larger.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>The standard deviation in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double> Stdev<TSource>(this IEnumerable<TSource> source, int blockSize, Func<TSource, double> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return Stdev(source.Select(S => selector(S)), blockSize);
    }

    /// <summary>
    /// Computes the standard deviation of blocks of elements in a sequence of Nullable&lt;System.Double&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Double&gt; values to calculate the standard deviation of.</param>
    /// <param name="blockSize">The number of elements in the standard deviation block. Block size must be two or larger.</param>
    /// <returns>The standard deviation in the sequence of values.</returns>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double?> Stdev<TSource>(this IEnumerable<TSource> source, int blockSize, Func<TSource, double?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return Stdev(source.Select(S => selector(S)), blockSize);
    }

    /// <summary>
    /// Computes the standard deviation of blocks of elements in a sequence of System.Single values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of System.Single values to calculate the standard deviation of.</param>
    /// <param name="blockSize">The number of elements in the standard deviation block. Block size must be two or larger.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>The standard deviation in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<float> Stdev<TSource>(this IEnumerable<TSource> source, int blockSize, Func<TSource, float> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return Stdev(source.Select(S => selector(S)), blockSize);
    }

    /// <summary>
    /// Computes the standard deviation of blocks of elements in a sequence of Nullable&lt;System.Single&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Single&gt; values to calculate the standard deviation of.</param>
    /// <param name="blockSize">The number of elements in the standard deviation block. Block size must be two or larger.</param>
    /// <returns>The standard deviation in the sequence of values.</returns>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<float?> Stdev<TSource>(this IEnumerable<TSource> source, int blockSize, Func<TSource, float?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return Stdev(source.Select(S => selector(S)), blockSize);
    }

    /// <summary>
    /// Computes the standard deviation of blocks of elements in a sequence of System.Decimal values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of System.Decimal values to calculate the standard deviation of.</param>
    /// <param name="blockSize">The number of elements in the standard deviation block. Block size must be two or larger.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>The standard deviation in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<decimal> Stdev<TSource>(this IEnumerable<TSource> source, int blockSize, Func<TSource, decimal> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return Stdev(source.Select(S => selector(S)), blockSize);
    }

    /// <summary>
    /// Computes the standard deviation of blocks of elements in a sequence of Nullable&lt;System.Decimal&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Decimal&gt; values to calculate the standard deviation of.</param>
    /// <param name="blockSize">The number of elements in the standard deviation block. Block size must be two or larger.</param>
    /// <returns>The standard deviation in the sequence of values.</returns>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>  
    public static IEnumerable<decimal?> Stdev<TSource>(this IEnumerable<TSource> source, int blockSize, Func<TSource, decimal?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return Stdev(source.Select(S => selector(S)), blockSize);
    }

    /// <summary>
    /// Computes the standard deviation of blocks of elements in a sequence of System.Int64 values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of System.Int64 values to calculate the standard deviation of.</param>
    /// <param name="blockSize">The number of elements in the standard deviation block. Block size must be two or larger.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>The standard deviation in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double> Stdev<TSource>(this IEnumerable<TSource> source, int blockSize, Func<TSource, long> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return Stdev(source.Select(S => selector(S)), blockSize);
    }

    /// <summary>
    /// Computes the standard deviation of blocks of elements in a sequence of Nullable&lt;System.Int64&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Int64&gt; values to calculate the standard deviation of.</param>
    /// <param name="blockSize">The number of elements in the standard deviation block. Block size must be two or larger.</param>
    /// <returns>The standard deviation in the sequence of values.</returns>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double?> Stdev<TSource>(this IEnumerable<TSource> source, int blockSize, Func<TSource, long?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return Stdev(source.Select(S => selector(S)), blockSize);
    }

    /// <summary>
    /// Computes the standard deviation of blocks of elements in a sequence of System.Int32 values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of System.Int32 values to calculate the standard deviation of.</param>
    /// <param name="blockSize">The number of elements in the standard deviation block. Block size must be two or larger.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <returns>The standard deviation in the sequence of values.</returns>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double> Stdev<TSource>(this IEnumerable<TSource> source, int blockSize, Func<TSource, int> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return Stdev(source.Select(S => selector(S)), blockSize);
    }

    /// <summary>
    /// Computes the standard deviation of blocks of elements in a sequence of Nullable&lt;System.Int32&gt; values that are obtained by invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of Nullable&lt;System.Int32&gt; values to calculate the standard deviation of.</param>
    /// <param name="blockSize">The number of elements in the standard deviation block. Block size must be two or larger.</param>
    /// <returns>The standard deviation in the sequence of values.</returns>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <exception cref="System.ArgumentNullException">source or selector is null.</exception>
    /// <exception cref="System.ArgumentException">Sequence must have at least two values.</exception>
    public static IEnumerable<double?> Stdev<TSource>(this IEnumerable<TSource> source, int blockSize, Func<TSource, int?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      return Stdev(source.Select(S => selector(S)), blockSize);
    }

    #endregion
  }
}