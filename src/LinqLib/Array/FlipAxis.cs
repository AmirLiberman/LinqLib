
using System;
namespace LinqLib.Array
{
  /// <summary>
  /// Describes the possible axis used to flip a multidimensional array.
  /// </summary>
  [Flags]
  public enum FlipAxis
  {
    /// <summary>
    /// No Flip Action.
    /// </summary>
    None = 0,
    /// <summary>
    /// Flip on the X Axis.
    /// </summary>
    FlipX = 1,
    /// <summary>
    /// Flip on the Y Axis.
    /// </summary>
    FlipY = 2,
    /// <summary>
    /// Flip on the X and Y Axis.
    /// </summary>
    FlipXY = 3,
    /// <summary>
    /// Flip on the Z Axis.
    /// </summary>
    FlipZ = 4,
    /// <summary>
    /// Flip on the X and Z Axis.
    /// </summary>
    FlipXZ = 5,
    /// <summary>
    /// Flip on the Y and Z Axis.
    /// </summary>
    FlipYZ = 6,
    /// <summary>
    /// Flip on the X, Y and Z Axis.
    /// </summary>
    FlipXYZ = 7
  }
}
