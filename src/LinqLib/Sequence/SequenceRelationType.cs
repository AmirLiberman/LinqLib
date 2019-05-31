namespace LinqLib.Sequence
{
  /// <summary>
  /// Describes the relations between two sequences.
  /// </summary>
  public enum SequenceRelationType
  {
    /// <summary>
    /// Nothing in common.
    /// </summary>
    None,
    /// <summary>
    /// All members are the same in and the correct order.
    /// </summary>
    Equal,
    /// <summary>
    /// All members are the same but out of order.
    /// </summary>
    Similar,
    /// <summary>
    /// Input contains Other.
    /// </summary>
    Contains,
    /// <summary>
    /// Other contains Input.
    /// </summary>
    Contained,
    /// <summary>
    /// Some elements are in common.
    /// </summary>
    Intersects
  }
}
