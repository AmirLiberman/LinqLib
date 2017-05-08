using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace LinqLib.Sequence
{
  /// <summary>
  /// Methods generating special sequence types.
  /// </summary>
  public static class SpecializedSets
  {
    #region Declarations

    /// <summary>
    /// a private static lookup table with 10 to power of 0 to 18.
    /// </summary>
    static readonly long[] powersOfTen = new[] { 1, 10, 100, 1000, 
                                                 10000, 
                                                 100000, 
                                                 1000000, 
                                                 10000000, 
                                                 100000000, 
                                                 1000000000, 
                                                 10000000000, 
                                                 100000000000, 
                                                 1000000000000, 
                                                 10000000000000, 
                                                 100000000000000, 
                                                 1000000000000000, 
                                                 10000000000000000, 
                                                 100000000000000000, 
                                                 1000000000000000000};

    #endregion

    #region Sets

    /// <summary>
    /// Generates a sequence of factorials.
    /// </summary>
    /// <returns>A sequence of factorials.</returns>
    /// <remarks>Sequence will return 19 elements only to avoid overflow with the long type.</remarks>
    public static IEnumerable<long> Factorials()
    {
      long n1 = 1;
      long n2 = 1;
      int i = 19; // After 19 items long will overflow
      while (i >= 0)
      {
        yield return n1;
        i--;
        n2++;
        n1 = n1 * n2;
      }
    }

    /// <summary>
    /// Generates a sequence of Fibonacci numbers.
    /// </summary>
    /// <returns>A sequence of Fibonacci numbers.</returns>
    /// <remarks>Sequence will return 90 elements only to avoid overflow with the long type.</remarks>
    public static IEnumerable<long> Fibs()
    {
      long n1 = 0;
      long n2 = 1;
      int i = 90; // After 90 items long will overflow
      while (i >= 0)
      {
        long n3 = n1 + n2;
        yield return n3;
        i--;
        n1 = n2;
        n2 = n3;
      }
    }

    /// <summary>
    /// Generates a sequence of prime numbers.
    /// </summary>
    /// <returns>A sequence of prime numbers.</returns>
    /// <remarks>Sequence will end at 9223372036854775783, the prime a long can hold.</remarks>
    public static IEnumerable<long> Primes()
    {
      return Primes(long.MaxValue);
    }

    /// <summary>
    /// Generates a sequence of prime numbers.
    /// </summary>
    /// <param name="max">The largest value to return. If Max is not a prime, the operation will stop at the nearest prime.</param>
    /// <returns>A sequence of prime numbers.</returns>
    /// <remarks>Sequence will end at 9223372036854775783, the prime a long can hold.</remarks>
    public static IEnumerable<long> Primes(long max)
    {
      return Primes(2, max);
    }

    /// <summary>
    /// Generates a sequence of prime numbers.
    /// </summary>
    /// <param name="min">The starting prime value to return. If Min is not a prime, the operation will start at the nearest prime that is larger than Min.</param>
    /// <param name="max">The largest value to return. If Max is not a prime, the operation will stop at the nearest prime that is smaller than Min.</param>
    /// <returns>A sequence of prime numbers.</returns>
    /// <remarks>Sequence will end at 9223372036854775783, the prime a long can hold.</remarks>
    public static IEnumerable<long> Primes(long min, long max)
    {
      if (min == 2)
        yield return 2;

      long i = min;
      if (i % 2 == 0)
        i++;

      if (max > 9223372036854775783)
        max = 9223372036854775783; // Largest long prime

      while (i <= max)
      {
        if (IsPrime(i))
          yield return i;
        i += 2;
      }
    }

    /// <summary>
    /// Creates a sequence of Phi values base on values in the source. 
    /// </summary>
    /// <param name="source">The sequence elements to calculate phi from.</param>
    /// <returns>A sequence of Phi values.</returns>
    public static IEnumerable<long> Phi(this IEnumerable<long> source)
    {
      return source.Select(ITEM => Phi(ITEM));
    }

    #endregion

    #region Factors and Divisors

    /// <summary>
    /// Generates a sequence of all prime divisors for the provided value.
    /// </summary>
    /// <param name="value">The value to get the prime divisors for.</param>
    /// <returns>A sequence of all prime divisors.</returns>
    public static IEnumerable<long> PrimeDivisors(long value)
    {
      foreach (long p in Primes(value))
      {
        if (value % p == 0)
        {
          yield return p;
          value = value / p;

          foreach (long item in PrimeDivisors(value))
            yield return item;

          break;
        }
        if (value < p)
          break;
      }
    }

    /// <summary>
    /// Generates a sequence of all prime factors for the provided value.
    /// </summary>
    /// <param name="value">The value to get the prime factors for.</param>
    /// <returns>A sequence of all prime factors.</returns>
    public static IEnumerable<long> PrimeFactors(long value)
    {
      foreach (long p in Primes(value))
      {
        if (value % p == 0)
        {
          yield return p;
          value = value / p;
        }
        if (value < p)
          break;
      }
    }

    /// <summary>
    /// Generates a sequence of all divisors for the provided value.
    /// </summary>
    /// <param name="value">The value to get the divisors for.</param>
    /// <returns>A sequence of all divisors.</returns>
    public static IEnumerable<long> Divisors(long value)
    {
      foreach (long divisor in ProperDivisors(value))
        yield return divisor;

      if (value != 1)
        yield return value;
    }

    /// <summary>
    /// Generates a sequence of all proper divisors for the provided value.
    /// </summary>
    /// <param name="value">The value to get the proper divisors for.</param>
    /// <returns>A sequence of all proper divisors.</returns>
    public static IEnumerable<long> ProperDivisors(long value)
    {
      yield return 1;

      long sr = (long)(Math.Sqrt(value) + .5);
      for (long i = 2; i <= sr; i++)
        if (value % i == 0)
        {
          yield return i;
          if (i * i != value)
            yield return value / i;
        }
    }

    /// <summary>
    /// Checks if a value is a prime number.
    /// </summary>
    /// <param name="value">The value to check for the prime property.</param>
    /// <returns>true id the value is a prime, false otherwise</returns>
    public static bool IsPrime(long value)
    {
      if (value <= 1)
        return false;

      if (value == 2)
        return true;

      if ((value | 1) != value)
        return false;

      long sr = (long)Math.Sqrt(value) + 1;
      for (long x = 3; x < sr; x += 2)
        if (value % x == 0)
          return false;
      return true;
    }

    /// <summary>
    /// Calculates the Phi of a value. 
    /// </summary>
    /// <param name="value">The value to calculate Phi for.</param>
    /// <returns>The Phi of a value.</returns>
    public static long Phi(long value)
    {
      foreach (long prime in PrimeFactors(value))
        value = value - value / prime;

      return value;
    }

    #endregion

    #region Digits Conversion

    /// <summary>
    /// Calculates the sum of digits of the provided value.
    /// </summary>
    /// <param name="value">A System.Int32 to operate on.</param>
    /// <returns>The sum of digits of the provided value.</returns>
    public static int SumOfDigits(int value)
    {
      return SumOfDigits(value.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// Calculates the sum of digits of the provided value.
    /// </summary>
    /// <param name="value">A System.Int64 to operate on.</param>
    /// <returns>The sum of digits of the provided value.</returns>
    public static int SumOfDigits(long value)
    {
      return SumOfDigits(value.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// Calculates the sum of digits of the provided value.
    /// </summary>
    /// <param name="value">A System.String representing a number to operate on.</param>
    /// <returns>The sum of digits of the provided value.</returns>
    /// <remarks>Values with non digit [0-9] characters will return unreliable results.</remarks>
    public static int SumOfDigits(string value)
    {
      return value.Sum(C => C - 48);
    }

    /// <summary>
    /// Converts a sequence of digits into a number.
    /// </summary>
    /// <param name="digits">The sequence of digits to convert.</param>
    /// <returns>A System.Int64 that is the number represented by the digits sequence.</returns>
    public static long ToNumber(this IEnumerable<int> digits)
    {
      if (digits == null)
        throw Error.ArgumentNull("digits");

      return digits.Select((D, I) => D * powersOfTen[I]).Sum();
    }

    /// <summary>
    /// Converts a number into a sequence of digits.
    /// </summary>
    /// <typeparam name="T">The Type of the provided number.</typeparam>
    /// <param name="number">The number to convert into digit sequence.</param>
    /// <returns>A sequence if digits.</returns>
    /// <remarks>If T is not a numeric type, the returned sequence will contain unreliable results.</remarks>
    public static IEnumerable<int> ToDigits<T>(T number)
    {
      return number.ToString().Select(C => C - 48);
    }

    #endregion
  }
}