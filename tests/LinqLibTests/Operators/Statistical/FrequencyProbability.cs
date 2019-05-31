using System;
using System.Collections.Generic;
using System.Linq;
using LinqLib.Operators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinqLibTests.Operators
{
  [TestClass()]
  public class FrequencyProbabilityTests
  {
    #region Frequency

    [TestMethod()]
    public void FrequencyTest1()
    {
      IEnumerable<int> input = new int[] { 1, 2, 3, 4, 3, 2, 3, 3, 3, 3, 2, 2, 2, 3, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 3, 2, 2, 3, 3, 2, 2, 2, 2, 2, 2, 3, 3, 1, 1, 2, 2, 1, 2, 1, 1, 2, 1, 2 };
      IEnumerable<KeyValuePair<int, int>> expected = new KeyValuePair<int, int>[] { new KeyValuePair<int, int>(1, 13), 
                                                                                    new KeyValuePair<int, int>(2, 23), 
                                                                                    new KeyValuePair<int, int>(3, 12), 
                                                                                    new KeyValuePair<int, int>(4, 1) };
      IEnumerable<KeyValuePair<int, int>> actual;
      actual = input.Frequency();
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void FrequencyTest2()
    {
      IEnumerable<int> input = new int[] { 1, 2, 3, 4, 3, 2, 3, 3, 3, 3, 2, 2, 2, 3, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 3, 2, 2, 3, 3, 2, 2, 2, 2, 2, 2, 3, 3, 1, 1, 2, 2, 1, 2, 1, 1, 2, 1, 2 };
      Func<int, string> bucketSelector = delegate(int X) { if (X <= 2)  return "2 and under"; else  return "Over 2"; };
      IEnumerable<KeyValuePair<string, int>> expected = new KeyValuePair<string, int>[] { new KeyValuePair<string, int>("2 and under", 36), new KeyValuePair<string, int>("Over 2", 13) };
      IEnumerable<KeyValuePair<string, int>> actual;
      actual = input.Frequency(bucketSelector);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void FrequencyTest3()
    {
      IEnumerable<int> input = new int[] { 1, 2, 3, 4, 3, 2, 3, 3, 3, 3, 2, 2, 2, 3, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 3, 2, 2, 3, 3, 2, 2, 2, 2, 2, 2, 3, 3, 1, 1, 2, 2, 1, 2, 1, 1, 2, 1, 2 };
      IEnumerable<string> buckets = new string[] { "a", "b", "c", "Z1", "Z2" };
      Func<int, IEnumerable<string>, string> bucketSelector = BucketSelector;
      IEnumerable<KeyValuePair<string, int>> expected = new KeyValuePair<string, int>[] { new KeyValuePair<string, int>("a", 13), 
                                                                                          new KeyValuePair<string, int>("b", 23), 
                                                                                          new KeyValuePair<string, int>("c", 12), 
                                                                                          new KeyValuePair<string, int>("Z1", 1), 
                                                                                          new KeyValuePair<string, int>("Z2", 0)};
      IEnumerable<KeyValuePair<string, int>> actual;
      actual = Statistical.Frequency(input, buckets, bucketSelector);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Probability

    [TestMethod()]
    public void ProbabilityTest1()
    {
      IEnumerable<int> input = new int[] { 1, 3, 3, 3, 3, 3, 3, 2, 2, 2, 3, 1, 2, 1, 2, 1, 2, 1, 2, 3, 2, 2, 3, 3, 2, 2, 2, 2, 2, 2, 3, 3, 1, 1, 2, 2, 1, 2, 1, 1 };
      int item = 3;
      double expected = 0.3;
      double actual;
      actual = input.Probability(item);
      Assert.AreEqual(expected, actual);
    }

    [TestMethod()]
    public void ProbabilityTest2()
    {
      IEnumerable<string> input;
      string item;
      double expected;
      double actual;

      input = new string[] { "a", "b", "c", "d", "a", "b", "c", "d", "a", "b", "c", "d", "a", "b", "c", "d", "a", "b", "c", "d", "a", "b", "c", "d", "a", "b", "c", "d", "a", "b", "c", "d" };
      item = "d";
      expected = 0.25;
      actual = input.Probability(item);
      Assert.AreEqual(expected, actual);

      //------------------------------//

      input = new string[] { "a", "b", "c", "d", "a", "b", "c", "d", "a", "b", "c", "d", "a", "b", "c", "d", "a", "b", "c", "d", "a", "b", "c", "d", "A", "B", "C", "D", "A", "B", "C", "D" };
      item = "d";
      expected = 0.1875;
      actual = input.Probability(item);
      Assert.AreEqual(expected, actual);

      //------------------------------//

      input = new string[] { "a", "b", "c", "d", "a", "b", "c", "d", "a", "b", "c", "d", "a", "b", "c", "d", "a", "b", "c", "d", "a", "b", "c", "d", "A", "B", "C", "D", "A", "B", "C", "D" };
      item = "d";
      expected = 0.25;

      actual = input.Probability(item, StringComparer.InvariantCultureIgnoreCase);
      Assert.AreEqual(expected, actual);
    }

    #endregion

    #region Helpers

    private string BucketSelector(int value, IEnumerable<string> buckets)
    {
      switch (value)
      {
        case 1:
          return "a";
        case 2:
          return "b";
        case 3:
          return "c";
        default:
          return "Z1";
      }
    }

    #endregion
  }
}
