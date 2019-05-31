using System.Linq;
using LinqLib.Array;
using LinqLib.Sequence;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinqLibTests.Sequence
{
  [TestClass()]
  public class CombinatoricsTests
  {
    [TestMethod()]
    public void UniqueCombinationsTest()
    {
      Assert.IsTrue(Combinatorics.UniqueCombinationsCount(8, 5) == 56);
      Assert.IsTrue(Combinatorics.UniqueCombinationsCount(8, 2) == 28);
      Assert.IsTrue(Combinatorics.UniqueCombinationsCount(18, 5) == 8568);
      Assert.IsTrue(Combinatorics.UniqueCombinationsCount(17, 6) == 12376);

      int[,] expected = new int[,] { { 0, 1, 2 }, { 0, 1, 3 }, { 0, 1, 4 }, { 0, 1, 5 }, 
                                     { 0, 2, 3 }, { 0, 2, 4 }, { 0, 2, 5 }, { 0, 3, 4 }, 
                                     { 0, 3, 5 }, { 0, 4, 5 }, { 1, 2, 3 }, { 1, 2, 4 }, 
                                     { 1, 2, 5 }, { 1, 3, 4 }, { 1, 3, 5 }, { 1, 4, 5 }, 
                                     { 2, 3, 4 }, { 2, 3, 5 }, { 2, 4, 5 }, { 3, 4, 5 } };

      int[] arr = Enumerable.Range(0, 6).ToArray();

      Assert.IsTrue(Combinatorics.UniqueCombinationsCount(arr, 3) == 20);

      int[,] x = arr.UniqueCombinations(3).Select(X => X.ToArray()).ToArray().FromJagged();
      Assert.IsTrue(expected.ArrayEquals(x));

      int[,] x1 = arr.Combinations(3, false).Select(X => X.ToArray()).ToArray().FromJagged();
      Assert.IsTrue(expected.ArrayEquals(x1));
    }

    [TestMethod()]
    public void CombinationsTest()
    {
      Assert.IsTrue(Combinatorics.CombinationsCount(8, 5) == 792);
      Assert.IsTrue(Combinatorics.CombinationsCount(8, 2) == 36);
      Assert.IsTrue(Combinatorics.CombinationsCount(18, 5) == 26334);
      Assert.IsTrue(Combinatorics.CombinationsCount(17, 6) == 74613);

      int[,] expected = new int[,] { { 0, 0, 0 }, { 0, 0, 1 }, { 0, 0, 2 }, { 0, 0, 3 }, { 0, 0, 4 }, { 0, 0, 5 }, { 0, 1, 1 }, { 0, 1, 2 }, 
                                     { 0, 1, 3 }, { 0, 1, 4 }, { 0, 1, 5 }, { 0, 2, 2 }, { 0, 2, 3 }, { 0, 2, 4 }, { 0, 2, 5 }, { 0, 3, 3 }, 
                                     { 0, 3, 4 }, { 0, 3, 5 }, { 0, 4, 4 }, { 0, 4, 5 }, { 0, 5, 5 }, { 1, 1, 1 }, { 1, 1, 2 }, { 1, 1, 3 }, 
                                     { 1, 1, 4 }, { 1, 1, 5 }, { 1, 2, 2 }, { 1, 2, 3 }, { 1, 2, 4 }, { 1, 2, 5 }, { 1, 3, 3 }, { 1, 3, 4 }, 
                                     { 1, 3, 5 }, { 1, 4, 4 }, { 1, 4, 5 }, { 1, 5, 5 }, { 2, 2, 2 }, { 2, 2, 3 }, { 2, 2, 4 }, { 2, 2, 5 }, 
                                     { 2, 3, 3 }, { 2, 3, 4 }, { 2, 3, 5 }, { 2, 4, 4 }, { 2, 4, 5 }, { 2, 5, 5 }, { 3, 3, 3 }, { 3, 3, 4 }, 
                                     { 3, 3, 5 }, { 3, 4, 4 }, { 3, 4, 5 }, { 3, 5, 5 }, { 4, 4, 4 }, { 4, 4, 5 }, { 4, 5, 5 }, { 5, 5, 5 } };

      int[] arr = Enumerable.Range(0, 6).ToArray();

      Assert.IsTrue(Combinatorics.CombinationsCount(arr, 3) == 56);

      int[,] x = arr.Combinations(3).Select(X => X.ToArray()).ToArray().FromJagged();
      Assert.IsTrue(expected.ArrayEquals(x));

      int[,] x1 = arr.Combinations(3, true).Select(X => X.ToArray()).ToArray().FromJagged();
      Assert.IsTrue(expected.ArrayEquals(x1));
    }

    [TestMethod()]
    public void UniquePermutationsTest()
    {
      Assert.IsTrue(Combinatorics.UniquePermutationsCount(8, 5) == 6720);
      Assert.IsTrue(Combinatorics.UniquePermutationsCount(8, 2) == 56);
      Assert.IsTrue(Combinatorics.UniquePermutationsCount(18, 5) == 1028160);
      Assert.IsTrue(Combinatorics.UniquePermutationsCount(17, 6) == 8910720);

      int[,] expected = new int[,] { { 0, 1, 2 }, { 0, 1, 3 }, { 0, 1, 4 }, { 0, 1, 5 }, { 0, 2, 1 }, { 0, 2, 3 }, { 0, 2, 4 }, { 0, 2, 5 },
                                     { 0, 3, 1 }, { 0, 3, 2 }, { 0, 3, 4 }, { 0, 3, 5 }, { 0, 4, 1 }, { 0, 4, 2 }, { 0, 4, 3 }, { 0, 4, 5 },
                                     { 0, 5, 1 }, { 0, 5, 2 }, { 0, 5, 3 }, { 0, 5, 4 }, { 1, 0, 2 }, { 1, 0, 3 }, { 1, 0, 4 }, { 1, 0, 5 },
                                     { 1, 2, 0 }, { 1, 2, 3 }, { 1, 2, 4 }, { 1, 2, 5 }, { 1, 3, 0 }, { 1, 3, 2 }, { 1, 3, 4 }, { 1, 3, 5 },
                                     { 1, 4, 0 }, { 1, 4, 2 }, { 1, 4, 3 }, { 1, 4, 5 }, { 1, 5, 0 }, { 1, 5, 2 }, { 1, 5, 3 }, { 1, 5, 4 },
                                     { 2, 0, 1 }, { 2, 0, 3 }, { 2, 0, 4 }, { 2, 0, 5 }, { 2, 1, 0 }, { 2, 1, 3 }, { 2, 1, 4 }, { 2, 1, 5 },
                                     { 2, 3, 0 }, { 2, 3, 1 }, { 2, 3, 4 }, { 2, 3, 5 }, { 2, 4, 0 }, { 2, 4, 1 }, { 2, 4, 3 }, { 2, 4, 5 },
                                     { 2, 5, 0 }, { 2, 5, 1 }, { 2, 5, 3 }, { 2, 5, 4 }, { 3, 0, 1 }, { 3, 0, 2 }, { 3, 0, 4 }, { 3, 0, 5 },
                                     { 3, 1, 0 }, { 3, 1, 2 }, { 3, 1, 4 }, { 3, 1, 5 }, { 3, 2, 0 }, { 3, 2, 1 }, { 3, 2, 4 }, { 3, 2, 5 },
                                     { 3, 4, 0 }, { 3, 4, 1 }, { 3, 4, 2 }, { 3, 4, 5 }, { 3, 5, 0 }, { 3, 5, 1 }, { 3, 5, 2 }, { 3, 5, 4 },
                                     { 4, 0, 1 }, { 4, 0, 2 }, { 4, 0, 3 }, { 4, 0, 5 }, { 4, 1, 0 }, { 4, 1, 2 }, { 4, 1, 3 }, { 4, 1, 5 },
                                     { 4, 2, 0 }, { 4, 2, 1 }, { 4, 2, 3 }, { 4, 2, 5 }, { 4, 3, 0 }, { 4, 3, 1 }, { 4, 3, 2 }, { 4, 3, 5 },
                                     { 4, 5, 0 }, { 4, 5, 1 }, { 4, 5, 2 }, { 4, 5, 3 }, { 5, 0, 1 }, { 5, 0, 2 }, { 5, 0, 3 }, { 5, 0, 4 },
                                     { 5, 1, 0 }, { 5, 1, 2 }, { 5, 1, 3 }, { 5, 1, 4 }, { 5, 2, 0 }, { 5, 2, 1 }, { 5, 2, 3 }, { 5, 2, 4 },
                                     { 5, 3, 0 }, { 5, 3, 1 }, { 5, 3, 2 }, { 5, 3, 4 }, { 5, 4, 0 }, { 5, 4, 1 }, { 5, 4, 2 }, { 5, 4, 3 } };

      int[] arr = Enumerable.Range(0, 6).ToArray();

      Assert.IsTrue(Combinatorics.UniquePermutationsCount(arr, 3) == 120);

      int[,] x = arr.UniquePermutations(3).Select(X => X.ToArray()).ToArray().FromJagged();
      Assert.IsTrue(expected.ArrayEquals(x));

      int[,] x1 = arr.Permutations(3, false).Select(X => X.ToArray()).ToArray().FromJagged();
      Assert.IsTrue(expected.ArrayEquals(x1));
    }

    [TestMethod()]
    public void PermutationsTest()
    {
      Assert.IsTrue(Combinatorics.PermutationsCount(8, 5) == 32768);
      Assert.IsTrue(Combinatorics.PermutationsCount(8, 2) == 64);
      Assert.IsTrue(Combinatorics.PermutationsCount(18, 5) == 1889568);
      Assert.IsTrue(Combinatorics.PermutationsCount(17, 6) == 24137569);

      int[,] expected = new int[,] { { 0, 0, 0 }, { 0, 0, 1 }, { 0, 0, 2 }, { 0, 0, 3 }, { 0, 0, 4 }, { 0, 0, 5 }, { 0, 1, 0 }, { 0, 1, 1 }, 
                                     { 0, 1, 2 }, { 0, 1, 3 }, { 0, 1, 4 }, { 0, 1, 5 }, { 0, 2, 0 }, { 0, 2, 1 }, { 0, 2, 2 }, { 0, 2, 3 }, 
                                     { 0, 2, 4 }, { 0, 2, 5 }, { 0, 3, 0 }, { 0, 3, 1 }, { 0, 3, 2 }, { 0, 3, 3 }, { 0, 3, 4 }, { 0, 3, 5 }, 
                                     { 0, 4, 0 }, { 0, 4, 1 }, { 0, 4, 2 }, { 0, 4, 3 }, { 0, 4, 4 }, { 0, 4, 5 }, { 0, 5, 0 }, { 0, 5, 1 }, 
                                     { 0, 5, 2 }, { 0, 5, 3 }, { 0, 5, 4 }, { 0, 5, 5 }, { 1, 0, 0 }, { 1, 0, 1 }, { 1, 0, 2 }, { 1, 0, 3 }, 
                                     { 1, 0, 4 }, { 1, 0, 5 }, { 1, 1, 0 }, { 1, 1, 1 }, { 1, 1, 2 }, { 1, 1, 3 }, { 1, 1, 4 }, { 1, 1, 5 }, 
                                     { 1, 2, 0 }, { 1, 2, 1 }, { 1, 2, 2 }, { 1, 2, 3 }, { 1, 2, 4 }, { 1, 2, 5 }, { 1, 3, 0 }, { 1, 3, 1 }, 
                                     { 1, 3, 2 }, { 1, 3, 3 }, { 1, 3, 4 }, { 1, 3, 5 }, { 1, 4, 0 }, { 1, 4, 1 }, { 1, 4, 2 }, { 1, 4, 3 }, 
                                     { 1, 4, 4 }, { 1, 4, 5 }, { 1, 5, 0 }, { 1, 5, 1 }, { 1, 5, 2 }, { 1, 5, 3 }, { 1, 5, 4 }, { 1, 5, 5 }, 
                                     { 2, 0, 0 }, { 2, 0, 1 }, { 2, 0, 2 }, { 2, 0, 3 }, { 2, 0, 4 }, { 2, 0, 5 }, { 2, 1, 0 }, { 2, 1, 1 }, 
                                     { 2, 1, 2 }, { 2, 1, 3 }, { 2, 1, 4 }, { 2, 1, 5 }, { 2, 2, 0 }, { 2, 2, 1 }, { 2, 2, 2 }, { 2, 2, 3 }, 
                                     { 2, 2, 4 }, { 2, 2, 5 }, { 2, 3, 0 }, { 2, 3, 1 }, { 2, 3, 2 }, { 2, 3, 3 }, { 2, 3, 4 }, { 2, 3, 5 }, 
                                     { 2, 4, 0 }, { 2, 4, 1 }, { 2, 4, 2 }, { 2, 4, 3 }, { 2, 4, 4 }, { 2, 4, 5 }, { 2, 5, 0 }, { 2, 5, 1 }, 
                                     { 2, 5, 2 }, { 2, 5, 3 }, { 2, 5, 4 }, { 2, 5, 5 }, { 3, 0, 0 }, { 3, 0, 1 }, { 3, 0, 2 }, { 3, 0, 3 }, 
                                     { 3, 0, 4 }, { 3, 0, 5 }, { 3, 1, 0 }, { 3, 1, 1 }, { 3, 1, 2 }, { 3, 1, 3 }, { 3, 1, 4 }, { 3, 1, 5 }, 
                                     { 3, 2, 0 }, { 3, 2, 1 }, { 3, 2, 2 }, { 3, 2, 3 }, { 3, 2, 4 }, { 3, 2, 5 }, { 3, 3, 0 }, { 3, 3, 1 }, 
                                     { 3, 3, 2 }, { 3, 3, 3 }, { 3, 3, 4 }, { 3, 3, 5 }, { 3, 4, 0 }, { 3, 4, 1 }, { 3, 4, 2 }, { 3, 4, 3 }, 
                                     { 3, 4, 4 }, { 3, 4, 5 }, { 3, 5, 0 }, { 3, 5, 1 }, { 3, 5, 2 }, { 3, 5, 3 }, { 3, 5, 4 }, { 3, 5, 5 }, 
                                     { 4, 0, 0 }, { 4, 0, 1 }, { 4, 0, 2 }, { 4, 0, 3 }, { 4, 0, 4 }, { 4, 0, 5 }, { 4, 1, 0 }, { 4, 1, 1 }, 
                                     { 4, 1, 2 }, { 4, 1, 3 }, { 4, 1, 4 }, { 4, 1, 5 }, { 4, 2, 0 }, { 4, 2, 1 }, { 4, 2, 2 }, { 4, 2, 3 }, 
                                     { 4, 2, 4 }, { 4, 2, 5 }, { 4, 3, 0 }, { 4, 3, 1 }, { 4, 3, 2 }, { 4, 3, 3 }, { 4, 3, 4 }, { 4, 3, 5 }, 
                                     { 4, 4, 0 }, { 4, 4, 1 }, { 4, 4, 2 }, { 4, 4, 3 }, { 4, 4, 4 }, { 4, 4, 5 }, { 4, 5, 0 }, { 4, 5, 1 }, 
                                     { 4, 5, 2 }, { 4, 5, 3 }, { 4, 5, 4 }, { 4, 5, 5 }, { 5, 0, 0 }, { 5, 0, 1 }, { 5, 0, 2 }, { 5, 0, 3 }, 
                                     { 5, 0, 4 }, { 5, 0, 5 }, { 5, 1, 0 }, { 5, 1, 1 }, { 5, 1, 2 }, { 5, 1, 3 }, { 5, 1, 4 }, { 5, 1, 5 }, 
                                     { 5, 2, 0 }, { 5, 2, 1 }, { 5, 2, 2 }, { 5, 2, 3 }, { 5, 2, 4 }, { 5, 2, 5 }, { 5, 3, 0 }, { 5, 3, 1 }, 
                                     { 5, 3, 2 }, { 5, 3, 3 }, { 5, 3, 4 }, { 5, 3, 5 }, { 5, 4, 0 }, { 5, 4, 1 }, { 5, 4, 2 }, { 5, 4, 3 }, 
                                     { 5, 4, 4 }, { 5, 4, 5 }, { 5, 5, 0 }, { 5, 5, 1 }, { 5, 5, 2 }, { 5, 5, 3 }, { 5, 5, 4 }, { 5, 5, 5 } };

      int[] arr = Enumerable.Range(0, 6).ToArray();

      Assert.IsTrue(Combinatorics.PermutationsCount(arr, 3) == 216);

      int[,] x = arr.Permutations(3).Select(X => X.ToArray()).ToArray().FromJagged();
      Assert.IsTrue(expected.ArrayEquals(x));

      int[,] x1 = arr.Permutations(3, true).Select(X => X.ToArray()).ToArray().FromJagged();
      Assert.IsTrue(expected.ArrayEquals(x1));
    }
  }
}
