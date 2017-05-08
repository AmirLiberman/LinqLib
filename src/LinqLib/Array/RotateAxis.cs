
namespace LinqLib.Array
{
  /// <summary>
  /// Describes the possible axis used to rotate a multidimensional array.
  /// </summary>
  public enum RotateAxis
  {
    /// <summary>
    /// No Rotation
    /// </summary>
    RotateNone,
    /// <summary>
    /// Rotate on the X Axis.
    /// </summary>
    RotateX,
    /// <summary>
    /// Rotate on the Y Axis.
    /// </summary>
    RotateY,
    /// <summary>
    /// Rotate on the Z Axis.
    /// </summary>
    RotateZ,
    /// <summary>
    /// Rotate on the A (also known as X') Axis.
    /// </summary>
    RotateA,
    /// <summary>
    /// Rotate on the B (also known as Y') Axis. Currently not in use. 
    /// </summary>
    RotateB,
    /// <summary>
    /// Rotate on the C (also known as Z') Axis. Currently not in use.
    /// </summary>
    RotateC
  }
}
