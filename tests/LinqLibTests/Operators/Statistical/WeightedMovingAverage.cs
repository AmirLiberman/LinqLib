using System;
using System.Collections.Generic;
using System.Linq;
using LinqLib.Operators;
using LinqLib.Sequence;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinqLibTests.Operators
{
  [TestClass()]
  public class WeightedMovingAverageTests
  {
    #region Weighted Moving Average

    #region Double

    [TestMethod()]
    public void WeightedMovingAverageDoubleTest()
    {
      IEnumerable<double> source;
      IEnumerable<double> expected;
      IEnumerable<double> actual;
      int blockSize = 4;

      source = new double[] { 1, 2, 3, 6, 7, 9, 11, double.NaN, 10, 11, 12, 12, 13 };
      expected = new double[] { 4.4, 6, 7.8, 9.7, double.NaN, double.NaN, double.NaN, double.NaN, 11.8, 12.5 };
      blockSize = 4;

      actual = Statistical.WeightedMovingAverage(source, blockSize, DoubleWeightGen);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new double[] { 1, 2, 3, 4, 5 };
        blockSize = 1;
        int x = Statistical.WeightedMovingAverage(source, blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "blockSize"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      try
      {
        source = new double[] { 1, 2, 3, 4, 5 };
        blockSize = 15;
        int x = Statistical.WeightedMovingAverage(source, blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "source"); }
      catch (Exception) { Assert.Fail(); }
    }

    [TestMethod()]
    public void WeightedMovingAverageNullableDoubleTest()
    {
      IEnumerable<double?> source;
      IEnumerable<double?> expected;
      IEnumerable<double?> actual;
      int blockSize;

      source = new double?[] { 1, 2, 3, 6, 7, 9, 11, double.NaN, 10, 11, null, 15, 25 };
      expected = new double?[] { 4.4, 6, 7.8, 9.7, double.NaN, double.NaN, double.NaN, double.NaN, 14, 21 };
      blockSize = 4;

      actual = Statistical.WeightedMovingAverage(source, blockSize, DoubleWeightGen);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new double?[] { 1, 2, 3, 4, 5 };
        blockSize = 1;
        int x = Statistical.WeightedMovingAverage(source, blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "blockSize"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      try
      {
        source = new double?[] { 1, 2, 3, 4, 5 };
        blockSize = 15;
        int x = Statistical.WeightedMovingAverage(source, blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "source"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      source = new double?[] { null, null, null, null, null };
      expected = new double?[] { null, null };
      blockSize = 4;

      actual = Statistical.WeightedMovingAverage(source, blockSize, DoubleWeightGen);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Float

    [TestMethod()]
    public void WeightedMovingAverageFloatTest()
    {
      IEnumerable<float> source;
      IEnumerable<float> expected;
      IEnumerable<float> actual;
      int blockSize;

      source = new float[] { 1, 2, 3, 6, 7, 9, 11, float.NaN, 10, 11, 12, 12, 13 };
      expected = new float[] { 4.4f, 6, 7.8f, 9.7f, float.NaN, float.NaN, float.NaN, float.NaN, 11.8f, 12.5f };
      blockSize = 4;

      actual = Statistical.WeightedMovingAverage(source, blockSize, FloatWeightGen);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new float[] { 1, 2, 3, 4, 5 };
        blockSize = 1;
        int x = Statistical.WeightedMovingAverage(source, blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "blockSize"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      try
      {
        source = new float[] { 1, 2, 3, 4, 5 };
        blockSize = 15;
        int x = Statistical.WeightedMovingAverage(source, blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "source"); }
      catch (Exception) { Assert.Fail(); }
    }

    [TestMethod()]
    public void WeightedMovingAverageNullableFloatTest()
    {
      IEnumerable<float?> source;
      IEnumerable<float?> expected;
      IEnumerable<float?> actual;
      int blockSize;

      source = new float?[] { 1, 2, 3, 6, 7, 9, 11, float.NaN, 10, 11, null, 15, 25 };
      expected = new float?[] { 4.4f, 6, 7.8f, 9.7f, float.NaN, float.NaN, float.NaN, float.NaN, 14, 21 };
      blockSize = 4;
      actual = Statistical.WeightedMovingAverage(source, blockSize, FloatWeightGen);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new float?[] { 1, 2, 3, 4, 5 };
        blockSize = 1;
        int x = Statistical.WeightedMovingAverage(source, blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "blockSize"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      try
      {
        source = new float?[] { 1, 2, 3, 4, 5 };
        blockSize = 15;
        int x = Statistical.WeightedMovingAverage(source, blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "source"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      source = new float?[] { null, null, null, null, null };
      expected = new float?[] { null, null };
      blockSize = 4;

      actual = Statistical.WeightedMovingAverage(source, blockSize, FloatWeightGen);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Decimal

    [TestMethod()]
    public void WeightedMovingAverageDecimalTest()
    {
      IEnumerable<decimal> source;
      IEnumerable<decimal> expected;
      IEnumerable<decimal> actual;
      int blockSize;

      source = new decimal[] { 1, 2, 3, 6, 7, 9, 11, 8, 10, 11, 12, 12, 13 };
      expected = new decimal[] { 4.4m, 6, 7.8m, 9.7m, 9, 9.5m, 10.3m, 11.3m, 11.8m, 12.5m };
      blockSize = 4;

      actual = Statistical.WeightedMovingAverage(source, blockSize, DecimalWeightGen);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new decimal[] { 1, 2, 3, 4, 5 };
        blockSize = 1;
        int x = Statistical.WeightedMovingAverage(source, blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "blockSize"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      try
      {
        source = new decimal[] { 1, 2, 3, 4, 5 };
        blockSize = 15;
        int x = Statistical.WeightedMovingAverage(source, blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "source"); }
      catch (Exception) { Assert.Fail(); }
    }

    [TestMethod()]
    public void WeightedMovingAverageNullableDecimalTest()
    {
      IEnumerable<decimal?> source;
      IEnumerable<decimal?> expected;
      IEnumerable<decimal?> actual;
      int blockSize;

      source = new decimal?[] { 1, 2, 3, 6, 7, 9, 11, 8, 10, 11, null, 15, 25 };
      expected = new decimal?[] { 4.4m, 6, 7.8m, 9.7m, 9, 9.5m, 10.3m, 10.5m, 14, 21 };
      blockSize = 4;

      actual = Statistical.WeightedMovingAverage(source, blockSize, DecimalWeightGen);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new decimal?[] { 1, 2, 3, 4, 5 };
        blockSize = 1;
        int x = Statistical.WeightedMovingAverage(source, blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "blockSize"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      try
      {
        source = new decimal?[] { 1, 2, 3, 4, 5 };
        blockSize = 15;
        int x = Statistical.WeightedMovingAverage(source, blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "source"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      source = new decimal?[] { null, null, null, null, null };
      expected = new decimal?[] { null, null };
      blockSize = 4;

      actual = Statistical.WeightedMovingAverage(source, blockSize, DecimalWeightGen);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Long

    [TestMethod()]
    public void WeightedMovingAverageLongTest()
    {
      IEnumerable<long> source;
      IEnumerable<double> expected;
      IEnumerable<double> actual;
      int blockSize;

      source = new long[] { 1, 2, 3, 6, 7, 9, 11, 8, 10, 11, 12, 12, 13 };
      expected = new double[] { 4.4, 6, 7.8, 9.7, 9, 9.5, 10.3, 11.3, 11.8, 12.5 };
      blockSize = 4;

      actual = Statistical.WeightedMovingAverage(source, blockSize, DoubleWeightGen);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new long[] { 1, 2, 3, 4, 5 };
        blockSize = 1;
        int x = Statistical.WeightedMovingAverage(source, blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "blockSize"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      try
      {
        source = new long[] { 1, 2, 3, 4, 5 };
        blockSize = 15;
        int x = Statistical.WeightedMovingAverage(source, blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "source"); }
      catch (Exception) { Assert.Fail(); }
    }

    [TestMethod()]
    public void WeightedMovingAverageNullableLongTest()
    {
      IEnumerable<long?> source;
      IEnumerable<double?> expected;
      IEnumerable<double?> actual;
      int blockSize;

      source = new long?[] { 1, 2, 3, 6, 7, 9, 11, 8, 10, 11, null, 15, 25 };
      expected = new double?[] { 4.4, 6, 7.8, 9.7, 9, 9.5, 10.3, 10.5, 14, 21 };
      blockSize = 4;

      actual = Statistical.WeightedMovingAverage(source, blockSize, DoubleWeightGen);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new long?[] { 1, 2, 3, 4, 5 };
        blockSize = 1;
        int x = Statistical.WeightedMovingAverage(source, blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "blockSize"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      try
      {
        source = new long?[] { 1, 2, 3, 4, 5 };
        blockSize = 15;
        int x = Statistical.WeightedMovingAverage(source, blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "source"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      source = new long?[] { null, null, null, null, null };
      expected = new double?[] { null, null };
      blockSize = 4;

      actual = Statistical.WeightedMovingAverage(source, blockSize, DoubleWeightGen);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Int

    [TestMethod()]
    public void WeightedMovingAverageIntTest()
    {
      IEnumerable<int> source;
      IEnumerable<double> expected;
      IEnumerable<double> actual;
      int blockSize;

      source = new int[] { 1, 2, 3, 6, 7, 9, 11, 8, 10, 11, 12, 12, 13 };
      expected = new double[] { 4.4, 6, 7.8, 9.7, 9, 9.5, 10.3, 11.3, 11.8, 12.5 };
      blockSize = 4;

      actual = Statistical.WeightedMovingAverage(source, blockSize, DoubleWeightGen);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new int[] { 1, 2, 3, 4, 5 };
        blockSize = 1;
        int x = Statistical.WeightedMovingAverage(source, blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "blockSize"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      try
      {
        source = new int[] { 1, 2, 3, 4, 5 };
        blockSize = 15;
        int x = Statistical.WeightedMovingAverage(source, blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "source"); }
      catch (Exception) { Assert.Fail(); }
    }

    [TestMethod()]
    public void WeightedMovingAverageNullableIntTest()
    {
      IEnumerable<int?> source;
      IEnumerable<double?> expected;
      IEnumerable<double?> actual;
      int blockSize;

      source = new int?[] { 1, 2, 3, 6, 7, 9, 11, 8, 10, 11, null, 15, 25 };
      expected = new double?[] { 4.4, 6, 7.8, 9.7, 9, 9.5, 10.3, 10.5, 14, 21 };
      blockSize = 4;

      actual = Statistical.WeightedMovingAverage(source, blockSize, DoubleWeightGen);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new int?[] { 1, 2, 3, 4, 5 };
        blockSize = 1;
        int x = Statistical.WeightedMovingAverage(source, blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "blockSize"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      try
      {
        source = new int?[] { 1, 2, 3, 4, 5 };
        blockSize = 15;
        int x = Statistical.WeightedMovingAverage(source, blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "source"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      source = new int?[] { null, null, null, null, null };
      expected = new double?[] { null, null };
      blockSize = 4;

      actual = Statistical.WeightedMovingAverage(source, blockSize, DoubleWeightGen);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #endregion

    #region Weighted Moving Average (Block Parameter Overload)

    #region Double

    [TestMethod()]
    public void WeightedMovingAverageDoubleTest2()
    {
      IEnumerable<Tuple<string, double>> source = Enumerator.Generate<Tuple<string, double>>(7, X => Generator.GenerateDouble(X, -1));
      IEnumerable<double> expected = new double[] { 2.571, 3.571, 4.571, 5.571, 6.571 };
      IEnumerable<double> actual;
      int blockSize = 3;

      actual = source.WeightedMovingAverage(blockSize, DoubleWeightGen, I => I.Item2).Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void WeightedMovingAverageNullableDoubleTest2()
    {
      IEnumerable<Tuple<string, double?>> source = Enumerator.Generate<Tuple<string, double?>>(7, X => Generator.GenerateNullableDouble(X, -1, -1));
      IEnumerable<double?> expected = new double?[] { 2.571, 3.571, 4.571, 5.571, 6.571 };
      IEnumerable<double?> actual;
      int blockSize = 3;

      actual = source.WeightedMovingAverage(blockSize, DoubleWeightGen, I => I.Item2).Select(S => S.Value).Round(3).Cast<double?>();
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Float

    [TestMethod()]
    public void WeightedMovingAverageFloatTest2()
    {
      IEnumerable<Tuple<string, float>> source = Enumerator.Generate<Tuple<string, float>>(7, X => Generator.GenerateFloat(X, -1));
      IEnumerable<float> expected = new float[] { 2.571f, 3.571f, 4.571f, 5.571f, 6.571f };
      IEnumerable<float> actual;
      int blockSize = 3;

      actual = source.WeightedMovingAverage(blockSize, FloatWeightGen, I => I.Item2).Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void WeightedMovingAverageNullableFloatTest2()
    {
      IEnumerable<Tuple<string, float?>> source = Enumerator.Generate<Tuple<string, float?>>(7, X => Generator.GenerateNullableFloat(X, -1, -1));
      IEnumerable<float?> expected = new float?[] { 2.571f, 3.571f, 4.571f, 5.571f, 6.571f };
      IEnumerable<float?> actual;
      int blockSize = 3;

      actual = source.WeightedMovingAverage(blockSize, FloatWeightGen, I => I.Item2).Select(S => S.Value).Round(3).Cast<float?>();
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Decimal

    [TestMethod()]
    public void WeightedMovingAverageDecimalTest2()
    {
      IEnumerable<Tuple<string, decimal>> source = Enumerator.Generate<Tuple<string, decimal>>(7, X => Generator.GenerateDecimal(X));
      IEnumerable<decimal> expected = new decimal[] { 2.571m, 3.571m, 4.571m, 5.571m, 6.571m };
      IEnumerable<decimal> actual;
      int blockSize = 3;

      actual = source.WeightedMovingAverage(blockSize, DecimalWeightGen, I => I.Item2).Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void WeightedMovingAverageNullableDecimalTest2()
    {
      IEnumerable<Tuple<string, decimal?>> source = Enumerator.Generate<Tuple<string, decimal?>>(7, X => Generator.GenerateNullableDecimal(X, -1));
      IEnumerable<decimal?> expected = new decimal?[] { 2.571m, 3.571m, 4.571m, 5.571m, 6.571m };
      IEnumerable<decimal?> actual;
      int blockSize = 3;

      actual = source.WeightedMovingAverage(blockSize, DecimalWeightGen, I => I.Item2).Select(S => S.Value).Round(3).Cast<decimal?>();
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Long

    [TestMethod()]
    public void WeightedMovingAverageLongTest2()
    {
      IEnumerable<Tuple<string, long>> source = Enumerator.Generate<Tuple<string, long>>(7, X => Generator.GenerateLong(X));
      IEnumerable<double> expected = new double[] { 2.571, 3.571, 4.571, 5.571, 6.571 };
      IEnumerable<double> actual;
      int blockSize = 3;

      actual = source.WeightedMovingAverage(blockSize, DoubleWeightGen, I => I.Item2).Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void WeightedMovingAverageNullableLongTest2()
    {
      IEnumerable<Tuple<string, long?>> source = Enumerator.Generate<Tuple<string, long?>>(7, X => Generator.GenerateNullableLong(X, -1));
      IEnumerable<double?> expected = new double?[] { 2.571, 3.571, 4.571, 5.571, 6.571 };
      IEnumerable<double?> actual;
      int blockSize = 3;

      actual = source.WeightedMovingAverage(blockSize, DoubleWeightGen, I => I.Item2).Select(S => S.Value).Round(3).Cast<double?>();
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Int

    [TestMethod()]
    public void WeightedMovingAverageIntTest2()
    {
      IEnumerable<Tuple<string, int>> source = Enumerator.Generate<Tuple<string, int>>(7, X => Generator.GenerateInt(X));
      IEnumerable<double> expected = new double[] { 2.571, 3.571, 4.571, 5.571, 6.571 };
      IEnumerable<double> actual;
      int blockSize = 3;

      actual = source.WeightedMovingAverage(blockSize, DoubleWeightGen, I => I.Item2).Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void WeightedMovingAverageNullableIntTest2()
    {
      IEnumerable<Tuple<string, int?>> source = Enumerator.Generate<Tuple<string, int?>>(7, X => Generator.GenerateNullableInt(X, -1));
      IEnumerable<double?> expected = new double?[] { 2.571, 3.571, 4.571, 5.571, 6.571 };
      IEnumerable<double?> actual;
      int blockSize = 3;

      actual = source.WeightedMovingAverage(blockSize, DoubleWeightGen, I => I.Item2).Select(S => S.Value).Round(3).Cast<double?>();
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #endregion

    #region Helpers

    private double DoubleWeightGen(int index)
    {
      return index * index;
    }

    private float FloatWeightGen(int index)
    {
      return index * index;
    }

    private decimal DecimalWeightGen(int index)
    {
      return index * index;
    }

    #endregion
  }
}