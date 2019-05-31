using System.Diagnostics;

namespace LinqLib.Sort
{
  /// <summary>
  /// Documentation
  /// </summary>
  /// <typeparam name="TKey"></typeparam>
  [DebuggerDisplayAttribute("{Index} - {Key}")]
  public struct MapItem<TKey>
  {
    /// <summary>
    /// Documentation
    /// </summary>
    /// <param name="index"></param>
    /// <param name="key"></param>
    public MapItem(int index, TKey key)
    {
      Index = index;
      Key = key;
    }

    internal readonly int Index;
    internal readonly TKey Key;

    /// <summary>
    /// Documentation
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
      return Index ^ Key.GetHashCode();
    }

    /// <summary>
    /// Documentation
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object obj)
    {
      if ((obj is MapItem<TKey>))
        return Equals((MapItem<TKey>)obj);

      return false;
    }

    /// <summary>
    /// Documentation
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(MapItem<TKey> other)
    {
      if (Index == other.Index)
        return Equals(Key, other.Key);

      return false;
    }

    /// <summary>
    /// Documentation
    /// </summary>
    /// <param name="item1"></param>
    /// <param name="item2"></param>
    /// <returns></returns>
    public static bool operator ==(MapItem<TKey> item1, MapItem<TKey> item2)
    {
      return item1.Equals(item2);
    }

    /// <summary>
    /// Documentation
    /// </summary>
    /// <param name="item1"></param>
    /// <param name="item2"></param>
    /// <returns></returns>
    public static bool operator !=(MapItem<TKey> item1, MapItem<TKey> item2)
    {
      return !item1.Equals(item2);
    }
  }
}
