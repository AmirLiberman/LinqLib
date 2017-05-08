using System;
using System.Collections.Generic;
using System.Linq;
using LinqLib.Operators;
using LinqLib.Sequence;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinqLibTests.Operators
{
  [TestClass()]
  public class StdevBlockTests
  {
    #region Standard Deviation (Block Parameter Overload)

    #region Double

    [TestMethod()]
    public void StdevDoubleTest()
    {
      IEnumerable<double> source;
      IEnumerable<double> expected;
      IEnumerable<double> actual;
      int blockSize;

      source = new double[] { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384 };
      expected = new double[] { 3.096, 6.191, 12.383, 24.766, 49.531, 99.062, 198.125, 396.249, 792.498, 1584.996, 3169.993, 6339.985 };
      blockSize = 4;

      actual = source.Stdev(blockSize).Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new double[] { 9 };
        blockSize = 4;
        int x = source.Stdev(blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "source"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      source = new double[] { 1, 2, 4, 8, 16, 32, 64, 128, 256, double.NaN, 1024, 2048, 4096, 8192, 16384 };
      blockSize = 4;
      expected = new double[] { 3.096, 6.191, 12.383, 24.766, 49.531, 99.062, double.NaN, double.NaN, double.NaN, double.NaN, 3169.993, 6339.985 };

      actual = source.Stdev(blockSize).Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new double[] { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384 };
        blockSize = 1;
        int x = source.Stdev(blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "blockSize"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      source = new double[] { 1, double.NaN, 4, 8, 16, 32 };
      blockSize = 4;
      expected = new double[] { double.NaN, double.NaN, 12.383 };

      actual = source.Stdev(blockSize).Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new double[] { 1, double.NaN };
      blockSize = 4;
      expected = new double[] { double.NaN };

      actual = source.Stdev(blockSize).Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new double[] { 1, 2 };
      blockSize = 4;
      expected = new double[] { 0.707 };

      actual = source.Stdev(blockSize).Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void StdevNullableDoubleTest()
    {
      IEnumerable<double?> source;
      IEnumerable<double?> expected;
      IEnumerable<double?> actual;
      int blockSize;

      source = new double?[] { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384 };
      expected = new double?[] { 3.096, 6.191, 12.383, 24.766, 49.531, 99.062, 198.125, 396.249, 792.498, 1584.996, 3169.993, 6339.985 };
      blockSize = 4;

      actual = source.Stdev(blockSize).Select(V => V.Value).Round(3).Cast<double?>();
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new double?[] { 9 };
        blockSize = 4;
        int x = source.Stdev(blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "source"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      source = new double?[] { 1, 2, 4, 8, 16, 32, 64, 128, 256, double.NaN, 1024, 2048, 4096, 8192, 16384 };
      blockSize = 4;
      expected = new double?[] { 3.096, 6.191, 12.383, 24.766, 49.531, 99.062, double.NaN, double.NaN, double.NaN, double.NaN, 3169.993, 6339.985 };

      actual = source.Stdev(blockSize).Select(V => V.Value).Round(3).Cast<double?>();
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new double?[] { 1, 2, 4, 8, 16, 32, 64, 128, null, 512, 1024, 2048, 4096, 8192, 16384 };
      blockSize = 4;
      expected = new double?[] { 3.096, 6.191, 12.383, 24.766, 49.531, 48.881, 242.300, 449.521, 782.093, 1584.996, 3169.993, 6339.985 };

      actual = source.Stdev(blockSize).Select(V => V.Value).Round(3).Cast<double?>();
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new double?[] { 1, 2, 4, 8, 16, 32, double.NaN, 128, null, 512, 1024, 2048, 4096, 8192, 16384 };
      blockSize = 4;
      expected = new double?[] { 3.096, 6.191, 12.383, double.NaN, double.NaN, double.NaN, double.NaN, 449.521, 782.093, 1584.996, 3169.993, 6339.985 };

      actual = source.Stdev(blockSize).Select(V => V.Value).Round(3).Cast<double?>();
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new double?[] { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384 };
        blockSize = 1;
        int x = source.Stdev(blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "blockSize"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      source = new double?[] { 1, double.NaN, 4, 8, 16, 32 };
      blockSize = 4;
      expected = new double?[] { double.NaN, double.NaN, 12.383 };

      actual = source.Stdev(blockSize).Select(V => V.Value).Round(3).Cast<double?>();
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new double?[] { 1, double.NaN };
      blockSize = 4;
      expected = new double?[] { double.NaN };

      actual = source.Stdev(blockSize).Select(V => V.Value).Round(3).Cast<double?>();
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new double?[] { 1, 2 };
      blockSize = 4;
      expected = new double?[] { 0.707 };

      actual = source.Stdev(blockSize).Select(V => V.Value).Round(3).Cast<double?>();
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new double?[] { 1, null, null, null, null, null };
      blockSize = 4;
      expected = new double?[] { null, null, null };

      actual = source.Stdev(blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Float

    [TestMethod()]
    public void StdevFloatTest()
    {
      IEnumerable<float> source;
      IEnumerable<float> expected;
      IEnumerable<float> actual;
      int blockSize;

      source = new float[] { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384 };
      expected = new float[] { 3.096f, 6.191f, 12.383f, 24.766f, 49.531f, 99.062f, 198.125f, 396.249f, 792.498f, 1584.996f, 3169.993f, 6339.985f };
      blockSize = 4;

      actual = source.Stdev(blockSize).Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new float[] { 9 };
        blockSize = 4;
        int x = source.Stdev(blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "source"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//     

      source = new float[] { 1, 2, 4, 8, 16, 32, 64, 128, 256, float.NaN, 1024, 2048, 4096, 8192, 16384 };
      blockSize = 4;
      expected = new float[] { 3.096f, 6.191f, 12.383f, 24.766f, 49.531f, 99.062f, float.NaN, float.NaN, float.NaN, float.NaN, 3169.993f, 6339.985f };

      actual = source.Stdev(blockSize).Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new float[] { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384 };
        blockSize = 1;
        int x = source.Stdev(blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "blockSize"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      source = new float[] { 1, float.NaN, 4, 8, 16, 32 };
      blockSize = 4;
      expected = new float[] { float.NaN, float.NaN, 12.383f };

      actual = source.Stdev(blockSize).Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new float[] { 1, float.NaN };
      blockSize = 4;
      expected = new float[] { float.NaN };

      actual = source.Stdev(blockSize).Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new float[] { 1, 2 };
      blockSize = 4;
      expected = new float[] { 0.707f };

      actual = source.Stdev(blockSize).Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void StdevNullableFloatTest()
    {
      IEnumerable<float?> source;
      IEnumerable<float?> expected;
      IEnumerable<float?> actual;
      int blockSize;

      source = new float?[] { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384 };
      expected = new float?[] { 3.096f, 6.191f, 12.383f, 24.766f, 49.531f, 99.062f, 198.125f, 396.249f, 792.498f, 1584.996f, 3169.993f, 6339.985f };
      blockSize = 4;

      actual = source.Stdev(blockSize).Select(V => V.Value).Round(3).Cast<float?>();
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new float?[] { 9 };
        blockSize = 4;
        int x = source.Stdev(blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "source"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      source = new float?[] { 1, 2, 4, 8, 16, 32, 64, 128, 256, float.NaN, 1024, 2048, 4096, 8192, 16384 };
      blockSize = 4;
      expected = new float?[] { 3.096f, 6.191f, 12.383f, 24.766f, 49.531f, 99.062f, float.NaN, float.NaN, float.NaN, float.NaN, 3169.993f, 6339.985f };

      actual = source.Stdev(blockSize).Select(V => V.Value).Round(3).Cast<float?>();
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new float?[] { 1, 2, 4, 8, 16, 32, 64, 128, null, 512, 1024, 2048, 4096, 8192, 16384 };
      blockSize = 4;
      expected = new float?[] { 3.096f, 6.191f, 12.383f, 24.766f, 49.531f, 48.881f, 242.300f, 449.521f, 782.093f, 1584.996f, 3169.993f, 6339.985f };

      actual = source.Stdev(blockSize).Select(V => V.Value).Round(3).Cast<float?>();
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new float?[] { 1, 2, 4, 8, 16, 32, float.NaN, 128, null, 512, 1024, 2048, 4096, 8192, 16384 };
      blockSize = 4;
      expected = new float?[] { 3.096f, 6.191f, 12.383f, float.NaN, float.NaN, float.NaN, float.NaN, 449.521f, 782.093f, 1584.996f, 3169.993f, 6339.985f };

      actual = source.Stdev(blockSize).Select(V => V.Value).Round(3).Cast<float?>();
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new float?[] { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384 };
        blockSize = 1;
        int x = source.Stdev(blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "blockSize"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      source = new float?[] { 1, float.NaN, 4, 8, 16, 32 };
      blockSize = 4;
      expected = new float?[] { float.NaN, float.NaN, 12.383f };

      actual = source.Stdev(blockSize).Select(V => V.Value).Round(3).Cast<float?>();
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new float?[] { 1, float.NaN };

      expected = new float?[] { float.NaN };

      actual = source.Stdev(blockSize).Select(V => V.Value).Round(3).Cast<float?>();
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new float?[] { 1, 2 };
      blockSize = 4;
      expected = new float?[] { 0.707f };

      actual = source.Stdev(blockSize).Select(V => V.Value).Round(3).Cast<float?>();
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new float?[] { 1, null, null, null, null, null };
      blockSize = 4;
      expected = new float?[] { null, null, null };

      actual = source.Stdev(blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Decimal

    [TestMethod()]
    public void StdevDecimalTest()
    {
      IEnumerable<decimal> source;
      IEnumerable<decimal> expected;
      IEnumerable<decimal> actual;
      int blockSize;

      source = new decimal[] { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384 };
      expected = new decimal[] { 3.096m, 6.191m, 12.383m, 24.766m, 49.531m, 99.062m, 198.125m, 396.249m, 792.498m, 1584.996m, 3169.993m, 6339.985m };
      blockSize = 4;

      actual = source.Stdev(blockSize).Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new decimal[] { 9 };
        blockSize = 4;
        int x = source.Stdev(blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "source"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      try
      {
        source = new decimal[] { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384 };
        blockSize = 1;
        int x = source.Stdev(blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "blockSize"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      source = new decimal[] { 1, 2 };
      blockSize = 4;
      expected = new decimal[] { 0.707m };

      actual = source.Stdev(blockSize).Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void StdevNullableDecimalTest()
    {
      IEnumerable<decimal?> source;
      IEnumerable<decimal?> expected;
      IEnumerable<decimal?> actual;
      int blockSize;

      source = new decimal?[] { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384 };
      expected = new decimal?[] { 3.096m, 6.191m, 12.383m, 24.766m, 49.531m, 99.062m, 198.125m, 396.249m, 792.498m, 1584.996m, 3169.993m, 6339.985m };
      blockSize = 4;

      actual = source.Stdev(blockSize).Select(V => V.Value).Round(3).Cast<decimal?>();
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new decimal?[] { 9 };
        blockSize = 4;
        int x = source.Stdev(blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "source"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      source = new decimal?[] { 1, 2, 4, 8, 16, 32, 64, 128, null, 512, 1024, 2048, 4096, 8192, 16384 };
      blockSize = 4;
      expected = new decimal?[] { 3.096m, 6.191m, 12.383m, 24.766m, 49.531m, 48.881m, 242.300m, 449.521m, 782.093m, 1584.996m, 3169.993m, 6339.985m };

      actual = source.Stdev(blockSize).Select(V => V.Value).Round(3).Cast<decimal?>();
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new decimal?[] { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384 };
        blockSize = 1;
        int x = source.Stdev(blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "blockSize"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      source = new decimal?[] { 1, 2 };
      blockSize = 4;
      expected = new decimal?[] { 0.707m };

      actual = source.Stdev(blockSize).Select(V => V.Value).Round(3).Cast<decimal?>();
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new decimal?[] { 1, null, null, null, null, null };
      blockSize = 4;
      expected = new decimal?[] { null, null, null };

      actual = source.Stdev(blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Long

    [TestMethod()]
    public void StdevLongTest()
    {
      IEnumerable<long> source;
      IEnumerable<double> expected;
      IEnumerable<double> actual;
      int blockSize;

      source = new long[] { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384 };
      expected = new double[] { 3.096, 6.191, 12.383, 24.766, 49.531, 99.062, 198.125, 396.249, 792.498, 1584.996, 3169.993, 6339.985 };
      blockSize = 4;

      actual = source.Stdev(blockSize).Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new long[] { 9 };
        blockSize = 4;
        int x = source.Stdev(blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "source"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      try
      {
        source = new long[] { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384 };
        blockSize = 1;
        int x = source.Stdev(blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "blockSize"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      source = new long[] { 1, 2 };
      blockSize = 4;
      expected = new double[] { 0.707 };

      actual = source.Stdev(blockSize).Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void StdevNullableLongTest()
    {
      IEnumerable<long?> source;
      IEnumerable<double?> expected;
      IEnumerable<double?> actual;
      int blockSize;

      source = new long?[] { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384 };
      expected = new double?[] { 3.096, 6.191, 12.383, 24.766, 49.531, 99.062, 198.125, 396.249, 792.498, 1584.996, 3169.993, 6339.985 };
      blockSize = 4;

      actual = source.Stdev(blockSize).Select(V => V.Value).Round(3).Cast<double?>();
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new long?[] { 9 };
        blockSize = 4;
        int x = source.Stdev(blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "source"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      source = new long?[] { 1, 2, 4, 8, 16, 32, 64, 128, null, 512, 1024, 2048, 4096, 8192, 16384 };
      blockSize = 4;
      expected = new double?[] { 3.096, 6.191, 12.383, 24.766, 49.531, 48.881, 242.300, 449.521, 782.093, 1584.996, 3169.993, 6339.985 };

      actual = source.Stdev(blockSize).Select(V => V.Value).Round(3).Cast<double?>();
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new long?[] { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384 };
        blockSize = 1;
        int x = source.Stdev(blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "blockSize"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      source = new long?[] { 1, 2 };
      blockSize = 4;
      expected = new double?[] { 0.707 };

      actual = source.Stdev(blockSize).Select(V => V.Value).Round(3).Cast<double?>();
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new long?[] { 1, null, null, null, null, null };
      blockSize = 4;
      expected = new double?[] { null, null, null };

      actual = source.Stdev(blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region int

    [TestMethod()]
    public void StdevIntTest()
    {
      IEnumerable<int> source;
      IEnumerable<double> expected;
      IEnumerable<double> actual;
      int blockSize;

      source = new int[] { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384 };
      expected = new double[] { 3.096, 6.191, 12.383, 24.766, 49.531, 99.062, 198.125, 396.249, 792.498, 1584.996, 3169.993, 6339.985 };
      blockSize = 4;

      actual = source.Stdev(blockSize).Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new int[] { 9 };
        blockSize = 4;
        int x = source.Stdev(blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "source"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      try
      {
        source = new int[] { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384 };
        blockSize = 1;
        int x = source.Stdev(blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "blockSize"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      source = new int[] { 1, 2 };
      blockSize = 4;
      expected = new double[] { 0.707 };

      actual = source.Stdev(blockSize).Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void StdevNullableIntTest()
    {
      IEnumerable<int?> source;
      IEnumerable<double?> expected;
      IEnumerable<double?> actual;
      int blockSize;

      source = new int?[] { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384 };
      expected = new double?[] { 3.096, 6.191, 12.383, 24.766, 49.531, 99.062, 198.125, 396.249, 792.498, 1584.996, 3169.993, 6339.985 };
      blockSize = 4;

      actual = source.Stdev(blockSize).Select(V => V.Value).Round(3).Cast<double?>();
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new int?[] { 9 };
        blockSize = 4;
        int x = source.Stdev(blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "source"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      source = new int?[] { 1, 2, 4, 8, 16, 32, 64, 128, null, 512, 1024, 2048, 4096, 8192, 16384 };
      blockSize = 4;
      expected = new double?[] { 3.096, 6.191, 12.383, 24.766, 49.531, 48.881, 242.300, 449.521, 782.093, 1584.996, 3169.993, 6339.985 };

      actual = source.Stdev(blockSize).Select(V => V.Value).Round(3).Cast<double?>();
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        source = new int?[] { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384 };
        blockSize = 1;
        int x = source.Stdev(blockSize).Count();
        Assert.Fail();
      }
      catch (ArgumentException e) { Assert.IsTrue(e.ParamName == "blockSize"); }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      source = new int?[] { 1, 2 };
      blockSize = 4;
      expected = new double?[] { 0.707 };

      actual = source.Stdev(blockSize).Select(V => V.Value).Round(3).Cast<double?>();
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      source = new int?[] { 1, null, null, null, null, null };
      blockSize = 4;
      expected = new double?[] { null, null, null };

      actual = source.Stdev(blockSize);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #endregion

    #region Standard Deviation (Block and Selector Parameter Overload)

    #region Double

    [TestMethod()]
    public void StdevDoubleTest1()
    {
      IEnumerable<Tuple<string, double>> source = Enumerator.Generate<Tuple<string, double>>(7, X => Generator.GenerateDouble((int)Math.Pow(2, X - 1), 10));
      IEnumerable<double> expected = new double[] { 3.096, 6.191, 12.383, 24.766 };
      IEnumerable<double> actual;

      actual = source.Stdev(4, X => X.Item2).Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void StdevNullableDoubleTest1()
    {
      IEnumerable<Tuple<string, double?>> source = Enumerator.Generate<Tuple<string, double?>>(7, X => Generator.GenerateNullableDouble((int)Math.Pow(2, X - 1), 10, 2));
      IEnumerable<double?> expected = new double?[] { 3.512, 6.110, 12.383, 24.766 };
      IEnumerable<double?> actual;

      actual = source.Stdev(4, X => X.Item2).Select(V => V.Value).Round(3).Cast<double?>();
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Float

    [TestMethod()]
    public void StdevFloatTest1()
    {
      IEnumerable<Tuple<string, float>> source = Enumerator.Generate<Tuple<string, float>>(7, X => Generator.GenerateFloat((int)Math.Pow(2, X - 1), 10));
      IEnumerable<float> expected = new float[] { 3.096f, 6.191f, 12.383f, 24.766f };
      IEnumerable<float> actual;

      actual = source.Stdev(4, X => X.Item2).Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void StdevNullableFloatTest1()
    {
      IEnumerable<Tuple<string, float?>> source = Enumerator.Generate<Tuple<string, float?>>(7, X => Generator.GenerateNullableFloat((int)Math.Pow(2, X - 1), 10, 2));
      IEnumerable<float?> expected = new float?[] { 3.512f, 6.110f, 12.383f, 24.766f };
      IEnumerable<float?> actual;

      actual = source.Stdev(4, X => X.Item2).Select(V => V.Value).Round(3).Cast<float?>();
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Decimal

    [TestMethod()]
    public void StdevDecimalTest1()
    {
      IEnumerable<Tuple<string, decimal>> source = Enumerator.Generate<Tuple<string, decimal>>(7, X => Generator.GenerateDecimal((int)Math.Pow(2, X - 1)));
      IEnumerable<decimal> expected = new decimal[] { 3.096m, 6.191m, 12.383m, 24.766m };
      IEnumerable<decimal> actual;

      actual = source.Stdev(4, X => X.Item2).Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void StdevNullableDecimalTest1()
    {
      IEnumerable<Tuple<string, decimal?>> source = Enumerator.Generate<Tuple<string, decimal?>>(7, X => Generator.GenerateNullableDecimal((int)Math.Pow(2, X - 1), 2));
      IEnumerable<decimal?> expected = new decimal?[] { 3.512m, 6.110m, 12.383m, 24.766m };
      IEnumerable<decimal?> actual;

      actual = source.Stdev(4, X => X.Item2).Select(V => V.Value).Round(3).Cast<decimal?>();
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Long

    [TestMethod()]
    public void StdevLongTest1()
    {
      IEnumerable<Tuple<string, long>> source = Enumerator.Generate<Tuple<string, long>>(7, X => Generator.GenerateLong((int)Math.Pow(2, X - 1)));
      IEnumerable<double> expected = new double[] { 3.096, 6.191, 12.383, 24.766 };
      IEnumerable<double> actual;

      actual = source.Stdev(4, X => X.Item2).Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void StdevNullableLongTest1()
    {
      IEnumerable<Tuple<string, long?>> source = Enumerator.Generate<Tuple<string, long?>>(7, X => Generator.GenerateNullableLong((int)Math.Pow(2, X - 1), 2));
      IEnumerable<double?> expected = new double?[] { 3.512, 6.110, 12.383, 24.766 };
      IEnumerable<double?> actual;

      actual = source.Stdev(4, X => X.Item2).Select(V => V.Value).Round(3).Cast<double?>();
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Int

    [TestMethod()]
    public void StdevIntTest1()
    {
      IEnumerable<Tuple<string, int>> source = Enumerator.Generate<Tuple<string, int>>(7, X => Generator.GenerateInt((int)Math.Pow(2, X - 1)));
      IEnumerable<double> expected = new double[] { 3.096, 6.191, 12.383, 24.766 };
      IEnumerable<double> actual;

      actual = source.Stdev(4, X => X.Item2).Round(3);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void StdevNullableIntTest1()
    {
      IEnumerable<Tuple<string, int?>> source = Enumerator.Generate<Tuple<string, int?>>(7, X => Generator.GenerateNullableInt((int)Math.Pow(2, X - 1), 2));
      IEnumerable<double?> expected = new double?[] { 3.512, 6.110, 12.383, 24.766 };
      IEnumerable<double?> actual;

      actual = source.Stdev(4, X => X.Item2).Select(V => V.Value).Round(3).Cast<double?>();
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #endregion
  }
}