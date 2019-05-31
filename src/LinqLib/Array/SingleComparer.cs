using System.Collections.Generic;

namespace LinqLib.Array
{
  /// <summary>
  /// A class that provides a way to compare float types while ignoring minor floating point arithmetic errors
  /// </summary>
  public class SingleComparer : IEqualityComparer<float>
  {
    private readonly float sensitivity;

    /// <summary>
    /// Initializes a new instance of SingleComparer class with default sensitivity value of 0.00001.
    /// </summary>
    public SingleComparer()
      : this(.00001f) { }

    /// <summary>
    /// Initializes a new instance of SingleComparer class.
    /// </summary>
    /// <param name="sensitivity">A value representing the sensitivity of the comparisons performed by this class.</param>
    public SingleComparer(float sensitivity)
    {
      this.sensitivity = sensitivity;
    }

    /// <summary>
    /// Determines whether the specified values are equal or within the sensitivity range.
    /// </summary>
    /// <param name="x">The first float to compare.</param>
    /// <param name="y">The second float to compare.</param>
    /// <returns>true if the specified objects are equal or within the sensitivity range; otherwise, false.</returns>
    public bool Equals(float x, float y)
    {
      return x + sensitivity > y && x - sensitivity < y;
    }

    /// <summary>
    /// Returns a hash code for the specified object.
    /// </summary>
    /// <param name="obj">The System.Object for which a hash code is to be returned.</param>
    /// <returns>A hash code for the specified object.</returns>
    public int GetHashCode(float obj)
    {
      return obj.GetHashCode();
    }
  }
}
