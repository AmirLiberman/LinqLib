using System.Collections.Generic;
using System.Runtime;

namespace LinqLib.Sort.Sorters
{
  /// <summary>
  /// Documentation
  /// </summary>
  /// <typeparam name="TKey"></typeparam>
  public abstract class Sort<TKey>
  {
    internal MapItem<TKey>[] Map;
    internal bool Descending;
    internal IComparer<TKey> Comparer;

    /// <summary>
    /// Documentation
    /// </summary>
    /// <param name="map"></param>
    /// <param name="comparer"></param>
    /// <param name="descending"></param>
    internal Sort(MapItem<TKey>[] map, IComparer<TKey> comparer, bool descending)
    {
      Descending = descending;
      Comparer = comparer;
      Map = map;
    }

    /// <summary>
    /// Documentation
    /// </summary>
    /// <param name="index1"></param>
    /// <param name="index2"></param>
    /// <returns></returns>
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    protected int CompareKeys(int index1, int index2)
    {
      int res = Comparer.Compare(Map[index1].Key, Map[index2].Key);
      return Descending ? -res : res;
    }

    /// <summary>
    /// Documentation
    /// </summary>
    /// <param name="key1"></param>
    /// <param name="key2"></param>
    /// <returns></returns>
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    protected int CompareKeys(TKey key1, TKey key2)
    {
      int res = Comparer.Compare(key1, key2);
      return Descending ? -res : res;
    }

    /// <summary>
    /// Documentation
    /// </summary>
    /// <param name="index1"></param>
    /// <param name="index2"></param>
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    protected void Swap(int index1, int index2)
    {
      MapItem<TKey> copy = Map[index2];
      Map[index2] = Map[index1];
      Map[index1] = copy;
    }
  }
}
