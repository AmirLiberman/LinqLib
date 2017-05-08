using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LinqLib.Operators
{
  /// <summary>
  /// Provides a set of static (Shared in Visual Basic) methods for applying arithmetic operations on objects that implement System.Collections.Generic.IEnumerable&lt;T&gt;. 
  /// </summary>
  public static class Arithmetic
  {
    #region Methods

    /// <summary>
    /// Adds elements of two sequences. If sequences are not equal in length, the returned sequence will be as long as the shorter sequence.
    /// </summary>
    /// <typeparam name="T">The type of the elements of leftSequence and rightSequence.</typeparam>
    /// <param name="leftSequence">The first sequence of values to use in the add operation.</param>
    /// <param name="rightSequence">The second sequence of values to use in the add operation.</param>
    /// <exception cref="System.ArgumentNullException">leftSequence or rightSequence is null</exception>
    /// <exception cref="System.InvalidOperationException">T is not a numeric type.</exception>
    /// <returns>A sequence of values that are the result of adding of the values from the left and right sequences.</returns>
    public static IEnumerable<T> Add<T>(this IEnumerable<T> leftSequence, IEnumerable<T> rightSequence)
    {
      Helper.InvalidateNonNumeric<T>("Add");
      if (leftSequence == null)
        throw Error.ArgumentNull("leftSequence");
      if (rightSequence == null)
        throw Error.ArgumentNull("rightSequence");

      using (IEnumerator<T> l = leftSequence.GetEnumerator())
      using (IEnumerator<T> r = rightSequence.GetEnumerator())
        while (l.MoveNext() && r.MoveNext())
          yield return Helper<T>.Add(l.Current, r.Current);
    }

    /// <summary>
    /// Subtracts elements of two sequences. If sequences are not equal in length, the returned sequence will be as long as the shorter sequence.
    /// </summary>
    /// <typeparam name="T">The type of the elements of leftSequence and rightSequence.</typeparam>
    /// <param name="leftSequence">The first sequence of values to use in the subtract operation.</param>
    /// <param name="rightSequence">The second sequence of values to use in the subtract operation.</param>
    /// <exception cref="System.ArgumentNullException">leftSequence or rightSequence is null</exception>
    /// <exception cref="System.InvalidOperationException">T is not a numeric type.</exception>
    /// <returns>A sequence of values that are the result of subtracting of the values of the right sequence from the left sequence.</returns>
    public static IEnumerable<T> Subtract<T>(this IEnumerable<T> leftSequence, IEnumerable<T> rightSequence)
    {
      Helper.InvalidateNonNumeric<T>("Subtract");
      if (leftSequence == null)
        throw Error.ArgumentNull("leftSequence");
      if (leftSequence == null)
        throw Error.ArgumentNull("leftSequence");

      using (IEnumerator<T> l = leftSequence.GetEnumerator())
      using (IEnumerator<T> r = rightSequence.GetEnumerator())
        while (l.MoveNext() && r.MoveNext())
          yield return Helper<T>.Subtract(l.Current, r.Current);
    }

    /// <summary>
    /// Multiplies elements of two sequences. If sequences are not equal in length, the returned sequence will be as long as the shorter sequence.
    /// </summary>
    /// <typeparam name="T">The type of the elements of leftSequence and rightSequence.</typeparam>
    /// <param name="leftSequence">The first sequence of values to use in the multiply operation.</param>
    /// <param name="rightSequence">The second sequence of values to use in the multiply operation.</param>
    /// <exception cref="System.ArgumentNullException">leftSequence or rightSequence is null</exception>
    /// <exception cref="System.InvalidOperationException">T is not a numeric type.</exception>
    /// <returns>A sequence of values that are the result of multiplying of the values from the left sequence with the right sequence.</returns>
    public static IEnumerable<T> Multiply<T>(this IEnumerable<T> leftSequence, IEnumerable<T> rightSequence)
    {
      Helper.InvalidateNonNumeric<T>("Multiply");
      if (leftSequence == null)
        throw Error.ArgumentNull("leftSequence");
      if (rightSequence == null)
        throw Error.ArgumentNull("rightSequence");

      using (IEnumerator<T> l = leftSequence.GetEnumerator())
      using (IEnumerator<T> r = rightSequence.GetEnumerator())
        while (l.MoveNext() && r.MoveNext())
          yield return Helper<T>.Multiply(l.Current, r.Current);
    }

    /// <summary>
    /// Divides elements of two sequences. If sequences are not equal in length, the returned sequence will be as long as the shorter sequence.
    /// </summary>
    /// <typeparam name="T">The type of the elements of leftSequence and rightSequence.</typeparam>
    /// <param name="leftSequence">The first sequence of values to use in the divide operation.</param>
    /// <param name="rightSequence">The second sequence of values to use in the divide operation.</param>
    /// <exception cref="System.ArgumentNullException">leftSequence or rightSequence is null</exception>
    /// <exception cref="System.InvalidOperationException">T is not a numeric type.</exception>
    /// <returns>A sequence of values that are the result of dividing of the values from the right sequence with the left sequence.</returns>
    public static IEnumerable<T> Divide<T>(this IEnumerable<T> leftSequence, IEnumerable<T> rightSequence)
    {
      Helper.InvalidateNonNumeric<T>("Divide");
      if (leftSequence == null)
        throw Error.ArgumentNull("leftSequence");
      if (rightSequence == null)
        throw Error.ArgumentNull("rightSequence");

      using (IEnumerator<T> l = leftSequence.GetEnumerator())
      using (IEnumerator<T> r = rightSequence.GetEnumerator())
        while (l.MoveNext() && r.MoveNext())
          yield return Helper<T>.Divide(l.Current, r.Current);

    }

    #endregion

    #region Apply Function Overloads

    // 2
    /// <summary>
    /// Applies a user supplied function to elements from two sequences and returns a sequence of results. If sequences are not equal in length, the returned sequence will be as long as the shorter sequence.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in the source sequences.</typeparam>
    /// <typeparam name="TResult">Type of elements in the results sequence.</typeparam>
    /// <param name="sources">A set of sequences to with elements to use as inputs for the supplied function.</param>
    /// <param name="func">The function to use to on the source elements.</param>
    /// <returns>A sequence of results returned by the custom function.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    public static IEnumerable<TResult> ApplyFunction<TSource, TResult>(this IEnumerable<IEnumerable<TSource>> sources, Func<TSource, TSource, TResult> func)
    {
      Helper.InvalidateEnumeratedParam(sources, 2, "sources");
      if (func == null)
        throw Error.ArgumentNull("func");

      IEnumerator<TSource>[] rators = sources.Select(S => S.GetEnumerator()).ToArray();

      try
      {
        while (MoveAll(rators))
          yield return func(rators[0].Current, rators[1].Current);
      }
      finally
      {
        rators.Where(R => R != null).ForEach(R => R.Dispose());
      }
    }

    // 3
    /// <summary>
    /// Applies a user supplied function to elements from three sequences and returns a sequence of results. If sequences are not equal in length, the returned sequence will be as long as the shorter sequence.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in the source sequences.</typeparam>
    /// <typeparam name="TResult">Type of elements in the results sequence.</typeparam>
    /// <param name="sources">A set of sequences to with elements to use as inputs for the supplied function.</param>
    /// <param name="func">The function to use to on the source elements.</param>
    /// <returns>A sequence of results returned by the custom function.</returns>
    public static IEnumerable<TResult> ApplyFunction<TSource, TResult>(this IEnumerable<IEnumerable<TSource>> sources, Func<TSource, TSource, TSource, TResult> func)
    {
      Helper.InvalidateEnumeratedParam(sources, 3, "sources");
      if (func == null)
        throw Error.ArgumentNull("func");

      IEnumerator<TSource>[] rators = sources.Select(S => S.GetEnumerator()).ToArray();

      try
      {
        while (MoveAll(rators))
          yield return func(rators[0].Current, rators[1].Current, rators[2].Current);
      }
      finally
      {
        rators.Where(R => R != null).ForEach(R => R.Dispose());
      }
    }

    // 4
    /// <summary>
    /// Applies a user supplied function to elements from four sequences and returns a sequence of results. If sequences are not equal in length, the returned sequence will be as long as the shorter sequence.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in the source sequences.</typeparam>
    /// <typeparam name="TResult">Type of elements in the results sequence.</typeparam>
    /// <param name="sources">A set of sequences to with elements to use as inputs for the supplied function.</param>
    /// <param name="func">The function to use to on the source elements.</param>
    /// <returns>A sequence of results returned by the custom function.</returns>
    public static IEnumerable<TResult> ApplyFunction<TSource, TResult>(this IEnumerable<IEnumerable<TSource>> sources, Func<TSource, TSource, TSource, TSource, TResult> func)
    {
      Helper.InvalidateEnumeratedParam(sources, 4, "sources");
      if (func == null)
        throw Error.ArgumentNull("func");

      IEnumerator<TSource>[] rators = sources.Select(S => S.GetEnumerator()).ToArray();

      try
      {
        while (MoveAll(rators))
          yield return func(rators[0].Current, rators[1].Current, rators[2].Current, rators[3].Current);
      }
      finally
      {
        rators.Where(R => R != null).ForEach(R => R.Dispose());
      }
    }

    // 5
    /// <summary>
    /// Applies a user supplied function to elements from five sequences and returns a sequence of results. If sequences are not equal in length, the returned sequence will be as long as the shorter sequence.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in the source sequences.</typeparam>
    /// <typeparam name="TResult">Type of elements in the results sequence.</typeparam>
    /// <param name="sources">A set of sequences to with elements to use as inputs for the supplied function.</param>
    /// <param name="func">The function to use to on the source elements.</param>
    /// <returns>A sequence of results returned by the custom function.</returns>
    public static IEnumerable<TResult> ApplyFunction<TSource, TResult>(this IEnumerable<IEnumerable<TSource>> sources, Func<TSource, TSource, TSource, TSource,
      TSource, TResult> func)
    {
      Helper.InvalidateEnumeratedParam(sources, 5, "sources");
      if (func == null)
        throw Error.ArgumentNull("func");

      IEnumerator<TSource>[] rators = sources.Select(S => S.GetEnumerator()).ToArray();

      try
      {
        while (MoveAll(rators))
          yield return func(rators[0].Current, rators[1].Current, rators[2].Current, rators[3].Current, rators[4].Current);
      }
      finally
      {
        rators.Where(R => R != null).ForEach(R => R.Dispose());
      }
    }

    // 6
    /// <summary>
    /// Applies a user supplied function to elements from six sequences and returns a sequence of results. If sequences are not equal in length, the returned sequence will be as long as the shorter sequence.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in the source sequences.</typeparam>
    /// <typeparam name="TResult">Type of elements in the results sequence.</typeparam>
    /// <param name="sources">A set of sequences to with elements to use as inputs for the supplied function.</param>
    /// <param name="func">The function to use to on the source elements.</param>
    /// <returns>A sequence of results returned by the custom function.</returns>
    public static IEnumerable<TResult> ApplyFunction<TSource, TResult>(this IEnumerable<IEnumerable<TSource>> sources, Func<TSource, TSource, TSource, TSource,
      TSource, TSource, TResult> func)
    {
      Helper.InvalidateEnumeratedParam(sources, 6, "sources");
      if (func == null)
        throw Error.ArgumentNull("func");

      IEnumerator<TSource>[] rators = sources.Select(S => S.GetEnumerator()).ToArray();

      try
      {
        while (MoveAll(rators))
          yield return func(rators[0].Current, rators[1].Current, rators[2].Current, rators[3].Current, rators[4].Current, rators[5].Current);
      }
      finally
      {
        rators.Where(R => R != null).ForEach(R => R.Dispose());
      }
    }

    // 7
    /// <summary>
    /// Applies a user supplied function to elements from seven sequences and returns a sequence of results. If sequences are not equal in length, the returned sequence will be as long as the shorter sequence.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in the source sequences.</typeparam>
    /// <typeparam name="TResult">Type of elements in the results sequence.</typeparam>
    /// <param name="sources">A set of sequences to with elements to use as inputs for the supplied function.</param>
    /// <param name="func">The function to use to on the source elements.</param>
    /// <returns>A sequence of results returned by the custom function.</returns>
    public static IEnumerable<TResult> ApplyFunction<TSource, TResult>(this IEnumerable<IEnumerable<TSource>> sources, Func<TSource, TSource, TSource, TSource,
      TSource, TSource, TSource, TResult> func)
    {
      Helper.InvalidateEnumeratedParam(sources, 7, "sources");
      if (func == null)
        throw Error.ArgumentNull("func");

      IEnumerator<TSource>[] rators = sources.Select(S => S.GetEnumerator()).ToArray();

      try
      {
        while (MoveAll(rators))
          yield return func(rators[0].Current, rators[1].Current, rators[2].Current, rators[3].Current, rators[4].Current, rators[5].Current, rators[6].Current);
      }
      finally
      {
        rators.Where(R => R != null).ForEach(R => R.Dispose());
      }
    }

    // 8
    /// <summary>
    /// Applies a user supplied function to elements from eight sequences and returns a sequence of results. If sequences are not equal in length, the returned sequence will be as long as the shorter sequence.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in the source sequences.</typeparam>
    /// <typeparam name="TResult">Type of elements in the results sequence.</typeparam>
    /// <param name="sources">A set of sequences to with elements to use as inputs for the supplied function.</param>
    /// <param name="func">The function to use to on the source elements.</param>
    /// <returns>A sequence of results returned by the custom function.</returns>
    public static IEnumerable<TResult> ApplyFunction<TSource, TResult>(this IEnumerable<IEnumerable<TSource>> sources, Func<TSource, TSource, TSource, TSource,
      TSource, TSource, TSource, TSource, TResult> func)
    {
      Helper.InvalidateEnumeratedParam(sources, 8, "sources");
      if (func == null)
        throw Error.ArgumentNull("func");

      IEnumerator<TSource>[] rators = sources.Select(S => S.GetEnumerator()).ToArray();

      try
      {
        while (MoveAll(rators))
          yield return func(rators[0].Current, rators[1].Current, rators[2].Current, rators[3].Current, rators[4].Current, rators[5].Current, rators[6].Current,
            rators[7].Current);
      }
      finally
      {
        rators.Where(R => R != null).ForEach(R => R.Dispose());
      }
    }

    // 9
    /// <summary>
    /// Applies a user supplied function to elements from nine sequences and returns a sequence of results. If sequences are not equal in length, the returned sequence will be as long as the shorter sequence.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in the source sequences.</typeparam>
    /// <typeparam name="TResult">Type of elements in the results sequence.</typeparam>
    /// <param name="sources">A set of sequences to with elements to use as inputs for the supplied function.</param>
    /// <param name="func">The function to use to on the source elements.</param>
    /// <returns>A sequence of results returned by the custom function.</returns>
    public static IEnumerable<TResult> ApplyFunction<TSource, TResult>(this IEnumerable<IEnumerable<TSource>> sources, Func<TSource, TSource, TSource, TSource,
      TSource, TSource, TSource, TSource, TSource, TResult> func)
    {
      Helper.InvalidateEnumeratedParam(sources, 9, "sources");
      if (func == null)
        throw Error.ArgumentNull("func");

      IEnumerator<TSource>[] rators = sources.Select(S => S.GetEnumerator()).ToArray();

      try
      {
        while (MoveAll(rators))
          yield return func(rators[0].Current, rators[1].Current, rators[2].Current, rators[3].Current, rators[4].Current, rators[5].Current, rators[6].Current,
            rators[7].Current, rators[8].Current);
      }
      finally
      {
        rators.Where(R => R != null).ForEach(R => R.Dispose());
      }
    }

    // 10
    /// <summary>
    /// Applies a user supplied function to elements from 10 sequences and returns a sequence of results. If sequences are not equal in length, the returned sequence will be as long as the shorter sequence.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in the source sequences.</typeparam>
    /// <typeparam name="TResult">Type of elements in the results sequence.</typeparam>
    /// <param name="sources">A set of sequences to with elements to use as inputs for the supplied function.</param>
    /// <param name="func">The function to use to on the source elements.</param>
    /// <returns>A sequence of results returned by the custom function.</returns>
    public static IEnumerable<TResult> ApplyFunction<TSource, TResult>(this IEnumerable<IEnumerable<TSource>> sources, Func<TSource, TSource, TSource, TSource,
      TSource, TSource, TSource, TSource, TSource, TSource, TResult> func)
    {
      Helper.InvalidateEnumeratedParam(sources, 10, "sources");
      if (func == null)
        throw Error.ArgumentNull("func");

      IEnumerator<TSource>[] rators = sources.Select(S => S.GetEnumerator()).ToArray();

      try
      {
        while (MoveAll(rators))
          yield return func(rators[0].Current, rators[1].Current, rators[2].Current, rators[3].Current, rators[4].Current, rators[5].Current, rators[6].Current,
            rators[7].Current, rators[8].Current, rators[9].Current);
      }
      finally
      {
        rators.Where(R => R != null).ForEach(R => R.Dispose());
      }
    }

    // 11
    /// <summary>
    /// Applies a user supplied function to elements from 11 sequences and returns a sequence of results. If sequences are not equal in length, the returned sequence will be as long as the shorter sequence.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in the source sequences.</typeparam>
    /// <typeparam name="TResult">Type of elements in the results sequence.</typeparam>
    /// <param name="sources">A set of sequences to with elements to use as inputs for the supplied function.</param>
    /// <param name="func">The function to use to on the source elements.</param>
    /// <returns>A sequence of results returned by the custom function.</returns>
    public static IEnumerable<TResult> ApplyFunction<TSource, TResult>(this IEnumerable<IEnumerable<TSource>> sources, Func<TSource, TSource, TSource, TSource,
      TSource, TSource, TSource, TSource, TSource, TSource, TSource, TResult> func)
    {
      Helper.InvalidateEnumeratedParam(sources, 11, "sources");
      if (func == null)
        throw Error.ArgumentNull("func");

      IEnumerator<TSource>[] rators = sources.Select(S => S.GetEnumerator()).ToArray();

      try
      {
        while (MoveAll(rators))
          yield return func(rators[0].Current, rators[1].Current, rators[2].Current, rators[3].Current, rators[4].Current, rators[5].Current, rators[6].Current,
            rators[7].Current, rators[8].Current, rators[9].Current, rators[10].Current);
      }
      finally
      {
        rators.Where(R => R != null).ForEach(R => R.Dispose());
      }
    }

    // 12
    /// <summary>
    /// Applies a user supplied function to elements from 12 sequences and returns a sequence of results. If sequences are not equal in length, the returned sequence will be as long as the shorter sequence.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in the source sequences.</typeparam>
    /// <typeparam name="TResult">Type of elements in the results sequence.</typeparam>
    /// <param name="sources">A set of sequences to with elements to use as inputs for the supplied function.</param>
    /// <param name="func">The function to use to on the source elements.</param>
    /// <returns>A sequence of results returned by the custom function.</returns>
    public static IEnumerable<TResult> ApplyFunction<TSource, TResult>(this IEnumerable<IEnumerable<TSource>> sources, Func<TSource, TSource, TSource, TSource,
      TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TResult> func)
    {
      Helper.InvalidateEnumeratedParam(sources, 12, "sources");
      if (func == null)
        throw Error.ArgumentNull("func");

      IEnumerator<TSource>[] rators = sources.Select(S => S.GetEnumerator()).ToArray();

      try
      {
        while (MoveAll(rators))
          yield return func(rators[0].Current, rators[1].Current, rators[2].Current, rators[3].Current, rators[4].Current, rators[5].Current, rators[6].Current,
            rators[7].Current, rators[8].Current, rators[9].Current, rators[10].Current, rators[11].Current);
      }
      finally
      {
        rators.Where(R => R != null).ForEach(R => R.Dispose());
      }
    }

    // 13
    /// <summary>
    /// Applies a user supplied function to elements from 13 sequences and returns a sequence of results. If sequences are not equal in length, the returned sequence will be as long as the shorter sequence.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in the source sequences.</typeparam>
    /// <typeparam name="TResult">Type of elements in the results sequence.</typeparam>
    /// <param name="sources">A set of sequences to with elements to use as inputs for the supplied function.</param>
    /// <param name="func">The function to use to on the source elements.</param>
    /// <returns>A sequence of results returned by the custom function.</returns>
    public static IEnumerable<TResult> ApplyFunction<TSource, TResult>(this IEnumerable<IEnumerable<TSource>> sources, Func<TSource, TSource, TSource, TSource,
      TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TResult> func)
    {
      Helper.InvalidateEnumeratedParam(sources, 13, "sources");
      if (func == null)
        throw Error.ArgumentNull("func");

      IEnumerator<TSource>[] rators = sources.Select(S => S.GetEnumerator()).ToArray();

      try
      {
        while (MoveAll(rators))
          yield return func(rators[0].Current, rators[1].Current, rators[2].Current, rators[3].Current, rators[4].Current, rators[5].Current, rators[6].Current,
            rators[7].Current, rators[8].Current, rators[9].Current, rators[10].Current, rators[11].Current, rators[12].Current);
      }
      finally
      {
        rators.Where(R => R != null).ForEach(R => R.Dispose());
      }
    }

    // 14
    /// <summary>
    /// Applies a user supplied function to elements from 14 sequences and returns a sequence of results. If sequences are not equal in length, the returned sequence will be as long as the shorter sequence.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in the source sequences.</typeparam>
    /// <typeparam name="TResult">Type of elements in the results sequence.</typeparam>
    /// <param name="sources">A set of sequences to with elements to use as inputs for the supplied function.</param>
    /// <param name="func">The function to use to on the source elements.</param>
    /// <returns>A sequence of results returned by the custom function.</returns>
    public static IEnumerable<TResult> ApplyFunction<TSource, TResult>(this IEnumerable<IEnumerable<TSource>> sources, Func<TSource, TSource, TSource, TSource,
      TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TResult> func)
    {
      Helper.InvalidateEnumeratedParam(sources, 14, "sources");
      if (func == null)
        throw Error.ArgumentNull("func");

      IEnumerator<TSource>[] rators = sources.Select(S => S.GetEnumerator()).ToArray();

      try
      {
        while (MoveAll(rators))
          yield return func(rators[0].Current, rators[1].Current, rators[2].Current, rators[3].Current, rators[4].Current, rators[5].Current, rators[6].Current,
            rators[7].Current, rators[8].Current, rators[9].Current, rators[10].Current, rators[11].Current, rators[12].Current, rators[13].Current);
      }
      finally
      {
        rators.Where(R => R != null).ForEach(R => R.Dispose());
      }
    }

    // 15
    /// <summary>
    /// Applies a user supplied function to elements from 15 sequences and returns a sequence of results. If sequences are not equal in length, the returned sequence will be as long as the shorter sequence.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in the source sequences.</typeparam>
    /// <typeparam name="TResult">Type of elements in the results sequence.</typeparam>
    /// <param name="sources">A set of sequences to with elements to use as inputs for the supplied function.</param>
    /// <param name="func">The function to use to on the source elements.</param>
    /// <returns>A sequence of results returned by the custom function.</returns>
    public static IEnumerable<TResult> ApplyFunction<TSource, TResult>(this IEnumerable<IEnumerable<TSource>> sources, Func<TSource, TSource, TSource, TSource,
      TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TResult> func)
    {
      Helper.InvalidateEnumeratedParam(sources, 15, "sources");
      if (func == null)
        throw Error.ArgumentNull("func");

      IEnumerator<TSource>[] rators = sources.Select(S => S.GetEnumerator()).ToArray();

      try
      {
        while (MoveAll(rators))
          yield return func(rators[0].Current, rators[1].Current, rators[2].Current, rators[3].Current, rators[4].Current, rators[5].Current, rators[6].Current,
            rators[7].Current, rators[8].Current, rators[9].Current, rators[10].Current, rators[11].Current, rators[12].Current, rators[13].Current,
            rators[14].Current);
      }
      finally
      {
        rators.Where(R => R != null).ForEach(R => R.Dispose());
      }
    }

    // 16
    /// <summary>
    /// Applies a user supplied function to elements from 16 sequences and returns a sequence of results. If sequences are not equal in length, the returned sequence will be as long as the shorter sequence.
    /// </summary>
    /// <typeparam name="TSource">Type of elements in the source sequences.</typeparam>
    /// <typeparam name="TResult">Type of elements in the results sequence.</typeparam>
    /// <param name="sources">A set of sequences to with elements to use as inputs for the supplied function.</param>
    /// <param name="func">The function to use to on the source elements.</param>
    /// <returns>A sequence of results returned by the custom function.</returns>
    public static IEnumerable<TResult> ApplyFunction<TSource, TResult>(this IEnumerable<IEnumerable<TSource>> sources, Func<TSource, TSource, TSource, TSource,
      TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TResult> func)
    {
      Helper.InvalidateEnumeratedParam(sources, 16, "sources");
      if (func == null)
        throw Error.ArgumentNull("func");

      IEnumerator<TSource>[] rators = sources.Select(S => S.GetEnumerator()).ToArray();

      try
      {
        while (MoveAll(rators))
          yield return func(rators[0].Current, rators[1].Current, rators[2].Current, rators[3].Current, rators[4].Current, rators[5].Current, rators[6].Current,
            rators[7].Current, rators[8].Current, rators[9].Current, rators[10].Current, rators[11].Current, rators[12].Current, rators[13].Current,
            rators[14].Current, rators[15].Current);
      }
      finally
      {
        rators.Where(R => R != null).ForEach(R => R.Dispose());
      }
    }

    private static bool MoveAll<TSource>(IEnumerable<IEnumerator<TSource>> rators)
    {
      return rators.All(R => R.MoveNext());
    }

    #endregion

    /// <summary>
    /// Static class that exposes complied expressions representing the four arithmetic operations.
    /// </summary>
    /// <typeparam name="T">The type of the elements to operate on.</typeparam>
    internal static class Helper<T>
    {
      #region Fields

      /// <summary>
      /// Add operation function.
      /// </summary>
      public static Func<T, T, T> Add = InitAdd();
      /// <summary>
      /// Subtract operation function.
      /// </summary>
      public static Func<T, T, T> Subtract = InitSubtract();
      /// <summary>
      /// Multiply operation function. 
      /// </summary>
      public static Func<T, T, T> Multiply = InitMultiply();
      /// <summary>
      /// Divide operation function.
      /// </summary>
      public static Func<T, T, T> Divide = InitDivide();

      #endregion

      #region Methods

      /// <summary>
      /// Add operation function initializer.
      /// </summary>
      /// <returns>Add operation function.</returns>
      private static Func<T, T, T> InitAdd()
      {
        Type type = typeof(T);
        ParameterExpression left = Expression.Parameter(type, "left");
        ParameterExpression right = Expression.Parameter(type, "right");
        return Expression.Lambda<Func<T, T, T>>(Expression.Add(left, right), left, right).Compile();
      }

      /// <summary>
      /// Subtract operation function initializer.
      /// </summary>
      /// <returns>Subtract operation function.</returns>
      private static Func<T, T, T> InitSubtract()
      {
        Type type = typeof(T);
        ParameterExpression left = Expression.Parameter(type, "left");
        ParameterExpression right = Expression.Parameter(type, "right");
        return Expression.Lambda<Func<T, T, T>>(Expression.Subtract(left, right), left, right).Compile();
      }

      /// <summary>
      /// Multiply operation function initializer.
      /// </summary>
      /// <returns>Multiply operation function.</returns>
      private static Func<T, T, T> InitMultiply()
      {
        Type type = typeof(T);
        ParameterExpression left = Expression.Parameter(type, "left");
        ParameterExpression right = Expression.Parameter(type, "right");
        return Expression.Lambda<Func<T, T, T>>(Expression.Multiply(left, right), left, right).Compile();
      }

      /// <summary>
      /// Divide operation function initializer.
      /// </summary>
      /// <returns>Divide operation function.</returns>
      private static Func<T, T, T> InitDivide()
      {
        Type type = typeof(T);
        ParameterExpression left = Expression.Parameter(type, "left");
        ParameterExpression right = Expression.Parameter(type, "right");
        return Expression.Lambda<Func<T, T, T>>(Expression.Divide(left, right), left, right).Compile();
      }

      #endregion
    }
  }
}

