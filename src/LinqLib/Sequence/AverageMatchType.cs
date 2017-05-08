
namespace LinqLib.Sequence
{
  /// <summary>
  /// Describes the type of match used when seeking the average element.
  /// </summary>
  public enum AverageMatchType
  {
    /// <summary>
    /// Match an element with the exact value.
    /// </summary>
    Exact,
    /// <summary>
    /// Match the element with the closest value.
    /// </summary>
    Closest,
    /// <summary>
    /// Match the element with the exact or larger value.
    /// </summary>
    ExactOrLarger,
    /// <summary>
    /// Match the element with the exact or smaller value.
    /// </summary>
    ExactOrSmaller
  }
}
