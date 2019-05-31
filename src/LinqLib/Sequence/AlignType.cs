
namespace LinqLib.Sequence
{
  /// <summary>
  /// Describes the possible outcomes of align operation.
  /// </summary>
  public enum AlignType
  {
    /// <summary>
    /// Left item and right item are a match.
    /// </summary>
    Match,
    /// <summary>
    /// No item on left match current item on right.
    /// </summary>
    LeftMissing,
    /// <summary>
    /// No item on right match current item on left.
    /// </summary>
    RightMissing
  }
}
