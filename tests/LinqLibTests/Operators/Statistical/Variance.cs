using System;
using System.Collections.Generic;
using System.Linq;
using LinqLib.Operators;
using LinqLib.Sequence;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinqLibTests.Operators
{
  [TestClass()]
  public class VarianceTests
  {
    #region Variance

    #region Double

    [TestMethod()]
    public void VarianceDoubleTest()
    {
      IEnumerable<Tuple<string, double>> source = Enumerator.Generate<Tuple<string, double>>(7, X => Generator.GenerateDouble(X, 10));
      double expected = 4.667;
      double actual;
      actual = (double)Math.Round(source.Variance(X => X.Item2), 3);
      Assert.IsTrue(expected.Equals(actual));
    }

    [TestMethod()]
    public void VarianceNullableDoubleTest()
    {
      IEnumerable<Tuple<string, double?>> source = Enumerator.Generate<Tuple<string, double?>>(7, X => Generator.GenerateNullableDouble(X, 10, 10));
      double? expected = 4.667;
      double? actual;
      actual = (double?)Math.Round(source.Variance(X => X.Item2).Value, 3);
      Assert.IsTrue(expected.Equals(actual));
    }

    #endregion

    #region Float

    [TestMethod()]
    public void VarianceFloatTest()
    {
      IEnumerable<Tuple<string, float>> source = Enumerator.Generate<Tuple<string, float>>(7, X => Generator.GenerateFloat(X, 10));
      float expected = 4.667f;
      float actual;
      actual = (float)Math.Round(source.Variance(X => X.Item2), 3);
      Assert.IsTrue(expected.Equals(actual));
    }

    [TestMethod()]
    public void VarianceNullableFloatTest()
    {
      IEnumerable<Tuple<string, float?>> source = Enumerator.Generate<Tuple<string, float?>>(7, X => Generator.GenerateNullableFloat(X, 10, 10));
      float? expected = 4.667f;
      float? actual;
      actual = (float?)Math.Round(source.Variance(X => X.Item2).Value, 3);
      Assert.IsTrue(expected.Equals(actual));
    }

    #endregion

    #region Decimal

    [TestMethod()]
    public void VarianceDecimalTest()
    {
      IEnumerable<Tuple<string, decimal>> source = Enumerator.Generate<Tuple<string, decimal>>(7, X => Generator.GenerateDecimal(X));
      decimal? expected = 4.667m;
      decimal? actual;
      actual = (decimal?)Math.Round(source.Variance(X => X.Item2), 3);
      Assert.IsTrue(expected.Equals(actual));
    }

    [TestMethod()]
    public void VarianceNullableDecimalTest()
    {
      IEnumerable<Tuple<string, decimal?>> source = Enumerator.Generate<Tuple<string, decimal?>>(7, X => Generator.GenerateNullableDecimal(X, 10));
      decimal? expected = 4.667m;
      decimal? actual;
      actual = (decimal?)Math.Round(source.Variance(X => X.Item2).Value, 3);
      Assert.IsTrue(expected.Equals(actual));
    }

    #endregion

    #region Long

    [TestMethod()]
    public void VarianceLongTest()
    {
      IEnumerable<Tuple<string, long>> source = Enumerator.Generate<Tuple<string, long>>(7, X => Generator.GenerateLong(X));
      double expected = 4.667;
      double actual;
      actual = (double)Math.Round(source.Variance(X => X.Item2), 3);
      Assert.IsTrue(expected.Equals(actual));
    }

    [TestMethod()]
    public void VarianceNullableLongTest()
    {
      IEnumerable<Tuple<string, long?>> source = Enumerator.Generate<Tuple<string, long?>>(7, X => Generator.GenerateNullableLong(X, 10));
      double? expected = 4.667;
      double? actual;
      actual = (double?)Math.Round(source.Variance(X => X.Item2).Value, 3);
      Assert.IsTrue(expected.Equals(actual));
    }

    #endregion

    #region Int

    [TestMethod()]
    public void VarianceIntTest()
    {
      IEnumerable<Tuple<string, int>> source = Enumerator.Generate<Tuple<string, int>>(7, X => Generator.GenerateInt(X));
      double expected = 4.667;
      double actual;
      actual = (double)Math.Round(source.Variance(X => X.Item2), 3);
      Assert.IsTrue(expected.Equals(actual));
    }

    [TestMethod()]
    public void VarianceNullableIntTest()
    {
      IEnumerable<Tuple<string, int?>> source = Enumerator.Generate<Tuple<string, int?>>(7, X => Generator.GenerateNullableInt(X, 10));
      double? expected = 4.667;
      double? actual;
      actual = (double?)Math.Round(source.Variance(X => X.Item2).Value, 3);
      Assert.IsTrue(expected.Equals(actual));
    }

    #endregion

    #endregion

    #region Variance (Block Parameter Overload)

    #region Double

    [TestMethod()]
    public void VarianceDoubleTest1()
    {
      IEnumerable<Tuple<string, double>> source = Enumerator.Generate<Tuple<string, double>>(5, X => Generator.GenerateDouble(X, 10));
      IEnumerable<double> expected = new double[] { 2.5 };
      IEnumerable<double> actual;
      actual = source.Variance(5, X => X.Item2).Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void VarianceNullableDoubleTest1()
    {
      IEnumerable<Tuple<string, double?>> source = Enumerator.Generate<Tuple<string, double?>>(5, X => Generator.GenerateNullableDouble(X, 10, 10));
      IEnumerable<double?> expected = new double?[] { 2.5 };
      IEnumerable<double?> actual;
      actual = source.Variance(5, X => X.Item2).Select(V => V.Value).Round(3).Cast<double?>();
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Float

    [TestMethod()]
    public void VarianceFloatTest1()
    {
      IEnumerable<Tuple<string, float>> source = Enumerator.Generate<Tuple<string, float>>(5, X => Generator.GenerateFloat(X, 10));
      IEnumerable<float> expected = new float[] { 2.5f };
      IEnumerable<float> actual;
      actual = source.Variance(5, X => X.Item2).Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void VarianceNullableFloatTest1()
    {
      IEnumerable<Tuple<string, float?>> source = Enumerator.Generate<Tuple<string, float?>>(5, X => Generator.GenerateNullableFloat(X, 10, 10));
      IEnumerable<float?> expected = new float?[] { 2.5f };
      IEnumerable<float?> actual;
      actual = source.Variance(5, X => X.Item2).Select(V => V.Value).Round(3).Cast<float?>();
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Decimal

    [TestMethod()]
    public void VarianceDecimalTest1()
    {
      IEnumerable<Tuple<string, decimal>> source = Enumerator.Generate<Tuple<string, decimal>>(5, X => Generator.GenerateDecimal(X));
      IEnumerable<decimal> expected = new decimal[] { 2.5m };
      IEnumerable<decimal> actual;
      actual = source.Variance(5, X => X.Item2).Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void VarianceNullableDecimalTest1()
    {
      IEnumerable<Tuple<string, decimal?>> source = Enumerator.Generate<Tuple<string, decimal?>>(5, X => Generator.GenerateNullableDecimal(X, 20));
      IEnumerable<decimal?> expected = new decimal?[] { 2.5m };
      IEnumerable<decimal?> actual;
      actual = source.Variance(5, X => X.Item2).Select(V => V.Value).Round(3).Cast<decimal?>();
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Long

    [TestMethod()]
    public void VarianceLongTest1()
    {
      IEnumerable<Tuple<string, long>> source = Enumerator.Generate<Tuple<string, long>>(5, X => Generator.GenerateLong(X));
      IEnumerable<double> expected = new double[] { 2.5 };
      IEnumerable<double> actual;
      actual = source.Variance(5, X => X.Item2).Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void VarianceNullableLongTest1()
    {
      IEnumerable<Tuple<string, long?>> source = Enumerator.Generate<Tuple<string, long?>>(5, X => Generator.GenerateNullableLong(X, 20));
      IEnumerable<double?> expected = new double?[] { 2.5 };
      IEnumerable<double?> actual;
      actual = source.Variance(5, X => X.Item2).Select(V => V.Value).Round(3).Cast<double?>();
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Int

    [TestMethod()]
    public void VarianceIntTest1()
    {
      IEnumerable<Tuple<string, int>> source = Enumerator.Generate<Tuple<string, int>>(5, X => Generator.GenerateInt(X));
      IEnumerable<double> expected = new double[] { 2.5 };
      IEnumerable<double> actual;
      actual = source.Variance(5, X => X.Item2).Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void VarianceNullableIntTest1()
    {
      IEnumerable<Tuple<string, int?>> source = Enumerator.Generate<Tuple<string, int?>>(5, X => Generator.GenerateNullableInt(X, 20));
      IEnumerable<double?> expected = new double?[] { 2.5 };
      IEnumerable<double?> actual;
      actual = source.Variance(5, X => X.Item2).Select(V => V.Value).Round(3).Cast<double?>();
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #endregion

    #region Cumulative Variance

    #region Double

    [TestMethod()]
    public void VarianceDoubleTest2()
    {
      IEnumerable<Tuple<string, double>> source = Enumerator.Generate<Tuple<string, double>>(5, X => Generator.GenerateDouble(X, 10));
      IEnumerable<double> expected = (new double[] { .707, 1, 1.291, 1.581 }).Select(X => X * X).Round(3);
      IEnumerable<double> actual;
      actual = source.CumulativeVariance(X => X.Item2).Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void VarianceNullableDoubleTest2()
    {
      IEnumerable<Tuple<string, double?>> source = Enumerator.Generate<Tuple<string, double?>>(5, X => Generator.GenerateNullableDouble(X, 10, 2));
      IEnumerable<double?> expected = new double?[] { double.NaN, 1.414, 1.528, 1.708 }.Select(X => X.Value * X.Value).Round(2).Cast<double?>();
      IEnumerable<double?> actual;
      actual = source.CumulativeVariance(X => X.Item2).Select(V => V.Value).Round(2).Cast<double?>();
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Float

    [TestMethod()]
    public void VarianceFloatTest2()
    {
      IEnumerable<Tuple<string, float>> source = Enumerator.Generate<Tuple<string, float>>(5, X => Generator.GenerateFloat(X, 10));
      IEnumerable<float> expected = new float[] { .707f, 1, 1.291f, 1.581f }.Select(X => X * X).Round(3);
      IEnumerable<float> actual;
      actual = source.CumulativeVariance(X => X.Item2).Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void VarianceNullableFloatTest2()
    {
      IEnumerable<Tuple<string, float?>> source = Enumerator.Generate<Tuple<string, float?>>(5, X => Generator.GenerateNullableFloat(X, 10, 2));
      IEnumerable<float?> expected = new float?[] { float.NaN, 1.414f, 1.528f, 1.708f }.Select(X => X.Value * X.Value).Round(2).Cast<float?>();
      IEnumerable<float?> actual;
      actual = source.CumulativeVariance(X => X.Item2).Select(V => V.Value).Round(2).Cast<float?>();
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Decimal

    [TestMethod()]
    public void VarianceDecimalTest2()
    {
      IEnumerable<Tuple<string, decimal>> source = Enumerator.Generate<Tuple<string, decimal>>(5, X => Generator.GenerateDecimal(X));
      IEnumerable<decimal> expected = new decimal[] { .707m, 1, 1.291m, 1.581m }.Select(X => X * X).Round(3);
      IEnumerable<decimal> actual;
      actual = source.CumulativeVariance(X => X.Item2).Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void VarianceNullableDecimalTest2()
    {
      IEnumerable<Tuple<string, decimal?>> source = Enumerator.Generate<Tuple<string, decimal?>>(5, X => Generator.GenerateNullableDecimal(X, 20));
      IEnumerable<decimal?> expected = new decimal?[] { 0.707m, 1.000m, 1.291m, 1.581m }.Select(X => X.Value * X.Value).Round(2).Cast<decimal?>();
      IEnumerable<decimal?> actual;
      actual = source.CumulativeVariance(X => X.Item2).Select(V => V.Value).Round(2).Cast<decimal?>();
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Long

    [TestMethod()]
    public void VarianceLongTest2()
    {
      IEnumerable<Tuple<string, long>> source = Enumerator.Generate<Tuple<string, long>>(5, X => Generator.GenerateLong(X));
      IEnumerable<double> expected = new double[] { .707, 1, 1.291, 1.581 }.Select(X => X * X).Round(3);
      IEnumerable<double> actual;
      actual = source.CumulativeVariance(X => X.Item2).Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void VarianceNullableLongTest2()
    {
      IEnumerable<Tuple<string, long?>> source = Enumerator.Generate<Tuple<string, long?>>(5, X => Generator.GenerateNullableLong(X, 20));
      IEnumerable<double?> expected = new double?[] { 0.707, 1.000, 1.291, 1.581 }.Select(X => X.Value * X.Value).Round(2).Cast<double?>();
      IEnumerable<double?> actual;
      actual = source.CumulativeVariance(X => X.Item2).Select(V => V.Value).Round(2).Cast<double?>();
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Int

    [TestMethod()]
    public void VarianceIntTest2()
    {
      IEnumerable<Tuple<string, int>> source = Enumerator.Generate<Tuple<string, int>>(5, X => Generator.GenerateInt(X));
      IEnumerable<double> expected = new double[] { .707, 1, 1.291, 1.581 }.Select(X => X * X).Round(3);
      IEnumerable<double> actual;
      actual = source.CumulativeVariance(X => X.Item2).Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void VarianceNullableIntTest2()
    {
      IEnumerable<Tuple<string, int?>> source = Enumerator.Generate<Tuple<string, int?>>(5, X => Generator.GenerateNullableInt(X, 20));
      IEnumerable<double?> expected = new double?[] { 0.707, 1.000, 1.291, 1.581 }.Select(X => X.Value * X.Value).Round(2).Cast<double?>();
      IEnumerable<double?> actual;
      actual = source.CumulativeVariance(X => X.Item2).Select(V => V.Value).Round(2).Cast<double?>();
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #endregion
  }
}

