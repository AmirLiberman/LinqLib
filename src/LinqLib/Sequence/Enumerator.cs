using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LinqLib.Operators;

namespace LinqLib.Sequence
{
  /// <summary>
  /// Generates new sequences.
  /// </summary>
  public static class Enumerator
  {
    #region Shuffle

    /// <summary>
    /// Randomly rearrange elements in a sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of the source sequence.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <returns>A random sequence of all elements in the source sequence. </returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    public static IEnumerable<TSource> Shuffle<TSource>(this IEnumerable<TSource> source)
    {
      Random rnd = new Random(Guid.NewGuid().GetHashCode());
      return Shuffle(source, rnd.Next());
    }

    /// <summary>
    /// Randomly rearrange elements in a sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of the source sequence.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="seed">A seed number to provide to the randomizer.</param>
    /// <returns>A random sequence of all elements in the source sequence. </returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    public static IEnumerable<TSource> Shuffle<TSource>(this IEnumerable<TSource> source, int seed)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      Random rnd = new Random(seed);
      TSource[] items = source.ToArray();
      int count = items.Length;

      for (int i = 0; i < count; i++)
      {
        int r = rnd.Next(i, count);
        yield return items[r];
        items[r] = items[i];
      }
    }

    #endregion

    #region Rotate and Cycle

    /// <summary>
    /// Rotates N elements in the source to the left.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of the source sequence.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="items">the number of elements to rotate.</param>
    /// <returns>The rotated version of the source sequence. </returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <remarks>Calling this method to rotate 2 elements in the a,b,c,d,e, sequence will return c,d,e,a,b.</remarks>
    public static IEnumerable<TSource> RotateLeft<TSource>(this IEnumerable<TSource> source, int items)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      TSource[] sourceArr = source.ToArray();
      foreach (TSource item in sourceArr.Skip(items))
        yield return item;

      foreach (TSource item in sourceArr.Take(items))
        yield return item;
    }

    /// <summary>
    /// Rotates N elements in the source to the right.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of the source sequence.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="items">the number of elements to rotate.</param>
    /// <returns>The rotated version of the source sequence. </returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <remarks>Calling this method to rotate 2 elements in the a,b,c,d,e, sequence will return d,e,a,b,c.</remarks>
    public static IEnumerable<TSource> RotateRight<TSource>(this IEnumerable<TSource> source, int items)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      TSource[] sourceArr = source.ToArray();
      int offset = sourceArr.Count() - items;

      return sourceArr.RotateLeft(offset);
    }

    /// <summary>
    /// Cycles through the source sequence. 
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of the source sequence.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <returns>Endless list of items repeating the source sequence.</returns>
    ///<exception cref="System.ArgumentNullException">source is null.</exception>
    public static IEnumerable<TSource> Cycle<TSource>(this IEnumerable<TSource> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      TSource[] sourceArr = source.ToArray();
      while (true)
      {
        foreach (TSource item in sourceArr)
          yield return item;
      }
    }

    /// <summary>
    /// Cycles through the source sequence. 
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of the source sequence.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="cycles">The number of times to cycle through the source sequence.</param>
    /// <returns>A list of items repeating the source sequence as indicated by the cycles parameter.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    public static IEnumerable<TSource> Cycle<TSource>(this IEnumerable<TSource> source, int cycles)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      TSource[] sourceArr = source.ToArray();

      while (cycles > 0)
      {
        cycles--;
        foreach (TSource item in sourceArr)
          yield return item;
      }
    }

    #endregion

    #region Sequence Generators

    /// <summary>
    /// Generates a sequence of elements.
    /// </summary>
    /// <typeparam name="T">The type of the elements of to generate.</typeparam>
    /// <param name="count">The number of elements to generate.</param>
    /// <param name="generator">A generator function that take no parameters and return a value/instance of type T.</param>
    /// <returns>A sequence of elements of the type of T.</returns>
    /// <exception cref="System.ArgumentNullException">generator is null.</exception>
    public static IEnumerable<T> Generate<T>(int count, Func<T> generator)
    {
      if (generator == null)
        throw Error.ArgumentNull("generator");

      for (int i = 1; i <= count; i++)
        yield return generator();
    }

    /// <summary>
    /// Generates a sequence of elements.
    /// </summary>
    /// <typeparam name="T">The type of the elements of to generate.</typeparam>
    /// <param name="count">The number of elements to generate.</param>
    /// <param name="generator">A generator function that take the index of item being generated as a parameter and return a value/instance of type T.</param>
    /// <returns>A sequence of elements of the type of T.</returns>
    /// <exception cref="System.ArgumentNullException">generator is null.</exception>
    public static IEnumerable<T> Generate<T>(int count, Func<int, T> generator)
    {
      return Generate(1, count, generator);
    }

    /// <summary>
    /// Generates a sequence of elements.
    /// </summary>
    /// <typeparam name="T">The type of the elements of to generate.</typeparam>
    /// <param name="start">The stating index offset.</param>
    /// <param name="count">The number of elements to generate.</param>
    /// <param name="generator">A generator function that take the index + start offset of item being generated as a parameter and return a value/instance of type T.</param>
    /// <returns>A sequence of elements of the type of T.</returns>
    /// <exception cref="System.ArgumentNullException">generator is null.</exception>
    public static IEnumerable<T> Generate<T>(int start, int count, Func<int, T> generator)
    {
      if (generator == null)
        throw Error.ArgumentNull("generator");

      int end = count + start;
      for (int i = start; i < end; i++)
        yield return generator(i);
    }

    /// <summary>
    /// Generates a sequence of elements.
    /// </summary>
    /// <typeparam name="T">The type of the elements of to generate.</typeparam>
    /// <param name="start">A starting seed element.</param>
    /// <param name="count">The number of elements to generate.</param>
    /// <param name="generator">A generator function that take starting element and an index of item being generated as a parameter and return a value/instance of type T.</param>
    /// <returns>A sequence of elements of the type of T.</returns>
    /// <exception cref="System.ArgumentNullException">generator is null.</exception>
    public static IEnumerable<T> Generate<T>(T start, int count, Func<T, int, T> generator)
    {
      if (generator == null)
        throw Error.ArgumentNull("generator");

      for (int i = 0; i < count; i++)
        yield return generator(start, i);
    }

    /// <summary>
    /// Generates a sequence of elements.
    /// </summary>
    /// <typeparam name="T">The type of the elements of to generate.</typeparam>
    /// <param name="start">A System.Double starting seed element.</param>
    /// <param name="step">A System.Double stepping value.</param>
    /// <param name="count">The number of elements to generate.</param>
    /// <param name="generator">A generator function that take starting and step element as a parameter and return a value/instance of type T.</param>
    /// <returns>A sequence of elements of the type of T.</returns>
    /// <exception cref="System.ArgumentNullException">generator is null.</exception>
    public static IEnumerable<T> Generate<T>(double start, double step, int count, Func<double, T> generator)
    {
      if (generator == null)
        throw Error.ArgumentNull("generator");

      double end = count * step + start;
      for (double i = start; i < end; i += step)
        yield return generator(i);
    }

    /// <summary>
    /// Generates a sequence of elements.
    /// </summary>
    /// <typeparam name="T">The type of the elements of to generate.</typeparam>
    /// <typeparam name="TStep">The type of the step parameter.</typeparam>
    /// <param name="start">A starting seed element.</param>
    /// <param name="step">A stepping value used by the generator function.</param>
    /// <param name="count">The number of elements to generate.</param>
    /// <param name="generator">A generator function that take starting and step element as a parameter and return a value/instance of type T.</param>
    /// <returns>A sequence of elements of the type of T.</returns>
    /// <exception cref="System.ArgumentNullException">generator is null.</exception>
    public static IEnumerable<T> Generate<T, TStep>(T start, TStep step, int count, Func<T, int, TStep, T> generator)
    {
      if (generator == null)
        throw Error.ArgumentNull("generator");

      for (int i = 0; i < count; i++)
      {
        yield return start;
        start = generator(start, i, step);
      }
    }

    /// <summary>
    /// Generates a sequence of dates.
    /// </summary>
    /// <param name="start">The starting date element.</param>
    /// <param name="step">A stepping time span.</param>
    /// <param name="count">The number of elements to generate.</param>
    /// <returns>A sequence of dates.</returns>
    /// <exception cref="System.ArgumentOutOfRangeException">step is Zero (TimeSpan.Zero).</exception>
    public static IEnumerable<DateTime> Generate(DateTime start, TimeSpan step, int count)
    {
      if (step == TimeSpan.Zero)
        throw Error.StepDuration("step");

      for (int i = 0; i < count; i++)
      {
        yield return start;
        start += step;
      }
    }

    /// <summary>
    /// Generates a sequence of numbers.
    /// </summary>
    /// <typeparam name="T">The type of the elements of to generate - T must be of numeric type.</typeparam>
    /// <param name="start">Sequence start element value.</param>
    /// <param name="step">Sequence increment value.</param>
    /// <param name="count">Number of elements to generate.</param>
    /// <returns>A sequence of numbers</returns>
    /// <exception cref="System.InvalidOperationException">Type of T is not a numeric or nullable numeric type.</exception>
    public static IEnumerable<T> Generate<T>(T start, T step, int count)
    {
      if (!Helper.IsNumeric(typeof(T)))
        throw Error.InvalidGenerateGenericType(typeof(T).Name);

      for (int i = 0; i < count; i++)
      {
        yield return start;
        start = Arithmetic.Helper<T>.Add(start, step);
      }
    }

    /// <summary>
    /// Generates a sequence of numbers.
    /// </summary>
    /// <typeparam name="T">The type of the elements of to generate - T must be of numeric type.</typeparam>
    /// <param name="start">Sequence start element value.</param>
    /// <param name="step">Sequence increment value.</param>
    /// <param name="stop">A value the last generate element cannot exceed.</param>
    /// <returns>A sequence of numbers</returns>
    /// <exception cref="System.InvalidOperationException">Type of T is not a numeric or nullable numeric type.</exception>
    public static IEnumerable<T> Range<T>(T start, T step, T stop)
    {
      int count;

      if (!Helper.IsNumeric(typeof(T)))
        throw Error.InvalidRangeGenericType(typeof(T).Name);

      T temp = Arithmetic.Helper<T>.Subtract(stop, start);
      temp = Arithmetic.Helper<T>.Divide(temp, step);
      checked
      {
        count = Convert.ToInt32(temp) + 1;
      }

      return Generate(start, step, count);
    }

    /// <summary>
    /// Generates a random sequence of numbers.
    /// </summary>
    /// <typeparam name="T">The type of the elements of to generate - T must be of numeric type.</typeparam>
    /// <param name="count">The Number of elements to generate</param>
    /// <returns>A sequence of random numbers</returns>
    /// <exception cref="System.InvalidOperationException">Type of T is not a numeric or nullable numeric type.</exception>
    public static IEnumerable<T> Random<T>(int count)
    {
      return Random<T>(count, Guid.NewGuid().GetHashCode());
    }

    /// <summary>
    /// Generates a random sequence of numbers.
    /// </summary>
    /// <typeparam name="T">The type of the elements of to generate - T must be of numeric type.</typeparam>
    /// <param name="count">The Number of elements to generate</param>
    /// <param name="seed">A seed number to provide to the randomizer.</param>
    /// <returns>A sequence of random numbers</returns>
    /// <exception cref="System.InvalidOperationException">Type of T is not a numeric or nullable numeric type.</exception>
    public static IEnumerable<T> Random<T>(int count, int seed)
    {
      if (!Helper.IsNumeric(typeof(T)))
        throw Error.InvalidGenerateGenericType(typeof(T).Name);

      Random rnd = new Random(seed);
      Type type = typeof(T);

      for (int i = 0; i < count; i++)
      {
        double val = rnd.Next();
        double div = rnd.Next(2, 2000);
        yield return (T)Convert.ChangeType(val / div, type);
      }
    }

    /// <summary>
    /// Generate a sequence that is the repetition of another sequence (creating a pattern).
    /// </summary>
    /// <typeparam name="T">The type of the elements of the source sequence.</typeparam>
    /// <param name="source">The sequence to repeat.</param>
    /// <param name="times">Number of times to repeat the source. </param>
    /// <returns>a sequence that is the repetition of another sequence.</returns>
    public static IEnumerable<T> Repeat<T>(this IEnumerable<T> source, int times)
    {
      if (source == null)
        yield break;

      T[] sourceArr = source.ToArray();
      for (int i = 0; i < times; i++)
        foreach (T item in sourceArr)
          yield return item;
    }

    #endregion

    #region Combine Elements

    /// <summary>
    /// Combines two sequences by applying a custom function on elements from each sequence.
    /// </summary>
    /// <typeparam name="TOutput">The Type of elements in the combined sequence.</typeparam>
    /// <typeparam name="TInput1">The Type of elements in the first sequence.</typeparam>
    /// <typeparam name="TInput2">The Type of elements in the second sequence.</typeparam>
    /// <param name="input1">First sequence to operate on.</param>
    /// <param name="input2">Second sequence to operate on.</param>
    /// <param name="func">A Function that take two parameters of TInput1 and TInput2 and return a TOutput representing the combined value of the inputs..</param>
    /// <exception cref="System.ArgumentNullException">input1, input2 or func is null.</exception>
    /// <returns>
    /// A sequence of elements that are the combination of the two inputs. 
    /// If input1 and input2 are not of the same length the returned sequence will have as many elements as the shorter of the two inputs.
    /// </returns>
    public static IEnumerable<TOutput> Combine<TOutput, TInput1, TInput2>(this IEnumerable<TInput1> input1, IEnumerable<TInput2> input2, Func<TInput1, TInput2, TOutput> func)
    {
      if (input1 == null)
        throw Error.ArgumentNull("input1");
      if (input2 == null)
        throw Error.ArgumentNull("input2");
      if (func == null)
        throw Error.ArgumentNull("func");

      using (IEnumerator<TInput1> e1 = input1.GetEnumerator())
      using (IEnumerator<TInput2> e2 = input2.GetEnumerator())
        while (e1.MoveNext() && e2.MoveNext())
          yield return func(e1.Current, e2.Current);
    }

    #endregion

    #region Concat Elements

    /// <summary>
    /// Appends the supplied parameters to the source sequence.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the source sequence.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="items">List of parameters to append to the source sequence.</param>
    /// <returns>A new sequence with elements from the source followed by the supplied parameters.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>    
    public static IEnumerable<T> Append<T>(this IEnumerable<T> source, params T[] items)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      return source.Concat(items);
    }

    /// <summary>
    /// Appends the source sequence to the supplied parameters.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the source sequence.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="items">List of parameters to insert in front of the source sequence.</param>
    /// <returns>A new sequence with elements the supplied parameters followed by the source sequence.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>    
    public static IEnumerable<T> Prepend<T>(this IEnumerable<T> source, params T[] items)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      return items.Concat(source);
    }

    #endregion

    #region Element Locators

    /// <summary>
    /// Returns the element that yields the minimum value when processed by the selector function. 
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of the source sequence.</typeparam>
    /// <typeparam name="TSelector">The type of the element returned by the selector of the source sequence. TSelector must implement the IComparable interface.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="selector">A function that return a comparable value used to evaluate the minimum sequence value.</param>
    /// <returns>The element that yields the minimum value when processed by the selector function.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>    
    /// <exception cref="System.ArgumentNullException">selector is null.</exception>    
    /// <exception cref="System.ArgumentException">source must have one or more elements.</exception>    
    public static TSource ElementAtMin<TSource, TSelector>(this IEnumerable<TSource> source, Func<TSource, TSelector> selector) where TSelector : IComparable
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");
      Helper.InvalidateEmptySequence(source, "source");

      bool first = true;
      IComparable temp = null;
      TSource minElement = default(TSource);

      foreach (TSource item in source)
      {
        if (first)
        {
          temp = selector(item);
          minElement = item;
          first = false;
          continue;
        }

        IComparable temp2 = selector(item);
        if (temp.CompareTo(temp2) <= 0)
          continue;

        temp = temp2;
        minElement = item;
      }

      return minElement;
    }

    /// <summary>
    /// Returns the element that yields the maximum value when processed by the selector function. 
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of the source sequence.</typeparam>
    /// <typeparam name="TSelector">The type of the element returned by the selector of the source sequence. TSelector must implement the IComparable interface.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="selector">A function that return a comparable value used to evaluate the maximum sequence value.</param>
    /// <returns>The element that yields the maximum value when processed by the selector function.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>    
    /// <exception cref="System.ArgumentNullException">selector is null.</exception>    
    /// <exception cref="System.ArgumentException">source must have one or more elements.</exception>    
    public static TSource ElementAtMax<TSource, TSelector>(this IEnumerable<TSource> source, Func<TSource, TSelector> selector) where TSelector : IComparable
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");
      Helper.InvalidateEmptySequence(source, "source");

      bool first = true;
      IComparable temp = null;
      TSource maxElement = default(TSource);

      foreach (TSource item in source)
      {
        if (first)
        {
          temp = selector(item);
          maxElement = item;
          first = false;
          continue;
        }

        IComparable temp2 = selector(item);

        if (temp.CompareTo(temp2) > 0)
          continue;

        temp = temp2;
        maxElement = item;
      }

      return maxElement;
    }

    /// <summary>
    /// Returns the element that is closest the the average of the values returned by the selector function. 
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of the source sequence.</typeparam>
    /// <typeparam name="TSelector">The type of the element returned by the selector of the source sequence. TSelector must be numeric.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="selector">A function that return a numeric value used to evaluate the average sequence value.</param>
    /// <returns>The element that is closest the the average of the values returned by the selector function.</returns>
    public static TSource ElementAtAverage<TSource, TSelector>(this IEnumerable<TSource> source, Func<TSource, TSelector> selector) where TSelector : IComparable
    {
      return ElementAtAverage(source, selector, AverageMatchType.Closest);
    }

    /// <summary>
    /// Returns the element that is closest the the average of the values returned by the selector function. 
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of the source sequence.</typeparam>
    /// <typeparam name="TSelector">The type of the element returned by the selector of the source sequence. TSelector must be numeric.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="selector">A function that return a numeric value used to evaluate the average sequence value.</param>
    /// <param name="matchType">The type of matching to apply when seeking the closest item.</param>
    /// <returns>The element that is closest the the average of the values returned by the selector function.</returns>
    /// <remarks>Closest is defined by the match type.</remarks>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>    
    /// <exception cref="System.ArgumentNullException">selector is null.</exception>    
    /// <exception cref="System.ArgumentException">source must have one or more elements.</exception>    
    /// <exception cref="System.InvalidOperationException">Selector must return a numeric value.</exception>    
    public static TSource ElementAtAverage<TSource, TSelector>(this IEnumerable<TSource> source, Func<TSource, TSelector> selector, AverageMatchType matchType)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");
      TSource[] sourceArr = source.ToArray();
      Helper.InvalidateEmptySequence(sourceArr, "source");
      Helper.InvalidateNonNumeric<TSelector>("ElementAtAverage");

      TSelector tempSum = default(TSelector);
      int count = 0;

      foreach (TSource item in sourceArr)
      {
        if (count == 0)
        {
          tempSum = selector(item);
          count = 1;
          continue;
        }

        tempSum = Arithmetic.Helper<TSelector>.Add(tempSum, selector(item));
        count++;
      }

      decimal ts = Convert.ToDecimal(tempSum);
      decimal tempAvg = ts / count;

      TSource minElement = default(TSource);
      TSource maxElement = default(TSource);
      decimal minValue = 0;
      decimal maxValue = 0;

      bool firstMin = true;
      bool firstMax = true;

      foreach (TSource item in sourceArr)
      {
        decimal temp = Convert.ToDecimal(selector(item));

        if (temp.CompareTo(tempAvg) <= 0)
          if (firstMin || minValue.CompareTo(temp) <= 0)
          {
            firstMin = false;
            minValue = temp;
            minElement = item;
          }

        if (temp.CompareTo(tempAvg) >= 0)
          if (firstMax || maxValue.CompareTo(temp) >= 0)
          {
            firstMax = false;
            maxValue = temp;
            maxElement = item;
          }
      }

      decimal distanceOfMaxValue = maxValue - tempAvg;
      decimal distanceOfMinValue = tempAvg - minValue;

      switch (matchType)
      {
        case AverageMatchType.Exact:
          if (distanceOfMaxValue == 0)
            return maxElement;
          return default(TSource);
        case AverageMatchType.Closest:
          if (distanceOfMaxValue < distanceOfMinValue)
            return maxElement;
          return minElement;
        case AverageMatchType.ExactOrLarger:
          return maxElement;
        case AverageMatchType.ExactOrSmaller:
          return minElement;
        default:
          throw Error.MatchType(matchType);
      }
    }

    /// <summary>
    /// Documentation
    /// </summary>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <param name="deviations"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static IEnumerable<TSource> ElementsWithinStdDev<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector, double deviations)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      TSource[] sourceArr = source.ToArray();
      Helper.InvalidateEmptySequence(sourceArr, "source");
      Helper.InvalidateNumericRange(deviations, 0, double.MaxValue, "deviations");

      double stdCalc = sourceArr.Stdev(selector) * deviations;
      double avg = sourceArr.Average(selector);
      double min = avg - stdCalc;
      double max = avg + stdCalc;

      return from element in sourceArr
             let x = selector(element)
             where x >= min && x <= max
             select element;
    }

    /// <summary>
    /// Documentation
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <param name="deviations"></param>
    /// <returns></returns>
    public static IEnumerable<TSource> ElementsOutsideStdDev<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector, double deviations)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      TSource[] sourceArr = source.ToArray();

      Helper.InvalidateEmptySequence(sourceArr, "source");
      Helper.InvalidateNumericRange(deviations, 0, double.MaxValue, "deviations");

      double stdCalc = sourceArr.Stdev(selector) * deviations;
      double avg = sourceArr.Average(selector);
      double min = avg - stdCalc;
      double max = avg + stdCalc;

      return from element in sourceArr
             let x = selector(element)
             where x < min || x > max
             select element;
    }

    #endregion

    #region Distinct

    /// <summary>
    /// Returns distinct elements from a sequence by using a user supplied key selector and the default equality comparer to compare values.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <typeparam name="TSelector">The type of the element returned by the key selector function.</typeparam>
    /// <param name="source">The sequence to remove duplicate key elements from.</param>
    /// <param name="selector">A function that return a unique key form a sequence elements. If duplicate keys are generated, subsequent elements will be omitted.</param>
    /// <returns>An System.Collections.Generic.IEnumerable&lt;TSource&gt; that contains elements with distinct keys from the source sequence.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>    
    /// <exception cref="System.ArgumentNullException">selector is null.</exception>    
    public static IEnumerable<TSource> Distinct<TSource, TSelector>(this IEnumerable<TSource> source, Func<TSource, TSelector> selector)
    {
      return Distinct(source, selector, null);
    }

    /// <summary>
    /// Returns distinct elements from a sequence by using a user supplied key selector and the default equality comparer to compare values.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <typeparam name="TSelector">The type of the element returned by the key selector function.</typeparam>
    /// <param name="source">The sequence to remove duplicate key elements from.</param>
    /// <param name="selector">A function that return a unique key form a sequence elements. If duplicate keys are generated, subsequent elements will be omitted.</param>
    /// <param name="comparer">An System.Collections.Generic.IEqualityComparer&lt;TSelector&gt; to compare the values generated by the selector.</param>
    /// <returns>An System.Collections.Generic.IEnumerable&lt;TSource&gt; that contains elements with distinct keys from the source sequence.</returns>    
    /// <exception cref="System.ArgumentNullException">source is null.</exception>    
    /// <exception cref="System.ArgumentNullException">selector is null.</exception>  
    public static IEnumerable<TSource> Distinct<TSource, TSelector>(this IEnumerable<TSource> source, Func<TSource, TSelector> selector, IEqualityComparer<TSelector> comparer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      HashSet<TSelector> set = new HashSet<TSelector>(comparer);
      foreach (TSource item in source)
      {
        if (!set.Add(selector(item)))
          continue;
        yield return item;
      }
    }

    /// <summary>
    /// Returns distinct elements from a sequence along with a value indicating the number of times each element appeared in the sequence by using the default equality comparer to compare values.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">The sequence to remove duplicate and count repeating elements from.</param>
    /// <returns>An System.Collections.Generic.IEnumerable&lt;ItemCounter&lt;TSource&gt;&gt; that contains distinct elements from the source sequence and their repetition count .</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>    
    public static IEnumerable<ItemCounter<TSource>> DistinctWithCount<TSource>(this IEnumerable<TSource> source)
    {
      return DistinctWithCount(source, X => X, null);
    }

    /// <summary>
    /// Returns distinct elements from a sequence along with a value indicating the number of times each element appeared in the sequence by using the supplied comparer to compare values.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">The sequence to remove duplicate and count repeating elements from.</param>
    /// <param name="comparer">An System.Collections.Generic.IEqualityComparer&lt;TSource&gt; to compare values.</param>
    /// <returns>An System.Collections.Generic.IEnumerable&lt;ItemCounter&lt;TSource&gt;&gt; that contains distinct elements from the source sequence and their repetition count .</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>    
    public static IEnumerable<ItemCounter<TSource>> DistinctWithCount<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
    {
      return DistinctWithCount(source, X => X, comparer);
    }

    /// <summary>
    /// Returns distinct elements from a sequence along with a value indicating the number of times each element appeared in the sequence by using a user supplied key selector and the default equality comparer to compare values.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <typeparam name="TSelector">The type of the element returned by the key selector function.</typeparam>
    /// <param name="source">The sequence to remove duplicate and count repeating elements from.</param>
    /// <param name="selector">A function that return a unique key form a sequence elements. If duplicate keys are generated, subsequent elements will be omitted.</param>
    /// <returns>An System.Collections.Generic.IEnumerable&lt;ItemCounter&lt;TSource&gt;&gt; that contains distinct elements from the source sequence and their repetition count .</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>    
    public static IEnumerable<ItemCounter<TSource>> DistinctWithCount<TSource, TSelector>(this IEnumerable<TSource> source, Func<TSource, TSelector> selector)
    {
      return DistinctWithCount(source, selector, null);
    }

    /// <summary>
    /// Returns distinct elements from a sequence along with a value indicating the number of times each element appeared in the sequence by using a user supplied key selector and the supplied equality comparer to compare values.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <typeparam name="TSelector">The type of the element returned by the key selector function.</typeparam>
    /// <param name="source">The sequence to remove duplicate and count repeating elements from.</param>
    /// <param name="selector">A function that return a unique key form a sequence elements. If duplicate keys are generated, subsequent elements will be omitted.</param>
    /// <param name="comparer">An System.Collections.Generic.IEqualityComparer&lt;TSource&gt; to compare values.</param>
    /// <returns>An System.Collections.Generic.IEnumerable&lt;ItemCounter&lt;TSource&gt;&gt; that contains distinct elements from the source sequence and their repetition count .</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>    
    public static IEnumerable<ItemCounter<TSource>> DistinctWithCount<TSource, TSelector>(this IEnumerable<TSource> source, Func<TSource, TSelector> selector, IEqualityComparer<TSelector> comparer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      Dictionary<TSelector, ItemCounter<TSource>> set = new Dictionary<TSelector, ItemCounter<TSource>>(comparer);
      foreach (TSource item in source)
      {
        TSelector result = selector(item);
        if (set.ContainsKey(result))
          set[result].IncrementCount();
        else
          set.Add(result, new ItemCounter<TSource>(item));
      }

      return set.Values;
    }

    #endregion

    /// <summary>
    /// Documentation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static IEnumerable<IEnumerable<T>> Interleave<T>(this IEnumerable<IEnumerable<T>> source)
    {
      List<IEnumerator<T>> enumerators = new List<IEnumerator<T>>();
      foreach (IEnumerable<T> subSequence in source)
        enumerators.Add(subSequence.GetEnumerator());

      int count = enumerators.Count;
      while (ForwardAllEnumerators(enumerators))
      {
        T[] inner = new T[count];
        for (int i = 0; i < count; i++)
          inner[i] = enumerators[i].Current;
        yield return inner;
      }
    }

    /// <summary>
    /// Documentation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static IEnumerable<IEnumerable<T>> Interleave<T>(this IEnumerable<IEnumerable<T>> source, T defaultValue)
    {
      List<IEnumerator<T>> enumerators = new List<IEnumerator<T>>();
      foreach (IEnumerable<T> subSequence in source)
        enumerators.Add(subSequence.GetEnumerator());

      int count = enumerators.Count;
      BitArray res = ForwardEnumerators(enumerators);
      while (res.Cast<bool>().Any(X => X))
      {
        T[] inner = new T[count];
        for (int i = 0; i < count; i++)
          inner[i] = res[i] ? enumerators[i].Current : defaultValue;
        yield return inner;
        res = ForwardEnumerators(enumerators);
      }
    }

    private static BitArray ForwardEnumerators<T>(IList<IEnumerator<T>> enumerators)
    {
      int count = enumerators.Count;
      BitArray res = new BitArray(count);

      for (int i = 0; i < count; i++)
        res[i] = enumerators[i].MoveNext();

      return res;
    }

    private static bool ForwardAllEnumerators<T>(IList<IEnumerator<T>> enumerators)
    {
      int count = enumerators.Count;
      for (int i = 0; i < count; i++)
        if (!enumerators[i].MoveNext())
          return false;

      return true;
    }
  }
}
