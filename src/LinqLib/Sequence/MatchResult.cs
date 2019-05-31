using System.Diagnostics;

namespace LinqLib.Sequence
{
  /// <summary>
  /// Represents the result of a match operation.
  /// </summary>
  /// <typeparam name="TLeft">Type of left element in the match result.</typeparam>
  /// <typeparam name="TRight">Type of right element in the match result.</typeparam>
  [DebuggerDisplay("{LeftItem}-{RightItem}")]
  public class MatchResult<TLeft, TRight>
  {
    /// <summary>
    /// Creates an a new instance of the MatchResult.
    /// </summary>
    /// <param name="leftItem">Left item matched.</param>
    /// <param name="rightItem">Right item matched.</param>
    public MatchResult(TLeft leftItem, TRight rightItem)
    {
      LeftItem = leftItem;
      RightItem = rightItem;
    }

    /// <summary>
    /// Left item.
    /// </summary>
    public TLeft LeftItem { get; private set; }

    /// <summary>
    /// Right item.
    /// </summary>
    public TRight RightItem { get; private set; }

    /// <summary>
    /// Determines whether the specified object's state is equal to the current instance's state.
    /// </summary>
    /// <param name="obj">The object to be compared with the current instance.</param>
    /// <returns>True if the specified object is a LinqLib.Operators.Logical.TruthTableItem type and is in a state equal to the current instance's state; otherwise, false.</returns>
    public override bool Equals(object obj)
    {
      MatchResult<TLeft, TRight> other = obj as MatchResult<TLeft, TRight>;

      if (other == null)
        return false;

      return (Equals(LeftItem, other.LeftItem) && Equals(RightItem, other.RightItem));
    }

    /// <summary>
    /// Returns the hash code for this instance's state.
    /// </summary>
    /// <returns>A 32-bit signed integer that is the hash code for this instance's state.</returns>
    public override int GetHashCode()
    {
      return LeftItem.GetHashCode() ^ RightItem.GetHashCode();
    }
  }
}
