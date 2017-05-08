using System;
using System.Collections.Generic;
using System.Linq;
using LinqLib.Operators;

namespace LinqLib.Sequence
{
  /// <summary>
  /// Provides methods for scrubbing sequences: Rounding, TRuncating, noise removal and filtering.
  /// </summary>
  public static class Scrub
  {
    #region Round

    /// <summary>
    /// Rounds a sequence of double-precision floating-point values to a specified number of fractional digits.
    /// </summary>
    /// <param name="source">A sequence of double-precision floating-point numbers to be rounded.</param>
    /// <param name="digits">The number of fractional digits in the return values.</param>
    /// <returns>A sequence double-precision floating-point values rounded to a specified number of fractional digits</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentOutOfRangeException">digits is less than 0 or greater than 15.</exception>
    public static IEnumerable<double> Round(this IEnumerable<double> source, int digits)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (digits < 0 || digits > 15)
        throw Error.RoundingDigits(15, "digits");

      return source.Select(S => Math.Round(S, digits));
    }

    /// <summary>
    /// Rounds a sequence of single-precision floating-point values to a specified number of fractional digits.
    /// </summary>
    /// <param name="source">A sequence of single-precision floating-point numbers to be rounded.</param>
    /// <param name="digits">The number of fractional digits in the return values.</param>
    /// <returns>A sequence single-precision floating-point values rounded to a specified number of fractional digits</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentOutOfRangeException">digits is less than 0 or greater than 15.</exception>
    public static IEnumerable<float> Round(this IEnumerable<float> source, int digits)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (digits < 0 || digits > 7)
        throw Error.RoundingDigits(7, "digits");

      return source.Select(S => (float)Math.Round(S, digits));
    }

    /// <summary>
    /// Rounds a sequence of decimal values to a specified number of fractional digits.
    /// </summary>
    /// <param name="source">A sequence of decimal numbers to be rounded.</param>
    /// <param name="digits">The number of fractional digits in the return values.</param>
    /// <returns>A sequence decimal values rounded to a specified number of fractional digits</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="System.ArgumentOutOfRangeException">digits is less than 0 or greater than 15.</exception>
    public static IEnumerable<decimal> Round(this IEnumerable<decimal> source, int digits)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (digits < 0 || digits > 28)
        throw Error.RoundingDigits(28, "digits");

      return source.Select(S => Math.Round(S, digits));
    }

    #endregion

    #region Truncate

    /// <summary>
    /// Calculates the integral part of a elements in a sequence of double-precision floating-point numbers.
    /// </summary>
    /// <param name="source">A sequence of numbers to truncate.</param>
    /// <returns>A sequence of the integral numbers.</returns>
    public static IEnumerable<double> Truncate(this IEnumerable<double> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      return source.Select(S => Math.Truncate(S));
    }

    /// <summary>
    /// Calculates the integral part of a elements in a sequence of single-precision floating-point numbers.
    /// </summary>
    /// <param name="source">A sequence of numbers to truncate.</param>
    /// <returns>A sequence of the integral numbers.</returns>
    public static IEnumerable<float> Truncate(this IEnumerable<float> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      return source.Select(ITEM => (float)Math.Truncate(ITEM));
    }

    /// <summary>
    /// Calculates the integral part of a elements in a sequence of decimal numbers.
    /// </summary>
    /// <param name="source">A sequence of numbers to truncate.</param>
    /// <returns>A sequence of the integral numbers.</returns>
    public static IEnumerable<decimal> Truncate(this IEnumerable<decimal> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      return source.Select(S => Math.Truncate(S));
    }

    #endregion

    #region Remove Nans and Nulls

    /// <summary>
    /// Removes all Nan elements from a sequence of double-precision floating-point numbers.
    /// </summary>
    /// <param name="source">The sequence to operate on.</param>
    /// <returns>A sequence of all values from source except Nan values.</returns>
    public static IEnumerable<double> RemoveNaNs(this IEnumerable<double> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      foreach (double item in source)
        if (!double.IsNaN(item))
          yield return item;
    }

    /// <summary>
    /// Removes all Nan elements from a sequence where the selector produce a Nan.
    /// </summary>
    /// <typeparam name="T">The type of elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of elements to operate on.</param>
    /// <param name="selector">A function that evaluates each source element and return a double-precision floating-point number, possibly a Nan.</param>
    /// <returns>A sequence of elements that evaluated to a value other than Nan.</returns>
    public static IEnumerable<T> RemoveNaNs<T>(this IEnumerable<T> source, Func<T, double> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      foreach (T item in source)
        if (!double.IsNaN(selector(item)))
          yield return item;
    }

    /// <summary>
    /// Removes all Nan elements from a sequence of single-precision floating-point numbers.
    /// </summary>
    /// <param name="source">The sequence to operate on.</param>
    /// <returns>A sequence of all values from source except Nan values.</returns>
    public static IEnumerable<float> RemoveNaNs(this IEnumerable<float> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      foreach (float item in source)
        if (!float.IsNaN(item))
          yield return item;
    }

    /// <summary>
    /// Removes all Nan elements from a sequence where the selector produce a Nan.
    /// </summary>
    /// <typeparam name="T">The type of elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of elements to operate on.</param>
    /// <param name="selector">A function that evaluates each source element and return a single-precision floating-point number, possibly a Nan.</param>
    /// <returns>A sequence of elements that evaluated to a value other than Nan.</returns>
    public static IEnumerable<T> RemoveNaNs<T>(this IEnumerable<T> source, Func<T, float> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      foreach (T item in source)
        if (!float.IsNaN(selector(item)))
          yield return item;
    }

    /// <summary>
    /// Removes all Nan elements from a sequence of nullable double-precision floating-point numbers.
    /// </summary>
    /// <param name="source">The sequence to operate on.</param>
    /// <returns>A sequence of all values from source except Nan values.</returns>
    public static IEnumerable<double?> RemoveNaNs(this IEnumerable<double?> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      foreach (double? item in source)
        if (!item.HasValue || !double.IsNaN(item.Value))
          yield return item;
    }

    /// <summary>
    /// Removes all Nan elements from a sequence where the selector produce a Nan.
    /// </summary>
    /// <typeparam name="T">The type of elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of elements to operate on.</param>
    /// <param name="selector">A function that evaluates each source element and return a nullable double-precision floating-point number, possibly a Nan.</param>
    /// <returns>A sequence of elements that evaluated to a value other than Nan.</returns>
    public static IEnumerable<T> RemoveNaNs<T>(this IEnumerable<T> source, Func<T, double?> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      foreach (T item in source)
      {
        double? v = selector(item);
        if (!v.HasValue || !double.IsNaN(v.Value))
          yield return item;
      }
    }

    /// <summary>
    /// Removes all Nan elements from a sequence of nullable single-precision floating-point numbers.
    /// </summary>
    /// <param name="source">The sequence to operate on.</param>
    /// <returns>A sequence of all values from source except Nan values.</returns>
    public static IEnumerable<float?> RemoveNaNs(this IEnumerable<float?> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      foreach (float? item in source)
        if (!item.HasValue || !float.IsNaN(item.Value))
          yield return item;
    }

    /// <summary>
    /// Removes all Nan elements from a sequence where the selector produce a Nan.
    /// </summary>
    /// <typeparam name="T">The type of elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of elements to operate on.</param>
    /// <param name="selector">A function that evaluates each source element and return a nullable single-precision floating-point number, possibly a Nan.</param>
    /// <returns>A sequence of elements that evaluated to a value other than Nan.</returns>
    public static IEnumerable<T> RemoveNaNs<T>(this IEnumerable<T> source, Func<T, float?> selector)
    {

      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");

      foreach (T item in source)
      {
        float? v = selector(item);
        if (!v.HasValue || !float.IsNaN(v.Value))
          yield return item;
      }
    }

    /// <summary>
    /// Removes all null elements from a sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in the source sequence.</typeparam>
    /// <param name="source">A sequence of elements to operate on.</param>
    /// <returns>A sequence of all values from source except nulls.</returns>
    public static IEnumerable<T> RemoveNulls<T>(this IEnumerable<T> source) 
    {
        if (source == null)
        throw Error.ArgumentNull("source");

        return source.Where(S => S != null);
    }

      #endregion

    #region Remove Noise (By Range Value)

    /// <summary>
    /// Replace noise values in a sequence with a predefined value or remove noise values if the replacer is null. 
    /// </summary>
    /// <param name="source">A sequence of double-precision floating-point numbers to operate on.</param>
    /// <param name="limit">The maximum percent or standard deviations above or below from the average that is considered valid (non noise) data.</param>
    /// <param name="noiseFilterType">Noise filter mode: Percent Of Average or Standard Deviations, this method do not accept the Absolute Value</param>
    /// <param name="noiseReplacer">the value to replace elements that are considered noise. Passing a null will cause noise elements to be skipped rather than replaced.</param>
    /// <returns>A sequence of all original values that fall within the range of non noise values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="InvalidOperationException">This override of RemoveNoise cannot be used with filter type 'AbsoluteValue'.</exception>
    public static IEnumerable<double> RemoveNoise(this  IEnumerable<double> source, double limit, NoiseFilterType noiseFilterType, double? noiseReplacer)
    {
      if (noiseFilterType == NoiseFilterType.AbsoluteValue)
        throw Error.InvalidNoiseFilterType();

      return RemoveNoise(source, limit, limit, noiseFilterType, noiseReplacer);
    }

    /// <summary>
    /// Replace noise values in a sequence with a predefined value or remove noise values if the replacer is null. 
    /// </summary>
    /// <param name="source">A sequence of single-precision floating-point numbers to operate on.</param>
    /// <param name="limit">The maximum percent or standard deviations above or below from the average that is considered valid (non noise) data.</param>
    /// <param name="noiseFilterType">Noise filter mode: Percent Of Average or Standard Deviations, this method do not accept the Absolute Value</param>
    /// <param name="noiseReplacer">the value to replace elements that are considered noise. Passing a null will cause noise elements to be skipped rather than replaced.</param>
    /// <returns>A sequence of all original values that fall within the range of non noise values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="InvalidOperationException">This override of RemoveNoise cannot be used with filter type 'AbsoluteValue'.</exception>
    public static IEnumerable<float> RemoveNoise(this  IEnumerable<float> source, float limit, NoiseFilterType noiseFilterType, float? noiseReplacer)
    {
      if (noiseFilterType == NoiseFilterType.AbsoluteValue)
        throw Error.InvalidNoiseFilterType();

      return RemoveNoise(source, limit, limit, noiseFilterType, noiseReplacer);
    }

    /// <summary>
    /// Replace noise values in a sequence with a predefined value or remove noise values if the replacer is null. 
    /// </summary>
    /// <param name="source">A sequence of decimal numbers to operate on.</param>
    /// <param name="limit">The maximum percent or standard deviations above or below from the average that is considered valid (non noise) data.</param>
    /// <param name="noiseFilterType">Noise filter mode: Percent Of Average or Standard Deviations, this method do not accept the Absolute Value</param>
    /// <param name="noiseReplacer">the value to replace elements that are considered noise. Passing a null will cause noise elements to be skipped rather than replaced.</param>
    /// <returns>A sequence of all original values that fall within the range of non noise values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="InvalidOperationException">This override of RemoveNoise cannot be used with filter type 'AbsoluteValue'.</exception>
    public static IEnumerable<decimal> RemoveNoise(this  IEnumerable<decimal> source, decimal limit, NoiseFilterType noiseFilterType, decimal? noiseReplacer)
    {
      if (noiseFilterType == NoiseFilterType.AbsoluteValue)
        throw Error.InvalidNoiseFilterType();

      return RemoveNoise(source, limit, limit, noiseFilterType, noiseReplacer);
    }

    /// <summary>
    /// Replace noise values in a sequence with a predefined value or remove noise values if the replacer is null. 
    /// </summary>
    /// <param name="source">A sequence of long numbers to operate on.</param>
    /// <param name="limit">The maximum percent or standard deviations above or below from the average that is considered valid (non noise) data.</param>
    /// <param name="noiseFilterType">Noise filter mode: Percent Of Average or Standard Deviations, this method do not accept the Absolute Value</param>
    /// <param name="noiseReplacer">the value to replace elements that are considered noise. Passing a null will cause noise elements to be skipped rather than replaced.</param>
    /// <returns>A sequence of all original values that fall within the range of non noise values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="InvalidOperationException">This override of RemoveNoise cannot be used with filter type 'AbsoluteValue'.</exception>
    public static IEnumerable<long> RemoveNoise(this  IEnumerable<long> source, double limit, NoiseFilterType noiseFilterType, long? noiseReplacer)
    {
      if (noiseFilterType == NoiseFilterType.AbsoluteValue)
        throw Error.InvalidNoiseFilterType();

      return RemoveNoise(source, limit, limit, noiseFilterType, noiseReplacer);
    }

    /// <summary>
    /// Replace noise values in a sequence with a predefined value or remove noise values if the replacer is null. 
    /// </summary>
    /// <param name="source">A sequence of integer numbers to operate on.</param>
    /// <param name="limit">The maximum percent or standard deviations above or below from the average that is considered valid (non noise) data.</param>
    /// <param name="noiseFilterType">Noise filter mode: Percent Of Average or Standard Deviations, this method do not accept the Absolute Value</param>
    /// <param name="noiseReplacer">the value to replace elements that are considered noise. Passing a null will cause noise elements to be skipped rather than replaced.</param>
    /// <returns>A sequence of all original values that fall within the range of non noise values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="InvalidOperationException">This override of RemoveNoise cannot be used with filter type 'AbsoluteValue'.</exception>
    public static IEnumerable<int> RemoveNoise(this  IEnumerable<int> source, double limit, NoiseFilterType noiseFilterType, int? noiseReplacer)
    {
      if (noiseFilterType == NoiseFilterType.AbsoluteValue)
        throw Error.InvalidNoiseFilterType();

      return RemoveNoise(source, limit, limit, noiseFilterType, noiseReplacer);
    }

    /// <summary>
    /// Replace noise values in a sequence with a predefined value or remove noise values if the replacer is null. 
    /// </summary>
    /// <param name="source">A sequence of double-precision floating-point numbers to operate on.</param>
    /// <param name="upperLimit">The maximum value, percent or standard deviations above the average that is considered valid (non noise) data.</param>
    /// <param name="lowerLimit">The minimum value, percent or standard deviations above the average that is considered valid (non noise) data.</param>
    /// <param name="noiseFilterType">Noise filter mode: Percent Of Average or Standard Deviations, this method do not accept the Absolute Value</param>
    /// <param name="noiseReplacer">the value to replace elements that are considered noise. Passing a null will cause noise elements to be skipped rather than replaced.</param>
    /// <returns>A sequence of all original values that fall within the range of non noise values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="InvalidOperationException">When calling RemoveNoise with 'AbsoluteValue' filter type option upperLimit cannot be equal to lowerLimit..</exception>
    public static IEnumerable<double> RemoveNoise(this  IEnumerable<double> source, double upperLimit, double lowerLimit, NoiseFilterType noiseFilterType, double? noiseReplacer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      double[] sourceArr = source.ToArray();
      switch (noiseFilterType)
      {
        case NoiseFilterType.AbsoluteValue:
          if (Math.Abs(upperLimit - lowerLimit) < double.Epsilon)
            throw Error.InvalidNoiseFilterTypeLimits();
          break;
        case NoiseFilterType.PercentOfAverage:
          {
            double avg = sourceArr.RemoveNaNs().Average();
            upperLimit = avg + upperLimit * avg;
            lowerLimit = avg - lowerLimit * avg;
          }
          break;
        case NoiseFilterType.StandardDeviation:
          {
            double[] s = sourceArr.RemoveNaNs().ToArray();
            double avg = s.Average();
            double stdev = s.Stdev();
            upperLimit = avg + stdev * upperLimit;
            lowerLimit = avg - stdev * lowerLimit;
          }
          break;
      }

      return sourceArr.RemoveNoise(upperLimit, lowerLimit, noiseReplacer);
    }

    /// <summary>
    /// Replace noise values in a sequence with a predefined value or remove noise values if the replacer is null. 
    /// </summary>
    /// <param name="source">A sequence of double-precision single-point numbers to operate on.</param>
    /// <param name="upperLimit">The maximum value, percent or standard deviations above the average that is considered valid (non noise) data.</param>
    /// <param name="lowerLimit">The minimum value, percent or standard deviations above the average that is considered valid (non noise) data.</param>
    /// <param name="noiseFilterType">Noise filter mode: Percent Of Average or Standard Deviations, this method do not accept the Absolute Value</param>
    /// <param name="noiseReplacer">the value to replace elements that are considered noise. Passing a null will cause noise elements to be skipped rather than replaced.</param>
    /// <returns>A sequence of all original values that fall within the range of non noise values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="InvalidOperationException">When calling RemoveNoise with 'AbsoluteValue' filter type option upperLimit cannot be equal to lowerLimit..</exception>
    public static IEnumerable<float> RemoveNoise(this  IEnumerable<float> source, float upperLimit, float lowerLimit, NoiseFilterType noiseFilterType, float? noiseReplacer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      float[] sourceArr = source.ToArray();
      switch (noiseFilterType)
      {
        case NoiseFilterType.AbsoluteValue:
          if (Math.Abs(upperLimit - lowerLimit) < float.Epsilon)
            throw Error.InvalidNoiseFilterTypeLimits();
          break;
        case NoiseFilterType.PercentOfAverage:
          {
            float avg = sourceArr.RemoveNaNs().Average();
            upperLimit = avg + upperLimit * avg;
            lowerLimit = avg - lowerLimit * avg;
          }
          break;
        case NoiseFilterType.StandardDeviation:
          {
            float[] s = sourceArr.RemoveNaNs().ToArray();
            float avg = s.Average();
            float stdev = s.Stdev();
            upperLimit = avg + stdev * upperLimit;
            lowerLimit = avg - stdev * lowerLimit;
          }
          break;
      }

      return sourceArr.RemoveNoise(upperLimit, lowerLimit, noiseReplacer);
    }

    /// <summary>
    /// Replace noise values in a sequence with a predefined value or remove noise values if the replacer is null. 
    /// </summary>
    /// <param name="source">A sequence of decimal numbers to operate on.</param>
    /// <param name="upperLimit">The maximum value, percent or standard deviations above the average that is considered valid (non noise) data.</param>
    /// <param name="lowerLimit">The minimum value, percent or standard deviations above the average that is considered valid (non noise) data.</param>
    /// <param name="noiseFilterType">Noise filter mode: Percent Of Average or Standard Deviations, this method do not accept the Absolute Value</param>
    /// <param name="noiseReplacer">the value to replace elements that are considered noise. Passing a null will cause noise elements to be skipped rather than replaced.</param>
    /// <returns>A sequence of all original values that fall within the range of non noise values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="InvalidOperationException">When calling RemoveNoise with 'AbsoluteValue' filter type option upperLimit cannot be equal to lowerLimit..</exception>
    public static IEnumerable<decimal> RemoveNoise(this  IEnumerable<decimal> source, decimal upperLimit, decimal lowerLimit, NoiseFilterType noiseFilterType, decimal? noiseReplacer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      decimal[] sourceArr = source.ToArray();
      switch (noiseFilterType)
      {
        case NoiseFilterType.AbsoluteValue:
          if (upperLimit == lowerLimit)
            throw Error.InvalidNoiseFilterTypeLimits();
          break;
        case NoiseFilterType.PercentOfAverage:
          {
            decimal avg = sourceArr.Average();
            upperLimit = avg + upperLimit * avg;
            lowerLimit = avg - lowerLimit * avg;
          }
          break;
        case NoiseFilterType.StandardDeviation:
          {
            decimal avg = sourceArr.Average();
            decimal stdev = sourceArr.Stdev();
            upperLimit = avg + stdev * upperLimit;
            lowerLimit = avg - stdev * lowerLimit;
          }
          break;
      }

      return sourceArr.RemoveNoise(upperLimit, lowerLimit, noiseReplacer);
    }

    /// <summary>
    /// Replace noise values in a sequence with a predefined value or remove noise values if the replacer is null. 
    /// </summary>
    /// <param name="source">A sequence of long numbers to operate on.</param>
    /// <param name="upperLimit">The maximum value, percent or standard deviations above the average that is considered valid (non noise) data.</param>
    /// <param name="lowerLimit">The minimum value, percent or standard deviations above the average that is considered valid (non noise) data.</param>
    /// <param name="noiseFilterType">Noise filter mode: Percent Of Average or Standard Deviations, this method do not accept the Absolute Value</param>
    /// <param name="noiseReplacer">the value to replace elements that are considered noise. Passing a null will cause noise elements to be skipped rather than replaced.</param>
    /// <returns>A sequence of all original values that fall within the range of non noise values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="InvalidOperationException">When calling RemoveNoise with 'AbsoluteValue' filter type option upperLimit cannot be equal to lowerLimit..</exception>
    public static IEnumerable<long> RemoveNoise(this  IEnumerable<long> source, double upperLimit, double lowerLimit, NoiseFilterType noiseFilterType, long? noiseReplacer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      long[] sourceArr = source.ToArray();
      switch (noiseFilterType)
      {
        case NoiseFilterType.AbsoluteValue:
          if (Math.Abs(upperLimit - lowerLimit) < double.Epsilon)
            throw Error.InvalidNoiseFilterTypeLimits();
          break;
        case NoiseFilterType.PercentOfAverage:
          {
            double avg = sourceArr.Average();
            upperLimit = avg + upperLimit * avg;
            lowerLimit = avg - lowerLimit * avg;
          }
          break;
        case NoiseFilterType.StandardDeviation:
          {
            double avg = sourceArr.Average();
            double stdev = sourceArr.Stdev();
            upperLimit = avg + stdev * upperLimit;
            lowerLimit = avg - stdev * lowerLimit;
          }
          break;
      }

      return sourceArr.RemoveNoise(upperLimit, lowerLimit, noiseReplacer);
    }

    /// <summary>
    /// Replace noise values in a sequence with a predefined value or remove noise values if the replacer is null. 
    /// </summary>
    /// <param name="source">A sequence of integer numbers to operate on.</param>
    /// <param name="upperLimit">The maximum value, percent or standard deviations above the average that is considered valid (non noise) data.</param>
    /// <param name="lowerLimit">The minimum value, percent or standard deviations above the average that is considered valid (non noise) data.</param>
    /// <param name="noiseFilterType">Noise filter mode: Percent Of Average or Standard Deviations, this method do not accept the Absolute Value</param>
    /// <param name="noiseReplacer">the value to replace elements that are considered noise. Passing a null will cause noise elements to be skipped rather than replaced.</param>
    /// <returns>A sequence of all original values that fall within the range of non noise values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    /// <exception cref="InvalidOperationException">When calling RemoveNoise with 'AbsoluteValue' filter type option upperLimit cannot be equal to lowerLimit..</exception>
    public static IEnumerable<int> RemoveNoise(this  IEnumerable<int> source, double upperLimit, double lowerLimit, NoiseFilterType noiseFilterType, int? noiseReplacer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      int[] sourceArr = source.ToArray();
      switch (noiseFilterType)
      {
        case NoiseFilterType.AbsoluteValue:
          if (Math.Abs(upperLimit - lowerLimit) < double.Epsilon)
            throw Error.InvalidNoiseFilterTypeLimits();
          break;
        case NoiseFilterType.PercentOfAverage:
          {
            double avg = sourceArr.Average();
            upperLimit = avg + upperLimit * avg;
            lowerLimit = avg - lowerLimit * avg;
          }
          break;
        case NoiseFilterType.StandardDeviation:
          {
            double avg = sourceArr.Average();
            double stdev = sourceArr.Stdev();
            upperLimit = avg + stdev * upperLimit;
            lowerLimit = avg - stdev * lowerLimit;
          }
          break;
      }

      return sourceArr.RemoveNoise(upperLimit, lowerLimit, noiseReplacer);
    }

    /// <summary>
    /// Replace noise values in a sequence with a predefined value or remove noise values if the replacer is null. 
    /// </summary>
    /// <param name="source">A sequence of double-precision floating-point numbers to operate on.</param>
    /// <param name="upperLimit">The maximum value, percent or standard deviations above the average that is considered valid (non noise) data.</param>
    /// <param name="lowerLimit">The minimum value, percent or standard deviations above the average that is considered valid (non noise) data.</param>
    /// <param name="noiseReplacer">the value to replace elements that are considered noise. Passing a null will cause noise elements to be skipped rather than replaced.</param>
    /// <returns>A sequence of all original values that fall within the range of non noise values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    private static IEnumerable<double> RemoveNoise(this  IEnumerable<double> source, double upperLimit, double lowerLimit, double? noiseReplacer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      bool skipNoise = noiseReplacer == null;
      double noiseReplacerValue = noiseReplacer.GetValueOrDefault();

      if (upperLimit < lowerLimit)
        Helper.Swap(ref upperLimit, ref lowerLimit);

      foreach (double item in source)
        if (item <= upperLimit && item >= lowerLimit)
          yield return item;
        else
          if (!skipNoise)
            yield return noiseReplacerValue;
    }

    /// <summary>
    /// Replace noise values in a sequence with a predefined value or remove noise values if the replacer is null. 
    /// </summary>
    /// <param name="source">A sequence of single-precision floating-point numbers to operate on.</param>
    /// <param name="upperLimit">The maximum value, percent or standard deviations above the average that is considered valid (non noise) data.</param>
    /// <param name="lowerLimit">The minimum value, percent or standard deviations above the average that is considered valid (non noise) data.</param>
    /// <param name="noiseReplacer">the value to replace elements that are considered noise. Passing a null will cause noise elements to be skipped rather than replaced.</param>
    /// <returns>A sequence of all original values that fall within the range of non noise values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    private static IEnumerable<float> RemoveNoise(this IEnumerable<float> source, float upperLimit, float lowerLimit, float? noiseReplacer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      bool skipNoise = noiseReplacer == null;
      float noiseReplacerValue = noiseReplacer.GetValueOrDefault();

      if (upperLimit < lowerLimit)
        Helper.Swap(ref upperLimit, ref lowerLimit);

      foreach (float item in source)
        if (item <= upperLimit && item >= lowerLimit)
          yield return item;
        else
          if (!skipNoise)
            yield return noiseReplacerValue;
    }

    /// <summary>
    /// Replace noise values in a sequence with a predefined value or remove noise values if the replacer is null. 
    /// </summary>
    /// <param name="source">A sequence of decimal numbers to operate on.</param>
    /// <param name="upperLimit">The maximum value, percent or standard deviations above the average that is considered valid (non noise) data.</param>
    /// <param name="lowerLimit">The minimum value, percent or standard deviations above the average that is considered valid (non noise) data.</param>
    /// <param name="noiseReplacer">the value to replace elements that are considered noise. Passing a null will cause noise elements to be skipped rather than replaced.</param>
    /// <returns>A sequence of all original values that fall within the range of non noise values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    private static IEnumerable<decimal> RemoveNoise(this IEnumerable<decimal> source, decimal upperLimit, decimal lowerLimit, decimal? noiseReplacer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      bool skipNoise = noiseReplacer == null;
      decimal noiseReplacerValue = noiseReplacer.GetValueOrDefault();

      if (upperLimit < lowerLimit)
        Helper.Swap(ref upperLimit, ref lowerLimit);

      foreach (decimal item in source)
        if (item <= upperLimit && item >= lowerLimit)
          yield return item;
        else
          if (!skipNoise)
            yield return noiseReplacerValue;
    }

    /// <summary>
    /// Replace noise values in a sequence with a predefined value or remove noise values if the replacer is null. 
    /// </summary>
    /// <param name="source">A sequence of long numbers to operate on.</param>
    /// <param name="upperLimit">The maximum value, percent or standard deviations above the average that is considered valid (non noise) data.</param>
    /// <param name="lowerLimit">The minimum value, percent or standard deviations above the average that is considered valid (non noise) data.</param>
    /// <param name="noiseReplacer">the value to replace elements that are considered noise. Passing a null will cause noise elements to be skipped rather than replaced.</param>
    /// <returns>A sequence of all original values that fall within the range of non noise values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    private static IEnumerable<long> RemoveNoise(this IEnumerable<long> source, double upperLimit, double lowerLimit, long? noiseReplacer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      bool skipNoise = noiseReplacer == null;
      long noiseReplacerValue = noiseReplacer.GetValueOrDefault();

      if (upperLimit < lowerLimit)
        Helper.Swap(ref upperLimit, ref lowerLimit);

      foreach (long item in source)
        if (item <= upperLimit && item >= lowerLimit)
          yield return item;
        else
          if (!skipNoise)
            yield return noiseReplacerValue;
    }

    /// <summary>
    /// Replace noise values in a sequence with a predefined value or remove noise values if the replacer is null. 
    /// </summary>
    /// <param name="source">A sequence of integer numbers to operate on.</param>
    /// <param name="upperLimit">The maximum value, percent or standard deviations above the average that is considered valid (non noise) data.</param>
    /// <param name="lowerLimit">The minimum value, percent or standard deviations above the average that is considered valid (non noise) data.</param>
    /// <param name="noiseReplacer">the value to replace elements that are considered noise. Passing a null will cause noise elements to be skipped rather than replaced.</param>
    /// <returns>A sequence of all original values that fall within the range of non noise values.</returns>
    /// <exception cref="System.ArgumentNullException">source is null.</exception>
    private static IEnumerable<int> RemoveNoise(this IEnumerable<int> source, double upperLimit, double lowerLimit, int? noiseReplacer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");

      bool skipNoise = noiseReplacer == null;
      int noiseReplacerValue = noiseReplacer.GetValueOrDefault();

      if (upperLimit < lowerLimit)
        Helper.Swap(ref upperLimit, ref lowerLimit);

      foreach (int item in source)
        if (item <= upperLimit && item >= lowerLimit)
          yield return item;
        else
          if (!skipNoise)
            yield return noiseReplacerValue;
    }

    #endregion

    #region Filter

    /// <summary>
    /// Filters a sequence of values based on a predicate. 
    /// </summary>
    /// <typeparam name="T">The type of the elements of source.</typeparam>
    /// <param name="source">A sequence of elements to filter.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>A n sequence of elements from the source sequence that satisfy the condition.</returns>
    /// <exception cref="System.ArgumentNullException">source or predicate is null.</exception>
    public static IEnumerable<T> Filter<T>(this IEnumerable<T> source, Predicate<T> predicate)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (predicate == null)
        throw Error.ArgumentNull("predicate");

      foreach (T item in source)
        if (predicate(item))
          yield return item;
    }

    /// <summary>
    /// Filters a sequence of values based on a predicate or predefined replacer. 
    /// </summary>
    /// <typeparam name="T">The type of the elements of source.</typeparam>
    /// <param name="source">A sequence of elements to filter.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="replacer">A replacer to return when the predicate fails.</param>
    /// <returns>A n sequence of elements from the source sequence that satisfy the condition or a replacer.</returns>
    /// <exception cref="System.ArgumentNullException">source or predicate is null.</exception>
    public static IEnumerable<T> Filter<T>(this IEnumerable<T> source, Predicate<T> predicate, T replacer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (predicate == null)
        throw Error.ArgumentNull("predicate");

      foreach (T item in source)
        if (predicate(item))
          yield return item;
        else
          yield return replacer;
    }

    /// <summary>
    /// Filters a sequence of values based on a predicate or predefined replacer. 
    /// </summary>
    /// <typeparam name="T">The type of the elements of source.</typeparam>
    /// <param name="source">A sequence of elements to filter.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="replacer">A delegate that will return a replacer when the sourceFilter is false.</param>
    /// <returns>A n sequence of elements from the source sequence that satisfy the condition or a replacer.</returns>
    /// <exception cref="System.ArgumentNullException">source, predicate or replacer is null.</exception>
    public static IEnumerable<T> Filter<T>(this IEnumerable<T> source, Predicate<T> predicate, Func<T, T> replacer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (predicate == null)
        throw Error.ArgumentNull("predicate");
      if (replacer == null)
        throw Error.ArgumentNull("replacer");

      foreach (T item in source)
        if (predicate(item))
          yield return item;
        else
          yield return replacer(item);
    }

    /// <summary>
    /// Filters a sequence of values based on a corresponding value in a sequence of Booleans. 
    /// </summary>
    /// <typeparam name="T">The type of the elements of source.</typeparam>
    /// <param name="source">A sequence of elements to filter.</param>
    /// <param name="sourceFilter">A sequence of Booleans that acts as the filter.</param>
    /// <returns>A n sequence of elements from the source sequence that have the position on true values in the filter sequence.</returns>
    /// <exception cref="System.ArgumentNullException">source or sourceFilter is null.</exception>
    public static IEnumerable<T> Filter<T>(this IEnumerable<T> source, IEnumerable<bool> sourceFilter)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (sourceFilter == null)
        throw Error.ArgumentNull("sourceFilter");

      using (IEnumerator<T> s = source.GetEnumerator())
      using (IEnumerator<bool> f = sourceFilter.GetEnumerator())
        while (s.MoveNext() && f.MoveNext())
          if (f.Current)
            yield return s.Current;
    }

    /// <summary>
    /// Filters a sequence of values or replacers based on a corresponding value in a sequence of Booleans. 
    /// </summary>
    /// <typeparam name="T">The type of the elements of source.</typeparam>
    /// <param name="source">A sequence of elements to filter.</param>
    /// <param name="sourceFilter">A sequence of Booleans that acts as the filter.</param>
    /// <returns>A n sequence of elements from the source sequence that have the position on true values in the filter sequence or a replacer.</returns>
    /// <param name="replacer">A replacer to return when the sourceFilter is false.</param>
    /// <exception cref="System.ArgumentNullException">source or sourceFilter is null.</exception>
    public static IEnumerable<T> Filter<T>(this IEnumerable<T> source, IEnumerable<bool> sourceFilter, T replacer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (sourceFilter == null)
        throw Error.ArgumentNull("sourceFilter");

      using (IEnumerator<T> s = source.GetEnumerator())
      using (IEnumerator<bool> f = sourceFilter.GetEnumerator())
        while (s.MoveNext() && f.MoveNext())
          if (f.Current)
            yield return s.Current;
          else
            yield return replacer;
    }

    /// <summary>
    /// Filters a sequence of values or replacers based on a corresponding value in a sequence of Booleans. 
    /// </summary>
    /// <typeparam name="T">The type of the elements of source.</typeparam>
    /// <param name="source">A sequence of elements to filter.</param>
    /// <param name="sourceFilter">A sequence of Booleans that acts as the filter.</param>
    /// <param name="replacer">A delegate that will return a replacer when the sourceFilter is false.</param>
    /// <returns>A n sequence of elements from the source sequence that have the position on true values in the filter sequence or a replacer.</returns>    
    /// <exception cref="System.ArgumentNullException">source, sourceFilter or replacer is null.</exception>
    public static IEnumerable<T> Filter<T>(this IEnumerable<T> source, IEnumerable<bool> sourceFilter, Func<T, T> replacer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (sourceFilter == null)
        throw Error.ArgumentNull("sourceFilter");
      if (replacer == null)
        throw Error.ArgumentNull("replacer");

      using (IEnumerator<T> s = source.GetEnumerator())
      using (IEnumerator<bool> f = sourceFilter.GetEnumerator())
        while (s.MoveNext() && f.MoveNext())
          if (f.Current)
            yield return s.Current;
          else
            yield return replacer(s.Current);
    }

    #endregion

    #region If

    /// <summary>
    ///  Filters a sequence of values based on a predicate and projects them into a new form.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <typeparam name="TResult">The type of the value returned by selector.</typeparam>
    /// <param name="source">A sequence to filter and transform from.</param>
    /// <param name="ifCondition">A function to test each element for a condition.</param>
    /// <param name="thenSelector">A transform function to apply to each element.</param>
    /// <returns>An sequence of elements that contains elements from the source sequence that satisfy the condition and transformed by the thenSelector.</returns>
    /// <remarks>This method is the equivalent of combining the Where extension method with the Select extension method.</remarks>
    /// <exception cref="System.ArgumentNullException">source or ifCondition or thenSelector is null.</exception>
    public static IEnumerable<TResult> If<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, bool> ifCondition, Func<TSource, TResult> thenSelector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (ifCondition == null)
        throw Error.ArgumentNull("ifCondition");
      if (thenSelector == null)
        throw Error.ArgumentNull("thenSelector");

      foreach (TSource item in source)
        if (ifCondition(item))
          yield return thenSelector(item);
    }

    /// <summary>
    ///  Filters a sequence of values based on a predicate and projects them into a new form.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <typeparam name="TResult">The type of the value returned by selector.</typeparam>
    /// <param name="source">A sequence to filter and transform from.</param>
    /// <param name="ifCondition">A function to test each element for a condition.</param>
    /// <param name="thenSelector">A transform function to apply to each element that meet the condition.</param>
    /// <param name="elseSelector">A transform function to apply to each element that fails to meet the condition.</param>
    /// <returns>An sequence of elements from the source sequence that satisfy the condition and transformed by the thenSelector or fails a condition and transformed by the elseSelector.</returns>
    /// <exception cref="System.ArgumentNullException">source or ifCondition or thenSelector or elseSelector is null.</exception>
    public static IEnumerable<TResult> If<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, bool> ifCondition, Func<TSource, TResult> thenSelector, Func<TSource, TResult> elseSelector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (ifCondition == null)
        throw Error.ArgumentNull("ifCondition");
      if (thenSelector == null)
        throw Error.ArgumentNull("thenSelector");
      if (elseSelector == null)
        throw Error.ArgumentNull("elseSelector");

      foreach (TSource item in source)
        if (ifCondition(item))
          yield return thenSelector(item);
        else
          yield return elseSelector(item);
    }

    #endregion
  }
}