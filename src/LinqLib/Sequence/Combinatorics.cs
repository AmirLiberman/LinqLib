using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqLib.Sequence
{
  /// <summary>
  /// Provides extensions for permutations and combinations of sequences
  /// For a good explanation of the various flavors of permutations and combinations go to:
  /// http://www.mathsisfun.com/combinatorics/combinations-permutations.html
  /// </summary>
  public static class Combinatorics
  {
    /// <summary>
    /// Creates a sequence of all permutations found in the supplied pool.
    /// </summary>
    /// <typeparam name="T">Type of pool elements.</typeparam>    
    /// <param name="pool">The pool of elements to use for permutations.</param>
    /// <param name="selectionSize">Number of elements in each permutation.</param>
    /// <param name="allowRepetition">Indicates if repetition is allowed within a permutation.</param>
    /// <returns>A sequence of all permutations found in the supplied pool.</returns>
    /// <exception cref="System.ArgumentNullException">pool is null.</exception>
    public static IEnumerable<IEnumerable<T>> Permutations<T>(this IEnumerable<T> pool, int selectionSize, bool allowRepetition)
    {
      if (pool == null)
        throw Error.ArgumentNull("pool");

      if (allowRepetition)
        return Permutations(pool, selectionSize);

      return UniquePermutations(pool, selectionSize);
    }

    /// <summary>
    /// Creates a sequence of all combinations found in the supplied pool.
    /// </summary>
    /// <typeparam name="T">Type of pool elements.</typeparam>    
    /// <param name="pool">The pool of elements to use for combinations.</param>
    /// <param name="selectionSize">Number of elements in each combination.</param>
    /// <param name="allowRepetition">Indicates if repetition is allowed within a combination.</param>
    /// <returns>A sequence of all combinations found in the supplied pool.</returns>
    /// <exception cref="System.ArgumentNullException">pool is null.</exception>
    public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> pool, int selectionSize, bool allowRepetition)
    {
      if (pool == null)
        throw Error.ArgumentNull("pool");

      if (allowRepetition)
        return Combinations(pool, selectionSize);

      return UniqueCombinations(pool, selectionSize);
    }

    #region Permutations with repetition

    /// <summary>
    /// Creates a sequence of all permutations found with repetition in the supplied pool.
    /// </summary>
    /// <typeparam name="T">Type of pool elements.</typeparam>    
    /// <param name="pool">The pool of elements to use for permutations.</param>
    /// <param name="selectionSize">Number of elements in each permutation.</param>
    /// <returns>A sequence of all permutations found in the supplied pool.</returns>
    /// <exception cref="System.ArgumentNullException">pool is null.</exception>
    public static IEnumerable<IEnumerable<T>> Permutations<T>(this IEnumerable<T> pool, int selectionSize)
    {
      if (pool == null)
        throw Error.ArgumentNull("pool");

      int[] idxs = new int[selectionSize];
      T[] pl = pool.ToArray();
      int length = pl.Length;

      do
      {
        yield return GetItems(pl, idxs, selectionSize);
        GetNextPermutation(idxs, selectionSize, length);
      } while (idxs[0] < length);
    }

    private static void GetNextPermutation(IList<int> idxs, int selectionSize, int poolSize)
    {
      int position = selectionSize - 1;

      idxs[position]++;
      while (idxs[position] == poolSize && position > 0)
      {
        idxs[position - 1]++;
        for (int i = position; i < selectionSize; i++)
          idxs[i] = 0;
        position--;
      }
    }

    /// <summary>
    /// Calculates the number of permutations found with repetition in the supplied pool.
    /// </summary>
    /// <typeparam name="T">Type of pool elements.</typeparam>    
    /// <param name="pool">The pool of elements to use for permutations.</param>
    /// <param name="selectionSize">Number of elements in each permutation.</param>
    /// <returns>Number of permutations found without repetition in the supplied pool.</returns>
    /// <exception cref="System.ArgumentNullException">pool is null.</exception>    
    [CLSCompliant(false)]
    public static ulong PermutationsCount<T>(this  IEnumerable<T> pool, int selectionSize)
    {
      if (pool == null)
        throw Error.ArgumentNull("pool");

      return PermutationsCount(pool.Count(), selectionSize);
    }

    /// <summary>
    /// Calculates the number of permutations found with repetition in the supplied pool.
    /// </summary>
    /// <param name="poolSize">The size of the pool.</param>
    /// <param name="selectionSize">Number of elements in each permutation.</param>
    /// <returns>Number of permutations found without repetition in the supplied pool size.</returns>
    [CLSCompliant(false)]
    public static ulong PermutationsCount(int poolSize, int selectionSize)
    {
      checked
      {
        ulong p = 1;

        for (int i = 0; i < selectionSize; i++)
          p = p * (uint)poolSize;

        return p;
      }
    }

    #endregion

    #region Permutations with no repetition

    /// <summary>
    /// Creates a sequence of all permutations found without repetition in the supplied pool.
    /// </summary>
    /// <typeparam name="T">Type of pool elements.</typeparam>    
    /// <param name="pool">The pool of elements to use for permutations.</param>
    /// <param name="selectionSize">Number of elements in each permutation.</param>
    /// <returns>A sequence of all permutations found in the supplied pool.</returns>
    /// <exception cref="System.ArgumentNullException">pool is null.</exception>    
    public static IEnumerable<IEnumerable<T>> UniquePermutations<T>(this IEnumerable<T> pool, int selectionSize)
    {
      if (pool == null)
        throw Error.ArgumentNull("pool");

      int[] idxs = Enumerable.Range(0, selectionSize).ToArray();
      T[] pl = pool.ToArray();
      int length = pl.Length;

      do
      {
        yield return GetItems(pl, idxs, selectionSize);
        GetNextUniquePermutation(idxs, selectionSize, length);
      } while (idxs[0] < length);
    }

    private static void GetNextUniquePermutation(IList<int> idxs, int selectionSize, int poolSize)
    {
      int startPos = selectionSize - 1;
      do
      {
        int position = startPos;

        idxs[position]++;
        while (idxs[position] == poolSize && position > 0)
        {
          idxs[position - 1]++;
          for (int i = position; i < selectionSize; i++)
            idxs[i] = 0;
          position--;
        }
      } while (new HashSet<int>(idxs).Count != selectionSize);
    }

    /// <summary>
    /// Calculates the number of permutations found without repetition in the supplied pool.
    /// </summary>
    /// <typeparam name="T">Type of pool elements.</typeparam>    
    /// <param name="pool">The pool of elements to use for permutations.</param>
    /// <param name="selectionSize">Number of elements in each permutation.</param>
    /// <returns>Number of permutations found without repetition in the supplied pool.</returns>
    /// <exception cref="System.ArgumentNullException">pool is null.</exception>    
    [CLSCompliant(false)]
    public static ulong UniquePermutationsCount<T>(this  IEnumerable<T> pool, int selectionSize)
    {
      if (pool == null)
        throw Error.ArgumentNull("pool");

      return UniquePermutationsCount(pool.Count(), selectionSize);
    }

    /// <summary>
    /// Calculates the number of permutations found without repetition in the supplied pool.
    /// </summary>
    /// <param name="poolSize">The size of the pool.</param>
    /// <param name="selectionSize">Number of elements in each permutation.</param>
    /// <returns>Number of permutations found without repetition in the supplied pool size.</returns>
    [CLSCompliant(false)]
    public static ulong UniquePermutationsCount(int poolSize, int selectionSize)
    {
      checked
      {
        ulong p = 1;

        for (uint i = (uint)poolSize; i > poolSize - selectionSize; i--)
          p = p * i;

        return p;
      }
    }

    #endregion

    #region Combinations with repetition

    /// <summary>
    /// Creates a sequence of all combinations found with repetition in the supplied pool.
    /// </summary>
    /// <typeparam name="T">Type of pool elements.</typeparam>    
    /// <param name="pool">The pool of elements to use for combinations.</param>
    /// <param name="selectionSize">Number of elements in each combination.</param>
    /// <returns>A sequence of all combinations found in the supplied pool.</returns>
    /// <exception cref="System.ArgumentNullException">pool is null.</exception>
    public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> pool, int selectionSize)
    {
      if (pool == null)
        throw Error.ArgumentNull("pool");

      int[] idxs = new int[selectionSize];
      T[] pl = pool.ToArray();
      int length = pl.Length;

      do
      {
        yield return GetItems(pl, idxs, selectionSize);
        GetNextCombination(idxs, selectionSize, length);
      } while (idxs[selectionSize - 1] < length);
    }

    private static void GetNextCombination(IList<int> idxs, int selectionSize, int poolSize)
    {
      int position = selectionSize - 1;

      idxs[position]++;
      while (idxs[position] == poolSize && position > 0)
      {
        idxs[position - 1]++;
        for (int i = position; i < selectionSize; i++)
          idxs[i] = idxs[i - 1];
        position--;
      }
    }

    /// <summary>
    /// Calculates the number of combinations found with repetition in the supplied pool.
    /// </summary>
    /// <typeparam name="T">Type of pool elements.</typeparam>    
    /// <param name="pool">The pool of elements to use for combinations.</param>
    /// <param name="selectionSize">Number of elements in each combination.</param>
    /// <returns>Number of combinations found without repetition in the supplied pool.</returns>
    /// <exception cref="System.ArgumentNullException">pool is null.</exception>    
    [CLSCompliant(false)]
    public static ulong CombinationsCount<T>(this  IEnumerable<T> pool, int selectionSize)
    {
      if (pool == null)
        throw Error.ArgumentNull("pool");

      return CombinationsCount(pool.Count(), selectionSize);
    }

    /// <summary>
    /// Calculates the number of combinations found with repetition in the supplied pool.
    /// </summary>
    /// <param name="poolSize">The size of the pool.</param>
    /// <param name="selectionSize">Number of elements in each combination.</param>
    /// <returns>Number of combinations found without repetition in the supplied pool size.</returns>
    [CLSCompliant(false)]
    public static ulong CombinationsCount(int poolSize, int selectionSize)
    {
      checked
      {
        ulong d1 = 1;
        ulong d2 = 1;

        for (uint i = (uint)(poolSize + selectionSize - 1); i >= poolSize; i--)
          d1 = d1 * i;

        for (uint i = 1; i <= selectionSize; i++)
          d2 = d2 * i;

        return d1 / d2;
      }
    }

    #endregion

    #region Combinations with no repetition

    /// <summary>
    /// Creates a sequence of all combinations found without repetition in the supplied pool.
    /// </summary>
    /// <typeparam name="T">Type of pool elements.</typeparam>    
    /// <param name="pool">The pool of elements to use for combinations.</param>
    /// <param name="selectionSize">Number of elements in each combination.</param>
    /// <returns>A sequence of all combinations found in the supplied pool.</returns>
    /// <exception cref="System.ArgumentNullException">pool is null.</exception>    
    public static IEnumerable<IEnumerable<T>> UniqueCombinations<T>(this IEnumerable<T> pool, int selectionSize)
    {
      if (pool == null)
        throw Error.ArgumentNull("pool");

      int[] idxs = Enumerable.Range(0, selectionSize).ToArray();
      T[] pl = pool.ToArray();
      int length = pl.Length;

      do
      {
        yield return GetItems(pl, idxs, selectionSize);
        GetNextUniqueCombination(idxs, selectionSize, length);
      } while (idxs[selectionSize - 1] < length);
    }

    private static void GetNextUniqueCombination(IList<int> idxs, int selectionSize, int poolSize)
    {
      int position = selectionSize - 1;
      int offset = 0;

      idxs[position]++;
      while (idxs[position] == poolSize - offset && position > 0)
      {
        idxs[position - 1]++;
        for (int i = position; i < selectionSize; i++)
          idxs[i] = idxs[i - 1] + 1;
        offset++;
        position--;
      }
    }

    /// <summary>
    /// Calculates the number of combinations found without repetition in the supplied pool.
    /// </summary>
    /// <typeparam name="T">Type of pool elements.</typeparam>    
    /// <param name="pool">The pool of elements to use for combinations.</param>
    /// <param name="selectionSize">Number of elements in each combination.</param>
    /// <returns>Number of combinations found without repetition in the supplied pool.</returns>
    /// <exception cref="System.ArgumentNullException">pool is null.</exception>    
    [CLSCompliant(false)]
    public static ulong UniqueCombinationsCount<T>(this  IEnumerable<T> pool, int selectionSize)
    {
      if (pool == null)
        throw Error.ArgumentNull("pool");

      return UniqueCombinationsCount(pool.Count(), selectionSize);
    }

    /// <summary>
    /// Calculates the number of combinations found without repetition in the supplied pool.
    /// </summary>
    /// <param name="poolSize">The size of the pool.</param>
    /// <param name="selectionSize">Number of elements in each combination.</param>
    /// <returns>Number of combinations found without repetition in the supplied pool size.</returns>
    [CLSCompliant(false)]
    public static ulong UniqueCombinationsCount(int poolSize, int selectionSize)
    {
      checked
      {
        ulong d1 = 1;
        ulong d2 = 1;

        for (uint i = (uint)poolSize; i > poolSize - selectionSize; i--)
          d1 = d1 * i;

        for (uint i = 1; i <= selectionSize; i++)
          d2 = d2 * i;

        return d1 / d2;
      }
    }

    #endregion

    private static IEnumerable<T> GetItems<T>(IList<T> items, IList<int> idxs, int length)
    {
      for (int i = 0; i < length; i++)
        yield return items[idxs[i]];
    }
  }
}
