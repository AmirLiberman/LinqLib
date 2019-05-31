using System.Collections.Generic;
using System.Linq;
using LinqLib.Array;
using LinqLib.Sequence;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinqLibTests.Array
{
  [TestClass()]
  public class ArrayExtensionssTests
  {
    #region Slice and Fuse

    [TestMethod()]
    public void Slice2D()
    {
      const int Y = 10;
      const int X = 8;
      int i = 0;

      int[,] arr1 = new int[Y, X];
      List<int[]> expected = new List<int[]>();

      for (int y = 0; y < Y; y++)
      {
        expected.Add(new int[X]);
        for (int x = 0; x < X; x++)
        {
          arr1[y, x] = i;
          expected[y][x] = i;
          i++;
        }
      }

      List<int[]> actual = arr1.Slice().ToList();

      for (int y = 0; y < Y; y++)
        for (int x = 0; x < X; x++)
          Assert.IsTrue(expected[y][x] == actual[y][x]);

      var actual2 = actual.Fuse();
      Assert.IsTrue(arr1.ArrayEquals(actual2));
    }

    [TestMethod()]
    public void Slice3D()
    {
      const int Z = 5;
      const int Y = 10;
      const int X = 8;
      int i = 0;

      int[, ,] arr1 = new int[Z, Y, X];
      List<int[,]> expected = new List<int[,]>();

      for (int z = 0; z < Z; z++)
      {
        expected.Add(new int[Y, X]);
        for (int y = 0; y < Y; y++)
          for (int x = 0; x < X; x++)
          {
            arr1[z, y, x] = i;
            expected[z][y, x] = i;
            i++;
          }
      }

      List<int[,]> actual = arr1.Slice().ToList();

      for (int z = 0; z < Z; z++)
        for (int y = 0; y < Y; y++)
          for (int x = 0; x < X; x++)
            Assert.IsTrue(expected[z][y, x] == actual[z][y, x]);

      var actual2 = actual.Fuse();
      Assert.IsTrue(arr1.ArrayEquals(actual2));
    }

    [TestMethod()]
    public void Slice4D()
    {
      const int A = 7;
      const int Z = 5;
      const int Y = 10;
      const int X = 8;
      int i = 0;

      int[, , ,] arr1 = new int[A, Z, Y, X];
      List<int[, ,]> expected = new List<int[, ,]>();

      for (int a = 0; a < A; a++)
      {
        expected.Add(new int[Z, Y, X]);
        for (int z = 0; z < Z; z++)
          for (int y = 0; y < Y; y++)
            for (int x = 0; x < X; x++)
            {
              arr1[a, z, y, x] = i;
              expected[a][z, y, x] = i;
              i++;
            }
      }

      List<int[, ,]> actual = arr1.Slice().ToList();

      for (int a = 0; a < A; a++)
        for (int z = 0; z < Z; z++)
          for (int y = 0; y < Y; y++)
            for (int x = 0; x < X; x++)
              Assert.IsTrue(expected[a][z, y, x] == actual[a][z, y, x]);

      var actual2 = actual.Fuse();
      Assert.IsTrue(arr1.ArrayEquals(actual2));
    }

    #endregion

    #region Split (up to 4D - any type)

    [TestMethod()]
    public void Split1D()
    {
      double[] src = new double[] { 1, 2, 3, 4, 5, 6, 7, 8 };
      var actual1 = src.CircularShift(4).ToArray().Fuse();
      var actual2 = src.CircularShift(3, 2).ToArray().Fuse();
      var expected1 = ArrayReader.GetDoubleArray("Split1D-1");
      var expected2 = ArrayReader.GetDoubleArray("Split1D-2");

      Assert.IsTrue(src.Split(3).ToArray().Fuse().ArrayEquals(src.CircularShift(3, 3).ToArray().Fuse()));
      Assert.IsTrue(actual1.ArrayEquals(expected1));
      Assert.IsTrue(actual2.ArrayEquals(expected2));
    }

    [TestMethod()]
    public void Split2D()
    {
      double[,] src = Enumerator.Generate<double>(1, 1, 12).ToArray(3, 4);

      var actual1 = src.CircularShift(2, 2).ToArray().Fuse();
      var actual2 = src.CircularShift(1, 2, 2, 2).ToArray().Fuse();
      var expected1 = ArrayReader.GetDoubleArray("Split2D-1");
      var expected2 = ArrayReader.GetDoubleArray("Split2D-2");

      Assert.IsTrue(src.Split(2, 2).ToArray().Fuse().ArrayEquals(src.CircularShift(2, 2, 2, 2).ToArray().Fuse()));
      Assert.IsTrue(actual1.ArrayEquals(expected1));
      Assert.IsTrue(actual2.ArrayEquals(expected2));
    }

    [TestMethod()]
    public void Split3D()
    {
      double[, ,] src = Enumerator.Generate<double>(1, 1, 36).ToArray(3, 3, 4);

      var actual1 = src.CircularShift(2, 2, 2).ToArray().Fuse();
      var actual2 = src.CircularShift(1, 2, 2, 1, 2, 2).ToArray().Fuse();
      var expected1 = ArrayReader.GetDoubleArray("Split3D-1");
      var expected2 = ArrayReader.GetDoubleArray("Split3D-2");

      Assert.IsTrue(src.Split(2, 2, 2).ToArray().Fuse().ArrayEquals(src.CircularShift(2, 2, 2, 2, 2, 2).ToArray().Fuse()));
      Assert.IsTrue(actual1.ArrayEquals(expected1));
      Assert.IsTrue(actual2.ArrayEquals(expected2));
    }

    [TestMethod()]
    public void Split4D()
    {
      double[, , ,] src = Enumerator.Generate<double>(1, 1, 256).ToArray(3, 3, 3, 3);

      var actual1 = src.CircularShift(2, 2, 3, 2).ToArray();
      var actual2 = src.CircularShift(1, 2, 2, 1, 2, 1, 1, 2).ToArray();

      var actual3 = src.Split(2, 2, 2, 2).ToArray();
      var actual4 = src.CircularShift(2, 2, 2, 2, 2, 2, 2, 2).ToArray();

      for (int i = 0; i < actual1.Length; i++)
        Assert.IsTrue(actual1[i].ArrayEquals(ArrayReader.GetDoubleArray(string.Format("Split4D-1.{0}", i))));
      for (int i = 0; i < actual2.Length; i++)
        Assert.IsTrue(actual2[i].ArrayEquals(ArrayReader.GetDoubleArray(string.Format("Split4D-2.{0}", i))));

      Assert.IsTrue(actual3.Length == actual4.Length);
      for (int i = 0; i < actual3.Length; i++)
        Assert.IsTrue(actual3[i].ArrayEquals(actual4[i]));
    }

    #endregion

    #region ToArray and AsEnumerable

    [TestMethod()]
    public void ToArrayTest()
    {
      int[] expected1 = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 0, 0, 0, 0 };
      int[] actual1 = Enumerable.Range(0, 16).ToArray(20);
      Assert.IsTrue(expected1.ArrayEquals(actual1));

      int[,] expected2 = new int[,] { { 0, 1, 2, 3, 4, 5, 6, 7 }, { 8, 9, 10, 11, 12, 13, 0, 0 } };
      int[,] actual2 = Enumerable.Range(0, 14).ToArray(2, 8);
      Assert.IsTrue(expected2.ArrayEquals(actual2));

      int[, ,] expected3 = new int[,,] { { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 } }, { { 9, 10, 11 }, { 12, 13, 14 }, { 15, 16, 17 } }, { { 18, 19, 20 }, { 21, 22, 23 }, { 24, 25, 0 } } };
      int[, ,] actual3 = Enumerable.Range(0, 26).ToArray(3, 3, 3);
      Assert.IsTrue(expected3.ArrayEquals(actual3));

      int[, , ,] expected4 = new int[,,,] { { { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 } }, { { 9, 10, 11 }, { 12, 13, 14 }, { 15, 16, 17 } } }, { { { 18, 19, 20 }, { 21, 22, 23 }, { 24, 25, 26 } }, { { 27, 28, 29 }, { 30, 31, 32 }, { 0, 0, 0 } } } };
      int[, , ,] actual4 = Enumerable.Range(0, 33).ToArray(2, 2, 3, 3);
      Assert.IsTrue(expected4.ArrayEquals(actual4));

      string[] expectedc1 = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P" };
      string[] actualc1 = Enumerator.Generate<string>(65, 16, (X) => ((char)X).ToString()).ToArray(16);
      Assert.IsTrue(expectedc1.ArrayEquals(actualc1));

      string[,] expectedc2 = new string[,] { { "A", "B", "C", "D", "E", "F", "G", "H" }, { "I", "J", "K", "L", "M", "N", "O", "P" } };
      string[,] actualc2 = Enumerator.Generate(65, 16, (X) => ((char)X).ToString()).ToArray(2, 8);
      Assert.IsTrue(expected2.ArrayEquals(actual2));

      string[, ,] expectedc3 = new string[,,] { { { "A", "B", "C" }, { "D", "E", "F" }, { "G", "H", "I" } }, { { "J", "K", "L" }, { "M", "N", "O" }, { "P", "Q", "R" } }, { { "S", "T", "U" }, { "V", "W", "X" }, { "Y", "Z", "[" } } };
      string[, ,] actualc3 = Enumerator.Generate(65, 27, (X) => ((char)X).ToString()).ToArray(3, 3, 3);
      Assert.IsTrue(expectedc3.ArrayEquals(actualc3));

      string[, , ,] expectedc4 = { { { { "A", "B", "C" }, { "D", "E", "F" }, { "G", "H", "I" } }, { { "J", "K", "L" }, { "M", "N", "O" }, { "P", "Q", "R" } } }, { { { "S", "T", "U" }, { "V", "W", "X" }, { "Y", "Z", "[" } }, { { "\\", "]", "^" }, { "_", "`", "a" }, { "b", "c", "d" } } } };
      string[, , ,] actualc4 = Enumerator.Generate(65, 36, (X) => ((char)X).ToString()).ToArray(2, 2, 3, 3);
      Assert.IsTrue(expectedc4.ArrayEquals(actualc4));
    }

    [TestMethod()]
    public void AsEnumerableTest()
    {
      int[, , ,] src1 = Enumerable.Range(0, 625).ToArray(5, 5, 5, 5);
      IEnumerable<int> expected1 = Enumerable.Range(0, 625);
      Assert.IsTrue(expected1.SequenceEqual(src1.AsEnumerable().OfType<int>()));

      int[][][] src2 = new int[][][] { new int[][] { new int[] { 0, 1, 2, 3 }, new int[] { 4 }, new int[] { 5, 6 } },
                                      new int[][] { new int[] { 7, 8 }, new int[] { 9 } },
                                      new int[0][] ,
                                      new int[][]{ new int[] { 10, 11 }, new int[0], new int[] { 12 } } };
      int[] actual2 = src2.AsEnumerable<int>().ToArray();
      int[] expected2 = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
      Assert.IsTrue(expected2.ArrayEquals(actual2));

      int[] actual2a = ((System.Array)src2).AsEnumerable().OfType<int>().ToArray();
      Assert.IsTrue(expected2.ArrayEquals(actual2a));
    }

    #endregion

    #region Resize

    private double add(double a, double b)
    {
      return a + b;
    }
    private double sub(double a, double b)
    {
      return a - b;
    }
    private double mul(double a, double b)
    {
      return (float)(a * b);
    }
    private double div(double a, double b)
    {
      return (float)(a / b);
    }

    private float add(float a, float b)
    {
      return a + b;
    }
    private float sub(float a, float b)
    {
      return a - b;
    }
    private float mul(float a, double b)
    {
      return (float)(a * b);
    }
    private float div(float a, double b)
    {
      return (float)(a / b);
    }

    [TestMethod()]
    public void Resize1DTest()
    {
      double[] src = Enumerator.Generate<double>(1, 2, 24).ToArray();

      Assert.IsTrue(src.Resize(7).ArrayEquals(ArrayReader.GetDoubleArray("Resize1D-1.0.0"), new DoubleComparer(.000001)));
      Assert.IsTrue(src.Resize(12).ArrayEquals(ArrayReader.GetDoubleArray("Resize1D-1.0.1"), new DoubleComparer(.000001)));
      Assert.IsTrue(src.Resize(17).ArrayEquals(ArrayReader.GetDoubleArray("Resize1D-1.0.2"), new DoubleComparer(.000001)));
      Assert.IsTrue(src.Resize(24).ArrayEquals(ArrayReader.GetDoubleArray("Resize1D-1.0.3"), new DoubleComparer(.000001)));
      Assert.IsTrue(src.Resize(37).ArrayEquals(ArrayReader.GetDoubleArray("Resize1D-1.0.4"), new DoubleComparer(.000001)));
      Assert.IsTrue(src.Resize(47).ArrayEquals(ArrayReader.GetDoubleArray("Resize1D-1.0.5"), new DoubleComparer(.000001)));

      Assert.IsTrue(src.Resize(add, sub, mul, div, 7).ArrayEquals(ArrayReader.GetDoubleArray("Resize1D-1.0.0"), new DoubleComparer(.0001)));
      Assert.IsTrue(src.Resize(add, sub, mul, div, 12).ArrayEquals(ArrayReader.GetDoubleArray("Resize1D-1.0.1"), new DoubleComparer(.0001)));
      Assert.IsTrue(src.Resize(add, sub, mul, div, 17).ArrayEquals(ArrayReader.GetDoubleArray("Resize1D-1.0.2"), new DoubleComparer(.0001)));
      Assert.IsTrue(src.Resize(add, sub, mul, div, 24).ArrayEquals(ArrayReader.GetDoubleArray("Resize1D-1.0.3"), new DoubleComparer(.0001)));
      Assert.IsTrue(src.Resize(add, sub, mul, div, 37).ArrayEquals(ArrayReader.GetDoubleArray("Resize1D-1.0.4"), new DoubleComparer(.0001)));
      Assert.IsTrue(src.Resize(add, sub, mul, div, 47).ArrayEquals(ArrayReader.GetDoubleArray("Resize1D-1.0.5"), new DoubleComparer(.0001)));

      float[] src2 = Enumerator.Generate<float>(1, 2, 24).ToArray();

      Assert.IsTrue(src2.Resize(7).ArrayEquals(ArrayReader.GetSingleArray("Resize1D-1.0.0"), new SingleComparer(.00001f)));
      Assert.IsTrue(src2.Resize(12).ArrayEquals(ArrayReader.GetSingleArray("Resize1D-1.0.1"), new SingleComparer(.00001f)));
      Assert.IsTrue(src2.Resize(17).ArrayEquals(ArrayReader.GetSingleArray("Resize1D-1.0.2"), new SingleComparer(.00001f)));
      Assert.IsTrue(src2.Resize(24).ArrayEquals(ArrayReader.GetSingleArray("Resize1D-1.0.3"), new SingleComparer(.00001f)));
      Assert.IsTrue(src2.Resize(37).ArrayEquals(ArrayReader.GetSingleArray("Resize1D-1.0.4"), new SingleComparer(.00001f)));
      Assert.IsTrue(src2.Resize(47).ArrayEquals(ArrayReader.GetSingleArray("Resize1D-1.0.5"), new SingleComparer(.00001f)));

      Assert.IsTrue(src2.Resize(add, sub, mul, div, 7).ArrayEquals(ArrayReader.GetSingleArray("Resize1D-1.0.0"), new SingleComparer(.0001f)));
      Assert.IsTrue(src2.Resize(add, sub, mul, div, 12).ArrayEquals(ArrayReader.GetSingleArray("Resize1D-1.0.1"), new SingleComparer(.0001f)));
      Assert.IsTrue(src2.Resize(add, sub, mul, div, 17).ArrayEquals(ArrayReader.GetSingleArray("Resize1D-1.0.2"), new SingleComparer(.0001f)));
      Assert.IsTrue(src2.Resize(add, sub, mul, div, 24).ArrayEquals(ArrayReader.GetSingleArray("Resize1D-1.0.3"), new SingleComparer(.0001f)));
      Assert.IsTrue(src2.Resize(add, sub, mul, div, 37).ArrayEquals(ArrayReader.GetSingleArray("Resize1D-1.0.4"), new SingleComparer(.0001f)));
      Assert.IsTrue(src2.Resize(add, sub, mul, div, 47).ArrayEquals(ArrayReader.GetSingleArray("Resize1D-1.0.5"), new SingleComparer(.0001f)));
    }

    [TestMethod()]
    public void Resize2DTest()
    {
      int r = 3 * 5;
      double[,] src = Enumerator.Generate<double>(1, 2, r).ToArray(3, 5);

      Assert.IsTrue(src.Resize(2, 2).ArrayEquals(ArrayReader.GetDoubleArray("Resize2D-1.0.0"), new DoubleComparer(.000001)));
      Assert.IsTrue(src.Resize(2, 12).ArrayEquals(ArrayReader.GetDoubleArray("Resize2D-1.0.1"), new DoubleComparer(.000001)));
      Assert.IsTrue(src.Resize(12, 2).ArrayEquals(ArrayReader.GetDoubleArray("Resize2D-1.0.2"), new DoubleComparer(.000001)));
      Assert.IsTrue(src.Resize(3, 5).ArrayEquals(ArrayReader.GetDoubleArray("Resize2D-1.0.3"), new DoubleComparer(.000001)));
      Assert.IsTrue(src.Resize(12, 12).ArrayEquals(ArrayReader.GetDoubleArray("Resize2D-1.0.4"), new DoubleComparer(.000001)));

      Assert.IsTrue(src.Resize(add, sub, mul, div, 2, 2).ArrayEquals(ArrayReader.GetDoubleArray("Resize2D-1.0.0"), new DoubleComparer(.0001)));
      Assert.IsTrue(src.Resize(add, sub, mul, div, 2, 12).ArrayEquals(ArrayReader.GetDoubleArray("Resize2D-1.0.1"), new DoubleComparer(.0001)));
      Assert.IsTrue(src.Resize(add, sub, mul, div, 12, 2).ArrayEquals(ArrayReader.GetDoubleArray("Resize2D-1.0.2"), new DoubleComparer(.0001)));
      Assert.IsTrue(src.Resize(add, sub, mul, div, 3, 5).ArrayEquals(ArrayReader.GetDoubleArray("Resize2D-1.0.3"), new DoubleComparer(.0001)));
      Assert.IsTrue(src.Resize(add, sub, mul, div, 12, 12).ArrayEquals(ArrayReader.GetDoubleArray("Resize2D-1.0.4"), new DoubleComparer(.0001)));

      float[,] src2 = Enumerator.Generate<float>(1, 2, r).ToArray(3, 5);

      Assert.IsTrue(src2.Resize(2, 2).ArrayEquals(ArrayReader.GetSingleArray("Resize2D-1.0.0"), new SingleComparer(.00001f)));
      Assert.IsTrue(src2.Resize(2, 12).ArrayEquals(ArrayReader.GetSingleArray("Resize2D-1.0.1"), new SingleComparer(.00001f)));
      Assert.IsTrue(src2.Resize(12, 2).ArrayEquals(ArrayReader.GetSingleArray("Resize2D-1.0.2"), new SingleComparer(.00001f)));
      Assert.IsTrue(src2.Resize(3, 5).ArrayEquals(ArrayReader.GetSingleArray("Resize2D-1.0.3"), new SingleComparer(.00001f)));
      Assert.IsTrue(src2.Resize(12, 12).ArrayEquals(ArrayReader.GetSingleArray("Resize2D-1.0.4"), new SingleComparer(.00001f)));

      Assert.IsTrue(src2.Resize(add, sub, mul, div, 2, 2).ArrayEquals(ArrayReader.GetSingleArray("Resize2D-1.0.0"), new SingleComparer(.0001f)));
      Assert.IsTrue(src2.Resize(add, sub, mul, div, 2, 12).ArrayEquals(ArrayReader.GetSingleArray("Resize2D-1.0.1"), new SingleComparer(.0001f)));
      Assert.IsTrue(src2.Resize(add, sub, mul, div, 12, 2).ArrayEquals(ArrayReader.GetSingleArray("Resize2D-1.0.2"), new SingleComparer(.0001f)));
      Assert.IsTrue(src2.Resize(add, sub, mul, div, 3, 5).ArrayEquals(ArrayReader.GetSingleArray("Resize2D-1.0.3"), new SingleComparer(.0001f)));
      Assert.IsTrue(src2.Resize(add, sub, mul, div, 12, 12).ArrayEquals(ArrayReader.GetSingleArray("Resize2D-1.0.4"), new SingleComparer(.0001f)));
    }

    [TestMethod()]
    public void Resize3DTest()
    {
      int r = 3 * 5 * 7;
      double[, ,] src = Enumerator.Generate<double>(1, 2, r).ToArray(3, 5, 7);

      Assert.IsTrue(src.Resize(2, 2, 3).ArrayEquals(ArrayReader.GetDoubleArray("Resize3D-1.0.0"), new DoubleComparer(.000001)));
      Assert.IsTrue(src.Resize(2, 3, 4).ArrayEquals(ArrayReader.GetDoubleArray("Resize3D-1.0.1"), new DoubleComparer(.000001)));
      Assert.IsTrue(src.Resize(5, 7, 11).ArrayEquals(ArrayReader.GetDoubleArray("Resize3D-1.0.2"), new DoubleComparer(.000001)));
      Assert.IsTrue(src.Resize(3, 5, 7).ArrayEquals(ArrayReader.GetDoubleArray("Resize3D-1.0.3"), new DoubleComparer(.000001)));

      Assert.IsTrue(src.Resize(add, sub, mul, div, 2, 2, 3).ArrayEquals(ArrayReader.GetDoubleArray("Resize3D-1.0.0"), new DoubleComparer(.0001)));
      Assert.IsTrue(src.Resize(add, sub, mul, div, 2, 3, 4).ArrayEquals(ArrayReader.GetDoubleArray("Resize3D-1.0.1"), new DoubleComparer(.0001)));
      Assert.IsTrue(src.Resize(add, sub, mul, div, 5, 7, 11).ArrayEquals(ArrayReader.GetDoubleArray("Resize3D-1.0.2"), new DoubleComparer(.0001)));
      Assert.IsTrue(src.Resize(add, sub, mul, div, 3, 5, 7).ArrayEquals(ArrayReader.GetDoubleArray("Resize3D-1.0.3"), new DoubleComparer(.0001)));

      float[, ,] src2 = Enumerator.Generate<float>(1, 2, r).ToArray(3, 5, 7);

      Assert.IsTrue(src2.Resize(2, 2, 3).ArrayEquals(ArrayReader.GetSingleArray("Resize3D-1.0.0"), new SingleComparer(.0001f)));
      Assert.IsTrue(src2.Resize(2, 3, 4).ArrayEquals(ArrayReader.GetSingleArray("Resize3D-1.0.1"), new SingleComparer(.0001f)));
      Assert.IsTrue(src2.Resize(5, 7, 11).ArrayEquals(ArrayReader.GetSingleArray("Resize3D-1.0.2"), new SingleComparer(.0001f)));
      Assert.IsTrue(src2.Resize(3, 5, 7).ArrayEquals(ArrayReader.GetSingleArray("Resize3D-1.0.3"), new SingleComparer(.0001f)));

      Assert.IsTrue(src2.Resize(add, sub, mul, div, 2, 2, 3).ArrayEquals(ArrayReader.GetSingleArray("Resize3D-1.0.0"), new SingleComparer(.0001f)));
      Assert.IsTrue(src2.Resize(add, sub, mul, div, 2, 3, 4).ArrayEquals(ArrayReader.GetSingleArray("Resize3D-1.0.1"), new SingleComparer(.0001f)));
      Assert.IsTrue(src2.Resize(add, sub, mul, div, 5, 7, 11).ArrayEquals(ArrayReader.GetSingleArray("Resize3D-1.0.2"), new SingleComparer(.0001f)));
      Assert.IsTrue(src2.Resize(add, sub, mul, div, 3, 5, 7).ArrayEquals(ArrayReader.GetSingleArray("Resize3D-1.0.3"), new SingleComparer(.0001f)));
    }

    [TestMethod()]
    public void Resize4DTest()
    {
      int r = 3 * 5 * 7 * 8;
      double[, , ,] src = Enumerator.Generate<double>(1, 2, r).ToArray(3, 5, 7, 8);

      Assert.IsTrue(src.Resize(2, 3, 4, 5).ArrayEquals(ArrayReader.GetDoubleArray("Resize4D-1.0.0"), new DoubleComparer(.000001)));
      Assert.IsTrue(src.Resize(5, 7, 4, 1).ArrayEquals(ArrayReader.GetDoubleArray("Resize4D-1.0.1"), new DoubleComparer(.000001)));
      Assert.IsTrue(src.Resize(3, 5, 7, 8).ArrayEquals(ArrayReader.GetDoubleArray("Resize4D-1.0.2"), new DoubleComparer(.000001)));

      Assert.IsTrue(src.Resize(add, sub, mul, div, 2, 3, 4, 5).ArrayEquals(ArrayReader.GetDoubleArray("Resize4D-1.0.0"), new DoubleComparer(.0001)));
      Assert.IsTrue(src.Resize(add, sub, mul, div, 5, 7, 4, 1).ArrayEquals(ArrayReader.GetDoubleArray("Resize4D-1.0.1"), new DoubleComparer(.0001)));
      Assert.IsTrue(src.Resize(add, sub, mul, div, 3, 5, 7, 8).ArrayEquals(ArrayReader.GetDoubleArray("Resize4D-1.0.2"), new DoubleComparer(.0001)));

      float[, , ,] src2 = Enumerator.Generate<float>(1, 2, r).ToArray(3, 5, 7, 8);

      Assert.IsTrue(src2.Resize(2, 3, 4, 5).ArrayEquals(ArrayReader.GetSingleArray("Resize4D-1.0.0"), new SingleComparer(.0002f)));
      Assert.IsTrue(src2.Resize(5, 7, 4, 1).ArrayEquals(ArrayReader.GetSingleArray("Resize4D-1.0.1"), new SingleComparer(.0002f)));
      Assert.IsTrue(src2.Resize(3, 5, 7, 8).ArrayEquals(ArrayReader.GetSingleArray("Resize4D-1.0.2"), new SingleComparer(.0002f)));

      Assert.IsTrue(src2.Resize(add, sub, mul, div, 2, 3, 4, 5).ArrayEquals(ArrayReader.GetSingleArray("Resize4D-1.0.0"), new SingleComparer(.0002f)));
      Assert.IsTrue(src2.Resize(add, sub, mul, div, 5, 7, 4, 1).ArrayEquals(ArrayReader.GetSingleArray("Resize4D-1.0.1"), new SingleComparer(.0002f)));
      Assert.IsTrue(src2.Resize(add, sub, mul, div, 3, 5, 7, 8).ArrayEquals(ArrayReader.GetSingleArray("Resize4D-1.0.2"), new SingleComparer(.0002f)));
    }

    #endregion

    #region Rotate

    [TestMethod()]
    public void Rotate2DTest()
    {
      int[,] src = Enumerable.Range(0, 12).ToArray(3, 4);
      int[,] expectedLeft = new int[,] { { 3, 7, 11 }, { 2, 6, 10 }, { 1, 5, 9 }, { 0, 4, 8 } };
      int[,] expectedRight = new int[,] { { 8, 4, 0 }, { 9, 5, 1 }, { 10, 6, 2 }, { 11, 7, 3 } };
      int[,] expected180 = new int[,] { { 11, 10, 9, 8 }, { 7, 6, 5, 4 }, { 3, 2, 1, 0 } };

      int[,] actualLeft = src.Rotate(270);
      int[,] actualRight = src.Rotate(90);
      int[,] actual180 = src.Rotate(180);
      int[,] actual000 = src.Rotate(360);

      try
      {
        src.Rotate(272);
        Assert.Fail();
      }
      catch (System.ArgumentException ex)
      { Assert.IsTrue(ex.ParamName == "angle"); }
      catch (System.Exception)
      { Assert.Fail(); }

      Assert.IsTrue(src.ArrayEquals(src.Rotate(90).Rotate(90).Rotate(90).Rotate(90)));
      Assert.IsTrue(src.ArrayEquals(src.Rotate(180).Rotate(90).Rotate(90)));
      Assert.IsTrue(src.ArrayEquals(src.Rotate(270).Rotate(90)));

      Assert.IsTrue(expectedLeft.ArrayEquals(actualLeft));
      Assert.IsTrue(expectedRight.ArrayEquals(actualRight));
      Assert.IsTrue(expected180.ArrayEquals(actual180));
      Assert.IsTrue(src.ArrayEquals(actual000));
    }

    [TestMethod()]
    public void Rotate3DTest()
    {
      int[, ,] src = Enumerable.Range(0, 24).ToArray(2, 3, 4);

      int[, ,] expected90Z = new int[,,] { { { 8, 4, 0 }, { 9, 5, 1 }, { 10, 6, 2 }, { 11, 7, 3 } }, { { 20, 16, 12 }, { 21, 17, 13 }, { 22, 18, 14 }, { 23, 19, 15 } } };
      int[, ,] expected90Y = new int[,,] { { { 3, 15 }, { 7, 19 }, { 11, 23 } }, { { 2, 14 }, { 6, 18 }, { 10, 22 } }, { { 1, 13 }, { 5, 17 }, { 9, 21 } }, { { 0, 12 }, { 4, 16 }, { 8, 20 } } };
      int[, ,] expected90X = new int[,,] { { { 12, 13, 14, 15 }, { 0, 1, 2, 3 } }, { { 16, 17, 18, 19 }, { 4, 5, 6, 7 } }, { { 20, 21, 22, 23 }, { 8, 9, 10, 11 } } };

      int[, ,] expected270Z = new int[,,] { { { 3, 7, 11 }, { 2, 6, 10 }, { 1, 5, 9 }, { 0, 4, 8 } }, { { 15, 19, 23 }, { 14, 18, 22 }, { 13, 17, 21 }, { 12, 16, 20 } } };
      int[, ,] expected270Y = new int[,,] { { { 12, 0 }, { 16, 4 }, { 20, 8 } }, { { 13, 1 }, { 17, 5 }, { 21, 9 } }, { { 14, 2 }, { 18, 6 }, { 22, 10 } }, { { 15, 3 }, { 19, 7 }, { 23, 11 } } };
      int[, ,] expected270X = new int[,,] { { { 8, 9, 10, 11 }, { 20, 21, 22, 23 } }, { { 4, 5, 6, 7 }, { 16, 17, 18, 19 } }, { { 0, 1, 2, 3 }, { 12, 13, 14, 15 } } };

      int[, ,] expected180Z = new int[,,] { { { 11, 10, 9, 8 }, { 7, 6, 5, 4 }, { 3, 2, 1, 0 } }, { { 23, 22, 21, 20 }, { 19, 18, 17, 16 }, { 15, 14, 13, 12 } } };
      int[, ,] expected180Y = new int[,,] { { { 15, 14, 13, 12 }, { 19, 18, 17, 16 }, { 23, 22, 21, 20 } }, { { 3, 2, 1, 0 }, { 7, 6, 5, 4 }, { 11, 10, 9, 8 } } };
      int[, ,] expected180X = new int[,,] { { { 20, 21, 22, 23 }, { 16, 17, 18, 19 }, { 12, 13, 14, 15 } }, { { 8, 9, 10, 11 }, { 4, 5, 6, 7 }, { 0, 1, 2, 3 } } };

      int[, ,] actual90Z = src.Rotate(RotateAxis.RotateZ, 90);
      int[, ,] actual90Y = src.Rotate(RotateAxis.RotateY, 90);
      int[, ,] actual90X = src.Rotate(RotateAxis.RotateX, 90);

      int[, ,] actual270Z = src.Rotate(RotateAxis.RotateZ, 270);
      int[, ,] actual270Y = src.Rotate(RotateAxis.RotateY, 270);
      int[, ,] actual270X = src.Rotate(RotateAxis.RotateX, 270);

      int[, ,] actual180Z = src.Rotate(RotateAxis.RotateZ, 180);
      int[, ,] actual180Y = src.Rotate(RotateAxis.RotateY, 180);
      int[, ,] actual180X = src.Rotate(RotateAxis.RotateX, 180);

      int[, ,] actual000Z = src.Rotate(RotateAxis.RotateZ, 360);
      int[, ,] actual000Y = src.Rotate(RotateAxis.RotateY, 360);
      int[, ,] actual000X = src.Rotate(RotateAxis.RotateX, 360);

      Assert.IsTrue(src.ArrayEquals(src.Rotate(RotateAxis.RotateX, 90).Rotate(RotateAxis.RotateX, 90).Rotate(RotateAxis.RotateX, 90).Rotate(RotateAxis.RotateX, 90)));
      Assert.IsTrue(src.ArrayEquals(src.Rotate(RotateAxis.RotateX, 180).Rotate(RotateAxis.RotateX, 90).Rotate(RotateAxis.RotateX, 90)));
      Assert.IsTrue(src.ArrayEquals(src.Rotate(RotateAxis.RotateX, 270).Rotate(RotateAxis.RotateX, 90)));

      Assert.IsTrue(src.ArrayEquals(src.Rotate(RotateAxis.RotateY, 90).Rotate(RotateAxis.RotateY, 90).Rotate(RotateAxis.RotateY, 90).Rotate(RotateAxis.RotateY, 90)));
      Assert.IsTrue(src.ArrayEquals(src.Rotate(RotateAxis.RotateY, 180).Rotate(RotateAxis.RotateY, 90).Rotate(RotateAxis.RotateY, 90)));
      Assert.IsTrue(src.ArrayEquals(src.Rotate(RotateAxis.RotateY, 270).Rotate(RotateAxis.RotateY, 90)));

      Assert.IsTrue(src.ArrayEquals(src.Rotate(RotateAxis.RotateZ, 90).Rotate(RotateAxis.RotateZ, 90).Rotate(RotateAxis.RotateZ, 90).Rotate(RotateAxis.RotateZ, 90)));
      Assert.IsTrue(src.ArrayEquals(src.Rotate(RotateAxis.RotateZ, 180).Rotate(RotateAxis.RotateZ, 90).Rotate(RotateAxis.RotateZ, 90)));
      Assert.IsTrue(src.ArrayEquals(src.Rotate(RotateAxis.RotateZ, 270).Rotate(RotateAxis.RotateZ, 90)));

      Assert.IsTrue(src.ArrayEquals(src.Rotate(RotateAxis.RotateX, 90).Rotate(RotateAxis.RotateY, 90).Rotate(RotateAxis.RotateZ, 90)
                                       .Rotate(RotateAxis.RotateX, 90).Rotate(RotateAxis.RotateY, 90).Rotate(RotateAxis.RotateZ, 90)
                                       .Rotate(RotateAxis.RotateX, 90).Rotate(RotateAxis.RotateY, 90).Rotate(RotateAxis.RotateZ, 90)
                                       .Rotate(RotateAxis.RotateX, 90).Rotate(RotateAxis.RotateY, 90).Rotate(RotateAxis.RotateZ, 90)));
      try
      {
        src.Rotate(RotateAxis.RotateX, 272);
        Assert.Fail();
      }
      catch (System.ArgumentException ex)
      { Assert.IsTrue(ex.ParamName == "angle"); }
      catch (System.Exception)
      { Assert.Fail(); }

      try
      {
        src.Rotate(RotateAxis.RotateA, 180);
        Assert.Fail();
      }
      catch (System.ArgumentException ex)
      { Assert.IsTrue(ex.ParamName == "axis"); }
      catch (System.Exception)
      { Assert.Fail(); }

      Assert.IsTrue(expected90Z.ArrayEquals(actual90Z));
      Assert.IsTrue(expected90Y.ArrayEquals(actual90Y));
      Assert.IsTrue(expected90X.ArrayEquals(actual90X));

      Assert.IsTrue(expected270Z.ArrayEquals(actual270Z));
      Assert.IsTrue(expected270Y.ArrayEquals(actual270Y));
      Assert.IsTrue(expected270X.ArrayEquals(actual270X));

      Assert.IsTrue(expected180Z.ArrayEquals(actual180Z));
      Assert.IsTrue(expected180Y.ArrayEquals(actual180Y));
      Assert.IsTrue(expected180X.ArrayEquals(actual180X));

      Assert.IsTrue(src.ArrayEquals(actual000Z));
      Assert.IsTrue(src.ArrayEquals(actual000Y));
      Assert.IsTrue(src.ArrayEquals(actual000X));

    }

    [TestMethod()]
    public void Rotate4DTest()
    {
      int[, , ,] src = Enumerable.Range(0, 360).ToArray(3, 4, 5, 6);

      try
      {
        src.Rotate(RotateAxis.RotateX, 272);
        Assert.Fail();
      }
      catch (System.ArgumentException ex)
      { Assert.IsTrue(ex.ParamName == "angle"); }
      catch (System.Exception)
      { Assert.Fail(); }

      try
      {
        src.Rotate(RotateAxis.RotateB, 180);
        Assert.Fail();
      }
      catch (System.ArgumentException ex)
      { Assert.IsTrue(ex.ParamName == "axis"); }
      catch (System.Exception)
      { Assert.Fail(); }

      Assert.IsTrue(src.ArrayEquals(src.Rotate(RotateAxis.RotateX, 90).Rotate(RotateAxis.RotateX, 90).Rotate(RotateAxis.RotateX, 90).Rotate(RotateAxis.RotateX, 90).Rotate(RotateAxis.RotateX, 90).Rotate(RotateAxis.RotateX, 90)));
      Assert.IsTrue(src.ArrayEquals(src.Rotate(RotateAxis.RotateX, 180).Rotate(RotateAxis.RotateX, 90).Rotate(RotateAxis.RotateX, 90).Rotate(RotateAxis.RotateX, 90).Rotate(RotateAxis.RotateX, 90)));
      Assert.IsTrue(src.ArrayEquals(src.Rotate(RotateAxis.RotateX, 270).Rotate(RotateAxis.RotateX, 90).Rotate(RotateAxis.RotateX, 90).Rotate(RotateAxis.RotateX, 90)));
      Assert.IsTrue(src.ArrayEquals(src.Rotate(RotateAxis.RotateX, 360).Rotate(RotateAxis.RotateX, 90).Rotate(RotateAxis.RotateX, 90)));
      Assert.IsTrue(src.ArrayEquals(src.Rotate(RotateAxis.RotateX, 450).Rotate(RotateAxis.RotateX, 90)));

      Assert.IsTrue(src.ArrayEquals(src.Rotate(RotateAxis.RotateY, 90).Rotate(RotateAxis.RotateY, 90).Rotate(RotateAxis.RotateY, 90).Rotate(RotateAxis.RotateY, 90).Rotate(RotateAxis.RotateY, 90).Rotate(RotateAxis.RotateY, 90)));
      Assert.IsTrue(src.ArrayEquals(src.Rotate(RotateAxis.RotateY, 180).Rotate(RotateAxis.RotateY, 90).Rotate(RotateAxis.RotateY, 90).Rotate(RotateAxis.RotateY, 90).Rotate(RotateAxis.RotateY, 90)));
      Assert.IsTrue(src.ArrayEquals(src.Rotate(RotateAxis.RotateY, 270).Rotate(RotateAxis.RotateY, 90).Rotate(RotateAxis.RotateY, 90).Rotate(RotateAxis.RotateY, 90)));
      Assert.IsTrue(src.ArrayEquals(src.Rotate(RotateAxis.RotateY, 360).Rotate(RotateAxis.RotateY, 90).Rotate(RotateAxis.RotateY, 90)));
      Assert.IsTrue(src.ArrayEquals(src.Rotate(RotateAxis.RotateY, 450).Rotate(RotateAxis.RotateY, 90)));

      Assert.IsTrue(src.ArrayEquals(src.Rotate(RotateAxis.RotateZ, 90).Rotate(RotateAxis.RotateZ, 90).Rotate(RotateAxis.RotateZ, 90).Rotate(RotateAxis.RotateZ, 90).Rotate(RotateAxis.RotateZ, 90).Rotate(RotateAxis.RotateZ, 90)));
      Assert.IsTrue(src.ArrayEquals(src.Rotate(RotateAxis.RotateZ, 180).Rotate(RotateAxis.RotateZ, 90).Rotate(RotateAxis.RotateZ, 90).Rotate(RotateAxis.RotateZ, 90).Rotate(RotateAxis.RotateZ, 90)));
      Assert.IsTrue(src.ArrayEquals(src.Rotate(RotateAxis.RotateZ, 270).Rotate(RotateAxis.RotateZ, 90).Rotate(RotateAxis.RotateZ, 90).Rotate(RotateAxis.RotateZ, 90)));
      Assert.IsTrue(src.ArrayEquals(src.Rotate(RotateAxis.RotateZ, 360).Rotate(RotateAxis.RotateZ, 90).Rotate(RotateAxis.RotateZ, 90)));
      Assert.IsTrue(src.ArrayEquals(src.Rotate(RotateAxis.RotateZ, 450).Rotate(RotateAxis.RotateZ, 90)));

      Assert.IsTrue(src.ArrayEquals(src.Rotate(RotateAxis.RotateA, 90).Rotate(RotateAxis.RotateA, 90).Rotate(RotateAxis.RotateA, 90).Rotate(RotateAxis.RotateA, 90).Rotate(RotateAxis.RotateA, 90).Rotate(RotateAxis.RotateA, 90)));
      Assert.IsTrue(src.ArrayEquals(src.Rotate(RotateAxis.RotateA, 180).Rotate(RotateAxis.RotateA, 90).Rotate(RotateAxis.RotateA, 90).Rotate(RotateAxis.RotateA, 90).Rotate(RotateAxis.RotateA, 90)));
      Assert.IsTrue(src.ArrayEquals(src.Rotate(RotateAxis.RotateA, 270).Rotate(RotateAxis.RotateA, 90).Rotate(RotateAxis.RotateA, 90).Rotate(RotateAxis.RotateA, 90)));
      Assert.IsTrue(src.ArrayEquals(src.Rotate(RotateAxis.RotateA, 360).Rotate(RotateAxis.RotateA, 90).Rotate(RotateAxis.RotateA, 90)));
      Assert.IsTrue(src.ArrayEquals(src.Rotate(RotateAxis.RotateA, 450).Rotate(RotateAxis.RotateA, 90)));

      Assert.IsTrue(src.ArrayEquals(src.Rotate(RotateAxis.RotateX, 180).Rotate(RotateAxis.RotateY, 180).Rotate(RotateAxis.RotateZ, 180).Rotate(RotateAxis.RotateA, 180)
                                       .Rotate(RotateAxis.RotateX, 180).Rotate(RotateAxis.RotateY, 180).Rotate(RotateAxis.RotateZ, 180).Rotate(RotateAxis.RotateA, 180)
                                       .Rotate(RotateAxis.RotateX, 180).Rotate(RotateAxis.RotateY, 180).Rotate(RotateAxis.RotateZ, 180).Rotate(RotateAxis.RotateA, 180)
                                       .Rotate(RotateAxis.RotateX, 180).Rotate(RotateAxis.RotateY, 180).Rotate(RotateAxis.RotateZ, 180).Rotate(RotateAxis.RotateA, 180)));

      Assert.IsTrue(src.ArrayEquals(src.Rotate(RotateAxis.RotateA, 540 + 270).Rotate(RotateAxis.RotateA, 270)));

      Assert.IsTrue(src.ArrayEquals(src.Rotate(RotateAxis.RotateNone, 450)));
      Assert.IsTrue(src.ArrayEquals(src.Rotate(RotateAxis.RotateNone, 540)));
    }

    #endregion

    #region Flip

    [TestMethod()]
    public void Flip1DTest()
    {
      double[] src = Enumerator.Generate<double>(0, 1, 12).ToArray();

      Assert.IsTrue(src.Flip().ArrayEquals(ArrayReader.GetDoubleArray("Flip1D-1")));
    }

    [TestMethod()]
    public void Flip2DTest()
    {
      double[,] src = Enumerator.Generate<double>(0, 1, 12).ToArray(3, 4);

      Assert.IsTrue(src.Flip(FlipAxis.None).ArrayEquals(src));
      Assert.IsTrue(src.Flip(FlipAxis.FlipX).ArrayEquals(ArrayReader.GetDoubleArray("Flip2D-1.X")));
      Assert.IsTrue(src.Flip(FlipAxis.FlipY).ArrayEquals(ArrayReader.GetDoubleArray("Flip2D-1.Y")));
      Assert.IsTrue(src.Flip(FlipAxis.FlipXY).ArrayEquals(ArrayReader.GetDoubleArray("Flip2D-1.XY")));

      try
      {
        src.Flip(FlipAxis.FlipZ);
        Assert.Fail();
      }
      catch (System.ArgumentException ex)
      {
        Assert.IsTrue(ex.ParamName == "axis");
      }
      catch
      {
        Assert.Fail();
      }
    }

    [TestMethod()]
    public void Flip3DTest()
    {
      double[, ,] src = Enumerator.Generate<double>(0, 1, 24).ToArray(2, 3, 4);

      Assert.IsTrue(src.Flip(FlipAxis.None).ArrayEquals(src));
      Assert.IsTrue(src.Flip(FlipAxis.FlipX).ArrayEquals(ArrayReader.GetDoubleArray("Flip3D-1.X")));
      Assert.IsTrue(src.Flip(FlipAxis.FlipY).ArrayEquals(ArrayReader.GetDoubleArray("Flip3D-1.Y")));
      Assert.IsTrue(src.Flip(FlipAxis.FlipZ).ArrayEquals(ArrayReader.GetDoubleArray("Flip3D-1.Z")));

      Assert.IsTrue(src.Flip(FlipAxis.FlipXY).ArrayEquals(ArrayReader.GetDoubleArray("Flip3D-1.XY")));
      Assert.IsTrue(src.Flip(FlipAxis.FlipXZ).ArrayEquals(ArrayReader.GetDoubleArray("Flip3D-1.XZ")));
      Assert.IsTrue(src.Flip(FlipAxis.FlipYZ).ArrayEquals(ArrayReader.GetDoubleArray("Flip3D-1.YZ")));

      Assert.IsTrue(src.Flip(FlipAxis.FlipXYZ).ArrayEquals(ArrayReader.GetDoubleArray("Flip3D-1.XYZ")));

      try
      {
        src.Flip((FlipAxis)63);
        Assert.Fail();
      }
      catch (System.ArgumentException ex)
      {
        Assert.IsTrue(ex.ParamName == "axis");
      }
      catch
      {
        Assert.Fail();
      }

    }
    #endregion

    #region Replace and Extract

    [TestMethod()]
    public void Replace1Test()
    {
      int[] src1 = Enumerable.Range(0, 12).ToArray(12);
      int[] src2 = Enumerable.Range(991, 3).ToArray(3);

      int[] expected = Enumerable.Range(0, 12).ToArray(12);
      int offset = 3;
      for (int i = 0; i < src2.Length; i++)
        expected[i + offset] = src2[i];

      src1.Replace(src2, offset);
      Assert.IsTrue(expected.ArrayEquals(src1));
    }

    [TestMethod()]
    public void Replace2Test()
    {
      int[,] src1 = Enumerable.Range(0, 12).ToArray(3, 4);
      int[,] src2 = Enumerable.Range(991, 4).ToArray(2, 2);

      int[,] expected = new int[,] { { 0, 1, 2, 3 }, { 4, 5, 991, 992 }, { 8, 9, 993, 994 } };

      int offsetX = 2;
      int offsetY = 1;

      src1.Replace(src2, offsetY, offsetX);
      Assert.IsTrue(expected.ArrayEquals(src1));
    }

    [TestMethod()]
    public void Replace3Test()
    {
      int[, ,] src1 = Enumerable.Range(0, 27).ToArray(3, 3, 3);
      int[, ,] src2 = Enumerable.Range(991, 8).ToArray(2, 2, 2);

      int[, ,] expected = new int[,,] { { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 } }, { { 9, 10, 11 }, { 12, 991, 992 }, { 15, 993, 994 } }, { { 18, 19, 20 }, { 21, 995, 996 }, { 24, 997, 998 } } };

      int offsetX = 1;
      int offsetY = 1;
      int offsetZ = 1;

      src1.Replace(src2, offsetZ, offsetY, offsetX);
      Assert.IsTrue(expected.ArrayEquals(src1));
    }

    [TestMethod()]
    public void Replace4Test()
    {
      int[, , ,] src1 = Enumerable.Range(0, 36).ToArray(2, 2, 3, 3);
      int[, , ,] src2 = Enumerable.Range(981, 16).ToArray(2, 2, 2, 2);

      int[, , ,] expected = new int[,,,] { { { { 0, 1, 2 }, { 3, 981, 982 }, { 6, 983, 984 } }, { { 9, 10, 11 }, { 12, 985, 986 }, { 15, 987, 988 } } }, { { { 18, 19, 20 }, { 21, 989, 990 }, { 24, 991, 992 } }, { { 27, 28, 29 }, { 30, 993, 994 }, { 33, 995, 996 } } } };

      int offsetX = 1;
      int offsetY = 1;
      int offsetZ = 0;
      int offsetA = 0;

      src1.Replace(src2, offsetZ, offsetA, offsetY, offsetX);
      Assert.IsTrue(expected.ArrayEquals(src1));
    }

    [TestMethod()]
    public void Extract1Test()
    {
      int[] src1 = Enumerable.Range(0, 12).ToArray(12);
      int[] expected = Enumerable.Range(3, 4).ToArray(4);
      int offset = 3;

      Assert.IsTrue(src1.Extract(offset, 4).ArrayEquals(expected));
    }

    [TestMethod()]
    public void Extract2Test()
    {
      double[,] src1 = (double[,])ArrayReader.GetDoubleArray("Extract3-0");
      double[,] expected = Enumerator.Generate<double>(991, 1, 4).ToArray(2, 2);
      double[,] expected2 = new double[,] { { 5, 991 }, { 9, 993 } };

      int offsetX = 2;
      int offsetY = 1;

      Assert.IsTrue(src1.Extract(offsetY, offsetX, 2, 2).ArrayEquals(expected));

      offsetX = 1;
      offsetY = 1;

      Assert.IsTrue(src1.Extract(offsetY, offsetX, 2, 2).ArrayEquals(expected2));
    }

    [TestMethod()]
    public void Extract3Test()
    {
      double[, ,] src1 = (double[, ,])ArrayReader.GetDoubleArray("Extract3-1");
      double[, ,] expected = Enumerator.Generate<double>(991, 1, 8).ToArray(2, 2, 2);

      int offsetX = 1;
      int offsetY = 1;
      int offsetZ = 1;

      Assert.IsTrue(src1.Extract(offsetZ, offsetY, offsetX, 2, 2, 2).ArrayEquals(expected));
    }

    [TestMethod()]
    public void Extract4Test()
    {
      double[, , ,] src1 = (double[, , ,])ArrayReader.GetDoubleArray("Extract4-1");
      double[, , ,] expected = Enumerator.Generate<double>(981, 1, 16).ToArray(2, 2, 2, 2);

      int offsetX = 1;
      int offsetY = 1;
      int offsetZ = 0;
      int offsetA = 0;

      Assert.IsTrue(src1.Extract(offsetA, offsetZ, offsetY, offsetX, 2, 2, 2, 2).ArrayEquals(expected));
    }
    #endregion

    #region Jagged / Fixed Converters

    [TestMethod()]
    public void ToFromJagged2DTest()
    {
      int[][] arr = new int[7][];

      arr[0] = new int[] { 1, 2, 3, 4, 5 };
      arr[1] = null;
      arr[2] = new int[0];
      arr[3] = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
      arr[4] = new int[] { 1, 2, 3 };
      arr[5] = new int[0];
      arr[6] = null;

      int[,] actual1 = arr.FromJagged();

      int[,] expected1 = new int[,] { { 1, 2, 3, 4, 5, 0, 0, 0, 0 }, 
                                      { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
                                      { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
                                      { 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                                      { 1, 2, 3, 0, 0, 0, 0, 0, 0 },
                                      { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
                                      { 0, 0, 0, 0, 0, 0, 0, 0, 0 } };

      Assert.IsTrue(actual1.ArrayEquals(expected1));

      int[,] actual2 = arr.FromJagged(4, 6);

      int[,] expected2 = new int[,] { { 1, 2, 3, 4, 5, 0 }, 
                                      { 0, 0, 0, 0, 0, 0 }, 
                                      { 0, 0, 0, 0, 0, 0 }, 
                                      { 1, 2, 3, 4, 5, 6 } };

      Assert.IsTrue(actual2.ArrayEquals(expected2));

      int[][] actualRT = actual1.ToJagged();

      int[][] expectedRT = arr;

      for (int i = 0; i < actualRT.Length; i++)
        if (actualRT[i] != null)
          Assert.IsTrue((actualRT[i].Length == 0 && expectedRT[i] == null) || actualRT[i].ArrayEquals(expectedRT[i]));

      int[][] actualRF = actual1.ToJagged(false);

      int[][] expectedRF = new int[7][];
      expectedRF[0] = new int[] { 1, 2, 3, 4, 5, 0, 0, 0, 0 };
      expectedRF[1] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
      expectedRF[2] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
      expectedRF[3] = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
      expectedRF[4] = new int[] { 1, 2, 3, 0, 0, 0, 0, 0, 0 };
      expectedRF[5] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
      expectedRF[6] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

      for (int i = 0; i < actualRF.Length; i++)
        Assert.IsTrue(actualRF[i].ArrayEquals(expectedRF[i]));
    }

    [TestMethod()]
    public void ToFromJagged3DTest()
    {
      int[][][] arr = GetJagged3DArray();

      int[, ,] actual1 = arr.FromJagged();

      int[, ,] expected1 = new int[,,] { { { 1, 2, 3, 4, 5, 0 }, { 0, 0, 0, 0, 0, 0 }, { 1, 2, 3, 0, 0, 0 } }, 
                                         { { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } },
                                         { { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } },
                                         { { 1, 2, 0, 0, 0, 0 }, { 1, 0, 0, 0, 0, 0 }, { 1, 2, 0, 0, 0, 0 } },
                                         { { 1, 2, 3, 4, 5, 6 }, { 1, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } },
                                         { { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } },
                                         { { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } } };

      Assert.IsTrue(actual1.ArrayEquals(expected1));

      int[, ,] actual2 = arr.FromJagged(4, 2, 3);

      int[, ,] expected2 = new int[,,] { { { 1, 2, 3 }, { 0, 0, 0 } }, 
                                         { { 0, 0, 0 }, { 0, 0, 0 } },
                                         { { 0, 0, 0 }, { 0, 0, 0 } },
                                         { { 1, 2, 0 }, { 1, 0, 0 } } };

      Assert.IsTrue(actual2.ArrayEquals(expected2));

      int[][][] actualRT = actual1.ToJagged();
      int[][][] expectedRT = arr;

      for (int i1 = 0; i1 < actualRT.Length; i1++)
        if (actualRT[i1] != null)
          for (int i2 = 0; i2 < actualRT[i1].Length; i2++)
            if (actualRT[i1][i2] != null)
              Assert.IsTrue((actualRT[i1][i2].Length == 0 && expectedRT[i1][i2] == null) || actualRT[i1][i2].ArrayEquals(expectedRT[i1][i2]));

      int[][][] actualRF = actual1.ToJagged(false);

      int[][][] expectedRF = GetJagged3DArray2();

      for (int i1 = 0; i1 < actualRF.Length; i1++)
        if (actualRF[i1] != null)
          for (int i2 = 0; i2 < actualRF[i1].Length; i2++)
            if (actualRF[i1][i2] != null)
              Assert.IsTrue((actualRF[i1][i2].Length == 0 && expectedRF[i1][i2] == null) || actualRF[i1][i2].ArrayEquals(expectedRF[i1][i2]));
    }

    [TestMethod()]
    public void ToFromJagged4DTest()
    {
      int[][][][] arr = new int[][][][] { GetJagged3DArray(), null, new int[0][][], GetJagged3DArray() };

      int[, , ,] actual1 = arr.FromJagged();

      int[, , ,] expected1 = new int[,,,] { { { { 1, 2, 3, 4, 5, 0 }, { 0, 0, 0, 0, 0, 0 }, { 1, 2, 3, 0, 0, 0 } }, 
                                              { { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } }, 
                                              { { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } }, 
                                              { { 1, 2, 0, 0, 0, 0 }, { 1, 0, 0, 0, 0, 0 }, { 1, 2, 0, 0, 0, 0 } }, 
                                              { { 1, 2, 3, 4, 5, 6 }, { 1, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } }, 
                                              { { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } }, 
                                              { { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } } },
                                            { { { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } }, 
                                              { { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } }, 
                                              { { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } }, 
                                              { { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } }, 
                                              { { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } }, 
                                              { { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } }, 
                                              { { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } } },                                                  
                                            { { { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } }, 
                                              { { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } }, 
                                              { { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } }, 
                                              { { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } }, 
                                              { { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } }, 
                                              { { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } }, 
                                              { { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } } },
                                            { { { 1, 2, 3, 4, 5, 0 }, { 0, 0, 0, 0, 0, 0 }, { 1, 2, 3, 0, 0, 0 } }, 
                                              { { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } }, 
                                              { { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } }, 
                                              { { 1, 2, 0, 0, 0, 0 }, { 1, 0, 0, 0, 0, 0 }, { 1, 2, 0, 0, 0, 0 } }, 
                                              { { 1, 2, 3, 4, 5, 6 }, { 1, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } }, 
                                              { { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } }, 
                                              { { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } } } };

      Assert.IsTrue(actual1.ArrayEquals(expected1));

      int[, , ,] actual2 = arr.FromJagged(2, 4, 2, 3);

      int[, , ,] expected2 = new int[,,,] { { { { 1, 2, 3 }, { 0, 0, 0 } },
                                              { { 0, 0, 0 }, { 0, 0, 0 } }, 
                                              { { 0, 0, 0 }, { 0, 0, 0 } }, 
                                              { { 1, 2, 0 }, { 1, 0, 0 } } },
                                            { { { 0, 0, 0 }, { 0, 0, 0 } }, 
                                              { { 0, 0, 0 }, { 0, 0, 0 } }, 
                                              { { 0, 0, 0 }, { 0, 0, 0 } }, 
                                              { { 0, 0, 0 }, { 0, 0, 0 } } } };

      Assert.IsTrue(actual2.ArrayEquals(expected2));

      int[][][][] actualRT = actual1.ToJagged();
      int[][][][] expectedRT = arr;

      for (int i1 = 0; i1 < actualRT.Length; i1++)
        if (actualRT[i1] != null)
          for (int i2 = 0; i2 < actualRT[i1].Length; i2++)
            if (actualRT[i1][i2] != null)
              for (int i3 = 0; i3 < actualRT[i1][i2].Length; i3++)
                if (actualRT[i1][i2][i3] != null)
                  Assert.IsTrue((actualRT[i1][i2][i3].Length == 0 && expectedRT[i1][i2][i3] == null) || actualRT[i1][i2][i3].ArrayEquals(expectedRT[i1][i2][i3]));

      int[][][][] actualRF = actual1.ToJagged(false);
      int[][][][] expectedRF = new int[][][][] { GetJagged3DArray2(), GetJagged3DArray3(), GetJagged3DArray3(), GetJagged3DArray2() };

      for (int i1 = 0; i1 < actualRF.Length; i1++)
        if (actualRF[i1] != null)
          for (int i2 = 0; i2 < actualRF[i1].Length; i2++)
            if (actualRF[i1][i2] != null)
              for (int i3 = 0; i3 < actualRF[i1][i2].Length; i3++)
                if (actualRF[i1][i2][i3] != null)
                  Assert.IsTrue((actualRF[i1][i2][i3].Length == 0 && expectedRF[i1][i2][i3] == null) || actualRF[i1][i2][i3].ArrayEquals(expectedRF[i1][i2][i3]));
    }

    private int[][][] GetJagged3DArray()
    {
      int[][][] arr = new int[7][][];
      arr[0] = new int[3][] { new int[] { 1, 2, 3, 4, 5 }, new int[] { }, new int[] { 1, 2, 3 } };
      arr[1] = null;
      arr[2] = new int[0][];
      arr[3] = new int[3][] { new int[] { 1, 2 }, new int[] { 1 }, new int[] { 1, 2 } };
      arr[4] = new int[2][] { new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 1 } };
      arr[5] = new int[0][];
      arr[6] = null;
      return arr;
    }

    private static int[][][] GetJagged3DArray2()
    {
      int[][][] arr = new int[7][][];
      arr[0] = new int[3][] { new int[] { 1, 2, 3, 4, 5, 0 }, new int[] { 0, 0, 0, 0, 0, 0 }, new int[] { 1, 2, 3, 0, 0, 0 } };
      arr[1] = new int[3][] { new int[] { 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0 } };
      arr[2] = new int[3][] { new int[] { 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0 } };
      arr[3] = new int[3][] { new int[] { 1, 2, 0, 0, 0, 0 }, new int[] { 1, 0, 0, 0, 0, 0 }, new int[] { 1, 2, 0, 0, 0, 0 } };
      arr[4] = new int[3][] { new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 1, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0 } };
      arr[5] = new int[3][] { new int[] { 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0 } };
      arr[6] = new int[3][] { new int[] { 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0 } };
      return arr;
    }

    private static int[][][] GetJagged3DArray3()
    {
      int[][][] arr = new int[7][][];
      arr[0] = new int[3][] { new int[] { 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0 } };
      arr[1] = new int[3][] { new int[] { 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0 } };
      arr[2] = new int[3][] { new int[] { 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0 } };
      arr[3] = new int[3][] { new int[] { 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0 } };
      arr[4] = new int[3][] { new int[] { 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0 } };
      arr[5] = new int[3][] { new int[] { 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0 } };
      arr[6] = new int[3][] { new int[] { 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0 } };
      return arr;
    }

    #endregion

    #region Misc

    [TestMethod()]
    public void ArrayEqualsTest()
    {
      int[,] arr = Enumerable.Range(0, 12).ToArray(3, 4);

      int[] arr2 = Enumerable.Range(0, 12).ToArray(12);
      int[,] arr3 = Enumerable.Range(0, 12).ToArray(2, 6);
      int[,] arr4 = Enumerable.Range(0, 9).ToArray(3, 3);
      int[,] arr5 = Enumerable.Range(100, 12).ToArray(3, 4);

      Assert.IsFalse(arr.ArrayEquals(arr2));
      Assert.IsFalse(arr.ArrayEquals(arr3));
      Assert.IsFalse(arr.ArrayEquals(arr4));
      Assert.IsFalse(arr.ArrayEquals(arr5));
    }

    [TestMethod()]
    public void EqualityComparerTest()
    {
      SingleComparer sc = new SingleComparer();
      Assert.IsTrue(sc.Equals(1.00001f, 1.000011f));
      Assert.IsFalse(sc.Equals(1.00001f, 1.00003f));

      Assert.IsTrue(sc.GetHashCode(1.00001f) == sc.GetHashCode(1.00001f));
      Assert.IsFalse(sc.GetHashCode(1.00001f) == sc.GetHashCode(1.000011f));

      DoubleComparer dc = new DoubleComparer();
      Assert.IsTrue(dc.Equals(1.000001, 1.0000011));
      Assert.IsFalse(dc.Equals(1.000001, 1.000003));

      Assert.IsTrue(dc.GetHashCode(1.000001) == dc.GetHashCode(1.000001));
      Assert.IsFalse(dc.GetHashCode(1.000001) == dc.GetHashCode(1.0000011));
    }

    #endregion
  }

}

