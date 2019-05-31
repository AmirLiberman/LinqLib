using System;
using System.Collections.Generic;
using LinqLib.Operators;
using LinqLib.Sequence;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinqLibTests.Operators
{
  [TestClass()]
  public class StdevTests
  {
    #region Standard Deviation

    #region Double

    [TestMethod()]
    public void StdevDoubleTest1()
    {
      IEnumerable<double> source;
      double expected;
      double actual;

      source = new double[] { 1, 2, 3, 4, 5, 6, 7 };
      expected = 2.160;

      actual = Math.Round(source.Stdev(), 3);
      Assert.IsTrue(expected.Equals(actual));

      //------------------------------//

      try
      {
        source = new double[] { 9 };
        actual = Math.Round(source.Stdev(), 3);
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "source"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      source = new double[] { 1, 2, double.NaN, 4, 5, 6, 7 };
      expected = double.NaN;

      actual = Math.Round(source.Stdev(), 3);
      Assert.IsTrue(expected.Equals(actual));
    }

    [TestMethod()]
    public void StdevNullableDoubleTest1()
    {
      IEnumerable<double?> source;
      double? expected;
      double? actual;

      source = new double?[] { 1, 2, 3, 4, 5, 6, 7 };
      expected = 2.160;

      actual = Math.Round(source.Stdev().Value, 3);
      Assert.IsTrue(expected.Equals(actual));

      //------------------------------//

      try
      {
        source = new double?[] { 9 };
        actual = Math.Round(source.Stdev().Value, 3);
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "source"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      source = new double?[] { 1, 2, double.NaN, 4, 5, 6, 7 };
      expected = double.NaN;

      actual = Math.Round(source.Stdev().Value, 3);
      Assert.IsTrue(expected.Equals(actual));

      //------------------------------//

      source = new double?[] { null, 2, 3, 4, 5, 6, 7 };
      expected = 1.871;

      actual = Math.Round(source.Stdev().Value, 3);
      Assert.IsTrue(expected.Equals(actual));

      //------------------------------//

      source = new double?[] { 1, 2, 3, 4, null, 6, 7 };
      expected = 2.317;

      actual = Math.Round(source.Stdev().Value, 3);
      Assert.IsTrue(expected.Equals(actual));
    }

    [TestMethod()]
    public void StdevDoubleTest2()
    {
      IEnumerable<Tuple<string, double>> source = Enumerator.Generate<Tuple<string, double>>(7, X => Generator.GenerateDouble(X, 10));
      double expected = 2.160;
      double actual;
      actual = Math.Round(source.Stdev(X => X.Item2), 3);
      Assert.IsTrue(expected.Equals(actual));
    }

    [TestMethod()]
    public void StdevNullableDoubleTest2()
    {
      IEnumerable<Tuple<string, double?>> source = LinqLib.Sequence.Enumerator.Generate<Tuple<string, double?>>(7, X => Generator.GenerateNullableDouble(X, 10, 10));
      double? expected = 2.160;
      double? actual;
      actual = (double?)Math.Round(source.Stdev(X => X.Item2).Value, 3);
      Assert.IsTrue(expected.Equals(actual));
    }

    #endregion

    #region Float

    [TestMethod()]
    public void StdevFloatTest1()
    {
      IEnumerable<float> source;
      float expected;
      float actual;

      source = new float[] { 1, 2, 3, 4, 5, 6, 7 };
      expected = 2.160f;
      actual = (float)Math.Round(source.Stdev(), 3);
      Assert.IsTrue(expected.Equals(actual));

      //------------------------------//

      try
      {
        source = new float[] { 9 };
        actual = (float)Math.Round(source.Stdev(), 3);
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "source"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      source = new float[] { 1, 2, float.NaN, 4, 5, 6, 7 };
      expected = float.NaN;

      actual = (float)Math.Round(source.Stdev(), 3);
      Assert.IsTrue(expected.Equals(actual));
    }

    [TestMethod()]
    public void StdevNullableFloatTest1()
    {
      IEnumerable<float?> source;
      float? expected;
      float? actual;

      source = new float?[] { 1, 2, 3, 4, 5, 6, 7 };
      expected = 2.160f;

      actual = (float)Math.Round(source.Stdev().Value, 3);
      Assert.IsTrue(expected.Equals(actual));

      //------------------------------//

      try
      {
        source = new float?[] { 9 };

        actual = (float)Math.Round(source.Stdev().Value, 3);
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "source"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      source = new float?[] { 1, 2, float.NaN, 4, 5, 6, 7 };
      expected = float.NaN;

      actual = (float)Math.Round(source.Stdev().Value, 3);
      Assert.IsTrue(expected.Equals(actual));

      //------------------------------//

      source = new float?[] { null, 2, 3, 4, 5, 6, 7 };
      expected = 1.871f;

      actual = (float)Math.Round(source.Stdev().Value, 3);
      Assert.IsTrue(expected.Equals(actual));

      //------------------------------//

      source = new float?[] { 1, 2, 3, 4, null, 6, 7 };
      expected = 2.317f;

      actual = (float)Math.Round(source.Stdev().Value, 3);
      Assert.IsTrue(expected.Equals(actual));
    }

    [TestMethod()]
    public void StdevFloatTest2()
    {
      IEnumerable<Tuple<string, float>> source = Enumerator.Generate<Tuple<string, float>>(7, X => Generator.GenerateFloat(X, 10));
      float expected = 2.160f;
      float actual;
      actual = (float)Math.Round(source.Stdev(X => X.Item2), 3);
      Assert.IsTrue(expected.Equals(actual));
    }

    [TestMethod()]
    public void StdevNullableFloatTest2()
    {
      IEnumerable<Tuple<string, float?>> source = Enumerator.Generate<Tuple<string, float?>>(7, X => Generator.GenerateNullableFloat(X, 10, 10));
      float? expected = 2.160f;
      float? actual;
      actual = (float?)Math.Round(source.Stdev(X => X.Item2).Value, 3);
      Assert.IsTrue(expected.Equals(actual));
    }

    #endregion

    #region Decimal

    [TestMethod()]
    public void StdevDecimalTest1()
    {
      IEnumerable<decimal> source;
      decimal expected;
      decimal actual;

      source = new decimal[] { 1, 2, 3, 4, 5, 6, 7 };
      expected = 2.160m;

      actual = Math.Round(source.Stdev(), 3);
      Assert.IsTrue(expected.Equals(actual));

      //------------------------------//

      try
      {
        source = new decimal[] { 9 };

        actual = Math.Round(source.Stdev(), 3);
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "source"); }
      catch (Exception) { Assert.Fail(); }
    }

    [TestMethod()]
    public void StdevNullableDecimalTest1()
    {
      IEnumerable<decimal?> source;
      decimal? expected;
      decimal? actual;

      source = new decimal?[] { 1, 2, 3, 4, 5, 6, 7 };
      expected = 2.160m;

      actual = Math.Round(source.Stdev().Value, 3);
      Assert.IsTrue(expected.Equals(actual));

      //------------------------------//

      try
      {
        source = new decimal?[] { 9 };

        actual = Math.Round(source.Stdev().Value, 3);
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "source"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      source = new decimal?[] { null, 2, 3, 4, 5, 6, 7 };
      expected = 1.871m;

      actual = Math.Round(source.Stdev().Value, 3);
      Assert.IsTrue(expected.Equals(actual));

      //------------------------------//

      source = new decimal?[] { 1, 2, 3, 4, null, 6, 7 };
      expected = 2.317m;

      actual = Math.Round(source.Stdev().Value, 3);
      Assert.IsTrue(expected.Equals(actual));
    }

    [TestMethod()]
    public void StdevDecimalTest2()
    {
      IEnumerable<Tuple<string, decimal>> source = Enumerator.Generate<Tuple<string, decimal>>(7, X => Generator.GenerateDecimal(X));
      decimal expected = 2.160m;
      decimal actual;
      actual = (decimal)Math.Round(source.Stdev(X => X.Item2), 3);
      Assert.IsTrue(expected.Equals(actual));
    }

    [TestMethod()]
    public void StdevNullableDecimalTest2()
    {
      IEnumerable<Tuple<string, decimal?>> source = Enumerator.Generate<Tuple<string, decimal?>>(7, X => Generator.GenerateNullableDecimal(X, 10));
      decimal? expected = 2.160m;
      decimal? actual;
      actual = (decimal?)Math.Round(source.Stdev(X => X.Item2).Value, 3);
      Assert.IsTrue(expected.Equals(actual));
    }

    #endregion

    #region Long

    [TestMethod()]
    public void StdevLongTest1()
    {
      IEnumerable<long> source;
      double expected;
      double actual;

      source = new long[] { 1, 2, 3, 4, 5, 6, 7 };
      expected = 2.160;

      actual = Math.Round(source.Stdev(), 3);
      Assert.IsTrue(expected.Equals(actual));

      //------------------------------//

      try
      {
        source = new long[] { 9 };

        actual = Math.Round(source.Stdev(), 3);
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "source"); }
      catch (Exception) { Assert.Fail(); }
    }

    [TestMethod()]
    public void StdevNullableLongTest1()
    {
      IEnumerable<long?> source;
      double? expected;
      double? actual;

      source = new long?[] { 1, 2, 3, 4, 5, 6, 7 };
      expected = 2.160;

      actual = Math.Round(source.Stdev().Value, 3);
      Assert.IsTrue(expected.Equals(actual));

      //------------------------------//

      try
      {
        source = new long?[] { 9 };
        actual = Math.Round(source.Stdev().Value, 3);
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "source"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      source = new long?[] { null, 2, 3, 4, 5, 6, 7 };
      expected = 1.871;

      actual = Math.Round(source.Stdev().Value, 3);
      Assert.IsTrue(expected.Equals(actual));

      //------------------------------//

      source = new long?[] { 1, 2, 3, 4, null, 6, 7 };
      expected = 2.317;

      actual = Math.Round(source.Stdev().Value, 3);
      Assert.IsTrue(expected.Equals(actual));
    }

    [TestMethod()]
    public void StdevLongTest2()
    {
      IEnumerable<Tuple<string, long>> source = Enumerator.Generate<Tuple<string, long>>(7, X => Generator.GenerateLong(X));
      double expected = 2.160;
      double actual;
      actual = Math.Round(source.Stdev(X => X.Item2), 3);
      Assert.IsTrue(expected.Equals(actual));
    }

    [TestMethod()]
    public void StdevNullableLongTest2()
    {
      IEnumerable<Tuple<string, long?>> source = Enumerator.Generate<Tuple<string, long?>>(7, X => Generator.GenerateNullableLong(X, 10));
      double? expected = 2.160;
      double? actual;
      actual = (double?)Math.Round(source.Stdev(X => X.Item2).Value, 3);
      Assert.IsTrue(expected.Equals(actual));
    }

    #endregion

    #region Int

    [TestMethod()]
    public void StdevIntTest1()
    {
      IEnumerable<int> source;
      double expected;
      double actual;

      source = new int[] { 1, 2, 3, 4, 5, 6, 7 };
      expected = 2.160;

      actual = Math.Round(source.Stdev(), 3);
      Assert.IsTrue(expected.Equals(actual));

      //------------------------------//

      try
      {
        source = new int[] { 9 };

        actual = Math.Round(source.Stdev(), 3);
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "source"); }
      catch (Exception) { Assert.Fail(); }
    }

    [TestMethod()]
    public void StdevIntLongTest1()
    {
      IEnumerable<int?> source;
      double? expected;
      double? actual;

      source = new int?[] { 1, 2, 3, 4, 5, 6, 7 };
      expected = 2.160;

      actual = Math.Round(source.Stdev().Value, 3);
      Assert.IsTrue(expected.Equals(actual));

      //------------------------------//

      try
      {
        source = new int?[] { 9 };

        actual = Math.Round(source.Stdev().Value, 3);
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "source"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      source = new int?[] { null, 2, 3, 4, 5, 6, 7 };
      expected = 1.871;

      actual = Math.Round(source.Stdev().Value, 3);
      Assert.IsTrue(expected.Equals(actual));

      //------------------------------//

      source = new int?[] { 1, 2, 3, 4, null, 6, 7 };
      expected = 2.317;

      actual = Math.Round(source.Stdev().Value, 3);
      Assert.IsTrue(expected.Equals(actual));
    }

    [TestMethod()]
    public void StdevIntTest2()
    {
      IEnumerable<Tuple<string, int>> source = Enumerator.Generate<Tuple<string, int>>(7, X => Generator.GenerateInt(X));
      double expected = 2.160;
      double actual;
      actual = Math.Round(source.Stdev(X => X.Item2), 3);
      Assert.IsTrue(expected.Equals(actual));
    }

    [TestMethod()]
    public void StdevIntLongTest2()
    {
      IEnumerable<Tuple<string, int?>> source = Enumerator.Generate<Tuple<string, int?>>(7, X => Generator.GenerateNullableInt(X, 10));
      double? expected = 2.160;
      double? actual;
      actual = (double?)Math.Round(source.Stdev(X => X.Item2).Value, 3);
      Assert.IsTrue(expected.Equals(actual));
    }

    #endregion

    #endregion
  }
}
