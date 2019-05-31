namespace LinqLib.Sequence
{
  /// <summary>
  /// Describes the filters available for noise reduction.
  /// </summary>
  public enum NoiseFilterType
  {
    /// <summary>
    /// Apply filters based on absolute values.
    /// </summary>
    AbsoluteValue,
    /// <summary>
    /// Apply filters based on percent of deviation from the average.
    /// </summary>
    PercentOfAverage,
    /// <summary>
    /// Apply filters based on distance from the standard deviation.
    /// </summary>
    StandardDeviation
  }
}
