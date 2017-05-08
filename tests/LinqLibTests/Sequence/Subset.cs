using System;
using System.Collections.Generic;
using System.Linq;
using LinqLib.Sequence;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinqLibTests.Sequence
{
  [TestClass()]
  public class SubsetTests
  {
    #region Take Pattern

    [TestMethod()]
    public void TakeEvenTest()
    {
      IEnumerable<int> source;
      IEnumerable<int> expected;
      IEnumerable<int> actual;

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
      expected = new int[] { 2, 4, 6, 8, 10 };

      actual = source.TakeEven();
      Assert.IsTrue(actual.SequenceEqual(expected));

      //------------------------------//

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 2, 4, 6, 8, 10 };

      actual = source.TakeEven();
      Assert.IsTrue(actual.SequenceEqual(expected));
    }

    [TestMethod()]
    public void TakeOddTest()
    {
      IEnumerable<int> source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
      IEnumerable<int> expected = new int[] { 1, 3, 5, 7, 9 };
      IEnumerable<int> actual;

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
      expected = new int[] { 1, 3, 5, 7, 9 };

      actual = source.TakeOdd();
      Assert.IsTrue(actual.SequenceEqual(expected));

      //------------------------------//

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 1, 3, 5, 7, 9 };

      actual = source.TakeOdd();
      Assert.IsTrue(actual.SequenceEqual(expected));
    }

    [TestMethod()]
    public void TakePatternTest()
    {
      IEnumerable<int> source;
      IEnumerable<int> expected;
      IEnumerable<int> actual;
      int take;
      int skip;
      int initialSkip;

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
      expected = new int[] { 1, 4, 7, 10 };
      take = 1;
      skip = 2;

      actual = source.TakePattern(take, skip);
      Assert.IsTrue(actual.SequenceEqual(expected));

      //------------------------------//

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 2, 3, 5, 6, 8, 9 };
      initialSkip = 1;
      take = 2;
      skip = 1;

      actual = source.TakePattern(initialSkip, take, skip);
      Assert.IsTrue(actual.SequenceEqual(expected));

      //------------------------------//

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
      expected = new int[0];
      initialSkip = 15;
      take = 2;
      skip = 1;

      actual = source.TakePattern(initialSkip, take, skip);
      Assert.IsTrue(actual.SequenceEqual(expected));

      //------------------------------//

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 2, 3, 4, 5, 10 };
      initialSkip = 1;
      take = 4;
      skip = 4;

      actual = source.TakePattern(initialSkip, take, skip);
      Assert.IsTrue(actual.SequenceEqual(expected));

      //------------------------------//

      try
      {
        source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        initialSkip = 1;
        take = -4;
        skip = 4;
        actual = source.TakePattern(initialSkip, take, skip).ToArray();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "take"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      try
      {
        source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        initialSkip = 1;
        take = 4;
        skip = -4;
        actual = source.TakePattern(initialSkip, take, skip).ToArray();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "skip"); }
      catch (Exception) { Assert.Fail(); }
    }

    #endregion

    #region Take Before

    [TestMethod()]
    public void TakeBeforeTest()
    {
      IEnumerable<int> source;
      IEnumerable<int> expected;
      IEnumerable<int> actual;
      TestEqualityComparer<int> et = new TestEqualityComparer<int>();
      int item;
      int count;

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 1, 2, 3, 4, 5 };
      item = 6;

      actual = source.TakeBefore(item);
      Assert.IsTrue(actual.SequenceEqual(expected));

      //------------------------------//

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 4, 5 };
      item = 6;
      count = 2;

      actual = source.TakeBefore(item, count);
      Assert.IsTrue(actual.SequenceEqual(expected));

      //------------------------------//

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 1, 2, 3, 4, 5 };
      item = 6;

      actual = source.TakeBefore(item, et);
      Assert.IsTrue(actual.SequenceEqual(expected));

      //------------------------------//

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 1, 2, 3, 4, 5 };
      item = 6;

      actual = source.TakeBefore(item);
      Assert.IsTrue(actual.SequenceEqual(expected));

      //------------------------------//

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 4, 5 };
      item = 6;
      count = 2;

      actual = source.TakeBefore(item, count, et);
      Assert.IsTrue(actual.SequenceEqual(expected));

      //------------------------------//

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 4, 5 };
      item = 6;
      count = 2;

      actual = source.TakeBefore(item, count);
      Assert.IsTrue(actual.SequenceEqual(expected));
    }

    [TestMethod()]
    public void TakeSelfAndBeforeTest()
    {
      IEnumerable<int> source;
      IEnumerable<int> expected;
      IEnumerable<int> actual;
      TestEqualityComparer<int> et = new TestEqualityComparer<int>();
      int item;
      int count;

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 1, 2, 3, 4, 5 };
      item = 5;

      actual = source.TakeSelfAndBefore(item);
      Assert.IsTrue(actual.SequenceEqual(expected));

      //------------------------------//

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 3, 4, 5 };
      item = 5;
      count = 2;

      actual = source.TakeSelfAndBefore(item, count);
      Assert.IsTrue(actual.SequenceEqual(expected));

      //------------------------------//

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 1, 2, 3, 4, 5 };
      item = 5;

      actual = source.TakeSelfAndBefore(item, et);
      Assert.IsTrue(actual.SequenceEqual(expected));

      //------------------------------//

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 1, 2, 3, 4, 5 };
      item = 5;

      actual = source.TakeSelfAndBefore(item);
      Assert.IsTrue(actual.SequenceEqual(expected));
      //------------------------------//

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 3, 4, 5 };
      item = 5;
      count = 2;

      actual = source.TakeSelfAndBefore(item, count, et);
      Assert.IsTrue(actual.SequenceEqual(expected));

      //------------------------------//

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 3, 4, 5 };
      item = 5;
      count = 2;

      actual = source.TakeSelfAndBefore(item, count, et);
      Assert.IsTrue(actual.SequenceEqual(expected));
    }

    #endregion

    #region Take After

    [TestMethod()]
    public void TakeAfterTest()
    {
      IEnumerable<int> source;
      IEnumerable<int> expected;
      IEnumerable<int> actual;
      TestEqualityComparer<int> et;
      int item;
      int count;

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 7, 8, 9, 10 };
      item = 6;

      actual = source.TakeAfter(item);
      Assert.IsTrue(actual.SequenceEqual(expected));

      //------------------------------//

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 7, 8 };
      item = 6;
      count = 2;

      actual = source.TakeAfter(item, count);
      Assert.IsTrue(actual.SequenceEqual(expected));

      //------------------------------//

      et = new TestEqualityComparer<int>();
      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 7, 8, 9, 10 };
      item = 6;

      actual = source.TakeAfter(item, et);
      Assert.IsTrue(actual.SequenceEqual(expected));

      //------------------------------//

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 7, 8, 9, 10 };
      item = 6;

      actual = source.TakeAfter(item);
      Assert.IsTrue(actual.SequenceEqual(expected));

      //------------------------------//

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 7, 8 };
      item = 6;
      count = 2;
      et = new TestEqualityComparer<int>();

      actual = source.TakeAfter(item, count, et);
      Assert.IsTrue(actual.SequenceEqual(expected));

      //------------------------------//

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 7, 8 };
      item = 6;
      count = 2;

      actual = source.TakeAfter(item, count);
      Assert.IsTrue(actual.SequenceEqual(expected));
    }

    [TestMethod()]
    public void TakeSelfAndAfterTest()
    {
      IEnumerable<int> source;
      IEnumerable<int> expected;
      IEnumerable<int> actual;
      TestEqualityComparer<int> et;
      int item;
      int count;

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 6, 7, 8, 9, 10 };
      item = 6;

      actual = source.TakeSelfAndAfter(item);
      Assert.IsTrue(actual.SequenceEqual(expected));

      //------------------------------//

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 6, 7, 8 };
      item = 6;
      count = 2;

      actual = source.TakeSelfAndAfter(item, count);
      Assert.IsTrue(actual.SequenceEqual(expected));

      //------------------------------//

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 6, 7, 8, 9, 10 };
      item = 6;
      et = new TestEqualityComparer<int>();

      actual = source.TakeSelfAndAfter(item, et);
      Assert.IsTrue(actual.SequenceEqual(expected));

      //------------------------------//

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 6, 7, 8, 9, 10 };
      item = 6;

      actual = source.TakeSelfAndAfter(item);
      Assert.IsTrue(actual.SequenceEqual(expected));

      //------------------------------//
      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 5, 6, 7 };
      item = 5;
      count = 2;
      et = new TestEqualityComparer<int>();

      actual = source.TakeSelfAndAfter(item, count, et);
      Assert.IsTrue(actual.SequenceEqual(expected));

      //------------------------------//

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 5, 6, 7 };
      item = 5;
      count = 2;

      actual = source.TakeSelfAndAfter(item, count);
      Assert.IsTrue(actual.SequenceEqual(expected));
    }

    #endregion

    #region Take Around

    [TestMethod()]
    public void TakeAroundTest()
    {
      IEnumerable<int> source;
      IEnumerable<int> expected;
      TestEqualityComparer<int> et;
      int item;
      int count;
      int before;
      int after;

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 4, 5, 6, 7, 8 };
      item = 6;
      count = 2;

      IEnumerable<int> actual;
      actual = source.TakeAround(item, count);
      Assert.IsTrue(actual.SequenceEqual(expected));

      //------------------------------//

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 4, 5, 6, 7 };
      item = 6;
      before = 2;
      after = 1;

      actual = source.TakeAround(item, before, after);
      Assert.IsTrue(actual.SequenceEqual(expected));

      //------------------------------//

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 4, 5, 6, 7, 8 };
      item = 6;
      count = 2;
      et = new TestEqualityComparer<int>();

      actual = source.TakeAround(item, count, et);
      Assert.IsTrue(actual.SequenceEqual(expected));

      //------------------------------//

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 4, 5, 6, 7, 8 };
      item = 6;
      count = 2;

      actual = source.TakeAround(item, count);
      Assert.IsTrue(actual.SequenceEqual(expected));

      //------------------------------//

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 4, 5, 6, 7 };
      item = 6;
      before = 2;
      after = 1;
      et = new TestEqualityComparer<int>();

      actual = source.TakeAround(item, before, after, et);
      Assert.IsTrue(actual.SequenceEqual(expected));

      //------------------------------//

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 4, 5, 6, 7 };
      item = 6;
      before = 2;
      after = 1;

      actual = source.TakeAround(item, before, after);
      Assert.IsTrue(actual.SequenceEqual(expected));
    }

    #endregion

    #region Take Top

    [TestMethod()]
    public void TakeTopTest()
    {
      IEnumerable<int> source;
      IEnumerable<int> expected;
      IEnumerable<int> actual;
      Func<int, bool> predicate;
      int count;
      double percent;

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 1, 2, 3 };
      count = 3;

      actual = source.TakeTop(count);
      Assert.IsTrue(actual.SequenceEqual(expected));

      //------------------------------//

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 2, 4, 6 };
      count = 3;
      predicate = (S) => S % 2 == 0;

      actual = source.TakeTop(count, predicate);
      Assert.IsTrue(actual.SequenceEqual(expected));

      //------------------------------//

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 1, 2, 3 };
      percent = .3;

      actual = source.TakeTopPercent(percent);
      Assert.IsTrue(actual.SequenceEqual(expected));

      //------------------------------//

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 2, 4, 6 };
      percent = .3;
      predicate = (S) => S % 2 == 0;

      actual = source.TakeTopPercent(percent, predicate);
      Assert.IsTrue(actual.SequenceEqual(expected));
    }

    #endregion

    #region Take Bottom

    [TestMethod()]
    public void TakeBottomTest()
    {
      IEnumerable<int> source;
      IEnumerable<int> expected;
      IEnumerable<int> actual;
      int count;

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 8, 9, 10 };
      count = 3;

      actual = source.TakeBottom(count);
      Assert.IsTrue(actual.SequenceEqual(expected));

      //------------------------------//

      Func<int, bool> predicate = (S) => S % 2 == 0;
      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 6, 8, 10 };
      count = 3;

      actual = source.TakeBottom(count, predicate);
      Assert.IsTrue(actual.SequenceEqual(expected));
    }

    [TestMethod()]
    public void TakeBottomPercentTest()
    {
      IEnumerable<int> source;
      IEnumerable<int> expected;
      IEnumerable<int> actual;
      double percent;

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 8, 9, 10 };
      percent = .3;

      actual = source.TakeBottomPercent(percent);
      Assert.IsTrue(actual.SequenceEqual(expected));

      //------------------------------//

      Func<int, bool> predicate = (S) => S % 2 == 0;
      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      expected = new int[] { 6, 8, 10 };
      percent = .3;

      actual = source.TakeBottomPercent(percent, predicate);
      Assert.IsTrue(actual.SequenceEqual(expected));
    }
    #endregion
  }
}
