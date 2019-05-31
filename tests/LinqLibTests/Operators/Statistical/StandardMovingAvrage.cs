using System;
using System.Collections.Generic;
using System.Linq;
using LinqLib.Operators;
using LinqLib.Sequence;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinqLibTests.Operators
{
  [TestClass()]
  public class StandardMovingAverageTests
  {
    #region Standard Moving Average

    #region Double

    [TestMethod()]
    public void StandardMovingAverageDoubleTest()
    {
      IEnumerable<double> source;
      IEnumerable<double> expected;
      IEnumerable<double> actual;
      int blockSize;

      source = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, double.NaN, 11, 12, 13 };
      expected = new double[] { 2, 3, 4, 5, 6, 7, 8, double.NaN, double.NaN, double.NaN, 12 };
      blockSize = 3;

      actual = Statistical.StandardMovingAverage(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new double[] { 1, 2, 3, 4, 5 };
      expected = new double[] { 3 };
      blockSize = 8;

      actual = Statistical.StandardMovingAverage(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new double[] { 1, double.NaN, 3, 4, 5 };
      expected = new double[] { double.NaN };
      blockSize = 8;

      actual = Statistical.StandardMovingAverage(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new double[] { 1, 2, 3, 4, 5 };
        blockSize = 1;

        int x = Statistical.StandardMovingAverage(source, blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "blockSize"); }
      catch (Exception) { Assert.Fail(); }

    }

    [TestMethod()]
    public void StandardMovingAverageNullableDoubleTest()
    {
      IEnumerable<double?> source;
      IEnumerable<double?> expected;
      IEnumerable<double?> actual;
      int blockSize;

      source = new double?[] { 1, null, 3, 4, 5, 6, 7, 8, 9, double.NaN, 11, 12, 13 };
      expected = new double?[] { 2, 3.5, 4, 5, 6, 7, 8, double.NaN, double.NaN, double.NaN, 12 };
      blockSize = 3;

      actual = Statistical.StandardMovingAverage(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new double?[] { 1, 2, null, 6 };
      expected = new double?[] { 3 };
      blockSize = 8;

      actual = Statistical.StandardMovingAverage(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new double?[] { 1, double.NaN, null, 6 };
      expected = new double?[] { double.NaN };
      blockSize = 8;
      actual = Statistical.StandardMovingAverage(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new double?[] { 1, 2, 3, 4, 5 };
        blockSize = 1;
        int x = Statistical.StandardMovingAverage(source, blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "blockSize"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      source = new double?[] { null, null, null, null };
      expected = new double?[] { null };
      blockSize = 8;

      actual = Statistical.StandardMovingAverage(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new double?[] { 1, null, null, null };
      expected = new double?[] { 1, null };
      blockSize = 3;

      actual = Statistical.StandardMovingAverage(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Float

    [TestMethod()]
    public void StandardMovingAverageFloatTest()
    {
      IEnumerable<float> source;
      IEnumerable<float> expected;
      IEnumerable<float> actual;
      int blockSize;

      source = new float[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, float.NaN, 11, 12, 13 };
      expected = new float[] { 2, 3, 4, 5, 6, 7, 8, float.NaN, float.NaN, float.NaN, 12 };
      blockSize = 3;

      actual = Statistical.StandardMovingAverage(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new float[] { 1, 2, 3, 4, 5 };
      expected = new float[] { 3 };
      blockSize = 8;

      actual = Statistical.StandardMovingAverage(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new float[] { 1, float.NaN, 3, 4, 5 };
      expected = new float[] { float.NaN };
      blockSize = 8;

      actual = Statistical.StandardMovingAverage(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new float[] { 1, 2, 3, 4, 5 };
        blockSize = 1;
        int x = Statistical.StandardMovingAverage(source, blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "blockSize"); }
      catch (Exception) { Assert.Fail(); }
    }

    [TestMethod()]
    public void StandardMovingAverageNullableFloatTest()
    {
      IEnumerable<float?> source;
      IEnumerable<float?> expected;
      IEnumerable<float?> actual;
      int blockSize;

      source = new float?[] { 1, null, 3, 4, 5, 6, 7, 8, 9, float.NaN, 11, 12, 13 };
      expected = new float?[] { 2, 3.5f, 4, 5, 6, 7, 8, float.NaN, float.NaN, float.NaN, 12 };
      blockSize = 3;

      actual = Statistical.StandardMovingAverage(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new float?[] { 1, 2, null, 6 };
      expected = new float?[] { 3 };
      blockSize = 8;

      actual = Statistical.StandardMovingAverage(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new float?[] { 1, float.NaN, null, 6 };
      expected = new float?[] { float.NaN };
      blockSize = 8;

      actual = Statistical.StandardMovingAverage(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new float?[] { 1, 2, 3, 4, 5 };
        blockSize = 1;
        int x = Statistical.StandardMovingAverage(source, blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "blockSize"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      source = new float?[] { null, null, null, null };
      expected = new float?[] { null };
      blockSize = 8;

      actual = Statistical.StandardMovingAverage(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new float?[] { 1, null, null, null };
      expected = new float?[] { 1, null };
      blockSize = 3;

      actual = Statistical.StandardMovingAverage(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Decimal

    [TestMethod()]
    public void StandardMovingAverageDecimalTest()
    {
      IEnumerable<decimal> source = new decimal[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
      int blockSize = 3;
      IEnumerable<decimal> expected = new decimal[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
      IEnumerable<decimal> actual;
      actual = Statistical.StandardMovingAverage(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new decimal[] { 1, 2, 3, 4, 5 };
      expected = new decimal[] { 3 };
      blockSize = 8;

      actual = Statistical.StandardMovingAverage(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new decimal[] { 1, 2, 3, 4, 5 };
        blockSize = 1;
        int x = Statistical.StandardMovingAverage(source, blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "blockSize"); }
      catch (Exception) { Assert.Fail(); }
    }

    [TestMethod()]
    public void StandardMovingAverageNullableDecimalTest()
    {
      IEnumerable<decimal?> source;
      IEnumerable<decimal?> expected;
      IEnumerable<decimal?> actual;
      int blockSize;

      source = new decimal?[] { 1, null, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
      expected = new decimal?[] { 2, 3.5m, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
      blockSize = 3;

      actual = Statistical.StandardMovingAverage(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new decimal?[] { 1, 2, null, 6 };
      expected = new decimal?[] { 3 };
      blockSize = 8;

      actual = Statistical.StandardMovingAverage(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new decimal?[] { 1, 2, 3, 4, 5 };
        blockSize = 1;
        int x = Statistical.StandardMovingAverage(source, blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "blockSize"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      source = new decimal?[] { null, null, null, null };
      expected = new decimal?[] { null };
      blockSize = 8;

      actual = Statistical.StandardMovingAverage(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new decimal?[] { 1, null, null, null };
      expected = new decimal?[] { 1, null, null };
      blockSize = 2;

      actual = Statistical.StandardMovingAverage(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Long

    [TestMethod()]
    public void StandardMovingAverageLongTest()
    {
      IEnumerable<long> source;
      IEnumerable<double> expected;
      IEnumerable<double> actual;
      int blockSize;

      source = new long[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
      expected = new double[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
      blockSize = 3;

      actual = Statistical.StandardMovingAverage(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new long[] { 1, 2, 3, 4, 5 };
      expected = new double[] { 3 };
      blockSize = 8;

      actual = Statistical.StandardMovingAverage(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new long[] { 1, 2, 3, 4, 5 };
        blockSize = 1;
        int x = Statistical.StandardMovingAverage(source, blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "blockSize"); }
      catch (Exception) { Assert.Fail(); }
    }

    [TestMethod()]
    public void StandardMovingAverageNullableLongTest()
    {
      IEnumerable<long?> source;
      IEnumerable<double?> expected;
      IEnumerable<double?> actual;
      int blockSize;

      source = new long?[] { 1, null, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
      expected = new double?[] { 2, 3.5, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
      blockSize = 3;

      actual = Statistical.StandardMovingAverage(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new long?[] { 1, 2, null, 9 };
      expected = new double?[] { 4 };
      blockSize = 8;

      actual = Statistical.StandardMovingAverage(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new long?[] { 1, 2, 3, 4, 5 };
        blockSize = 1;
        int x = Statistical.StandardMovingAverage(source, blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "blockSize"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      source = new long?[] { null, null, null, null };
      expected = new double?[] { null };
      blockSize = 8;

      actual = Statistical.StandardMovingAverage(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new long?[] { 1, null, null, null };
      expected = new double?[] { 1, null, null };
      blockSize = 2;

      actual = Statistical.StandardMovingAverage(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Int

    [TestMethod()]
    public void StandardMovingAverageIntTest()
    {
      IEnumerable<int> source;
      IEnumerable<double> expected;
      IEnumerable<double> actual;
      int blockSize;

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
      expected = new double[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
      blockSize = 3;

      actual = Statistical.StandardMovingAverage(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new int[] { 1, 2, 3, 4, 5 };
      expected = new double[] { 3 };
      blockSize = 8;

      actual = Statistical.StandardMovingAverage(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new int[] { 1, 2, 3, 4, 5 };
        blockSize = 1;
        int x = Statistical.StandardMovingAverage(source, blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "blockSize"); }
      catch (Exception) { Assert.Fail(); }
    }

    [TestMethod()]
    public void StandardMovingAverageNullableIntTest()
    {
      IEnumerable<int?> source;
      IEnumerable<double?> expected;
      IEnumerable<double?> actual;
      int blockSize;

      source = new int?[] { 1, null, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
      expected = new double?[] { 2, 3.5, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
      blockSize = 3;

      actual = Statistical.StandardMovingAverage(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new int?[] { 1, 2, null, 6 };
      expected = new double?[] { 3 };
      blockSize = 8;

      actual = Statistical.StandardMovingAverage(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new int?[] { 1, 2, 3, 4, 5 };
        blockSize = 1;
        int x = Statistical.StandardMovingAverage(source, blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "blockSize"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      source = new int?[] { null, null, null, null };
      expected = new double?[] { null };
      blockSize = 8;

      actual = Statistical.StandardMovingAverage(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new int?[] { 1, null, null, null };
      expected = new double?[] { 1, null, null };
      blockSize = 2;

      actual = Statistical.StandardMovingAverage(source, blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #endregion

    #region Standard Moving Average (Selector Parameter Overload)

    #region Double

    [TestMethod()]
    public void StandardMovingAverageDoubleTest1()
    {
      IEnumerable<Tuple<string, double>> source = Enumerator.Generate<Tuple<string, double>>(13, X => Generator.GenerateDouble(X, 10));
      int blockSize = 3;
      IEnumerable<double> expected = new double[] { 2, 3, 4, 5, 6, 7, 8, double.NaN, double.NaN, double.NaN, 12 };
      IEnumerable<double> actual;
      actual = Statistical.StandardMovingAverage(source, blockSize, S => S.Item2);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void StandardMovingAverageNullableDoubleTest1()
    {
      IEnumerable<Tuple<string, double?>> source = Enumerator.Generate<Tuple<string, double?>>(13, X => Generator.GenerateNullableDouble(X, 10, 2));
      int blockSize = 3;
      IEnumerable<double?> expected = new double?[] { 2, 3.5, 4, 5, 6, 7, 8, double.NaN, double.NaN, double.NaN, 12 };
      IEnumerable<double?> actual;
      actual = Statistical.StandardMovingAverage(source, blockSize, S => S.Item2);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Float

    [TestMethod()]
    public void StandardMovingAverageFloatTest1()
    {
      IEnumerable<Tuple<string, float>> source = Enumerator.Generate<Tuple<string, float>>(13, X => Generator.GenerateFloat(X, 10));
      int blockSize = 3;
      IEnumerable<float> expected = new float[] { 2, 3, 4, 5, 6, 7, 8, float.NaN, float.NaN, float.NaN, 12 };
      IEnumerable<float> actual;
      actual = Statistical.StandardMovingAverage(source, blockSize, S => S.Item2);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void StandardMovingAverageNullableFloatTest1()
    {
      IEnumerable<Tuple<string, float?>> source = Enumerator.Generate<Tuple<string, float?>>(13, X => Generator.GenerateNullableFloat(X, 10, 2));
      int blockSize = 3;
      IEnumerable<float?> expected = new float?[] { 2, 3.5f, 4, 5, 6, 7, 8, float.NaN, float.NaN, float.NaN, 12 };
      IEnumerable<float?> actual;
      actual = Statistical.StandardMovingAverage(source, blockSize, S => S.Item2);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Decimal

    [TestMethod()]
    public void StandardMovingAverageDecimalTest1()
    {
      IEnumerable<Tuple<string, decimal>> source = Enumerator.Generate<Tuple<string, decimal>>(13, X => Generator.GenerateDecimal(X));
      int blockSize = 3;
      IEnumerable<decimal> expected = new decimal[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
      IEnumerable<decimal> actual;
      actual = Statistical.StandardMovingAverage(source, blockSize, S => S.Item2);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void StandardMovingAverageNullableDecimalTest1()
    {
      IEnumerable<Tuple<string, decimal?>> source = Enumerator.Generate<Tuple<string, decimal?>>(13, X => Generator.GenerateNullableDecimal(X, 2));
      int blockSize = 3;
      IEnumerable<decimal?> expected = new decimal?[] { 2, 3.5m, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
      IEnumerable<decimal?> actual;
      actual = Statistical.StandardMovingAverage(source, blockSize, S => S.Item2);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Long

    [TestMethod()]
    public void StandardMovingAverageLongTest1()
    {
      IEnumerable<Tuple<string, long>> source = Enumerator.Generate<Tuple<string, long>>(13, X => Generator.GenerateLong(X));
      int blockSize = 3;
      IEnumerable<double> expected = new double[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
      IEnumerable<double> actual;
      actual = Statistical.StandardMovingAverage(source, blockSize, S => S.Item2);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void StandardMovingAverageNullableLongTest1()
    {
      IEnumerable<Tuple<string, long?>> source = Enumerator.Generate<Tuple<string, long?>>(13, X => Generator.GenerateNullableLong(X, 2));
      int blockSize = 3;
      IEnumerable<double?> expected = new double?[] { 2, 3.5, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
      IEnumerable<double?> actual;
      actual = Statistical.StandardMovingAverage(source, blockSize, S => S.Item2);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Int

    [TestMethod()]
    public void StandardMovingAverageIntTest1()
    {
      IEnumerable<Tuple<string, int>> source = Enumerator.Generate<Tuple<string, int>>(13, X => Generator.GenerateInt(X));
      int blockSize = 3;
      IEnumerable<double> expected = new double[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
      IEnumerable<double> actual;
      actual = Statistical.StandardMovingAverage(source, blockSize, S => S.Item2);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void StandardMovingAverageNullableIntTest1()
    {
      IEnumerable<Tuple<string, int?>> source = Enumerator.Generate<Tuple<string, int?>>(13, X => Generator.GenerateNullableInt(X, 2));
      int blockSize = 3;
      IEnumerable<double?> expected = new double?[] { 2, 3.5, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
      IEnumerable<double?> actual;
      actual = Statistical.StandardMovingAverage(source, blockSize, S => S.Item2);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #endregion
  }
}

