using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqLib.Array;

namespace LinqLibTests.Array
{
  public static class ArrayReader
  {
    const string START_OF_ARRAY = "~~SOA~~";
    const string END_OF_ARRAY = "~~EOA~~";
    const string NAME_REFIX = "A:";
    const string DIM_REFIX = "D:";

    private static Dictionary<string, System.Array> data;

    static ArrayReader()
    {
      data = new Dictionary<string, System.Array>();
      StringBuilder arrBldr = new StringBuilder();
      bool soa = false;
      foreach (string arrLine in LinqLibTests.Properties.Resources.ExpectedArrayData.Split(new char[] { '\r', '\n' }))
      {
        if (arrLine.StartsWith("//"))
          continue;

        soa |= arrLine.IndexOf(START_OF_ARRAY) != -1;

        arrBldr.Append(arrLine);
        if (arrLine.EndsWith(END_OF_ARRAY))
        {
          ProcessArray(arrBldr.ToString().Trim());
          arrBldr.Clear();
          soa = false;
        }
        else if (soa)
          arrBldr.Append(",");
      }
    }

    private static void ProcessArray(string arrayData)
    {
      int dimPos = arrayData.IndexOf(DIM_REFIX);
      int startPos = arrayData.IndexOf(START_OF_ARRAY);
      int arrLen = arrayData.IndexOf(END_OF_ARRAY) - startPos;
      int[] dims = new int[] { };

      int endOfName;
      if (dimPos == -1) //No dim token
        endOfName = startPos - 1;
      else
      {
        string dimsSection = new string(arrayData.Substring(dimPos + 2).TakeWhile(C => C != ' ' && C != '~').ToArray());
        endOfName = dimPos - 1;
        dims = dimsSection.Split(new char[] { ':', ',', 'x', 'X' }).Select(D => int.Parse(D)).ToArray();
      }

      startPos += 7;
      arrLen -= 7;

      string name = arrayData.Substring(2, endOfName - 1).TrimEnd();
      string arr = arrayData.Substring(startPos, arrLen).Trim(new char[] { ' ', ',' }).Replace(" ", "");
      while (arr.IndexOf(",,") != -1)
        arr = arr.Replace(",,", ",");

      System.Array values = null;
      switch (dims.Length)
      {
        case 0:
          values = arr.Split(',').Select(X => double.Parse(X)).ToArray();
          break;
        case 1:
          values = arr.Split(',').Select(X => double.Parse(X)).ToArray(dims[0]);
          break;
        case 2:
          values = arr.Split(',').Select(X => double.Parse(X)).ToArray(dims[0], dims[1]);
          break;
        case 3:
          values = arr.Split(',').Select(X => double.Parse(X)).ToArray(dims[0], dims[1], dims[2]);
          break;
        case 4:
          values = arr.Split(',').Select(X => double.Parse(X)).ToArray(dims[0], dims[1], dims[2], dims[3]);
          break;
      }

      data.Add(name, values);
    }

    public static System.Array GetDoubleArray(string arrayName)
    {
      if (data.ContainsKey(arrayName))
        return data[arrayName];

      return null;
    }

    public static System.Array GetSingleArray(string arrayName)
    {
      if (data.ContainsKey(arrayName))
      {
        System.Array arr = data[arrayName];

        switch (data[arrayName].Rank)
        {
          case 1:
            return data[arrayName].AsEnumerable<double>().Select(X => (float)X).ToArray(arr.GetLength(0));
          case 2:
            return data[arrayName].AsEnumerable<double>().Select(X => (float)X).ToArray(arr.GetLength(0), arr.GetLength(1));
          case 3:
            return data[arrayName].AsEnumerable<double>().Select(X => (float)X).ToArray(arr.GetLength(0), arr.GetLength(1), arr.GetLength(2));
          case 4:
            return data[arrayName].AsEnumerable<double>().Select(X => (float)X).ToArray(arr.GetLength(0), arr.GetLength(1), arr.GetLength(2), arr.GetLength(3));
        }
      }

      return null;
    }
  }
}
