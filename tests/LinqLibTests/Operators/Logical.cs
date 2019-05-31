using System.Collections.Generic;
using System.Linq;
using LinqLib.Operators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinqLibTests.Operators
{
  [TestClass()]
  public class LogicalTests
  { 
    [TestMethod()]
    public void NotTest()
    {
      IEnumerable<bool> input;
      IEnumerable<bool> expected;
      IEnumerable<bool> actual;

      input = new bool[] { true, false };
      expected = new bool[] { false, true };

      actual = Gates.Not(input);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void AndTest()
    {
      IEnumerable<bool> inputA;
      IEnumerable<bool> inputB;
      IEnumerable<bool> expected;
      IEnumerable<bool> actual;

      inputA = new bool[] { false, true, false, true };
      inputB = new bool[] { false, false, true, true };
      expected = new bool[] { false, false, false, true };

      actual = Gates.And(inputA, inputB);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void OrTest()
    {
      IEnumerable<bool> inputA;
      IEnumerable<bool> inputB;
      IEnumerable<bool> expected;
      IEnumerable<bool> actual;

      inputA = new bool[] { false, true, false, true };
      inputB = new bool[] { false, false, true, true };
      expected = new bool[] { false, true, true, true };

      actual = Gates.Or(inputA, inputB);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void XorTest()
    {
      IEnumerable<bool> inputA;
      IEnumerable<bool> inputB;
      IEnumerable<bool> expected;
      IEnumerable<bool> actual;

      inputA = new bool[] { false, true, false, true };
      inputB = new bool[] { false, false, true, true };
      expected = new bool[] { false, true, true, false };

      actual = Gates.Xor(inputA, inputB);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }
  }
}

