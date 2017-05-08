using System;
using System.Collections.Generic;
using System.Linq;
using LinqLib.Operators;
using LinqLib.Sequence;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinqLibTests.Operators
{
  [TestClass()]
  public class CumulativeMovingAverageTests
  {
    #region Cumulative Moving Average

    #region  Double

    [TestMethod()]
    public void CumulativeMovingAverageDoubleTest1()
    {
      IEnumerable<double> source = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, double.NaN, 11, 12, 13 };
      IEnumerable<double> expected = new double[] { 1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5, 5, double.NaN, double.NaN, double.NaN, double.NaN };
      IEnumerable<double> actual;
      actual = Statistical.CumulativeMovingAverage(source);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void CumulativeMovingAverageNullableDoubleTest()
    {
      IEnumerable<double?> source = new double?[] { 1, 2, null, null, 3, 4, 5, 6, 7, 8, 9, double.NaN, 11, 12, 13 };
      IEnumerable<double?> expected = new double?[] { 1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5, 5, double.NaN, double.NaN, double.NaN, double.NaN };
      IEnumerable<double?> actual;
      actual = Statistical.CumulativeMovingAverage(source);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Float

    [TestMethod()]
    public void CumulativeMovingAverageFloatTest1()
    {
      IEnumerable<float> source = new float[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, float.NaN, 11, 12, 13 };
      IEnumerable<float> expected = new float[] { 1, 1.5f, 2, 2.5f, 3, 3.5f, 4, 4.5f, 5, float.NaN, float.NaN, float.NaN, float.NaN };
      IEnumerable<float> actual;
      actual = Statistical.CumulativeMovingAverage(source);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void CumulativeMovingAverageNullableFloatTest()
    {
      IEnumerable<float?> source = new float?[] { 1, 2, null, null, 3, 4, 5, 6, 7, 8, 9, float.NaN, 11, 12, 13 };
      IEnumerable<float?> expected = new float?[] { 1, 1.5f, 2, 2.5f, 3, 3.5f, 4, 4.5f, 5, float.NaN, float.NaN, float.NaN, float.NaN };
      IEnumerable<float?> actual;
      actual = Statistical.CumulativeMovingAverage(source);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Decimal

    [TestMethod()]
    public void CumulativeMovingAverageDecimalTest1()
    {
      IEnumerable<decimal> source = new decimal[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
      IEnumerable<decimal> expected = new decimal[] { 1, 1.5m, 2, 2.5m, 3, 3.5m, 4, 4.5m, 5, 5.5m, 6, 6.5m, 7 };
      IEnumerable<decimal> actual;
      actual = Statistical.CumulativeMovingAverage(source);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void CumulativeMovingAverageNullableDecimalTest()
    {
      IEnumerable<decimal?> source = new decimal?[] { 1, 2, 3, 4, null, null, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
      IEnumerable<decimal?> expected = new decimal?[] { 1, 1.5m, 2, 2.5m, 3, 3.5m, 4, 4.5m, 5, 5.5m, 6, 6.5m, 7 };
      IEnumerable<decimal?> actual;
      actual = Statistical.CumulativeMovingAverage(source);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Long

    [TestMethod()]
    public void CumulativeMovingAverageLongTest1()
    {
      IEnumerable<long> source = new long[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
      IEnumerable<double> expected = new double[] { 1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5, 5, 5.5, 6, 6.5, 7 };
      IEnumerable<double> actual;
      actual = Statistical.CumulativeMovingAverage(source);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void CumulativeMovingAverageNullableLongTest()
    {
      IEnumerable<long?> source = new long?[] { 1, 2, null, null, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
      IEnumerable<double?> expected = new double?[] { 1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5, 5, 5.5, 6, 6.5, 7 };
      IEnumerable<double?> actual;
      actual = Statistical.CumulativeMovingAverage(source);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Int

    [TestMethod()]
    public void CumulativeMovingAverageIntTest1()
    {
      IEnumerable<int> source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
      IEnumerable<double> expected = new double[] { 1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5, 5, 5.5, 6, 6.5, 7 };
      IEnumerable<double> actual;
      actual = Statistical.CumulativeMovingAverage(source);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void CumulativeMovingAverageNullableIntTest()
    {
      IEnumerable<int?> source = new int?[] { 1, 2, null, null, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
      IEnumerable<double?> expected = new double?[] { 1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5, 5, 5.5, 6, 6.5, 7 };
      IEnumerable<double?> actual;
      actual = Statistical.CumulativeMovingAverage(source);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #endregion

    #region Cumulative Moving Average (Selector Parameter Overload)

    #region  Double

    [TestMethod()]
    public void CumulativeMovingAverageDoubleTest2()
    {
      IEnumerable<Tuple<string, double>> source = Enumerator.Generate<Tuple<string, double>>(13, X => Generator.GenerateDouble(X, -1));
      IEnumerable<double> expected = Enumerator.Generate<double>(13, X => ((double)X + 1) / 2);
      IEnumerable<double> actual;
      actual = source.CumulativeMovingAverage(X => X.Item2);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void CumulativeMovingAverageDoubleTest3()
    {
      IEnumerable<Tuple<string, double?>> source = Enumerator.Generate<Tuple<string, double?>>(13, X => Generator.GenerateNullableDouble(X, -1, -1));
      IEnumerable<double?> expected = Enumerator.Generate<double?>(13, X => ((double)X + 1) / 2);
      IEnumerable<double?> actual;
      actual = source.CumulativeMovingAverage(X => X.Item2);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Float

    [TestMethod()]
    public void CumulativeMovingAverageFloatTest2()
    {
      IEnumerable<Tuple<string, float>> source = Enumerator.Generate<Tuple<string, float>>(13, X => Generator.GenerateFloat(X, -1));
      IEnumerable<float> expected = Enumerator.Generate<float>(13, X => ((float)X + 1) / 2);
      IEnumerable<float> actual;
      actual = source.CumulativeMovingAverage(X => X.Item2);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void CumulativeMovingAverageFloatTest3()
    {
      IEnumerable<Tuple<string, float?>> source = Enumerator.Generate<Tuple<string, float?>>(13, X => Generator.GenerateNullableFloat(X, -1, -1));
      IEnumerable<float?> expected = Enumerator.Generate<float?>(13, X => ((float)X + 1) / 2);
      IEnumerable<float?> actual;
      actual = source.CumulativeMovingAverage(X => X.Item2);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Decimal

    [TestMethod()]
    public void CumulativeMovingAverageDecimalTest2()
    {
      IEnumerable<Tuple<string, decimal>> source = Enumerator.Generate<Tuple<string, decimal>>(13, X => Generator.GenerateDecimal(X));
      IEnumerable<decimal> expected = Enumerator.Generate<decimal>(13, X => ((decimal)X + 1) / 2);
      IEnumerable<decimal> actual;
      actual = source.CumulativeMovingAverage(X => X.Item2);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void CumulativeMovingAverageDecimalTest3()
    {
      IEnumerable<Tuple<string, decimal?>> source = Enumerator.Generate<Tuple<string, decimal?>>(13, X => Generator.GenerateNullableDecimal(X, -1));
      IEnumerable<decimal?> expected = Enumerator.Generate<decimal?>(13, X => ((decimal)X + 1) / 2);
      IEnumerable<decimal?> actual;
      actual = source.CumulativeMovingAverage(X => X.Item2);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Long

    [TestMethod()]
    public void CumulativeMovingAverageLongTest2()
    {
      IEnumerable<Tuple<string, long>> source = Enumerator.Generate<Tuple<string, long>>(13, X => Generator.GenerateLong(X));
      IEnumerable<double> expected = Enumerator.Generate<double>(13, X => ((double)X + 1) / 2);
      IEnumerable<double> actual;
      actual = source.CumulativeMovingAverage(X => X.Item2);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void CumulativeMovingAverageLongTest3()
    {
      IEnumerable<Tuple<string, long?>> source = Enumerator.Generate<Tuple<string, long?>>(13, X => Generator.GenerateNullableLong(X, -1));
      IEnumerable<double?> expected = Enumerator.Generate<double?>(13, X => ((double?)X + 1) / 2);
      IEnumerable<double?> actual;
      actual = source.CumulativeMovingAverage(X => X.Item2);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }
    #endregion

    #region Int

    [TestMethod()]
    public void CumulativeMovingAverageIntTest2()
    {
      IEnumerable<Tuple<string, int>> source = Enumerator.Generate<Tuple<string, int>>(13, X => Generator.GenerateInt(X));
      IEnumerable<double> expected = Enumerator.Generate<double>(13, X => ((double)X + 1) / 2);
      IEnumerable<double> actual;
      actual = source.CumulativeMovingAverage(X => X.Item2);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void CumulativeMovingAverageIntTest3()
    {
      IEnumerable<Tuple<string, int?>> source = Enumerator.Generate<Tuple<string, int?>>(13, X => Generator.GenerateNullableInt(X, -1));
      IEnumerable<double?> expected = Enumerator.Generate<double?>(13, X => ((double)X + 1) / 2);
      IEnumerable<double?> actual;
      actual = source.CumulativeMovingAverage(X => X.Item2);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #endregion
  }
}
