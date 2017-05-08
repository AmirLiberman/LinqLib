using System;
using System.Collections.Generic;
using System.Linq;
using LinqLib.Operators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinqLibTests.Operators
{
  [TestClass()]
  public class ArithmeticTests
  {
    #region Add

    #region Double

    [TestMethod()]
    public void AddDoubleTest()
    {
      IEnumerable<double> leftSequence;
      IEnumerable<double> rightSequence;
      IEnumerable<double> expected;
      IEnumerable<double> actual;

      leftSequence = new double[] { 1, 2, 3, 4, 5 };
      rightSequence = new double[] { 1, 2, 3, 4, 5 };
      expected = new double[] { 2, 4, 6, 8, 10 };

      actual = Arithmetic.Add(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      leftSequence = new double[] { 1, 2, 3, double.NaN, double.PositiveInfinity };
      rightSequence = new double[] { 1, 2, 3, 4, 5 };
      expected = new double[] { 2, 4, 6, double.NaN, double.PositiveInfinity };

      actual = Arithmetic.Add(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void AddNullableDoubleTest()
    {
      IEnumerable<double?> leftSequence;
      IEnumerable<double?> rightSequence;
      IEnumerable<double?> expected;
      IEnumerable<double?> actual;

      leftSequence = new double?[] { 1, 2, 3, 4, 5 };
      rightSequence = new double?[] { 1, 2, 3, 4, 5 };
      expected = new double?[] { 2, 4, 6, 8, 10 };

      actual = Arithmetic.Add(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      leftSequence = new double?[] { 1, 2, 3, double.NaN, double.PositiveInfinity };
      rightSequence = new double?[] { 1, 2, 3, 4, 5 };
      expected = new double?[] { 2, 4, 6, double.NaN, double.PositiveInfinity };

      actual = Arithmetic.Add(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      leftSequence = new double?[] { 1, null, 3, double.NaN, double.PositiveInfinity };
      rightSequence = new double?[] { 1, 2, 3, 4, 5 };
      expected = new double?[] { 2, null, 6, double.NaN, double.PositiveInfinity };

      actual = Arithmetic.Add(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Float

    [TestMethod()]
    public void AddFloatTest()
    {
      IEnumerable<float> leftSequence;
      IEnumerable<float> rightSequence;
      IEnumerable<float> expected;
      IEnumerable<float> actual;

      leftSequence = new float[] { 1, 2, 3, 4, 5 };
      rightSequence = new float[] { 1, 2, 3, 4, 5 };
      expected = new float[] { 2, 4, 6, 8, 10 };

      actual = Arithmetic.Add(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      leftSequence = new float[] { 1, 2, 3, float.NaN, float.PositiveInfinity };
      rightSequence = new float[] { 1, 2, 3, 4, 5 };
      expected = new float[] { 2, 4, 6, float.NaN, float.PositiveInfinity };

      actual = Arithmetic.Add(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void AddNullableFloatTest()
    {
      IEnumerable<float?> leftSequence;
      IEnumerable<float?> rightSequence;
      IEnumerable<float?> expected;
      IEnumerable<float?> actual;

      leftSequence = new float?[] { 1, 2, 3, 4, 5 };
      rightSequence = new float?[] { 1, 2, 3, 4, 5 };
      expected = new float?[] { 2, 4, 6, 8, 10 };

      actual = Arithmetic.Add(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      leftSequence = new float?[] { 1, 2, 3, float.NaN, float.PositiveInfinity };
      rightSequence = new float?[] { 1, 2, 3, 4, 5 };
      expected = new float?[] { 2, 4, 6, float.NaN, float.PositiveInfinity };

      actual = Arithmetic.Add(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      leftSequence = new float?[] { 1, null, 3, float.NaN, float.PositiveInfinity };
      rightSequence = new float?[] { 1, 2, 3, 4, 5 };
      expected = new float?[] { 2, null, 6, float.NaN, float.PositiveInfinity };

      actual = Arithmetic.Add(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Decimal

    [TestMethod()]
    public void AddDecimalTest()
    {
      IEnumerable<decimal> leftSequence;
      IEnumerable<decimal> rightSequence;
      IEnumerable<decimal> expected;
      IEnumerable<decimal> actual;

      leftSequence = new decimal[] { 1, 2, 3, 4, 5 };
      rightSequence = new decimal[] { 1, 2, 3, 4, 5 };
      expected = new decimal[] { 2, 4, 6, 8, 10 };

      actual = Arithmetic.Add(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void AddNullableDecimalTest()
    {
      IEnumerable<decimal?> leftSequence;
      IEnumerable<decimal?> rightSequence;
      IEnumerable<decimal?> expected;
      IEnumerable<decimal?> actual;

      leftSequence = new decimal?[] { 1, 2, 3, 4, 5 };
      rightSequence = new decimal?[] { 1, 2, 3, 4, 5 };
      expected = new decimal?[] { 2, 4, 6, 8, 10 };

      actual = Arithmetic.Add(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      leftSequence = new decimal?[] { 1, null, 3, 4, 5 };
      rightSequence = new decimal?[] { 1, 2, 3, null, 5 };
      expected = new decimal?[] { 2, null, 6, null, 10 };

      actual = Arithmetic.Add(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Long

    [TestMethod()]
    public void AddLongTest()
    {
      IEnumerable<long> leftSequence;
      IEnumerable<long> rightSequence;
      IEnumerable<long> expected;
      IEnumerable<long> actual;

      leftSequence = new long[] { 1, 2, 3, 4, 5 };
      rightSequence = new long[] { 1, 2, 3, 4, 5 };
      expected = new long[] { 2, 4, 6, 8, 10 };

      actual = Arithmetic.Add(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void AddNullableLongTest()
    {
      IEnumerable<long?> leftSequence;
      IEnumerable<long?> rightSequence;
      IEnumerable<long?> expected;
      IEnumerable<long?> actual;

      leftSequence = new long?[] { 1, 2, 3, 4, 5 };
      rightSequence = new long?[] { 1, 2, 3, 4, 5 };
      expected = new long?[] { 2, 4, 6, 8, 10 };

      actual = Arithmetic.Add(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      leftSequence = new long?[] { 1, null, 3, 4, 5 };
      rightSequence = new long?[] { 1, 2, 3, null, 5 };
      expected = new long?[] { 2, null, 6, null, 10 };

      actual = Arithmetic.Add(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Int

    [TestMethod()]
    public void AddIntTest()
    {
      IEnumerable<int> leftSequence;
      IEnumerable<int> rightSequence;
      IEnumerable<int> expected;
      IEnumerable<int> actual;

      leftSequence = new int[] { 1, 2, 3, 4, 5 };
      rightSequence = new int[] { 1, 2, 3, 4, 5 };
      expected = new int[] { 2, 4, 6, 8, 10 };

      actual = Arithmetic.Add(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void AddNullableIntTest()
    {
      IEnumerable<int?> leftSequence;
      IEnumerable<int?> rightSequence;
      IEnumerable<int?> expected;
      IEnumerable<int?> actual;

      leftSequence = new int?[] { 1, 2, 3, 4, 5 };
      rightSequence = new int?[] { 1, 2, 3, 4, 5 };
      expected = new int?[] { 2, 4, 6, 8, 10 };

      actual = Arithmetic.Add(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      leftSequence = new int?[] { 1, null, 3, 4, 5 };
      rightSequence = new int?[] { 1, 2, 3, null, 5 };
      expected = new int?[] { 2, null, 6, null, 10 };

      actual = Arithmetic.Add(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    [TestMethod()]
    public void AddErrTest()
    {
      try
      {
        IEnumerable<bool> leftSequence;
        IEnumerable<bool> rightSequence;

        leftSequence = new bool[] { true, true, true, true, true };
        rightSequence = new bool[] { true, true, true, true, true };

        bool[] res = leftSequence.Add(rightSequence).ToArray();
        Assert.Fail();
      }
      catch (InvalidOperationException) { }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//

      try
      {
        IEnumerable<object> leftSequence;
        IEnumerable<object> rightSequence;

        leftSequence = new object[] { 'a', 1, null };
        rightSequence = new object[] { 'a', 1, null };

        object[] res = leftSequence.Add(rightSequence).ToArray();
        Assert.Fail();
      }
      catch (InvalidOperationException) { }
      catch (Exception) { Assert.Fail(); }
    }

    #endregion

    #region Subtract

    #region Double

    [TestMethod()]
    public void SubtractDoubleTest()
    {
      IEnumerable<double> leftSequence;
      IEnumerable<double> rightSequence;
      IEnumerable<double> expected;
      IEnumerable<double> actual;

      leftSequence = new double[] { 1, 2, 3, 4, 5 };
      rightSequence = new double[] { 1, 2, 3, 4, 5 };
      expected = new double[] { 0, 0, 0, 0, 0 };

      actual = Arithmetic.Subtract(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      leftSequence = new double[] { 2, 4, 6, double.NaN, double.PositiveInfinity };
      rightSequence = new double[] { 1, 2, 3, 4, 5 };
      expected = new double[] { 1, 2, 3, double.NaN, double.PositiveInfinity };

      actual = Arithmetic.Subtract(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void SubtractNullableDoubleTest()
    {
      IEnumerable<double?> leftSequence;
      IEnumerable<double?> rightSequence;
      IEnumerable<double?> expected;
      IEnumerable<double?> actual;

      leftSequence = new double?[] { 2, 4, 6, 8, 10 };
      rightSequence = new double?[] { 1, 2, 3, 4, 5 };
      expected = new double?[] { 1, 2, 3, 4, 5 };

      actual = Arithmetic.Subtract(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      leftSequence = new double?[] { 2, 4, 6, double.NaN, double.PositiveInfinity };
      rightSequence = new double?[] { 1, 2, 3, 4, 5 };
      expected = new double?[] { 1, 2, 3, double.NaN, double.PositiveInfinity };

      actual = Arithmetic.Subtract(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      leftSequence = new double?[] { 2, 4, 6, double.NaN, double.PositiveInfinity };
      rightSequence = new double?[] { 1, null, 3, 4, 5 };
      expected = new double?[] { 1, null, 3, double.NaN, double.PositiveInfinity };

      actual = Arithmetic.Subtract(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Float

    [TestMethod()]
    public void SubtractFloatTest()
    {
      IEnumerable<float> leftSequence;
      IEnumerable<float> rightSequence;
      IEnumerable<float> expected;
      IEnumerable<float> actual;

      leftSequence = new float[] { 1, 2, 3, 4, 5 };
      rightSequence = new float[] { 1, 2, 3, 4, 5 };
      expected = new float[] { 0, 0, 0, 0, 0 };

      actual = Arithmetic.Subtract(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      leftSequence = new float[] { 2, 4, 6, float.NaN, float.PositiveInfinity };
      rightSequence = new float[] { 1, 2, 3, 4, 5 };
      expected = new float[] { 1, 2, 3, float.NaN, float.PositiveInfinity };

      actual = Arithmetic.Subtract(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void SubtractNullableFloatTest()
    {
      IEnumerable<float?> leftSequence;
      IEnumerable<float?> rightSequence;
      IEnumerable<float?> expected;
      IEnumerable<float?> actual;

      leftSequence = new float?[] { 2, 4, 6, 8, 10 };
      rightSequence = new float?[] { 1, 2, 3, 4, 5 };
      expected = new float?[] { 1, 2, 3, 4, 5 };

      actual = Arithmetic.Subtract(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      leftSequence = new float?[] { 2, 4, 6, float.NaN, float.PositiveInfinity };
      rightSequence = new float?[] { 1, 2, 3, 4, 5 };
      expected = new float?[] { 1, 2, 3, float.NaN, float.PositiveInfinity };

      actual = Arithmetic.Subtract(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      leftSequence = new float?[] { 2, 4, 6, float.NaN, float.PositiveInfinity };
      rightSequence = new float?[] { 1, null, 3, 4, 5 };
      expected = new float?[] { 1, null, 3, float.NaN, float.PositiveInfinity };

      actual = Arithmetic.Subtract(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Decimal

    [TestMethod()]
    public void SubtractDecimalTest()
    {
      IEnumerable<decimal> leftSequence;
      IEnumerable<decimal> rightSequence;
      IEnumerable<decimal> expected;
      IEnumerable<decimal> actual;

      leftSequence = new decimal[] { 2, 4, 6, 8, 10 };
      rightSequence = new decimal[] { 1, 2, 3, 4, 5 };
      expected = new decimal[] { 1, 2, 3, 4, 5 };

      actual = Arithmetic.Subtract(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void SubtractNullableDecimalTest()
    {
      IEnumerable<decimal?> leftSequence;
      IEnumerable<decimal?> rightSequence;
      IEnumerable<decimal?> expected;
      IEnumerable<decimal?> actual;

      leftSequence = new decimal?[] { 2, 4, 6, 8, 10 };
      rightSequence = new decimal?[] { 1, 2, 3, 4, 5 };
      expected = new decimal?[] { 1, 2, 3, 4, 5 };

      actual = Arithmetic.Subtract(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      leftSequence = new decimal?[] { 2, null, 6, null, 10 };
      rightSequence = new decimal?[] { 1, 2, 3, null, 5 };
      expected = new decimal?[] { 1, null, 3, null, 5 };

      actual = Arithmetic.Subtract(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Long

    [TestMethod()]
    public void SubtractLongTest()
    {
      IEnumerable<long> leftSequence;
      IEnumerable<long> rightSequence;
      IEnumerable<long> expected;
      IEnumerable<long> actual;

      leftSequence = new long[] { 2, 4, 6, 8, 10 };
      rightSequence = new long[] { 1, 2, 3, 4, 5 };
      expected = new long[] { 1, 2, 3, 4, 5 };

      actual = Arithmetic.Subtract(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void SubtractNullableLongTest()
    {
      IEnumerable<long?> leftSequence;
      IEnumerable<long?> rightSequence;
      IEnumerable<long?> expected;
      IEnumerable<long?> actual;

      leftSequence = new long?[] { 2, 4, 6, 8, 10 };
      rightSequence = new long?[] { 1, 2, 3, 4, 5 };
      expected = new long?[] { 1, 2, 3, 4, 5 };

      actual = Arithmetic.Subtract(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      leftSequence = new long?[] { 2, null, 6, 8, 10 };
      rightSequence = new long?[] { 1, 2, 3, null, 5 };
      expected = new long?[] { 1, null, 3, null, 5 };

      actual = Arithmetic.Subtract(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Int

    [TestMethod()]
    public void SubtractIntTest()
    {
      IEnumerable<int> leftSequence;
      IEnumerable<int> rightSequence;
      IEnumerable<int> expected;
      IEnumerable<int> actual;

      leftSequence = new int[] { 2, 4, 6, 8, 10 };
      rightSequence = new int[] { 1, 2, 3, 4, 5 };
      expected = new int[] { 1, 2, 3, 4, 5 };

      actual = Arithmetic.Subtract(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void SubtractNullableIntTest()
    {
      IEnumerable<int?> leftSequence;
      IEnumerable<int?> rightSequence;
      IEnumerable<int?> expected;
      IEnumerable<int?> actual;

      leftSequence = new int?[] { 2, 4, 6, 8, 10 };
      rightSequence = new int?[] { 1, 2, 3, 4, 5 };
      expected = new int?[] { 1, 2, 3, 4, 5 };

      actual = Arithmetic.Subtract(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      leftSequence = new int?[] { 2, null, 6, 8, 10 };
      rightSequence = new int?[] { 1, 2, 3, null, 5 };
      expected = new int?[] { 1, null, 3, null, 5 };

      actual = Arithmetic.Subtract(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    [TestMethod()]
    public void SubtractErrTest()
    {
      try
      {
        IEnumerable<bool> leftSequence;
        IEnumerable<bool> rightSequence;

        leftSequence = new bool[] { true, true, true, true, true };
        rightSequence = new bool[] { true, true, true, true, true };

        bool[] res = leftSequence.Subtract(rightSequence).ToArray();
        Assert.Fail();
      }
      catch (InvalidOperationException) { }
      catch (Exception) { Assert.Fail(); }
    }

    #endregion

    #region Multiply

    #region Double

    [TestMethod()]
    public void MultiplyDoubleTest()
    {
      IEnumerable<double> leftSequence;
      IEnumerable<double> rightSequence;
      IEnumerable<double> expected;
      IEnumerable<double> actual;

      leftSequence = new double[] { 1, 2, 3, 4, 5 };
      rightSequence = new double[] { 1, 2, 3, 4, 5 };
      expected = new double[] { 1, 4, 9, 16, 25 };

      actual = Arithmetic.Multiply(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      leftSequence = new double[] { 1, 2, 3, double.NaN, double.PositiveInfinity };
      rightSequence = new double[] { 1, 2, 3, 4, 5 };
      expected = new double[] { 1, 4, 9, double.NaN, double.PositiveInfinity };

      actual = Arithmetic.Multiply(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void MultiplyNullableDoubleTest()
    {
      IEnumerable<double?> leftSequence;
      IEnumerable<double?> rightSequence;
      IEnumerable<double?> expected;
      IEnumerable<double?> actual;

      leftSequence = new double?[] { 1, 2, 3, 4, 5 };
      rightSequence = new double?[] { 1, 2, 3, 4, 5 };
      expected = new double?[] { 1, 4, 9, 16, 25 };

      actual = Arithmetic.Multiply(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      leftSequence = new double?[] { 1, 2, 3, double.NaN, double.PositiveInfinity };
      rightSequence = new double?[] { 1, 2, 3, 4, 5 };
      expected = new double?[] { 1, 4, 9, double.NaN, double.PositiveInfinity };

      actual = Arithmetic.Multiply(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      leftSequence = new double?[] { 1, null, 3, double.NaN, double.PositiveInfinity };
      rightSequence = new double?[] { 1, 2, 3, 4, 5 };
      expected = new double?[] { 1, null, 9, double.NaN, double.PositiveInfinity };

      actual = Arithmetic.Multiply(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Float

    [TestMethod()]
    public void MultiplyFloatTest()
    {
      IEnumerable<float> leftSequence;
      IEnumerable<float> rightSequence;
      IEnumerable<float> expected;
      IEnumerable<float> actual;

      leftSequence = new float[] { 1, 2, 3, 4, 5 };
      rightSequence = new float[] { 1, 2, 3, 4, 5 };
      expected = new float[] { 1, 4, 9, 16, 25 };

      actual = Arithmetic.Multiply(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      leftSequence = new float[] { 1, 2, 3, float.NaN, float.PositiveInfinity };
      rightSequence = new float[] { 1, 2, 3, 4, 5 };
      expected = new float[] { 1, 4, 9, float.NaN, float.PositiveInfinity };

      actual = Arithmetic.Multiply(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void MultiplyNullableFloatTest()
    {
      IEnumerable<float?> leftSequence;
      IEnumerable<float?> rightSequence;
      IEnumerable<float?> expected;
      IEnumerable<float?> actual;

      leftSequence = new float?[] { 1, 2, 3, 4, 5 };
      rightSequence = new float?[] { 1, 2, 3, 4, 5 };
      expected = new float?[] { 1, 4, 9, 16, 25 };

      actual = Arithmetic.Multiply(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      leftSequence = new float?[] { 1, 2, 3, float.NaN, float.PositiveInfinity };
      rightSequence = new float?[] { 1, 2, 3, 4, 5 };
      expected = new float?[] { 1, 4, 9, float.NaN, float.PositiveInfinity };

      actual = Arithmetic.Multiply(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      leftSequence = new float?[] { 1, null, 3, float.NaN, float.PositiveInfinity };
      rightSequence = new float?[] { 1, 2, 3, 4, 5 };
      expected = new float?[] { 1, null, 9, float.NaN, float.PositiveInfinity };

      actual = Arithmetic.Multiply(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Decimal

    [TestMethod()]
    public void MultiplyDecimalTest()
    {
      IEnumerable<decimal> leftSequence = new decimal[] { 1, 2, 3, 4, 5 };
      IEnumerable<decimal> rightSequence = new decimal[] { 1, 2, 3, 4, 5 };
      IEnumerable<decimal> expected = new decimal[] { 1, 4, 9, 16, 25 };

      IEnumerable<decimal> actual;
      actual = Arithmetic.Multiply(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void MultiplyNullableDecimalTest()
    {
      IEnumerable<decimal?> leftSequence;
      IEnumerable<decimal?> rightSequence;
      IEnumerable<decimal?> expected;
      IEnumerable<decimal?> actual;

      leftSequence = new decimal?[] { 1, 2, 3, 4, 5 };
      rightSequence = new decimal?[] { 1, 2, 3, 4, 5 };
      expected = new decimal?[] { 1, 4, 9, 16, 25 };

      actual = Arithmetic.Multiply(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      leftSequence = new decimal?[] { 1, null, 3, 4, 5 };
      rightSequence = new decimal?[] { 1, 2, 3, null, 5 };
      expected = new decimal?[] { 1, null, 9, null, 25 };

      actual = Arithmetic.Multiply(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Long

    [TestMethod()]
    public void MultiplyLongTest()
    {
      IEnumerable<long> leftSequence = new long[] { 1, 2, 3, 4, 5 };
      IEnumerable<long> rightSequence = new long[] { 1, 2, 3, 4, 5 };
      IEnumerable<long> expected = new long[] { 1, 4, 9, 16, 25 };

      IEnumerable<long> actual;
      actual = Arithmetic.Multiply(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void MultiplyNullableLongTest()
    {
      IEnumerable<long?> leftSequence;
      IEnumerable<long?> rightSequence;
      IEnumerable<long?> expected;
      IEnumerable<long?> actual;

      leftSequence = new long?[] { 1, 2, 3, 4, 5 };
      rightSequence = new long?[] { 1, 2, 3, 4, 5 };
      expected = new long?[] { 1, 4, 9, 16, 25 };

      actual = Arithmetic.Multiply(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      leftSequence = new long?[] { 1, null, 3, 4, 5 };
      rightSequence = new long?[] { 1, 2, 3, null, 5 };
      expected = new long?[] { 1, null, 9, null, 25 };

      actual = Arithmetic.Multiply(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Int

    [TestMethod()]
    public void MultiplyIntTest()
    {
      IEnumerable<int> leftSequence = new int[] { 1, 2, 3, 4, 5 };
      IEnumerable<int> rightSequence = new int[] { 1, 2, 3, 4, 5 };
      IEnumerable<int> expected = new int[] { 1, 4, 9, 16, 25 };

      IEnumerable<int> actual;
      actual = Arithmetic.Multiply(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void MultiplyNullableIntTest()
    {
      IEnumerable<int?> leftSequence;
      IEnumerable<int?> rightSequence;
      IEnumerable<int?> expected;
      IEnumerable<int?> actual;

      leftSequence = new int?[] { 1, 2, 3, 4, 5 };
      rightSequence = new int?[] { 1, 2, 3, 4, 5 };
      expected = new int?[] { 1, 4, 9, 16, 25 };

      actual = Arithmetic.Multiply(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      leftSequence = new int?[] { 1, null, 3, 4, 5 };
      rightSequence = new int?[] { 1, 2, 3, null, 5 };
      expected = new int?[] { 1, null, 9, null, 25 };

      actual = Arithmetic.Multiply(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    [TestMethod()]
    public void MultiplyErrTest()
    {
      try
      {
        IEnumerable<bool> leftSequence;
        IEnumerable<bool> rightSequence;

        leftSequence = new bool[] { true, true, true, true, true };
        rightSequence = new bool[] { true, true, true, true, true };

        bool[] res = leftSequence.Multiply(rightSequence).ToArray();
        Assert.Fail();
      }
      catch (InvalidOperationException) { }
      catch (Exception) { Assert.Fail(); }
    }

    #endregion

    #region Divide

    #region Double

    [TestMethod()]
    public void DivideDoubleTest()
    {
      IEnumerable<double> leftSequence;
      IEnumerable<double> rightSequence;
      IEnumerable<double> expected;
      IEnumerable<double> actual;

      leftSequence = new double[] { 1, 2, 3, 4, 5 };
      rightSequence = new double[] { 1, 2, 3, 4, 5 };
      expected = new double[] { 1, 1, 1, 1, 1 };

      actual = Arithmetic.Divide(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      leftSequence = new double[] { 1, 2, 3, double.NaN, double.PositiveInfinity };
      rightSequence = new double[] { 1, 2, 3, 4, 5 };
      expected = new double[] { 1, 1, 1, double.NaN, double.PositiveInfinity };

      actual = Arithmetic.Divide(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      leftSequence = new double[] { 1, 2, 3, double.NaN, double.PositiveInfinity };
      rightSequence = new double[] { 1, 0, 3, 4, 5 };
      expected = new double[] { 1, double.PositiveInfinity, 1, double.NaN, double.PositiveInfinity };

      actual = Arithmetic.Divide(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void DivideNullableDoubleTest()
    {
      IEnumerable<double?> leftSequence;
      IEnumerable<double?> rightSequence;
      IEnumerable<double?> expected;
      IEnumerable<double?> actual;

      leftSequence = new double?[] { 1, 2, 3, 4, 5 };
      rightSequence = new double?[] { 1, 2, 3, 4, 5 };
      expected = new double?[] { 1, 1, 1, 1, 1 };

      actual = Arithmetic.Divide(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      leftSequence = new double?[] { 1, 2, 3, double.NaN, double.PositiveInfinity };
      rightSequence = new double?[] { 1, 2, 3, 4, 5 };
      expected = new double?[] { 1, 1, 1, double.NaN, double.PositiveInfinity };

      actual = Arithmetic.Divide(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      leftSequence = new double?[] { 1, null, 3, double.NaN, double.PositiveInfinity };
      rightSequence = new double?[] { 1, 2, 3, 4, 5 };
      expected = new double?[] { 1, null, 1, double.NaN, double.PositiveInfinity };

      actual = Arithmetic.Divide(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      leftSequence = new double?[] { 1, 2, 3, double.NaN, double.PositiveInfinity };
      rightSequence = new double?[] { 1, 0, 3, 4, 5 };
      expected = new double?[] { 1, double.PositiveInfinity, 1, double.NaN, double.PositiveInfinity };

      actual = Arithmetic.Divide(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Float

    [TestMethod()]
    public void DivideFloatTest()
    {
      IEnumerable<float> leftSequence;
      IEnumerable<float> rightSequence;
      IEnumerable<float> expected;
      IEnumerable<float> actual;

      leftSequence = new float[] { 1, 2, 3, 4, 5 };
      rightSequence = new float[] { 1, 2, 3, 4, 5 };
      expected = new float[] { 1, 1, 1, 1, 1 };

      actual = Arithmetic.Divide(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      leftSequence = new float[] { 1, 2, 3, float.NaN, float.PositiveInfinity };
      rightSequence = new float[] { 1, 2, 3, 4, 5 };
      expected = new float[] { 1, 1, 1, float.NaN, float.PositiveInfinity };

      actual = Arithmetic.Divide(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      leftSequence = new float[] { 1, 2, 3, float.NaN, float.PositiveInfinity };
      rightSequence = new float[] { 1, 0, 3, 4, 5 };
      expected = new float[] { 1, float.PositiveInfinity, 1, float.NaN, float.PositiveInfinity };

      actual = Arithmetic.Divide(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void DivideNullableFloatTest()
    {
      IEnumerable<float?> leftSequence;
      IEnumerable<float?> rightSequence;
      IEnumerable<float?> expected;
      IEnumerable<float?> actual;

      leftSequence = new float?[] { 1, 2, 3, 4, 5 };
      rightSequence = new float?[] { 1, 2, 3, 4, 5 };
      expected = new float?[] { 1, 1, 1, 1, 1 };

      actual = Arithmetic.Divide(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      leftSequence = new float?[] { 1, 2, 3, float.NaN, float.PositiveInfinity };
      rightSequence = new float?[] { 1, 2, 3, 4, 5 };
      expected = new float?[] { 1, 1, 1, float.NaN, float.PositiveInfinity };

      actual = Arithmetic.Divide(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      leftSequence = new float?[] { 1, null, 3, float.NaN, float.PositiveInfinity };
      rightSequence = new float?[] { 1, 2, 3, 4, 5 };
      expected = new float?[] { 1, null, 1, float.NaN, float.PositiveInfinity };

      actual = Arithmetic.Divide(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      leftSequence = new float?[] { 1, 2, 3, float.NaN, float.PositiveInfinity };
      rightSequence = new float?[] { 1, 0, 3, 4, 5 };
      expected = new float?[] { 1, float.PositiveInfinity, 1, float.NaN, float.PositiveInfinity };

      actual = Arithmetic.Divide(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    #endregion

    #region Decimal

    [TestMethod()]
    public void DivideDecimalTest()
    {
      IEnumerable<decimal> leftSequence;
      IEnumerable<decimal> rightSequence;
      IEnumerable<decimal> expected;
      IEnumerable<decimal> actual;

      leftSequence = new decimal[] { 1, 2, 3, 4, 5 };
      rightSequence = new decimal[] { 1, 2, 3, 4, 5 };
      expected = new decimal[] { 1, 1, 1, 1, 1 };

      actual = Arithmetic.Divide(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        leftSequence = new decimal[] { 1, 2, 3, 4, 5 };
        rightSequence = new decimal[] { 1, 0, 3, 4, 5 };
        expected = new decimal[] { 1, 1, 1, 1, 1 };

        actual = Arithmetic.Divide(leftSequence, rightSequence);
        Assert.IsTrue(expected.SequenceEqual(actual));
        int x = actual.Count();
        Assert.Fail();
      }
      catch (DivideByZeroException) { }
      catch (Exception) { Assert.Fail(); }
    }

    [TestMethod()]
    public void DivideNullableDecimalTest()
    {
      IEnumerable<decimal?> leftSequence;
      IEnumerable<decimal?> rightSequence;
      IEnumerable<decimal?> expected;
      IEnumerable<decimal?> actual;

      leftSequence = new decimal?[] { 1, 2, 3, 4, 5 };
      rightSequence = new decimal?[] { 1, 2, 3, 4, 5 };
      expected = new decimal?[] { 1, 1, 1, 1, 1 };

      actual = Arithmetic.Divide(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      leftSequence = new decimal?[] { 1, null, 3, 4, 5 };
      rightSequence = new decimal?[] { 1, 2, 3, null, 5 };
      expected = new decimal?[] { 1, null, 1, null, 1 };

      actual = Arithmetic.Divide(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        leftSequence = new decimal?[] { 1, 2, 3, 4, 5 };
        rightSequence = new decimal?[] { 1, 0, 3, null, 5 };
        expected = new decimal?[] { 1, null, 1, null, 1 };

        actual = Arithmetic.Divide(leftSequence, rightSequence);
        Assert.IsTrue(expected.SequenceEqual(actual));
        int x = actual.Count();
        Assert.Fail();
      }
      catch (DivideByZeroException) { }
      catch (Exception) { Assert.Fail(); }
    }

    #endregion

    #region Long

    [TestMethod()]
    public void DivideLongTest()
    {
      IEnumerable<long> leftSequence;
      IEnumerable<long> rightSequence;
      IEnumerable<long> expected;
      IEnumerable<long> actual;

      leftSequence = new long[] { 1, 2, 3, 4, 5 };
      rightSequence = new long[] { 1, 2, 3, 4, 5 };
      expected = new long[] { 1, 1, 1, 1, 1 };

      actual = Arithmetic.Divide(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        leftSequence = new long[] { 1, 2, 3, 4, 5 };
        rightSequence = new long[] { 1, 0, 3, 4, 5 };
        expected = new long[] { 1, 1, 1, 1, 1 };

        actual = Arithmetic.Divide(leftSequence, rightSequence);
        int x = actual.Count();
        Assert.Fail();
      }
      catch (DivideByZeroException) { }
      catch (Exception) { Assert.Fail(); }
    }

    [TestMethod()]
    public void DivideNullableLongTest()
    {
      IEnumerable<long?> leftSequence;
      IEnumerable<long?> rightSequence;
      IEnumerable<long?> expected;
      IEnumerable<long?> actual;

      leftSequence = new long?[] { 1, null, 3, 4, 5 };
      rightSequence = new long?[] { 1, 2, 3, null, 5 };
      expected = new long?[] { 1, null, 1, null, 1 };

      actual = Arithmetic.Divide(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        leftSequence = new long?[] { 1, 2, 3, 4, 5 };
        rightSequence = new long?[] { 1, 0, 3, 4, 5 };
        expected = new long?[] { 1, 1, 1, 1, 1 };

        actual = Arithmetic.Divide(leftSequence, rightSequence);
        Assert.IsTrue(expected.SequenceEqual(actual));
        int x = actual.Count();
        Assert.Fail();
      }
      catch (DivideByZeroException) { }
      catch (Exception) { Assert.Fail(); }

      //------------------------------//
      try
      {
        leftSequence = new long?[] { 1, 2, 3, 4, 5 };
        rightSequence = new long?[] { 1, 0, 3, null, 5 };
        expected = new long?[] { 1, null, 1, null, 1 };

        actual = Arithmetic.Divide(leftSequence, rightSequence);
        Assert.IsTrue(expected.SequenceEqual(actual));
        int x = actual.Count();
        Assert.Fail();
      }
      catch (DivideByZeroException) { }
      catch (Exception) { Assert.Fail(); }

    }

    #endregion

    #region Int

    [TestMethod()]
    public void DivideIntTest()
    {
      IEnumerable<int> leftSequence;
      IEnumerable<int> rightSequence;
      IEnumerable<int> expected;
      IEnumerable<int> actual;

      leftSequence = new int[] { 1, 2, 3, 4, 5 };
      rightSequence = new int[] { 1, 2, 3, 4, 5 };
      expected = new int[] { 1, 1, 1, 1, 1 };

      actual = Arithmetic.Divide(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        leftSequence = new int[] { 1, 2, 3, 4, 5 };
        rightSequence = new int[] { 1, 0, 3, 4, 5 };
        expected = new int[] { 1, 1, 1, 1, 1 };

        actual = Arithmetic.Divide(leftSequence, rightSequence);
        int x = actual.Count();
        Assert.Fail();
      }
      catch (DivideByZeroException) { }
      catch (Exception) { Assert.Fail(); }
    }

    [TestMethod()]
    public void DivideNullableIntTest()
    {
      IEnumerable<int?> leftSequence;
      IEnumerable<int?> rightSequence;
      IEnumerable<int?> expected;
      IEnumerable<int?> actual;

      leftSequence = new int?[] { 1, 2, 3, 4, 5 };
      rightSequence = new int?[] { 1, 2, 3, 4, 5 };
      expected = new int?[] { 1, 1, 1, 1, 1 };

      actual = Arithmetic.Divide(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      leftSequence = new int?[] { 1, null, 3, 4, 5 };
      rightSequence = new int?[] { 1, 2, 3, null, 5 };
      expected = new int?[] { 1, null, 1, null, 1 };

      actual = Arithmetic.Divide(leftSequence, rightSequence);
      Assert.IsTrue(expected.SequenceEqual(actual));

      //------------------------------//

      try
      {
        leftSequence = new int?[] { 1, 2, 3, 4, 5 };
        rightSequence = new int?[] { 1, 0, 3, null, 5 };
        expected = new int?[] { 1, null, 1, null, 1 };

        actual = Arithmetic.Divide(leftSequence, rightSequence);
        Assert.IsTrue(expected.SequenceEqual(actual));
        int x = actual.Count();
        Assert.Fail();
      }
      catch (DivideByZeroException) { }
      catch (Exception) { Assert.Fail(); }
    }

    #endregion

    [TestMethod()]
    public void DivideErrTest()
    {
      try
      {
        IEnumerable<bool> leftSequence;
        IEnumerable<bool> rightSequence;

        leftSequence = new bool[] { true, true, true, true, true };
        rightSequence = new bool[] { true, true, true, true, true };

        bool[] res = leftSequence.Divide(rightSequence).ToArray();
        Assert.Fail();
      }
      catch (InvalidOperationException) { }
      catch (Exception) { Assert.Fail(); }
    }

    #endregion

    [TestMethod()]
    public void ApplyFunction2Test()
    {
      IEnumerable<decimal>[] sources = { new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 }};
      IEnumerable<decimal> expected = new decimal[] { 1, 4, 9, 16, 25 };

      IEnumerable<decimal> actual;
      actual = Arithmetic.ApplyFunction(sources, (P0, P1) => P0 * P1);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void ApplyFunction3Test()
    {
      IEnumerable<decimal>[] sources = { new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 }};
      IEnumerable<decimal> expected = new decimal[] { 1 + 1, 4 + 2, 9 + 3, 16 + 4, 25 + 5 };

      IEnumerable<decimal> actual;
      actual = Arithmetic.ApplyFunction(sources, (P0, P1, P2) => P0 * P1 + P2);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void ApplyFunction4Test()
    {
      IEnumerable<decimal>[] sources = { new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 }};
      IEnumerable<decimal> expected = new decimal[] { 1, 4, 9, 16, 25 };

      IEnumerable<decimal> actual;
      actual = Arithmetic.ApplyFunction(sources, (P0, P1, P2, P3) => P0 * P1 + P2 - P3);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void ApplyFunction5Test()
    {
      IEnumerable<decimal>[] sources = { new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 }};
      IEnumerable<decimal> expected = new decimal[] { 1 + 1, 4 + 2, 9 + 3, 16 + 4, 25 + 5 };

      IEnumerable<decimal> actual;
      actual = Arithmetic.ApplyFunction(sources, (P0, P1, P2, P3, P4) => P0 * P1 + P2 - P3 + P4);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void ApplyFunction6Test()
    {
      IEnumerable<decimal>[] sources = { new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 }};
      IEnumerable<decimal> expected = new decimal[] { 1, 4, 9, 16, 25 };

      IEnumerable<decimal> actual;
      actual = Arithmetic.ApplyFunction(sources, (P0, P1, P2, P3, P4, P5) => P0 * P1 + P2 - P3 + P4 - P5);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void ApplyFunction7Test()
    {
      IEnumerable<decimal>[] sources = { new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 }};
      IEnumerable<decimal> expected = new decimal[] { 1 + 1, 4 + 2, 9 + 3, 16 + 4, 25 + 5 };

      IEnumerable<decimal> actual;
      actual = Arithmetic.ApplyFunction(sources, (P0, P1, P2, P3, P4, P5, P6) => P0 * P1 + P2 - P3 + P4 - P5 + P6);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void ApplyFunction8Test()
    {
      IEnumerable<decimal>[] sources = { new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 }};
      IEnumerable<decimal> expected = new decimal[] { 1, 4, 9, 16, 25 };

      IEnumerable<decimal> actual;
      actual = Arithmetic.ApplyFunction(sources, (P0, P1, P2, P3, P4, P5, P6, P7) => P0 * P1 + P2 - P3 + P4 - P5 + P6 - P7);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void ApplyFunction9Test()
    {
      IEnumerable<decimal>[] sources = { new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 }};
      IEnumerable<decimal> expected = new decimal[] { 1 + 1, 4 + 2, 9 + 3, 16 + 4, 25 + 5 };

      IEnumerable<decimal> actual;
      actual = Arithmetic.ApplyFunction(sources, (P0, P1, P2, P3, P4, P5, P6, P7, P8) => P0 * P1 + P2 - P3 + P4 - P5 + P6 - P7 + P8);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void ApplyFunction10Test()
    {
      IEnumerable<decimal>[] sources = { new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 }};
      IEnumerable<decimal> expected = new decimal[] { 1, 4, 9, 16, 25 };

      IEnumerable<decimal> actual;
      actual = Arithmetic.ApplyFunction(sources, (P0, P1, P2, P3, P4, P5, P6, P7, P8, P9) => P0 * P1 + P2 - P3 + P4 - P5 + P6 - P7 + P8 - P9);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void ApplyFunction11Test()
    {
      IEnumerable<decimal>[] sources = { new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 }};
      IEnumerable<decimal> expected = new decimal[] { 1 + 1, 4 + 2, 9 + 3, 16 + 4, 25 + 5 };

      IEnumerable<decimal> actual;
      actual = Arithmetic.ApplyFunction(sources, (P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, PA) => P0 * P1 + P2 - P3 + P4 - P5 + P6 - P7 + P8 - P9 + PA);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void ApplyFunction12Test()
    {
      IEnumerable<decimal>[] sources = { new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 }};
      IEnumerable<decimal> expected = new decimal[] { 1, 4, 9, 16, 25 };

      IEnumerable<decimal> actual;
      actual = Arithmetic.ApplyFunction(sources, (P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, PA, PB) => P0 * P1 + P2 - P3 + P4 - P5 + P6 - P7 + P8 - P9 + PA - PB);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void ApplyFunction13Test()
    {
      IEnumerable<decimal>[] sources = { new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 }};
      IEnumerable<decimal> expected = new decimal[] { 1 + 1, 4 + 2, 9 + 3, 16 + 4, 25 + 5 };

      IEnumerable<decimal> actual;
      actual = Arithmetic.ApplyFunction(sources, (P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, PA, PB, PC) => P0 * P1 + P2 - P3 + P4 - P5 + P6 - P7 + P8 - P9 + PA - PB + PC);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void ApplyFunction14Test()
    {
      IEnumerable<decimal>[] sources = { new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 }};
      IEnumerable<decimal> expected = new decimal[] { 1, 4, 9, 16, 25 };

      IEnumerable<decimal> actual;
      actual = Arithmetic.ApplyFunction(sources, (P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, PA, PB, PC, PD) => P0 * P1 + P2 - P3 + P4 - P5 + P6 - P7 + P8 - P9 + PA - PB + PC - PD);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void ApplyFunction15Test()
    {
      IEnumerable<decimal>[] sources = { new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 }};
      IEnumerable<decimal> expected = new decimal[] { 1 + 1, 4 + 2, 9 + 3, 16 + 4, 25 + 5 };

      IEnumerable<decimal> actual;
      actual = Arithmetic.ApplyFunction(sources, (P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, PA, PB, PC, PD, PE) => P0 * P1 + P2 - P3 + P4 - P5 + P6 - P7 + P8 - P9 + PA - PB + PC - PD + PE);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    [TestMethod()]
    public void ApplyFunction16Test()
    {
      IEnumerable<decimal>[] sources = { new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 },
                                         new decimal[] { 1, 2, 3, 4, 5 }};
      IEnumerable<decimal> expected = new decimal[] { 1, 4, 9, 16, 25 };

      IEnumerable<decimal> actual;
      actual = Arithmetic.ApplyFunction(sources, (P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, PA, PB, PC, PD, PE, PF) => P0 * P1 + P2 - P3 + P4 - P5 + P6 - P7 + P8 - P9 + PA - PB + PC - PD + PE - PF);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }
  }
}
