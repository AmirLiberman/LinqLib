using System;
using System.Collections.Generic;
using System.Linq;
using LinqLib.Operators;
using LinqLib.Sequence;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinqLibTests.Operators
{
  [TestClass()]
  public class MovingSumTests
  {
    #region Moving Sum

    #region Double

    [TestMethod()]
    public void MovingSumDoubleTest()
    {
      IEnumerable<double> source;
      IEnumerable<double> expected;
      IEnumerable<double> actual;
      int blockSize;

      source = new double[] { 1, double.NaN, 3, 4, 5, 6, 7, 8, 9, double.NaN, 11, 12, 13 };
      expected = new double[] { double.NaN, double.NaN, 12, 15, 18, 21, 24, double.NaN, double.NaN, double.NaN, 36 };
      blockSize = 3;

      actual = Statistical.MovingSum(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new double[] { 1, 2, 3, 4 };
      expected = new double[] { 10 };
      blockSize = 8;

      actual = Statistical.MovingSum(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new double[] { 1, double.NaN, 3, 4 };
      expected = new double[] { double.NaN, };
      blockSize = 8;

      actual = Statistical.MovingSum(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new double[] { 1, 2, 3, 4 };
        blockSize = 1;
        int x = Statistical.MovingSum(source, blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "blockSize"); }
      catch (Exception) { Assert.Fail(); }
    }

    [TestMethod()]
    public void MovingSumNullableDoubleTest()
    {
      IEnumerable<double?> source;
      IEnumerable<double?> expected;
      IEnumerable<double?> actual;
      int blockSize;

      source = new double?[] { 1, null, 3, 4, 5, 6, 7, 8, 9, double.NaN, 11, 12, 13 };
      expected = new double?[] { 4, 7, 12, 15, 18, 21, 24, double.NaN, double.NaN, double.NaN, 36 };
      blockSize = 3;

      actual = Statistical.MovingSum(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new double?[] { 1, 2, null, 4 };
      expected = new double?[] { 7 };
      blockSize = 8;

      actual = Statistical.MovingSum(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new double?[] { 1, double.NaN, null, 4 };
      expected = new double?[] { double.NaN };
      blockSize = 8;

      actual = Statistical.MovingSum(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new double?[] { 1, 2, null, 4 };
        blockSize = 1;
        int x = Statistical.MovingSum(source, blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "blockSize"); }
      catch (Exception) { Assert.Fail(); }
    }

    #endregion

    #region Float

    [TestMethod()]
    public void MovingSumFloatTest()
    {
      IEnumerable<float> source;
      IEnumerable<float> expected;
      IEnumerable<float> actual;
      int blockSize;

      source = new float[] { 1, float.NaN, 3, 4, 5, 6, 7, 8, 9, float.NaN, 11, 12, 13 };
      expected = new float[] { float.NaN, float.NaN, 12, 15, 18, 21, 24, float.NaN, float.NaN, float.NaN, 36 };
      blockSize = 3;

      actual = Statistical.MovingSum(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new float[] { 1, 2, 3, 4 };
      expected = new float[] { 10 };
      blockSize = 8;

      actual = Statistical.MovingSum(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new float[] { 1, 2, float.NaN, 4 };
      expected = new float[] { float.NaN };
      blockSize = 8;

      actual = Statistical.MovingSum(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new float[] { 1, 2, 3, 4 };
        blockSize = 1;
        int x = Statistical.MovingSum(source, blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "blockSize"); }
      catch (Exception) { Assert.Fail(); }
    }

    [TestMethod()]
    public void MovingSumNullableFloatTest()
    {
      IEnumerable<float?> source;
      IEnumerable<float?> expected;
      IEnumerable<float?> actual;
      int blockSize;

      source = new float?[] { 1, null, 3, 4, 5, 6, 7, 8, 9, float.NaN, 11, 12, 13 };
      expected = new float?[] { 4, 7, 12, 15, 18, 21, 24, float.NaN, float.NaN, float.NaN, 36 };
      blockSize = 3;

      actual = Statistical.MovingSum(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new float?[] { 1, 2, null, 4 };
      expected = new float?[] { 7 };
      blockSize = 8;

      actual = Statistical.MovingSum(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new float?[] { 1, float.NaN, null, 4 };
      blockSize = 8;
      expected = new float?[] { float.NaN };

      actual = Statistical.MovingSum(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new float?[] { 1, 2, null, 4 };
        blockSize = 1;
        int x = Statistical.MovingSum(source, blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "blockSize"); }
      catch (Exception) { Assert.Fail(); }
    }

    #endregion

    #region Decimal

    [TestMethod()]
    public void MovingSumDecimalTest()
    {
      IEnumerable<decimal> source;
      IEnumerable<decimal> expected;
      IEnumerable<decimal> actual;
      int blockSize;

      source = new decimal[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
      expected = new decimal[] { 6, 9, 12, 15, 18, 21, 24, 27, 30, 33, 36 };
      blockSize = 3;

      actual = Statistical.MovingSum(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new decimal[] { 1, 2, 3, 4 };
      expected = new decimal[] { 10 };
      blockSize = 8;

      actual = Statistical.MovingSum(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new decimal[] { 1, 2, 3, 4 };
        blockSize = 1;
        int x = Statistical.MovingSum(source, blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "blockSize"); }
      catch (Exception) { Assert.Fail(); }
    }

    [TestMethod()]
    public void MovingSumNullableDecimalTest()
    {
      IEnumerable<decimal?> source;
      IEnumerable<decimal?> expected;
      IEnumerable<decimal?> actual;
      int blockSize;

      source = new decimal?[] { 1, null, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
      expected = new decimal?[] { 4, 7, 12, 15, 18, 21, 24, 27, 30, 33, 36 };
      blockSize = 3;

      actual = Statistical.MovingSum(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new decimal?[] { 1, 2, null, 4 };
      expected = new decimal?[] { 7 };
      blockSize = 8;

      actual = Statistical.MovingSum(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new decimal?[] { 1, 2, null, 4 };
        blockSize = 1;
        int x = Statistical.MovingSum(source, blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "blockSize"); }
      catch (Exception) { Assert.Fail(); }
    }

    #endregion

    #region Long

    [TestMethod()]
    public void MovingSumLongTest()
    {
      IEnumerable<long> source;
      IEnumerable<long> expected;
      IEnumerable<long> actual;
      int blockSize;

      source = new long[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
      expected = new long[] { 6, 9, 12, 15, 18, 21, 24, 27, 30, 33, 36 };
      blockSize = 3;

      actual = Statistical.MovingSum(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new long[] { 1, 2, 3, 4 };
      expected = new long[] { 10 };
      blockSize = 8;

      actual = Statistical.MovingSum(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new long[] { 1, 2, 3, 4 };
        blockSize = 1;
        int x = Statistical.MovingSum(source, blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "blockSize"); }
      catch (Exception) { Assert.Fail(); }
    }

    [TestMethod()]
    public void MovingSumNullableLongTest()
    {
      IEnumerable<long?> source;
      IEnumerable<long?> expected;
      IEnumerable<long?> actual;
      int blockSize;

      source = new long?[] { 1, null, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
      expected = new long?[] { 4, 7, 12, 15, 18, 21, 24, 27, 30, 33, 36 };
      blockSize = 3;

      actual = Statistical.MovingSum(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new long?[] { 1, 2, null, 4 };
      expected = new long?[] { 7 };
      blockSize = 8;

      actual = Statistical.MovingSum(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//
      
      try
      {
        source = new long?[] { 1, 2, null, 4 };
        blockSize = 1;
        int x = Statistical.MovingSum(source, blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "blockSize"); }
      catch (Exception) { Assert.Fail(); }
    }

    #endregion

    #region Int

    [TestMethod()]
    public void MovingSumIntTest()
    {
      IEnumerable<int> source;
      int blockSize;
      IEnumerable<int> expected;
      IEnumerable<int> actual;

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
      expected = new int[] { 6, 9, 12, 15, 18, 21, 24, 27, 30, 33, 36 };
      blockSize = 3;

      actual = Statistical.MovingSum(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new int[] { 1, 2, 3, 4 };
      expected = new int[] { 10 };
      blockSize = 8;

      actual = Statistical.MovingSum(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new int[] { 1, 2, 3, 4 };
        blockSize = 1;
        int x = Statistical.MovingSum(source, blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "blockSize"); }
      catch (Exception) { Assert.Fail(); }
    }

    [TestMethod()]
    public void MovingSumNullableIntTest()
    {
      IEnumerable<int?> source;
      IEnumerable<int?> expected;
      IEnumerable<int?> actual;
      int blockSize;

      source = new int?[] { 1, null, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
      expected = new int?[] { 4, 7, 12, 15, 18, 21, 24, 27, 30, 33, 36 };
      blockSize = 3;

      actual = Statistical.MovingSum(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new int?[] { 1, 2, null, 4 };
      expected = new int?[] { 7 };
      blockSize = 8;

      actual = Statistical.MovingSum(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new int?[] { 1, 2, null, 4 };
        blockSize = 1;
        int x = Statistical.MovingSum(source, blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "blockSize"); }
      catch (Exception) { Assert.Fail(); }

    }

    #endregion

    #endregion

    #region Moving Sum (Selector Parameter Overload)

    #region Double

    [TestMethod()]
    public void MovingSumDoubleTest2()
    {
      IEnumerable<Tuple<string, double>> source = Enumerator.Generate<Tuple<string, double>>(13, X => Generator.GenerateDouble(X, 10));
      IEnumerable<double> expected = new double[] { 6, 9, 12, 15, 18, 21, 24, double.NaN, double.NaN, double.NaN, 36 };
      IEnumerable<double> actual;
      int blockSize = 3;

      actual = Statistical.MovingSum(source, blockSize, S => S.Item2);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void MovingSumDoubleTest3()
    {
      IEnumerable<Tuple<string, double?>> source = Enumerator.Generate<Tuple<string, double?>>(13, X => Generator.GenerateNullableDouble(X, 10, 2));
      IEnumerable<double?> expected = new double?[] { 4, 7, 12, 15, 18, 21, 24, double.NaN, double.NaN, double.NaN, 36 };
      IEnumerable<double?> actual;
      int blockSize = 3;

      actual = Statistical.MovingSum(source, blockSize, S => S.Item2);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Float

    [TestMethod()]
    public void MovingSumFloatTest2()
    {
      IEnumerable<Tuple<string, float>> source = Enumerator.Generate<Tuple<string, float>>(13, X => Generator.GenerateFloat(X, 10));
      int blockSize = 3;
      IEnumerable<float> expected = new float[] { 6, 9, 12, 15, 18, 21, 24, float.NaN, float.NaN, float.NaN, 36 };
      IEnumerable<float> actual;
      actual = Statistical.MovingSum(source, blockSize, S => S.Item2);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void MovingSumFloatTest3()
    {
      IEnumerable<Tuple<string, float?>> source = Enumerator.Generate<Tuple<string, float?>>(13, X => Generator.GenerateNullableFloat(X, 10, 2));
      int blockSize = 3;
      IEnumerable<float?> expected = new float?[] { 4, 7, 12, 15, 18, 21, 24, float.NaN, float.NaN, float.NaN, 36 };
      IEnumerable<float?> actual;
      actual = Statistical.MovingSum(source, blockSize, S => S.Item2);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Decimal

    [TestMethod()]
    public void MovingSumDecimalTest2()
    {
      IEnumerable<Tuple<string, decimal>> source = Enumerator.Generate<Tuple<string, decimal>>(13, X => Generator.GenerateDecimal(X));
      int blockSize = 3;
      IEnumerable<decimal> expected = new decimal[] { 6, 9, 12, 15, 18, 21, 24, 27, 30, 33, 36 };
      IEnumerable<decimal> actual;
      actual = Statistical.MovingSum(source, blockSize, S => S.Item2);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void MovingSumDecimalTest3()
    {
      IEnumerable<Tuple<string, decimal?>> source = Enumerator.Generate<Tuple<string, decimal?>>(13, X => Generator.GenerateNullableDecimal(X, 2));
      int blockSize = 3;
      IEnumerable<decimal?> expected = new decimal?[] { 4, 7, 12, 15, 18, 21, 24, 27, 30, 33, 36 };
      IEnumerable<decimal?> actual;
      actual = Statistical.MovingSum(source, blockSize, S => S.Item2);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Long

    [TestMethod()]
    public void MovingSumLongTest2()
    {
      IEnumerable<Tuple<string, long>> source = Enumerator.Generate<Tuple<string, long>>(13, X => Generator.GenerateLong(X));
      int blockSize = 3;
      IEnumerable<long> expected = new long[] { 6, 9, 12, 15, 18, 21, 24, 27, 30, 33, 36 };
      IEnumerable<long> actual;
      actual = Statistical.MovingSum(source, blockSize, S => S.Item2);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void MovingSumLongTest3()
    {
      IEnumerable<Tuple<string, long?>> source = Enumerator.Generate<Tuple<string, long?>>(13, X => Generator.GenerateNullableLong(X, 2));
      int blockSize = 3;
      IEnumerable<long?> expected = new long?[] { 4, 7, 12, 15, 18, 21, 24, 27, 30, 33, 36 };
      IEnumerable<long?> actual;
      actual = Statistical.MovingSum(source, blockSize, S => S.Item2);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Int

    [TestMethod()]
    public void MovingSumIntTest2()
    {
      IEnumerable<Tuple<string, int>> source = Enumerator.Generate<Tuple<string, int>>(13, X => Generator.GenerateInt(X));
      int blockSize = 3;
      IEnumerable<int> expected = new int[] { 6, 9, 12, 15, 18, 21, 24, 27, 30, 33, 36 };
      IEnumerable<int> actual;
      actual = Statistical.MovingSum(source, blockSize, S => S.Item2);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void MovingSumIntTest3()
    {
      IEnumerable<Tuple<string, int?>> source = Enumerator.Generate<Tuple<string, int?>>(13, X => Generator.GenerateNullableInt(X, 2));
      int blockSize = 3;
      IEnumerable<int?> expected = new int?[] { 4, 7, 12, 15, 18, 21, 24, 27, 30, 33, 36 };
      IEnumerable<int?> actual;
      actual = Statistical.MovingSum(source, blockSize, S => S.Item2);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #endregion
  }
}
