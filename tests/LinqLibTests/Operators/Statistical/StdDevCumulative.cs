﻿using System;
using System.Collections.Generic;
using System.Linq;
using LinqLib.Operators;
using LinqLib.Sequence;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinqLibTests.Operators
{
  [TestClass()]
  public class StdevCumulativeTests
  {
    #region Cumulative Standard Deviation

    #region Double

    [TestMethod()]
    public void CumulativeStdevDoubleTest()
    {
      IEnumerable<double> source;
      IEnumerable<double> expected;
      IEnumerable<double> actual;

      source = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, double.NaN, 11 };
      expected = new double[] { .707, 1, 1.291, 1.581, 1.871, 2.16, 2.449, 2.739, double.NaN, double.NaN };

      actual = source.CumulativeStdev().Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new double[] { 9 };
        int x = source.CumulativeStdev().Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "source"); }
      catch (Exception) { Assert.Fail(); }
    }

    [TestMethod()]
    public void CumulativeStdevNullableDoubleTest()
    {
      IEnumerable<double?> source;
      IEnumerable<double?> expected;
      IEnumerable<double?> actual;

      source = new double?[] { 1, 2, 3, 4, null, 6, 7, 8, 9, double.NaN, 11 };
      expected = new double?[] { .707, 1, 1.291, 1.291, 1.924, 2.317, 2.637, 2.928, double.NaN, double.NaN };

      actual = source.CumulativeStdev().Select(V => V.Value).Round(3).Cast<double?>();
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new double?[] { 9 };
        int x = source.CumulativeStdev().Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "source"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      source = new double?[] { null, null, null, 4, 5, null, 7, 8, 9, 10, 11 };
      expected = new double?[] { double.NaN, double.NaN, double.NaN, 0.707, 0.707, 1.528, 1.826, 2.074, 2.317, 2.563 };

      actual = source.CumulativeStdev().Select(V => V.Value).Round(3).Cast<double?>();
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Float

    [TestMethod()]
    public void CumulativeStdevFloatTest()
    {
      IEnumerable<float> source;
      IEnumerable<float> expected;
      IEnumerable<float> actual;

      source = new float[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, float.NaN, 11 };
      expected = new float[] { .707f, 1, 1.291f, 1.581f, 1.871f, 2.16f, 2.449f, 2.739f, float.NaN, float.NaN };

      actual = source.CumulativeStdev().Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new float[] { 9 };
        int x = source.CumulativeStdev().Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "source"); }
      catch (Exception) { Assert.Fail(); }
    }

    [TestMethod()]
    public void CumulativeStdevNullableFloatTest()
    {
      IEnumerable<float?> source;
      IEnumerable<float?> expected;
      IEnumerable<float?> actual;

      source = new float?[] { 1, 2, 3, 4, null, 6, 7, 8, 9, float.NaN, 11 };
      expected = new float?[] { .707f, 1, 1.291f, 1.291f, 1.924f, 2.317f, 2.637f, 2.928f, float.NaN, float.NaN };

      actual = source.CumulativeStdev().Select(V => V.Value).Round(3).Cast<float?>();
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new float?[] { 9 };
        int x = source.CumulativeStdev().Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "source"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      source = new float?[] { null, null, null, 4, 5, null, 7, 8, 9, 10, 11 };
      expected = new float?[] { float.NaN, float.NaN, float.NaN, 0.707f, 0.707f, 1.528f, 1.826f, 2.074f, 2.317f, 2.563f };

      actual = source.CumulativeStdev().Select(V => V.Value).Round(3).Cast<float?>();
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Decimal

    [TestMethod()]
    public void CumulativeStdevDecimalTest()
    {
      IEnumerable<decimal> source;
      IEnumerable<decimal> expected;
      IEnumerable<decimal> actual;

      source = new decimal[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
      expected = new decimal[] { 0.707m, 1, 1.291m, 1.581m, 1.871m, 2.160m, 2.449m, 2.739m, 3.028m, 3.317m };

      actual = source.CumulativeStdev().Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new decimal[] { 9 };
        int x = source.CumulativeStdev().Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "source"); }
      catch (Exception) { Assert.Fail(); }
    }

    [TestMethod()]
    public void CumulativeStdevNullableDecimalTest()
    {
      IEnumerable<decimal?> source;
      IEnumerable<decimal?> expected;
      IEnumerable<decimal?> actual;

      source = new decimal?[] { 1, 2, 3, 4, null, 6, 7, 8, 9, 10, 11 };
      expected = new decimal?[] { .707m, 1, 1.291m, 1.291m, 1.924m, 2.317m, 2.637m, 2.928m, 3.206m, 3.479m };

      actual = source.CumulativeStdev().Select(V => V.Value).Round(3).Cast<decimal?>();
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new decimal?[] { 9 };
        int x = source.CumulativeStdev().Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "source"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      try
      {
        source = new decimal?[] { null, null, null, 4, 5, null, 7, 8, 9, 10, 11 };
        int x = source.CumulativeStdev().Count();
        Assert.Fail();
      }
      catch (DivideByZeroException)
      {
        Assert.IsTrue(true);
      }
    }

    #endregion

    #region Long

    [TestMethod()]
    public void CumulativeStdevLongTest()
    {
      IEnumerable<long> source;
      IEnumerable<double> expected;
      IEnumerable<double> actual;

      source = new long[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
      expected = new double[] { .707, 1, 1.291, 1.581, 1.871, 2.160, 2.449, 2.739, 3.028, 3.317 };

      actual = source.CumulativeStdev().Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new long[] { 9 };
        int x = source.CumulativeStdev().Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "source"); }
      catch (Exception) { Assert.Fail(); }
    }

    [TestMethod()]
    public void CumulativeStdevNullableLongTest()
    {
      IEnumerable<long?> source;
      IEnumerable<double?> expected;
      IEnumerable<double?> actual;

      source = new long?[] { 1, 2, 3, 4, null, 6, 7, 8, 9, 10, 11 };
      expected = new double?[] { .707, 1, 1.291, 1.291, 1.924, 2.317, 2.637, 2.928, 3.206, 3.479 };

      actual = source.CumulativeStdev().Select(V => V.Value).Round(3).Cast<double?>();
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new long?[] { 9 };
        int x = source.CumulativeStdev().Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "source"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      try
      {
        source = new long?[] { null, null, null, 4, 5, null, 7, 8, 9, 10, 11 };
        int x = source.CumulativeStdev().Count();
        Assert.Fail();
      }
      catch (DivideByZeroException)
      {
        Assert.IsTrue(true);
      }
    }

    #endregion

    #region Int

    [TestMethod()]
    public void CumulativeStdevIntTest()
    {
      IEnumerable<int> source;
      IEnumerable<double> expected;
      IEnumerable<double> actual;

      source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
      expected = new double[] { 0.707, 1, 1.291, 1.581, 1.871, 2.160, 2.449, 2.739, 3.028, 3.317 };

      actual = source.CumulativeStdev().Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new int[] { 9 };
        int x = source.CumulativeStdev().Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "source"); }
      catch (Exception) { Assert.Fail(); }
    }

    [TestMethod()]
    public void CumulativeStdevNullableIntTest()
    {
      IEnumerable<int?> source;
      IEnumerable<double?> expected;
      IEnumerable<double?> actual;

      source = new int?[] { 1, 2, 3, 4, null, 6, 7, 8, 9, 10, 11 };
      expected = new double?[] { .707, 1, 1.291, 1.291, 1.924, 2.317, 2.637, 2.928, 3.206, 3.479 };

      actual = source.CumulativeStdev().Select(V => V.Value).Round(3).Cast<double?>();
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new int?[] { 9 };
        int x = source.CumulativeStdev().Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "source"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      try
      {
        source = new int?[] { null, null, null, 4, 5, null, 7, 8, 9, 10, 11 };
        int x = source.CumulativeStdev().Count();
        Assert.Fail();
      }
      catch (DivideByZeroException)
      {
        Assert.IsTrue(true);
      }
    }

    #endregion

    #endregion

    #region Cumulative Standard Deviation (Selector Parameter Overload)

    #region Double

    [TestMethod()]
    public void CumulativeStdevDoubleTest1()
    {
      IEnumerable<Tuple<string, double>> source = Enumerator.Generate<Tuple<string, double>>(5, X => Generator.GenerateDouble(X, 10));
      IEnumerable<double> expected = new double[] { .707, 1, 1.291, 1.581 };
      IEnumerable<double> actual;
      actual = source.CumulativeStdev(X => X.Item2).Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void CumulativeStdevNullableDoubleTest1()
    {
      IEnumerable<Tuple<string, double?>> source = Enumerator.Generate<Tuple<string, double?>>(5, X => Generator.GenerateNullableDouble(X, 10, 2));
      IEnumerable<double?> expected = new double?[] { double.NaN, 1.414, 1.528, 1.708 };
      IEnumerable<double?> actual;
      actual = source.CumulativeStdev(X => X.Item2).Select(V => V.Value).Round(3).Cast<double?>();
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Float

    [TestMethod()]
    public void CumulativeStdevFloatTest1()
    {
      IEnumerable<Tuple<string, float>> source = Enumerator.Generate<Tuple<string, float>>(5, X => Generator.GenerateFloat(X, 10));
      IEnumerable<float> expected = new float[] { .707f, 1, 1.291f, 1.581f };
      IEnumerable<float> actual;
      actual = source.CumulativeStdev(X => X.Item2).Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void CumulativeStdevNullableFloatTest1()
    {
      IEnumerable<Tuple<string, float?>> source = Enumerator.Generate<Tuple<string, float?>>(5, X => Generator.GenerateNullableFloat(X, 10, 2));
      IEnumerable<float?> expected = new float?[] { float.NaN, 1.414f, 1.528f, 1.708f };
      IEnumerable<float?> actual;
      actual = source.CumulativeStdev(X => X.Item2).Select(V => V.Value).Round(3).Cast<float?>();
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Decimal

    [TestMethod()]
    public void CumulativeStdevDecimalTest1()
    {
      IEnumerable<Tuple<string, decimal>> source = Enumerator.Generate<Tuple<string, decimal>>(5, X => Generator.GenerateDecimal(X));
      IEnumerable<decimal> expected = new decimal[] { .707m, 1, 1.291m, 1.581m };
      IEnumerable<decimal> actual;
      actual = source.CumulativeStdev(X => X.Item2).Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void CumulativeStdevNullableDecimalTest1()
    {
      IEnumerable<Tuple<string, decimal?>> source = Enumerator.Generate<Tuple<string, decimal?>>(5, X => Generator.GenerateNullableDecimal(X, 20));
      IEnumerable<decimal?> expected = new decimal?[] { 0.707m, 1.000m, 1.291m, 1.581m };
      IEnumerable<decimal?> actual;
      actual = source.CumulativeStdev(X => X.Item2).Select(V => V.Value).Round(3).Cast<decimal?>();
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Long

    [TestMethod()]
    public void CumulativeStdevLongTest1()
    {
      IEnumerable<Tuple<string, long>> source = Enumerator.Generate<Tuple<string, long>>(5, X => Generator.GenerateLong(X));
      IEnumerable<double> expected = new double[] { .707, 1, 1.291, 1.581 };
      IEnumerable<double> actual;
      actual = source.CumulativeStdev(X => X.Item2).Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void CumulativeStdevNullableLongTest1()
    {
      IEnumerable<Tuple<string, long?>> source = Enumerator.Generate<Tuple<string, long?>>(5, X => Generator.GenerateNullableLong(X, 20));
      IEnumerable<double?> expected = new double?[] { 0.707, 1.000, 1.291, 1.581 };
      IEnumerable<double?> actual;
      actual = source.CumulativeStdev(X => X.Item2).Select(V => V.Value).Round(3).Cast<double?>();
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Int

    [TestMethod()]
    public void CumulativeStdevIntTest1()
    {
      IEnumerable<Tuple<string, int>> source = Enumerator.Generate<Tuple<string, int>>(5, X => Generator.GenerateInt(X));
      IEnumerable<double> expected = new double[] { .707, 1, 1.291, 1.581 };
      IEnumerable<double> actual;
      actual = source.CumulativeStdev(X => X.Item2).Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void CumulativeStdevNullableIntTest1()
    {
      IEnumerable<Tuple<string, int?>> source = Enumerator.Generate<Tuple<string, int?>>(5, X => Generator.GenerateNullableInt(X, 20));
      IEnumerable<double?> expected = new double?[] { 0.707, 1.000, 1.291, 1.581 };
      IEnumerable<double?> actual;
      actual = source.CumulativeStdev(X => X.Item2).Select(V => V.Value).Round(3).Cast<double?>();
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #endregion
  }
}