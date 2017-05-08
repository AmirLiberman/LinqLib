using System.Collections.Generic;

namespace LinqLibTests
{
  public class TestComparer : IComparer<int>
  {
    public int Compare(int x, int y)
    {
      return x.CompareTo(y);
    }
  }
}
