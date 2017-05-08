using System.Collections.Generic;

namespace LinqLib.Array
{
  /// <summary>
  /// A class that provides a way to compare double types while ignoring minor floating point arithmetic errors
  /// </summary>
  public class DoubleComparer : IEqualityComparer<double>
  {
    private readonly double sensitivity;

    /// <summary>
    /// Initializes a new instance of DoubleComparer class with default sensitivity value of 0.000001.
    /// </summary>
    public DoubleComparer()
      : this(.000001) { }

    /// <summary>
    /// Initializes a new instance of DoubleComparer class.
    /// </summary>
    /// <param name="sensitivity">A value representing the sensitivity of the comparisons performed by this class.</param>
    public DoubleComparer(double sensitivity)
    {
      this.sensitivity = sensitivity;
    }

    /// <summary>
    /// Determines whether the specified values are equal or within the sensitivity range.
    /// </summary>
    /// <param name="x">The first double to compare.</param>
    /// <param name="y">The second double to compare.</param>
    /// <returns>true if the specified objects are equal or within the sensitivity range; otherwise, false.</returns>
    public bool Equals(double x, double y)
    {
      return x + sensitivity > y && x - sensitivity < y;
    }

    /// <summary>
    /// Returns a hash code for the specified object.
    /// </summary>
    /// <param name="obj">The System.Object for which a hash code is to be returned.</param>
    /// <returns>A hash code for the specified object.</returns>
    public int GetHashCode(double obj)
    {
      return obj.GetHashCode();
    }
  }
}

