using System;
using System.Collections.Generic;
using System.Linq;
using LinqLib.Operators;
using LinqLib.Sequence;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinqLibTests.Sequence
{
  [TestClass()]
  public class ScrubTests
  {
    #region Round and Truncate

    [TestMethod()]
    public void RoundDoubleTest()
    {
      IEnumerable<double> input = new double[] { 1.1234555, 1.1234444, 1.12344446 };
      IEnumerable<double> expected = new double[] { 1.1235, 1.1234, 1.1234 };
      IEnumerable<double> actual;
      actual = input.Round(4);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        input = new double[] { 1.1234555, 1.1234444, 1.12344446 };
        int x = input.Round(19).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "digits"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      try
      {
        input = new double[] { 1.1234555, 1.1234444, 1.12344446 };
        int x = input.Round(-19).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "digits"); }
      catch (Exception) { Assert.Fail(); }
    }

    [TestMethod()]
    public void RoundFloatTest()
    {
      IEnumerable<float> input = new float[] { 1.1234555f, 1.1234444f, 1.12344446f };
      IEnumerable<float> expected = new float[] { 1.1235f, 1.1234f, 1.1234f };
      IEnumerable<float> actual;
      actual = input.Round(4);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        input = new float[] { 1.1234555f, 1.1234444f, 1.12344446f };
        int x = input.Round(11).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "digits"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      try
      {
        input = new float[] { 1.1234555f, 1.1234444f, 1.12344446f };
        int x = input.Round(-19).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "digits"); }
      catch (Exception) { Assert.Fail(); }

    }

    [TestMethod()]
    public void RoundDecimalTest()
    {
      IEnumerable<decimal> input = new decimal[] { 1.1234555m, 1.1234444m, 1.12344446m };
      IEnumerable<decimal> expected = new decimal[] { 1.1235m, 1.1234m, 1.1234m };
      IEnumerable<decimal> actual;
      actual = input.Round(4);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        input = new decimal[] { 1.1234555m, 1.1234444m, 1.12344446m };
        int x = input.Round(29).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "digits"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      try
      {
        input = new decimal[] { 1.1234555m, 1.1234444m, 1.12344446m };
        int x = input.Round(-1).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "digits"); }
      catch (Exception) { Assert.Fail(); }
    }

    [TestMethod()]
    public void TruncateDoubleTest()
    {
      IEnumerable<double> input = new double[] { 1.1234555, 2.1234444, 3.12344446 };
      IEnumerable<double> expected = new double[] { 1, 2, 3 };
      IEnumerable<double> actual;
      actual = input.Truncate();
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void TruncateFloatTest()
    {
      IEnumerable<float> input = new float[] { 1.1234555f, 2.1234444f, 3.12344446f };
      IEnumerable<float> expected = new float[] { 1, 2, 3 };
      IEnumerable<float> actual;
      actual = input.Truncate();
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void TruncateDecimalTest()
    {
      IEnumerable<decimal> input = new decimal[] { 1.1234555m, 2.1234444m, 3.12344446m };
      IEnumerable<decimal> expected = new decimal[] { 1, 2, 3 };
      IEnumerable<decimal> actual;
      actual = input.Truncate();
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Remove Nans and Nulls

    [TestMethod()]
    public void RemoveNansDoubleTest1()
    {
      IEnumerable<double> input = new double[] { 1, 2, double.NaN, 4, 5, double.NaN, 7, 8 };
      IEnumerable<double> expected = new double[] { 1, 2, 4, 5, 7, 8 };
      IEnumerable<double> actual;
      actual = input.RemoveNaNs();
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void RemoveNansDoubleTest2()
    {
      IEnumerable<Tuple<string, double>> input = Enumerator.Generate<Tuple<string, double>>(15, X => new Tuple<string, double>("", (double)(X * (X % 3)) / (X % 3)));
      IEnumerable<Tuple<string, double>> expected = input.Where(I => !double.IsNaN(I.Item2));
      IEnumerable<Tuple<string, double>> actual;
      actual = input.RemoveNaNs(X => X.Item2);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void RemoveNansFloatTest1()
    {
      IEnumerable<float> input = new float[] { 1, 2, float.NaN, 4, 5, float.NaN, 7, 8 };
      IEnumerable<float> expected = new float[] { 1, 2, 4, 5, 7, 8 };
      IEnumerable<float> actual;
      actual = input.RemoveNaNs();
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void RemoveNansFloatTest2()
    {
      IEnumerable<Tuple<string, float>> input = Enumerator.Generate<Tuple<string, float>>(15, X => new Tuple<string, float>("", (float)(X * (X % 3)) / (X % 3)));
      IEnumerable<Tuple<string, float>> expected = input.Where(I => !float.IsNaN(I.Item2));
      IEnumerable<Tuple<string, float>> actual;
      actual = input.RemoveNaNs(X => X.Item2);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void RemoveNansNullableDoubleTest1()
    {
      IEnumerable<double?> input = new double?[] { 1, null, double.NaN, 4, null, double.NaN, 7, 8 };
      IEnumerable<double?> expected = new double?[] { 1, null, 4, null, 7, 8 };
      IEnumerable<double?> actual;
      actual = input.RemoveNaNs();
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void RemoveNansNullableDoubleTest2()
    {
      IEnumerable<Tuple<string, double?>> input = Enumerator.Generate<Tuple<string, double?>>(15, X => new Tuple<string, double?>("", X == 2 ? null : (double?)(X * (X % 3)) / (X % 3)));
      input.Skip(5).Take(4).ForEach(E => new Tuple<string, double?>("", null));
      IEnumerable<Tuple<string, double?>> expected = input.Where(I => !double.IsNaN(I.Item2.GetValueOrDefault()));
      IEnumerable<Tuple<string, double?>> actual;
      actual = input.RemoveNaNs(X => X.Item2);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void RemoveNansNullableFloatTest1()
    {
      IEnumerable<float?> input = new float?[] { 1, null, float.NaN, 4, null, float.NaN, 7, 8 };
      IEnumerable<float?> expected = new float?[] { 1, null, 4, null, 7, 8 };
      IEnumerable<float?> actual;
      actual = input.RemoveNaNs();
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void RemoveNansNullableFloatTest2()
    {
      IEnumerable<Tuple<string, float?>> input = Enumerator.Generate<Tuple<string, float?>>(15, X => new Tuple<string, float?>("", X == 2 ? null : (float?)(X * (X % 3)) / (X % 3)));
      IEnumerable<Tuple<string, float?>> expected = input.Where(I => !float.IsNaN(I.Item2.GetValueOrDefault()));
      IEnumerable<Tuple<string, float?>> actual;
      actual = input.RemoveNaNs(X => X.Item2);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void RemoveNullsTest()
    {
      IEnumerable<int?> input = new int?[] { 1, null, 3, 4, null, 6, 7, 8 };
      IEnumerable<int?> expected = new int?[] { 1, 3, 4, 6, 7, 8 };
      IEnumerable<int?> actual;
      actual = input.RemoveNulls();
      Assert.IsTrue(expected.SequenceEqual(actual));


      IEnumerable<int> input1 = new int[] { 1, 3, 4, 6, 7, 8 };
      IEnumerable<int> expected1 = new int[] { 1, 3, 4, 6, 7, 8 };
      IEnumerable<int> actual1;
      actual1 = input1.RemoveNulls();
      Assert.IsTrue(expected1.SequenceEqual(actual1));
    }

    #endregion

    #region Remove Noise

    [TestMethod()]
    public void RemoveNoiseDoubleTest()
    {
      IEnumerable<double> input;
      IEnumerable<double> expected;
      IEnumerable<double> actual;

      input = new double[] { -1, 2, 3, 2, 1, 2, 3, 2, 3, 4, 3, 12, 2, 45, 23, 3, 2, 2, 3, 3, 4, 2, 4, 3, 2, 2 };
      expected = new double[] { -999, 2, 3, 2, 1, 2, 3, 2, 3, 4, 3, -999, 2, -999, -999, 3, 2, 2, 3, 3, 4, 2, 4, 3, 2, 2 };

      actual = input.RemoveNoise(6, 1, NoiseFilterType.AbsoluteValue, -999);
      Assert.IsTrue(expected.SequenceEqual(actual));

      actual = input.RemoveNoise(1, 6, NoiseFilterType.AbsoluteValue, -999);
      Assert.IsTrue(expected.SequenceEqual(actual));

      actual = input.RemoveNoise(.95, NoiseFilterType.PercentOfAverage, -999);
      Assert.IsTrue(expected.SequenceEqual(actual));

      actual = input.RemoveNoise(.25, .95, NoiseFilterType.PercentOfAverage, -999);
      Assert.IsTrue(expected.SequenceEqual(actual));

      expected = expected.Where(E => E != -999);
      actual = input.RemoveNoise(.25, .95, NoiseFilterType.PercentOfAverage, null);
      Assert.IsTrue(expected.SequenceEqual(actual));

      try
      {
        actual = input.RemoveNoise(4, 4, NoiseFilterType.AbsoluteValue, -999);
        Assert.Fail();
      }
      catch (InvalidOperationException) { }
      catch (Exception) { Assert.Fail(); }

      try
      {
        actual = input.RemoveNoise(4, NoiseFilterType.AbsoluteValue, -999);
        Assert.Fail();
      }
      catch (InvalidOperationException) { }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      input = new double[] { 2, 2, 3, 1, 3, 12, 1, 1, 1, 2, 2, 3, 3, -8, 1, 2 };
      expected = new double[] { 2, 2, 3, 1, 3, -999, 1, 1, 1, 2, 2, 3, 3, -999, 1, 2 };

      actual = input.RemoveNoise(1, 1, NoiseFilterType.StandardDeviation, -999);
      Assert.IsTrue(expected.SequenceEqual(actual));

      expected = expected.Where(E => E != -999);
      actual = input.RemoveNoise(1, 1, NoiseFilterType.StandardDeviation, null);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void RemoveNoiseFloatTest()
    {
      IEnumerable<float> input;
      IEnumerable<float> expected;
      IEnumerable<float> actual;

      input = new float[] { -1, 2, 3, 2, 1, 2, 3, 2, 3, 4, 3, 12, 2, 45, 23, 3, 2, 2, 3, 3, 4, 2, 4, 3, 2, 2 };
      expected = new float[] { -999, 2, 3, 2, 1, 2, 3, 2, 3, 4, 3, -999, 2, -999, -999, 3, 2, 2, 3, 3, 4, 2, 4, 3, 2, 2 };

      actual = input.RemoveNoise(6, 1, NoiseFilterType.AbsoluteValue, -999);
      Assert.IsTrue(expected.SequenceEqual(actual));

      actual = input.RemoveNoise(1, 6, NoiseFilterType.AbsoluteValue, -999);
      Assert.IsTrue(expected.SequenceEqual(actual));

      actual = input.RemoveNoise(.95f, NoiseFilterType.PercentOfAverage, -999);
      Assert.IsTrue(expected.SequenceEqual(actual));

      actual = input.RemoveNoise(.25f, .95f, NoiseFilterType.PercentOfAverage, -999);
      Assert.IsTrue(expected.SequenceEqual(actual));

      expected = expected.Where(E => E != -999);
      actual = input.RemoveNoise(.25f, .95f, NoiseFilterType.PercentOfAverage, null);
      Assert.IsTrue(expected.SequenceEqual(actual));

      try
      {
        actual = input.RemoveNoise(4, 4, NoiseFilterType.AbsoluteValue, -999);
        Assert.Fail();
      }
      catch (InvalidOperationException) { }
      catch (Exception) { Assert.Fail(); }

      try
      {
        actual = input.RemoveNoise(4, NoiseFilterType.AbsoluteValue, -999);
        Assert.Fail();
      }
      catch (InvalidOperationException) { }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      input = new float[] { 2, 2, 3, 1, 3, 12, 1, 1, 1, 2, 2, 3, 3, -8, 1, 2 };
      expected = new float[] { 2, 2, 3, 1, 3, -999, 1, 1, 1, 2, 2, 3, 3, -999, 1, 2 };

      actual = input.RemoveNoise(1, 1, NoiseFilterType.StandardDeviation, -999);
      Assert.IsTrue(expected.SequenceEqual(actual));

      expected = expected.Where(E => E != -999);
      actual = input.RemoveNoise(1, 1, NoiseFilterType.StandardDeviation, null);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void RemoveNoiseDecimalTest()
    {
      IEnumerable<decimal> input;
      IEnumerable<decimal> expected;
      IEnumerable<decimal> actual;

      input = new decimal[] { -1, 2, 3, 2, 1, 2, 3, 2, 3, 4, 3, 12, 2, 45, 23, 3, 2, 2, 3, 3, 4, 2, 4, 3, 2, 2 };
      expected = new decimal[] { -999, 2, 3, 2, 1, 2, 3, 2, 3, 4, 3, -999, 2, -999, -999, 3, 2, 2, 3, 3, 4, 2, 4, 3, 2, 2 };

      actual = input.RemoveNoise(6, 1, NoiseFilterType.AbsoluteValue, -999);
      Assert.IsTrue(expected.SequenceEqual(actual));

      actual = input.RemoveNoise(1, 6, NoiseFilterType.AbsoluteValue, -999);
      Assert.IsTrue(expected.SequenceEqual(actual));

      actual = input.RemoveNoise(.95m, NoiseFilterType.PercentOfAverage, -999);
      Assert.IsTrue(expected.SequenceEqual(actual));

      actual = input.RemoveNoise(.25m, .95m, NoiseFilterType.PercentOfAverage, -999);
      Assert.IsTrue(expected.SequenceEqual(actual));

      expected = expected.Where(E => E != -999);
      actual = input.RemoveNoise(.25m, .95m, NoiseFilterType.PercentOfAverage, null);
      Assert.IsTrue(expected.SequenceEqual(actual));

      try
      {
        actual = input.RemoveNoise(4, 4, NoiseFilterType.AbsoluteValue, -999);
        Assert.Fail();
      }
      catch (InvalidOperationException) { }
      catch (Exception) { Assert.Fail(); }

      try
      {
        actual = input.RemoveNoise(4, NoiseFilterType.AbsoluteValue, -999);
        Assert.Fail();
      }
      catch (InvalidOperationException) { }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      input = new decimal[] { 2, 2, 3, 1, 3, 12, 1, 1, 1, 2, 2, 3, 3, -8, 1, 2 };
      expected = new decimal[] { 2, 2, 3, 1, 3, -999, 1, 1, 1, 2, 2, 3, 3, -999, 1, 2 };

      actual = input.RemoveNoise(1, 1, NoiseFilterType.StandardDeviation, -999);
      Assert.IsTrue(expected.SequenceEqual(actual));

      expected = expected.Where(E => E != -999);
      actual = input.RemoveNoise(1, 1, NoiseFilterType.StandardDeviation, null);
      Assert.IsTrue(expected.SequenceEqual(actual));

    }

    [TestMethod()]
    public void RemoveNoiseLongTest()
    {
      IEnumerable<long> input;
      IEnumerable<long> expected;
      IEnumerable<long> actual;

      input = new long[] { -1, 2, 3, 2, 1, 2, 3, 2, 3, 4, 3, 12, 2, 45, 23, 3, 2, 2, 3, 3, 4, 2, 4, 3, 2, 2 };
      expected = new long[] { -999, 2, 3, 2, 1, 2, 3, 2, 3, 4, 3, -999, 2, -999, -999, 3, 2, 2, 3, 3, 4, 2, 4, 3, 2, 2 };

      actual = input.RemoveNoise(6, 1, NoiseFilterType.AbsoluteValue, -999);
      Assert.IsTrue(expected.SequenceEqual(actual));

      actual = input.RemoveNoise(1, 6, NoiseFilterType.AbsoluteValue, -999);
      Assert.IsTrue(expected.SequenceEqual(actual));

      actual = input.RemoveNoise(.95, NoiseFilterType.PercentOfAverage, -999);
      Assert.IsTrue(expected.SequenceEqual(actual));

      actual = input.RemoveNoise(.25, .95, NoiseFilterType.PercentOfAverage, -999);
      Assert.IsTrue(expected.SequenceEqual(actual));

      expected = expected.Where(E => E != -999);
      actual = input.RemoveNoise(.25, .95, NoiseFilterType.PercentOfAverage, null);
      Assert.IsTrue(expected.SequenceEqual(actual));

      try
      {
        actual = input.RemoveNoise(4, 4, NoiseFilterType.AbsoluteValue, -999);
        Assert.Fail();
      }
      catch (InvalidOperationException) { }
      catch (Exception) { Assert.Fail(); }

      try
      {
        actual = input.RemoveNoise(4, NoiseFilterType.AbsoluteValue, -999);
        Assert.Fail();
      }
      catch (InvalidOperationException) { }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      input = new long[] { 2, 2, 3, 1, 3, 12, 1, 1, 1, 2, 2, 3, 3, -8, 1, 2 };
      expected = new long[] { 2, 2, 3, 1, 3, -999, 1, 1, 1, 2, 2, 3, 3, -999, 1, 2 };

      actual = input.RemoveNoise(1, 1, NoiseFilterType.StandardDeviation, -999);
      Assert.IsTrue(expected.SequenceEqual(actual));

      expected = expected.Where(E => E != -999);
      actual = input.RemoveNoise(1, 1, NoiseFilterType.StandardDeviation, null);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void RemoveNoiseIntTest()
    {
      IEnumerable<int> input;
      IEnumerable<int> expected;
      IEnumerable<int> actual;

      input = new int[] { -1, 2, 3, 2, 1, 2, 3, 2, 3, 4, 3, 12, 2, 45, 23, 3, 2, 2, 3, 3, 4, 2, 4, 3, 2, 2 };
      expected = new int[] { -999, 2, 3, 2, 1, 2, 3, 2, 3, 4, 3, -999, 2, -999, -999, 3, 2, 2, 3, 3, 4, 2, 4, 3, 2, 2 };

      actual = input.RemoveNoise(6, 1, NoiseFilterType.AbsoluteValue, -999);
      Assert.IsTrue(expected.SequenceEqual(actual));

      actual = input.RemoveNoise(1, 6, NoiseFilterType.AbsoluteValue, -999);
      Assert.IsTrue(expected.SequenceEqual(actual));

      actual = input.RemoveNoise(.95, NoiseFilterType.PercentOfAverage, -999);
      Assert.IsTrue(expected.SequenceEqual(actual));

      actual = input.RemoveNoise(.25, .95, NoiseFilterType.PercentOfAverage, -999);
      Assert.IsTrue(expected.SequenceEqual(actual));

      expected = expected.Where(E => E != -999);
      actual = input.RemoveNoise(.25, .95, NoiseFilterType.PercentOfAverage, null);
      Assert.IsTrue(expected.SequenceEqual(actual));

      try
      {
        actual = input.RemoveNoise(4, 4, NoiseFilterType.AbsoluteValue, -999);
        Assert.Fail();
      }
      catch (InvalidOperationException) { }
      catch (Exception) { Assert.Fail(); }

      try
      {
        actual = input.RemoveNoise(4, NoiseFilterType.AbsoluteValue, -999);
        Assert.Fail();
      }
      catch (InvalidOperationException) { }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      input = new int[] { 2, 2, 3, 1, 3, 12, 1, 1, 1, 2, 2, 3, 3, -8, 1, 2 };
      expected = new int[] { 2, 2, 3, 1, 3, -999, 1, 1, 1, 2, 2, 3, 3, -999, 1, 2 };

      actual = input.RemoveNoise(1, 1, NoiseFilterType.StandardDeviation, -999);
      Assert.IsTrue(expected.SequenceEqual(actual));

      expected = expected.Where(E => E != -999);
      actual = input.RemoveNoise(1, 1, NoiseFilterType.StandardDeviation, null);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Filter / If

    [TestMethod()]
    public void FilterTest()
    {
      IEnumerable<string> input;
      IEnumerable<string> expected;
      IEnumerable<string> actual;

      input = new string[] { "A", "B", "A", "C", "D", "E", "E", "F", "A", "B", "C", "B", "E", "F", "A", "A", "B", "A" };
      expected = new string[] { "A", "A", "A", "A", "A", "A" };

      actual = input.Filter(input.Select(x => x == "A"));
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      expected = new string[] { "B", "C", "D", "E", "E", "F", "B", "C", "B", "E", "F", "B" };

      actual = input.Filter(x => x != "A");
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      expected = new string[] { "A", "b", "A", "c", "d", "e", "e", "f", "A", "b", "c", "b", "e", "f", "A", "A", "b", "A" };

      actual = input.Filter(x => x == "A", x=> x.ToLower());
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      expected = new string[] { "A", "?", "A", "?", "?", "?", "?", "?", "A", "?", "?", "?", "?", "?", "A", "A", "?", "A" };

      actual = input.Filter(x => x == "A", "?");
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      expected = new string[] { "A", "?", "A", "?", "?", "?", "?", "?", "A", "?", "?", "?", "?", "?", "A", "A", "?", "A" };

      actual = input.Filter(input.Select(x => x == "A"), "?");
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      expected = new string[] { "A", "b", "A", "c", "d", "e", "e", "f", "A", "b", "c", "b", "e", "f", "A", "A", "b", "A" };

      actual = input.Filter(input.Select(x => x == "A"), x=>x.ToLower());
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void IfTest()
    {
      IEnumerable<string> input;
      IEnumerable<string> expected;
      IEnumerable<string> actual;

      input = new string[] { "A", "B", "A", "C", "D", "E", "E", "F", "A", "B", "C", "B", "E", "F", "A", "A", "B", "A" };
      expected = new string[] { "a", "b", "a", "C-C", "D-D", "E-E", "E-E", "F-F", "a", "b", "C-C", "b", "E-E", "F-F", "a", "a", "b", "a" };

      actual = input.If(x => x == "A" || x == "B", x => x.ToLower(), x => x + "-" + x);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      expected = new string[] { "a", "b", "a", "a", "b", "b", "a", "a", "b", "a" };

      actual = input.If(x => x == "A" || x == "B", x => x.ToLower());
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion
  }
}
