using System;
using System.Collections.Generic;
using System.Linq;
using LinqLib.Sequence;
using LinqLib.Sort;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinqLibTests.Sort
{
  [TestClass()]
  public class SortTests
  {

    private DataItem[] GetTestData(int elements)
    {
      return Enumerator.Generate(elements, X => new DataItem(X)).ToArray();
    }

    [TestMethod()]
    public void SortTest1()
    {
      for (int i = 0; i < 10; i++)
      {
        DataItem[] data = GetTestData(1200);
        IEnumerable<DataItem> actual;

        IEnumerable<DataItem> excpected = data.OrderBy(X => X.P1).ThenBy(X => X.P2).ThenBy(X => X.P3).
                                               ThenBy(X => X.P4).ThenBy(X => X.P5).ThenBy(X => X.Index);

        actual = data.OrderBy(X => X.P1, SortType.Bubble).ThenBy(X => X.P2, SortType.Bubble).ThenBy(X => X.P3, SortType.Bubble).
                      ThenBy(X => X.P4, SortType.Bubble).ThenBy(X => X.P5, SortType.Bubble).ThenBy(X => X.Index, SortType.Bubble);
        Assert.IsTrue(excpected.SequenceRelation(actual) == SequenceRelationType.Equal);

        actual = data.OrderBy(X => X.P1, SortType.Heap).ThenBy(X => X.P2, SortType.Heap).ThenBy(X => X.P3, SortType.Heap).
                      ThenBy(X => X.P4, SortType.Heap).ThenBy(X => X.P5, SortType.Heap).ThenBy(X => X.Index, SortType.Heap);
        Assert.IsTrue(excpected.SequenceRelation(actual) == SequenceRelationType.Equal);

        actual = data.OrderBy(X => X.P1, SortType.Insert).ThenBy(X => X.P2, SortType.Insert).ThenBy(X => X.P3, SortType.Insert).
                      ThenBy(X => X.P4, SortType.Insert).ThenBy(X => X.P5, SortType.Insert).ThenBy(X => X.Index, SortType.Insert);
        Assert.IsTrue(excpected.SequenceRelation(actual) == SequenceRelationType.Equal);

        actual = data.OrderBy(X => X.P1, SortType.Merge).ThenBy(X => X.P2, SortType.Merge).ThenBy(X => X.P3, SortType.Merge).
                      ThenBy(X => X.P4, SortType.Merge).ThenBy(X => X.P5, SortType.Merge).ThenBy(X => X.Index, SortType.Merge);
        Assert.IsTrue(excpected.SequenceRelation(actual) == SequenceRelationType.Equal);

        actual = data.OrderBy(X => X.P1, SortType.Quick).ThenBy(X => X.P2, SortType.Quick).ThenBy(X => X.P3, SortType.Quick).
                      ThenBy(X => X.P4, SortType.Quick).ThenBy(X => X.P5, SortType.Quick).ThenBy(X => X.Index, SortType.Quick);
        Assert.IsTrue(excpected.SequenceRelation(actual) == SequenceRelationType.Equal);

        actual = data.OrderBy(X => X.P1, SortType.Select).ThenBy(X => X.P2, SortType.Select).ThenBy(X => X.P3, SortType.Select).
                      ThenBy(X => X.P4, SortType.Select).ThenBy(X => X.P5, SortType.Select).ThenBy(X => X.Index, SortType.Select);
        Assert.IsTrue(excpected.SequenceRelation(actual) == SequenceRelationType.Equal);

        actual = data.OrderBy(X => X.P1, SortType.Shell).ThenBy(X => X.P2, SortType.Shell).ThenBy(X => X.P3, SortType.Shell).
                      ThenBy(X => X.P4, SortType.Shell).ThenBy(X => X.P5, SortType.Shell).ThenBy(X => X.Index, SortType.Shell);
        Assert.IsTrue(excpected.SequenceRelation(actual) == SequenceRelationType.Equal);
      }
    }

    [TestMethod()]
    public void SortTest2()
    {
      for (int i = 0; i < 10; i++)
      {
        DataItem[] data = GetTestData(1200);
        IEnumerable<DataItem> actual;

        IEnumerable<DataItem> excpected = data.OrderByDescending(X => X.P1).ThenByDescending(X => X.P2).ThenByDescending(X => X.P3).
                                               ThenByDescending(X => X.P4).ThenByDescending(X => X.P5).ThenByDescending(X => X.Index);

        actual = data.OrderByDescending(X => X.P1, SortType.Bubble).ThenByDescending(X => X.P2, SortType.Bubble).ThenByDescending(X => X.P3, SortType.Bubble).
                      ThenByDescending(X => X.P4, SortType.Bubble).ThenByDescending(X => X.P5, SortType.Bubble).ThenByDescending(X => X.Index, SortType.Bubble);
        Assert.IsTrue(excpected.SequenceRelation(actual) == SequenceRelationType.Equal);

        actual = data.OrderByDescending(X => X.P1, SortType.Heap).ThenByDescending(X => X.P2, SortType.Heap).ThenByDescending(X => X.P3, SortType.Heap).
                      ThenByDescending(X => X.P4, SortType.Heap).ThenByDescending(X => X.P5, SortType.Heap).ThenByDescending(X => X.Index, SortType.Heap);
        Assert.IsTrue(excpected.SequenceRelation(actual) == SequenceRelationType.Equal);

        actual = data.OrderByDescending(X => X.P1, SortType.Insert).ThenByDescending(X => X.P2, SortType.Insert).ThenByDescending(X => X.P3, SortType.Insert).
                      ThenByDescending(X => X.P4, SortType.Insert).ThenByDescending(X => X.P5, SortType.Insert).ThenByDescending(X => X.Index, SortType.Insert);
        Assert.IsTrue(excpected.SequenceRelation(actual) == SequenceRelationType.Equal);

        actual = data.OrderByDescending(X => X.P1, SortType.Merge).ThenByDescending(X => X.P2, SortType.Merge).ThenByDescending(X => X.P3, SortType.Merge).
                      ThenByDescending(X => X.P4, SortType.Merge).ThenByDescending(X => X.P5, SortType.Merge).ThenByDescending(X => X.Index, SortType.Merge);
        Assert.IsTrue(excpected.SequenceRelation(actual) == SequenceRelationType.Equal);

        actual = data.OrderByDescending(X => X.P1, SortType.Quick).ThenByDescending(X => X.P2, SortType.Quick).ThenByDescending(X => X.P3, SortType.Quick).
                      ThenByDescending(X => X.P4, SortType.Quick).ThenByDescending(X => X.P5, SortType.Quick).ThenByDescending(X => X.Index, SortType.Quick);
        Assert.IsTrue(excpected.SequenceRelation(actual) == SequenceRelationType.Equal);

        actual = data.OrderByDescending(X => X.P1, SortType.Select).ThenByDescending(X => X.P2, SortType.Select).ThenByDescending(X => X.P3, SortType.Select).
                      ThenByDescending(X => X.P4, SortType.Select).ThenByDescending(X => X.P5, SortType.Select).ThenByDescending(X => X.Index, SortType.Select);
        Assert.IsTrue(excpected.SequenceRelation(actual) == SequenceRelationType.Equal);

        actual = data.OrderByDescending(X => X.P1, SortType.Shell).ThenByDescending(X => X.P2, SortType.Shell).ThenByDescending(X => X.P3, SortType.Shell).
                      ThenByDescending(X => X.P4, SortType.Shell).ThenByDescending(X => X.P5, SortType.Shell).ThenByDescending(X => X.Index, SortType.Shell);
        Assert.IsTrue(excpected.SequenceRelation(actual) == SequenceRelationType.Equal);
      }
    }

    [TestMethod()]
    public void SortTest3()
    {
      for (int i = 0; i < 10; i++)
      {
        DataItem[] data = GetTestData(1200);
        IEnumerable<DataItem> actual;

        IEnumerable<DataItem> excpected = data.OrderBy(X => X.P1).ThenBy(X => X.P2).ThenByDescending(X => X.P3).
                                               ThenBy(X => X.P4).ThenBy(X => X.P5).ThenByDescending(X => X.Index);

        actual = data.OrderBy(X => X.P1, SortType.Bubble).ThenBy(X => X.P2, SortType.Bubble).ThenByDescending(X => X.P3, SortType.Bubble).
                      ThenBy(X => X.P4, SortType.Bubble).ThenBy(X => X.P5, SortType.Bubble).ThenByDescending(X => X.Index, SortType.Bubble);
        Assert.IsTrue(excpected.SequenceRelation(actual) == SequenceRelationType.Equal);

        actual = data.OrderBy(X => X.P1, SortType.Heap).ThenBy(X => X.P2, SortType.Heap).ThenByDescending(X => X.P3, SortType.Heap).
                      ThenBy(X => X.P4, SortType.Heap).ThenBy(X => X.P5, SortType.Heap).ThenByDescending(X => X.Index, SortType.Heap);
        Assert.IsTrue(excpected.SequenceRelation(actual) == SequenceRelationType.Equal);

        actual = data.OrderBy(X => X.P1, SortType.Insert).ThenBy(X => X.P2, SortType.Insert).ThenByDescending(X => X.P3, SortType.Insert).
                      ThenBy(X => X.P4, SortType.Insert).ThenBy(X => X.P5, SortType.Insert).ThenByDescending(X => X.Index, SortType.Insert);
        Assert.IsTrue(excpected.SequenceRelation(actual) == SequenceRelationType.Equal);

        actual = data.OrderBy(X => X.P1, SortType.Merge).ThenBy(X => X.P2, SortType.Merge).ThenByDescending(X => X.P3, SortType.Merge).
                      ThenBy(X => X.P4, SortType.Merge).ThenBy(X => X.P5, SortType.Merge).ThenByDescending(X => X.Index, SortType.Merge);
        Assert.IsTrue(excpected.SequenceRelation(actual) == SequenceRelationType.Equal);

        actual = data.OrderBy(X => X.P1, SortType.Quick).ThenBy(X => X.P2, SortType.Quick).ThenByDescending(X => X.P3, SortType.Quick).
                      ThenBy(X => X.P4, SortType.Quick).ThenBy(X => X.P5, SortType.Quick).ThenByDescending(X => X.Index, SortType.Quick);
        Assert.IsTrue(excpected.SequenceRelation(actual) == SequenceRelationType.Equal);

        actual = data.OrderBy(X => X.P1, SortType.Select).ThenBy(X => X.P2, SortType.Select).ThenByDescending(X => X.P3, SortType.Select).
                      ThenBy(X => X.P4, SortType.Select).ThenBy(X => X.P5, SortType.Select).ThenByDescending(X => X.Index, SortType.Select);
        Assert.IsTrue(excpected.SequenceRelation(actual) == SequenceRelationType.Equal);

        actual = data.OrderBy(X => X.P1, SortType.Shell).ThenBy(X => X.P2, SortType.Shell).ThenByDescending(X => X.P3, SortType.Shell).
                      ThenBy(X => X.P4, SortType.Shell).ThenBy(X => X.P5, SortType.Shell).ThenByDescending(X => X.Index, SortType.Shell);
        Assert.IsTrue(excpected.SequenceRelation(actual) == SequenceRelationType.Equal);

      }
    }

    [TestMethod()]
    public void SortTest4()
    {
      DateTime runTill = DateTime.Now.AddSeconds(15);
      do
      {
        DataItem[] data = GetTestData(200000);
        IEnumerable<DataItem> actual;

        IEnumerable<DataItem> excpected = data.OrderBy(X => X.P1).ThenBy(X => X.P2).ThenByDescending(X => X.P3).
                                               ThenBy(X => X.P4).ThenBy(X => X.P5).ThenByDescending(X => X.Index);

        actual = data.OrderBy(X => X.P1, SortType.Heap).ThenBy(X => X.P2, SortType.Heap).ThenByDescending(X => X.P3, SortType.Heap).
                      ThenBy(X => X.P4, SortType.Heap).ThenBy(X => X.P5, SortType.Heap).ThenByDescending(X => X.Index, SortType.Heap);
        Assert.IsTrue(excpected.SequenceRelation(actual) == SequenceRelationType.Equal);

        actual = data.OrderBy(X => X.P1, SortType.Merge).ThenBy(X => X.P2, SortType.Merge).ThenByDescending(X => X.P3, SortType.Merge).
                      ThenBy(X => X.P4, SortType.Merge).ThenBy(X => X.P5, SortType.Merge).ThenByDescending(X => X.Index, SortType.Merge);
        Assert.IsTrue(excpected.SequenceRelation(actual) == SequenceRelationType.Equal);

        actual = data.OrderBy(X => X.P1, SortType.Quick).ThenBy(X => X.P2, SortType.Quick).ThenByDescending(X => X.P3, SortType.Quick).
                      ThenBy(X => X.P4, SortType.Quick).ThenBy(X => X.P5, SortType.Quick).ThenByDescending(X => X.Index, SortType.Quick);
        Assert.IsTrue(excpected.SequenceRelation(actual) == SequenceRelationType.Equal);
      } while (runTill > DateTime.Now);
    }
  }
}
