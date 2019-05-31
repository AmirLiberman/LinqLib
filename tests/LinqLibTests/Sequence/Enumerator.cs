using System;
using System.Collections.Generic;
using System.Linq;
using LinqLib.Sequence;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinqLibTests.Sequence
{
  [TestClass()]
  public class EnumeratorTests
  {
    #region Shuffle

    [TestMethod()]
    public void ShuffleTest1()
    {
      IEnumerable<int> source = Enumerable.Range(1, 1000).ToArray();
      IEnumerable<int> expected = source;
      IEnumerable<int> actual;

      actual = source.Shuffle();
      Assert.AreEqual(expected.Count(), actual.Count());
      Assert.AreEqual(expected.Sum(), actual.Sum());
      Assert.IsTrue(expected.SequenceRelation(actual) == SequenceRelationType.Similar);
    }

    [TestMethod()]
    public void ShuffleTest2()
    {
      int seed = 1231;
      IEnumerable<int> source = Enumerable.Range(1, 1000).ToArray();
      IEnumerable<int> expected = source;
      IEnumerable<int> actual;

      actual = source.Shuffle(seed);
      Assert.AreEqual(expected.Count(), actual.Count());
      Assert.AreEqual(expected.Sum(), actual.Sum());
      Assert.IsTrue(expected.SequenceRelation(actual) == SequenceRelationType.Similar);
    }

    #endregion

    #region Rotate and Cycle

    [TestMethod()]
    public void RotateLeftTest()
    {
      IEnumerable<int> source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
      IEnumerable<int> expected = new int[] { 4, 5, 6, 7, 8, 9, 1, 2, 3 };
      IEnumerable<int> actual;

      actual = source.RotateLeft(3);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void RotateLeftRight()
    {
      IEnumerable<int> source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
      IEnumerable<int> expected = new int[] { 7, 8, 9, 1, 2, 3, 4, 5, 6 };
      IEnumerable<int> actual;

      actual = source.RotateRight(3);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void Cycle()
    {
      IEnumerable<int> source = new int[] { 1, 2, 3 };
      IEnumerable<int> expected = new int[] { 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3 };
      IEnumerable<int> actual;

      actual = source.Cycle().Take(21);
      Assert.IsTrue(expected.SequenceEqual(actual));

      actual = source.Cycle(7);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Sequence Generators

    [TestMethod()]
    public void SequenceGeneratorTest1()
    {
      IEnumerable<TestPet> expected;
      IEnumerable<TestPet> actual;

      expected = new TestPet[] { new TestPet(), new TestPet() };
      actual = Enumerator.Generate<TestPet>(2, () => (new TestPet()));
      Assert.IsTrue(expected.SequenceRelation(actual) == SequenceRelationType.Equal);

      //------------------------------//

      expected = new TestPet[] { new TestPet { Name = "Daisy", Type = "Dog" }, new TestPet { Name = "Tux", Type = "Cat" } };
      actual = Enumerator.Generate<TestPet>(2, (x) => (new TestPet(x)));
      Assert.IsTrue(expected.SequenceRelation(actual, new PetComparer()) == SequenceRelationType.Equal && expected.SequenceRelation(actual) == SequenceRelationType.Equal);
    }

    [TestMethod()]
    public void SequenceGeneratorTest2()
    {
      IEnumerable<decimal> expected = new decimal[] { .03m, .04m, .05m, .06m, .07m, .08m, .09m };
      decimal factor = .01m;
      IEnumerable<decimal> actual = Enumerator.Generate<decimal>(3, 7, (x) => x * factor);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void SequenceGeneratorTest3()
    {
      IEnumerable<int> expected;
      IEnumerable<int> actual;

      expected = new int[] { 9, 16, 25, 36, 49, 64, 81 };
      actual = Enumerator.Generate<int>(3, 7, (x) => x * x);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      expected = new int[] { 3, 6, 9, 12, 15, 18, 21 };
      actual = Enumerator.Generate<int>(3, 7, (x, y) => x + x * y);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      expected = new int[] { 1, 3, 5, 7, 9, 11 };
      actual = Enumerator.Generate(1, 2, 6);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void SequenceGeneratorTest4()
    {
      IEnumerable<int> expected = new int[] { 1, 4, 9, 16, 25, 36, 49, 64, 81 };
      IEnumerable<int> actual = Enumerator.Generate<int>(9, (x) => x * x);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      expected = new int[] { 10, 11, 13, 16, 20, 25, 31 };
      actual = Enumerator.Generate<int, double>(10, .5, 7, (x, y, z) => (int)(x + y + z * 2));
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void SequenceGeneratorTest5()
    {
      IEnumerable<double> expected = new double[] { 1, 3, 5, 7, 9, 11 };
      IEnumerable<double> actual = Enumerator.Generate(1d, 2, 6);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void SequenceGeneratorTest6()
    {
      IEnumerable<float> expected = new float[] { 1, 3, 5, 7, 9, 11 };
      IEnumerable<float> actual = Enumerator.Generate(1f, 2, 6);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void SequenceGeneratorTest7()
    {
      IEnumerable<decimal> expected = new decimal[] { 1, 3, 5, 7, 9, 11 };
      IEnumerable<decimal> actual = Enumerator.Generate(1m, 2, 6);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void SequenceGeneratorTest8()
    {
      IEnumerable<long> expected = new long[] { 1, 3, 5, 7, 9, 11 };
      IEnumerable<long> actual = Enumerator.Generate(1L, 2, 6);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void SequenceGeneratorTest9()
    {
      double angleX = Math.PI * 45 / 180.0;
      IEnumerable<double> expected = new double[] { Math.Sin(angleX + .000), Math.Sin(angleX + .004), Math.Sin(angleX + .008), 
                                                    Math.Sin(angleX + .012), Math.Sin(angleX + .016), Math.Sin(angleX + .020), 
                                                    Math.Sin(angleX + .024), Math.Sin(angleX + .028), Math.Sin(angleX + .032), 
                                                    Math.Sin(angleX + .036), Math.Sin(angleX + .040), Math.Sin(angleX + .044)};
      IEnumerable<double> actual = Enumerator.Generate<double>(angleX, .004d, 12, (X) => Math.Sin(X));
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void SequenceGeneratorTest10()
    {
      try
      {
        IEnumerable<bool> source = new bool[] { true, true, true, true };
        bool[] actual = Enumerator.Generate(false, true, 3).ToArray();
        Assert.Fail();
      }
      catch (InvalidOperationException e) { Assert.IsTrue(e.Message == "Generate<T> cannot be invoked using 'Boolean' type, only numeric values are valid."); }
      catch (Exception) { Assert.Fail(); }
    }

    [TestMethod()]
    public void SequenceGeneratorTest11()
    {
      IEnumerable<DateTime> expected = new DateTime[] { new DateTime(2011, 1, 1, 0, 0, 0), 
                                                        new DateTime(2011, 1, 2, 0, 0, 0), 
                                                        new DateTime(2011, 1, 3, 0, 0, 0), 
                                                        new DateTime(2011, 1, 4, 0, 0, 0), 
                                                        new DateTime(2011, 1, 5, 0, 0, 0), 
                                                        new DateTime(2011, 1, 6, 0, 0, 0) };
      IEnumerable<DateTime> actual = Enumerator.Generate(new DateTime(2011, 1, 1, 0, 0, 0), new TimeSpan(24, 0, 0), 6);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        DateTime[] actualRes = Enumerator.Generate(new DateTime(2011, 1, 1, 0, 0, 0), TimeSpan.Zero, 6).ToArray();
        Assert.IsTrue(expected.SequenceEqual(actual));

      }
      catch (ArgumentOutOfRangeException e) { Assert.IsTrue(e.ParamName == "step"); }
      catch (Exception) { Assert.Fail(); }
    }

    [TestMethod()]
    public void RangeTest1()
    {
      IEnumerable<int> expected = new int[] { 1, 3, 5, 7, 9, 11 };
      IEnumerable<int> actual = Enumerator.Range(1, 2, 11);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void RangeTest2()
    {
      IEnumerable<double> expected = new double[] { 1.25, 2.5, 3.75, 5, 6.25, 7.5 };
      IEnumerable<double> actual = Enumerator.Range(1.25, 1.25, 7.555);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void RangeTest3()
    {
      try
      {
        IEnumerable<bool> source = new bool[] { true, true, true, true };
        bool[] actual = Enumerator.Range(false, true, true).ToArray();
        Assert.Fail();
      }
      catch (InvalidOperationException e) { Assert.IsTrue(e.Message == "Range<T> cannot be invoked using 'Boolean' type, only numeric values are valid."); }
      catch (Exception) { Assert.Fail(); }
    }

    [TestMethod()]
    public void RepeatTest()
    {
      IEnumerable<int> expected = new int[] { 1, 1, 2, 3, 1, 1, 2, 3, 1, 1, 2, 3, 1, 1, 2, 3 };
      IEnumerable<int> actual = Enumerator.Repeat(new int[] { 1, 1, 2, 3 }, 4);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      expected = new int[0];
      actual = Enumerator.Repeat((IEnumerable<int>)null, 1);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void RandomTest()
    {
      IEnumerable<int> r1 = Enumerator.Random<int>(10);
      IEnumerable<int> r2 = Enumerator.Random<int>(10);
      Assert.IsTrue(!r1.SequenceEqual(r2));
    }

    [TestMethod()]
    public void RandomTest2()
    {
      IEnumerable<int> r1 = Enumerator.Random<int>(10, 123);
      IEnumerable<int> r2 = Enumerator.Random<int>(10, 123);
      Assert.IsTrue(r1.SequenceEqual(r2));
    }

    [TestMethod()]
    public void RandomTest3()
    {
      IEnumerable<int> r1 = Enumerator.Random<int>(10, 123);
      IEnumerable<int> r2 = Enumerator.Random<int>(10, 124);
      Assert.IsTrue(!r1.SequenceEqual(r2));
    }

    [TestMethod()]
    public void RandomTest4()
    {
      IEnumerable<int> r1 = Enumerator.Random<int>(10, 123);
      IEnumerable<int> r2 = Enumerator.Random<int>(10, 124);
      Assert.IsTrue(!r1.SequenceEqual(r2));
    }

    [TestMethod()]
    public void RandomTest5()
    {
      try
      {
        int count = Enumerator.Random<DateTime>(10, 123).Count();
        Assert.Fail();
      }
      catch (InvalidOperationException) { }
      catch (Exception) { Assert.Fail(); }
    }

    #endregion

    #region Combine Elements

    [TestMethod()]
    public void CombineTest()
    {
      IEnumerable<string> s1 = new string[] { "A", "B", "C", "D", "E" };
      IEnumerable<int> s2 = new int[] { 1, 2, 3, 4, 5 };
      IEnumerable<string> expected = new string[] { "A1A", "B2B", "C3C", "D4D", "E5E" };
      Func<string, int, string> combine = (S, I) => string.Format("{0}{1}{0}", S, I);

      IEnumerable<string> actual;
      actual = s1.Combine<string, string, int>(s2, combine);
      Assert.IsTrue(actual.SequenceEqual(expected));
    }

    #endregion

    #region Concat Elements

    [TestMethod()]
    public void AppendTest()
    {
      IEnumerable<string> s1 = new string[] { "A", "B", "C", "D", "E" };
      IEnumerable<string> expected = new string[] { "A", "B", "C", "D", "E", "F", "G" };

      IEnumerable<string> actual;
      actual = s1.Append("F", "G");
      Assert.IsTrue(actual.SequenceEqual(expected));

      actual = s1.Append();
      Assert.IsTrue(actual.SequenceEqual(s1));
    }

    [TestMethod()]
    public void PrependTest()
    {
      IEnumerable<string> s1 = new string[] { "A", "B", "C", "D", "E" };
      IEnumerable<string> expected = new string[] { "8", "9", "A", "B", "C", "D", "E" };

      IEnumerable<string> actual;
      actual = s1.Prepend("8", "9");
      Assert.IsTrue(actual.SequenceEqual(expected));

      actual = s1.Prepend();
      Assert.IsTrue(actual.SequenceEqual(s1));
    }

    #endregion

    [TestMethod()]
    public void MinTest()
    {
      IEnumerable<TestPet> s1 = GetPets();
      TestPet youngest = s1.ElementAtMin(x => x.Age);
      Assert.IsTrue(youngest.Name == "Fiona");
    }

    [TestMethod()]
    public void MaxTest()
    {
      IEnumerable<TestPet> s1 = GetPets();
      TestPet oldest = s1.ElementAtMax(x => x.Age);
      Assert.IsTrue(oldest.Name == "Tux");
    }

    [TestMethod()]
    public void AvgTest()
    {
      IEnumerable<TestPet> s1 = GetPets();
      TestPet actual;

      actual = s1.ElementAtAverage(x => x.Age);
      Assert.IsTrue(actual.Name == "MilkChocolate");

      actual = s1.ElementAtAverage(x => x.Age, AverageMatchType.Exact);
      Assert.IsTrue(actual.Name == "MilkChocolate");
      actual = s1.ElementAtAverage(x => x.Age, AverageMatchType.Closest);
      Assert.IsTrue(actual.Name == "MilkChocolate");
      actual = s1.ElementAtAverage(x => x.Age, AverageMatchType.ExactOrLarger);
      Assert.IsTrue(actual.Name == "MilkChocolate");
      actual = s1.ElementAtAverage(x => x.Age, AverageMatchType.ExactOrSmaller);
      Assert.IsTrue(actual.Name == "MilkChocolate");

      s1.Last().Age += 3.5;

      actual = s1.ElementAtAverage(x => x.Age, AverageMatchType.Exact);
      Assert.IsTrue(actual == null);
      actual = s1.ElementAtAverage(x => x.Age, AverageMatchType.Closest);
      Assert.IsTrue(actual.Name == "MilkChocolate");
      actual = s1.ElementAtAverage(x => x.Age, AverageMatchType.ExactOrLarger);
      Assert.IsTrue(actual.Name == "Ace");
      actual = s1.ElementAtAverage(x => x.Age, AverageMatchType.ExactOrSmaller);
      Assert.IsTrue(actual.Name == "MilkChocolate");

      s1.Last().Age += 3.5;
      actual = s1.ElementAtAverage(x => x.Age, AverageMatchType.Closest);
      Assert.IsTrue(actual.Name == "Ace");

      try
      {
        actual = s1.ElementAtAverage(x => x.Age, (AverageMatchType)99);
        Assert.Fail();
      }
      catch (ArgumentOutOfRangeException)
      {

      }
      catch (Exception)
      {
        Assert.Fail();
      }
    }

    [TestMethod()]
    public void DistinctTest()
    {
      string[] source = new string[] { "A1", "A1", "a1", "A2", "A3", "B1", "C1", "d1", "D1", "D1", "D1", "D1", "D1", "D1", "D2", "D2", "D3" };

      string[] expected1 = new string[] { "A1", "a1", "B1", "C1", "d1", "D1" };
      string[] actual1 = source.Distinct(x => x[0]).ToArray();
      Assert.IsTrue(expected1.SequenceEqual(actual1));

      string[] expected2 = new string[] { "A1", "B1", "C1", "d1" };
      string[] actual2 = source.Distinct(x => x[0], new CasseInsensitiveCharComparer()).ToArray();
      Assert.IsTrue(expected2.SequenceEqual(actual2));

      string[] expected3a = new string[] { "A1", "a1", "A2", "A3", "B1", "C1", "d1", "D1", "D2", "D3" };
      int[] expected3b = new int[] { 2, 1, 1, 1, 1, 1, 1, 6, 2, 1 };
      ItemCounter<string>[] actual3 = source.DistinctWithCount().ToArray();
      Assert.IsTrue(expected3a.SequenceEqual(actual3.Select(A => A.Item)));
      Assert.IsTrue(expected3b.SequenceEqual(actual3.Select(A => A.Count)));

      string[] expected4a = new string[] { "A1", "A2", "A3", "B1", "C1", "d1", "D2", "D3" };
      int[] expected4b = new int[] { 3, 1, 1, 1, 1, 7, 2, 1 };
      ItemCounter<string>[] actual4 = source.DistinctWithCount(new CasseInsensitiveStringComparer()).ToArray();
      Assert.IsTrue(expected4a.SequenceEqual(actual4.Select(A => A.Item)));
      Assert.IsTrue(expected4b.SequenceEqual(actual4.Select(A => A.Count)));

      string[] expected5a = new string[] { "A1", "a1", "B1", "C1", "d1", "D1" };
      int[] expected5b = new int[] { 4, 1, 1, 1, 1, 9 };
      ItemCounter<string>[] actual5 = source.DistinctWithCount(x => x[0]).ToArray();
      Assert.IsTrue(expected5a.SequenceEqual(actual5.Select(A => A.Item)));
      Assert.IsTrue(expected5b.SequenceEqual(actual5.Select(A => A.Count)));

      string[] expected6a = new string[] { "A1", "B1", "C1", "d1" };
      int[] expected6b = new int[] { 5, 1, 1, 10 };
      ItemCounter<string>[] actual6 = source.DistinctWithCount(x => x[0], new CasseInsensitiveCharComparer()).ToArray();
      Assert.IsTrue(expected6a.SequenceEqual(actual6.Select(A => A.Item)));
      Assert.IsTrue(expected6b.SequenceEqual(actual6.Select(A => A.Count)));
    }

    [TestMethod()]
    public void ElementsWithinStdDevTest()
    {
      int[] source = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 4, -5 };
      int[] expected = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2 };
      IEnumerable<int> result = source.ElementsWithinStdDev(x => x, 1);

      Assert.IsTrue(result.SequenceEqual(expected));
    }

    [TestMethod()]
    public void ElementsOutsideStdDevTest()
    {
      int[] source = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 4, -5 };
      int[] expected = new int[] { 4, -5 };
      IEnumerable<int> result = source.ElementsOutsideStdDev(x => x, 1);

      Assert.IsTrue(result.SequenceEqual(expected));
    }

    [TestMethod()]
    public void InterleaveTest()
    {
      int[][] source = new int[][] { new int[] { 1, 2, 3 }, new int[] { 1, 2, 3 }, new int[] { 1, 2, 3 }, new int[] { 1, 2, 3 } };
      int[][] expected = new int[][] { new int[] { 1, 1, 1, 1 }, new int[] { 2, 2, 2, 2 }, new int[] { 3, 3, 3, 3 } };
      IEnumerable<int>[] res = source.Interleave().ToArray();

      Assert.IsTrue(res[0].SequenceEqual(expected[0]));
      Assert.IsTrue(res[1].SequenceEqual(expected[1]));
      Assert.IsTrue(res[2].SequenceEqual(expected[2]));
      Assert.IsTrue(res.Length == 3);

      int[][] source2 = new int[][] { new int[] { 1, 2, 3 }, new int[] { 1, 2, 3 }, new int[] { 1, 2 }, new int[] { 1, 2, 3 } };
      int[][] expected2 = new int[][] { new int[] { 1, 1, 1, 1 }, new int[] { 2, 2, 2, 2 } };
      IEnumerable<int>[] res2 = source2.Interleave().ToArray();

      Assert.IsTrue(res2[0].SequenceEqual(expected2[0]));
      Assert.IsTrue(res2[1].SequenceEqual(expected2[1]));
      Assert.IsTrue(res2.Length == 2);
    }

    [TestMethod()]
    public void InterleaveTest2()
    {
      int[][] source = new int[][] { new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 1, 2, 3, 4 }, new int[] { 1, 2, 3 } };
      int[][] expected = new int[][] { new int[] { 1, 1, 1, 1 }, new int[] { 2, 2, 2, 2 }, new int[] { 3, 3, 3, 3 }, new int[] { 4, 4, 4, -1 }, new int[] { 5, 5, -1, -1 }, new int[] { 6, 6, -1, -1 }, new int[] { 7, -1, -1, -1 }, new int[] { 8, -1, -1, -1 }, new int[] { 9, -1, -1, -1 } };
      IEnumerable<int>[] res = source.Interleave(-1).ToArray();

      Assert.IsTrue(res.Length == 9);
      for (int i = 0; i < 9; i++)
        Assert.IsTrue(res[i].SequenceEqual(expected[i]));
    }

    private static IEnumerable<TestPet> GetPets()
    {
      return new TestPet[] { new TestPet { Name = "Daisy", Type = "Dog", Age = 2.5 }, 
                             new TestPet { Name = "Tux", Type = "Cat", Age = 5.2 }, 
                             new TestPet { Name = "Peanut", Type = "Cat", Age = 2.2 }, 
                             new TestPet { Name = "Fiona", Type = "Donkey", Age = 0.8 }, 
                             new TestPet { Name = "MilkChocolate", Type = "Goat", Age = 3.0 }, 
                             new TestPet { Name = "Ace", Type = "Horse", Age = 4.2 } ,
                             new TestPet { Name = "Star", Type = "Horse", Age = 3.1 } };
    }
  }

  internal class CasseInsensitiveCharComparer : IEqualityComparer<char>
  {
    #region IEqualityComparer<char> Members

    public bool Equals(char x, char y)
    {
      return char.ToLowerInvariant(x) == char.ToLowerInvariant(y);
    }

    public int GetHashCode(char obj)
    {
      return char.ToLowerInvariant(obj).GetHashCode();
    }

    #endregion
  }

  internal class CasseInsensitiveStringComparer : IEqualityComparer<string>
  {
    #region IEqualityComparer<string> Members

    public bool Equals(string x, string y)
    {
      return x.ToLowerInvariant() == y.ToLowerInvariant();
    }

    public int GetHashCode(string obj)
    {
      return obj.ToLowerInvariant().GetHashCode();
    }

    #endregion
  }
}
