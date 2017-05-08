using System.Diagnostics;

namespace LinqLib.Sequence
{
  /// <summary>
  /// Represents the result of am align operation.
  /// </summary>
  /// <typeparam name="TLeft">Type of left element in the match result.</typeparam>
  /// <typeparam name="TRight">Type of right element in the match result.</typeparam>
  [DebuggerDisplay("{LeftItem}-{RightItem}  {AlignType}")]
  public class AlignResult<TLeft, TRight> : MatchResult<TLeft, TRight>
  {
    /// <summary>
    /// Creates an a new instance of the AlignResult type with a specific AlignType.
    /// </summary>
    /// <param name="leftItem">Left item matched.</param>
    /// <param name="rightItem">Right item matched.</param>
    /// <param name="alignType">Match result.</param>
    public AlignResult(TLeft leftItem, TRight rightItem, AlignType alignType)
      : base(leftItem, rightItem)
    {
      AlignType = alignType;
    }

    /// <summary>
    /// Creates an a new instance of the AlignResult type with a AlignType of type 'Match'.
    /// </summary>
    /// <param name="leftItem">Left item matched.</param>
    /// <param name="rightItem">Right item matched.</param>
    public AlignResult(TLeft leftItem, TRight rightItem)
      : this(leftItem, rightItem, AlignType.Match)
    { }

    /// <summary>
    /// The type of match relation between the left and right items.
    /// </summary>
    public AlignType AlignType { get; private set; }

    /// <summary>
    /// Determines whether the specified object's state is equal to the current instance's state.
    /// </summary>
    /// <param name="obj">The object to be compared with the current instance.</param>
    /// <returns>True if the specified object is a LinqLib.Operators.Logical.TruthTableItem type and is in a state equal to the current instance's state; otherwise, false.</returns>
    public override bool Equals(object obj)
    {
      AlignResult<TLeft, TRight> other = obj as AlignResult<TLeft, TRight>;

      if (other == null)
        return false;

      return (base.Equals(other) && AlignType.Equals(other.AlignType));
    }

    /// <summary>
    /// Returns the hash code for this instance's state.
    /// </summary>
    /// <returns>A 32-bit signed integer that is the hash code for this instance's state.</returns>
    public override int GetHashCode()
    {
      return base.GetHashCode() ^ AlignType.GetHashCode();
    }
  }
}
