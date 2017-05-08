using System;
using System.Collections.Generic;

namespace LinqLib.DynamicCodeGenerator
{
  /// <summary>
  /// Used by the Dynamic Class Generator to create dynamic object when pivoting collections of known types.
  /// </summary>
  public interface IDynamicPivotObject
  {
    /// <summary>
    /// Returns the property at the provided index. 
    /// </summary>
    /// <param name="index">The index of the property to return.</param>
    /// <returns>The property at the provided index.</returns>
    object this[int index] { get; set; }

    /// <summary>
    /// Returns the property with the provided name. 
    /// </summary>
    /// <param name="name">The name of the property to return.</param>
    /// <returns>The property with the provided name.</returns>
    object this[string name] { get; set; }

    /// <summary>
    /// Returns the name of a property from its index.
    /// </summary>
    /// <param name="index">The index of the property.</param>
    /// <returns>The name of the property at the provided index location.</returns>
    string GetPropertyName(int index);

    /// <summary>
    /// Returns the index of a property from its name.
    /// </summary>
    /// <param name="name">The name of the property.</param>
    /// <returns>The index of the property with the provided name.</returns>
    int GetPropertyIndex(string name);

    /// <summary>
    /// Returns the type of a property from its index.
    /// </summary>
    /// <param name="index">The index of the property.</param>
    /// <returns>The type of the property at the provided index location.</returns>
    Type GetPropertyType(int index);

    /// <summary>
    /// Returns the type of a property from its name.
    /// </summary>
    /// <param name="name">The name of the property.</param>
    /// <returns>The type of the property with the provided name.</returns>
    Type GetPropertyType(string name);

    /// <summary>
    /// Returns the number of custom properties.
    /// </summary>
    int PropertiesCount { get; }

    /// <summary>
    /// Returns a sequence of all custom properties names.
    /// </summary>
    IEnumerable<string> PropertiesNames { get; }
  }
}
