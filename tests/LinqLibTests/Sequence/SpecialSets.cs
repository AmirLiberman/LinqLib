using System.Collections.Generic;
using System.Linq;
using LinqLib.Sequence;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinqLibTests.Sequence
{
  [TestClass()]
  public class SpecialSetsTests
  {
    [TestMethod()]
    public void FactorialsTest()
    {
      IEnumerable<long> expected = new long[] { 1, 2, 6, 24, 120, 720, 5040, 40320, 362880, 3628800 };
      IEnumerable<long> actual = SpecializedSets.Factorials().ToArray().Take(10); //Force calculating all (19) factorials
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void FibsTest()
    {
      IEnumerable<long> expected = new long[] { 1, 2, 3, 5, 8, 13, 21, 34, 55, 89 };
      IEnumerable<long> actual = SpecializedSets.Fibs().ToArray().Take(10); //Force calculating all (90) fibs
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void PrimesTest1()
    {
      IEnumerable<long> expected = new long[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29 };
      IEnumerable<long> actual = SpecializedSets.Primes().Take(10);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void PrimesTest2()
    {
      IEnumerable<long> expected = new long[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31 };
      IEnumerable<long> actual = SpecializedSets.Primes(31);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void PrimeDivisorsTest()
    {
      IEnumerable<long> expected;
      IEnumerable<long> actual;

      expected = new long[] { 233, 577, 4871 };
      actual = SpecializedSets.PrimeDivisors(654862111);
      Assert.IsTrue(expected.SequenceEqual(actual));

      expected = new long[] { 2, 2, 2, 2, 2, 37, 553093 };
      actual = SpecializedSets.PrimeDivisors(654862112);
      Assert.IsTrue(expected.SequenceEqual(actual));

      expected = new long[] { 3, 31, 101, 697181 };
      actual = SpecializedSets.PrimeDivisors(6548621133);
      Assert.IsTrue(expected.SequenceEqual(actual));

      expected = new long[] { 2, 1429, 229133 };
      actual = SpecializedSets.PrimeDivisors(654862114);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void PrimeFactorsTest()
    {
      IEnumerable<long> expected;
      IEnumerable<long> actual;

      expected = new long[] { 233, 577, 4871 };
      actual = SpecializedSets.PrimeFactors(654862111);
      Assert.IsTrue(expected.SequenceEqual(actual));

      expected = new long[] { 2, 37, 553093 };
      actual = SpecializedSets.PrimeFactors(654862112);
      Assert.IsTrue(expected.SequenceEqual(actual));

      expected = new long[] { 2, 3, 11, 17 };
      actual = SpecializedSets.PrimeFactors(57222);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void DivisorsTest()
    {
      IEnumerable<long> expected;
      IEnumerable<long> actual;

      expected = new long[] { 1, 2, 2823439, 5646878 };
      actual = SpecializedSets.Divisors(5646878).OrderBy(x => x);
      Assert.IsTrue(expected.SequenceEqual(actual));

      expected = new long[] { 1, 3, 7, 9, 21, 63, 89633, 268899, 627431, 806697, 1882293, 5646879 };
      actual = SpecializedSets.Divisors(5646879).OrderBy(x => x);
      Assert.IsTrue(expected.SequenceEqual(actual));

      expected = new long[] { 1, 3, 9, 19, 57, 171, 3829603, 11488809, 34466427, 72762457, 218287371, 654862113 };
      actual = SpecializedSets.Divisors(654862113).OrderBy(x => x);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void IsPromeTest()
    {
      bool actual;

      actual = SpecializedSets.IsPrime(1);
      Assert.IsFalse(actual);

      actual = SpecializedSets.IsPrime(2);
      Assert.IsTrue(actual);

      actual = SpecializedSets.IsPrime(3);
      Assert.IsTrue(actual);

      actual = SpecializedSets.IsPrime(4);
      Assert.IsFalse(actual);
    }

    [TestMethod()]
    public void ProperDivisorsTest()
    {
      IEnumerable<long> expected;
      IEnumerable<long> actual;

      expected = new long[] { 1, 2, 2823439 };
      actual = SpecializedSets.ProperDivisors(5646878).OrderBy(x => x);
      Assert.IsTrue(expected.SequenceEqual(actual));

      expected = new long[] { 1, 3, 7, 9, 21, 63, 89633, 268899, 627431, 806697, 1882293 };
      actual = SpecializedSets.ProperDivisors(5646879).OrderBy(x => x);
      Assert.IsTrue(expected.SequenceEqual(actual));

      expected = new long[] { 1, 3, 9, 19, 57, 171, 3829603, 11488809, 34466427, 72762457, 218287371 };
      actual = SpecializedSets.ProperDivisors(654862113).OrderBy(x => x);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void SumOfDigitsTest1()
    {
      int value = 123456;
      int expected = 21;
      int actual = SpecializedSets.SumOfDigits(value);
      Assert.IsTrue(expected == actual);
    }

    [TestMethod()]
    public void SumOfDigitsTest2()
    {
      long value = 123456;
      long expected = 21;
      int actual = SpecializedSets.SumOfDigits(value);
      Assert.IsTrue(expected == actual);
    }

    [TestMethod()]
    public void ToNumberTest()
    {
      int[] digits = new int[] { 1, 2, 3, 4, 5, 6 };
      long expected = 654321;
      long actual = SpecializedSets.ToNumber(digits);
      Assert.IsTrue(expected == actual);
    }

    [TestMethod()]
    public void ToDigitsTest()
    {
      long input = 12345678;
      IEnumerable<int> expected = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
      IEnumerable<int> actual = SpecializedSets.ToDigits(input);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void IsPrimeTest()
    {
      long input = 1;
      bool expected = false;
      bool actual = SpecializedSets.IsPrime(input);
      Assert.IsTrue(actual == expected);
    }

    [TestMethod()]
    public void PhisTest()
    {
      IEnumerable<long> input = Enumerator.Range<long>(1, 1, 99);
      IEnumerable<long> expected = new long[] { 1, 1, 2, 2, 4, 2, 6, 4, 6, 
                                                4, 10, 4, 12, 6, 8, 8, 16, 6, 18,
                                                8, 12, 10, 22, 8, 20, 12, 18, 12, 28,
                                                8, 30, 16, 20, 16, 24, 12, 36, 18, 24, 
                                                16, 40, 12, 42, 20, 24, 22, 46, 16, 42, 
                                                20, 32, 24, 52, 18, 40, 24, 36, 28, 58, 
                                                16, 60, 30, 36, 32, 48, 20, 66, 32, 44, 
                                                24, 70, 24, 72, 36, 40, 36, 60, 24, 78, 
                                                32, 54, 40, 82, 24, 64, 42, 56, 40, 88, 
                                                24, 72, 44, 60, 46, 72, 32, 96, 42, 60 };

      IEnumerable<long> actual;
      actual = input.Phi();
      Assert.IsTrue(expected.SequenceEqual(actual));
    }
  }
}
