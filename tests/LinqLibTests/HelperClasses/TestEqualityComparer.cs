using System.Collections.Generic;

namespace LinqLibTests
{
  internal class TestEqualityComparer<T> : IEqualityComparer<T>
  {
    #region IEqualityComparer<T> Members

    public bool Equals(T x, T y)
    {
      return object.Equals(x, y);
    }

    public int GetHashCode(T obj)
    {
      return obj.GetHashCode();
    }

    #endregion
  }
}