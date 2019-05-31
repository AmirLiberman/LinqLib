using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqLib
{
  internal static class Helper
  {
    #region Declarations

    // Static hash set that holds primitive numeric types for fast lookup.   
    private static readonly HashSet<string> numericTypes = new HashSet<string> {"Byte",
                                                                                "Int16",                                                                      
                                                                                "Int32",
                                                                                "Int64",
                                                                                "SByte",
                                                                                "UInt16",
                                                                                "UInt32",
                                                                                "UInt64",
                                                                                "Decimal",
                                                                                "Double",
                                                                                "Single"};

    #endregion

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal static void InvalidateEnumeratedParam<T>(IEnumerable<T> parameter, int count, string parameterName)
    {
      if (parameter == null)
        throw Error.ArgumentNull(parameterName);


      T[] paramArr = parameter.ToArray();
      if (paramArr.Count() != count)
        throw Error.InvalidItemsCount(count, parameterName);

      if (typeof(T).IsValueType)
        return;

      int idx = 0;
      foreach (T element in paramArr)
      {
        if (element == null)
          throw Error.SourceSequenceIsNull(idx, parameterName);

        idx++;
      }
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal static void InvalidateEmptySequence<T>(IEnumerable<T> parameter, string parameterName)
    {
      if (parameter == null)
        throw Error.ArgumentNull(parameterName);

      if (!parameter.Any())
        throw Error.SequenceMinOne(parameterName);
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal static void InvalidateNonNumeric<T>(string caller)
    {
      Type tt = typeof(T);
      if (!IsNumeric(tt))
        throw Error.ApplyAttempt(caller, tt.Name);
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal static void InvalidateNonPremitive<T>(string caller)
    {
      Type tt = typeof(T);
      if (!tt.IsPrimitive)
        throw Error.ApplyAttempt(caller, tt.Name);
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal static void InvalidateNumericRange(double value, double min, double max, string parameterName)
    {
      if (value < min)
        throw Error.ParamMinRange(parameterName, min);
      if (value > max)
        throw Error.ParamMaxRange(parameterName, max);
    }

    /// <summary>
    /// Evaluates if a type is of numeric type or of nullable numeric type. 
    /// </summary>
    /// <param name="value">A System.Type to evaluate.</param>
    /// <returns>true if type is numeric or generic nullable of numeric type, false otherwise.</returns>
    public static bool IsNumeric(Type value)
    {
      string name = value.Name;
      if (value.IsValueType && numericTypes.Contains(name)) // If type is primitive and name is found in lookup table
        return true;

      if (name == "Nullable`1") // If type is nullable<T> and T is numeric.
        return IsNumeric(value.GetGenericArguments()[0]);

      return false;
    }

    /// <summary>
    /// Swaps two references.
    /// </summary>
    /// <typeparam name="T">The data type of the references to swap.</typeparam>
    /// <param name="a">First reference</param>
    /// <param name="b">Second reference</param>
    public static void Swap<T>(ref T a, ref T b)
    {
      T temp = b;
      b = a;
      a = temp;
    }
  }
}
