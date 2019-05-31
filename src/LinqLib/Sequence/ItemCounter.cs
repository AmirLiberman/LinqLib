using System.Diagnostics;

namespace LinqLib.Sequence
{
  /// <summary>
  /// A class that represent an item and tracks its appearance count in a sequence.
  /// </summary>
  /// <typeparam name="T">The type of the item this class represents.</typeparam>
  [DebuggerDisplay("{Item} : {Count}")]
  public class ItemCounter<T>
  {
    /// <summary>
    /// Initializes a new instance of ItemCounter class with item count of 1.
    /// </summary>
    /// <param name="item">The item this class represent.</param>
    public ItemCounter(T item)
    {
      Item = item;
      Count = 1;
    }

    /// <summary>
    /// Returns the item this class represent.
    /// </summary>
    public T Item { get; private set; }

    /// <summary>
    /// The number of times the item this class represent appeared in a sequence.
    /// </summary>
    public int Count { get; private set; }

    internal void IncrementCount()
    {
      Count++;
    }
  }
}
