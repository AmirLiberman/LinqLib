using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LinqLib.DynamicCodeGenerator;

namespace LinqLib.Sequence
{
  /// <summary>
  /// Provides methods that return a the pivoted transformation of sequences and their sub sequences.  /// 
  /// </summary>
  public static class Transformer
  {
    private static readonly Dictionary<string, DynamicClassGenerator> generatedClasses = new Dictionary<string, DynamicClassGenerator>();
    private static readonly Dictionary<string, string> safeNames = new Dictionary<string, string>();

    private static string safeNamePrefix = "_";

    /// <summary>
    /// Prefix to use when converting fields content to CLR names. If field content starts with a digit, the prefix will be added to make the name CLR compatible. default prefix is an underscore.
    /// </summary>
    public static string SafeNamePrefix
    {
      get { return safeNamePrefix; }
      set { safeNamePrefix = value; }
    }

    /// <summary>
    /// Applies a pivot on a sequence property (sub sequence) in a sequence of objects.
    /// </summary>
    /// <typeparam name="TSource">Type of items in the source sequence.</typeparam>
    /// <typeparam name="TPivot">Type of items in the sequence to pivot.</typeparam>
    /// <param name="source">Source sequence to operate on.</param>
    /// <param name="sequenceToPivot">The sub sequence to pivot.</param>
    /// <param name="nameColumn">The property name in the sub sequence type to use for the pivot column name. The content of that property will become a property in to pivoted output.</param>
    /// <param name="jaggedSequence">A Boolean indicating if all sub sequence have same number of members with the same type of content. When set to false, only the first row is inspected leading to better performance.</param>
    /// <returns>A sequence of objects representing the pivoted values of the original sequence inline with the sub sequence.</returns>
    /// <remarks>If tow or more instances of the type in the sequenceToPivot have the same name (nameColumn may not be distinct) the last instance in the sequence will be used.</remarks>
    public static IEnumerable<object> Pivot<TSource, TPivot>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TPivot>> sequenceToPivot, Func<TPivot, string> nameColumn, bool jaggedSequence)
    {
      return Pivot(source, sequenceToPivot, nameColumn, jaggedSequence, null);
    }

    /// <summary>
    /// Applies a pivot on a sequence property (sub sequence) in a sequence of objects.
    /// </summary>
    /// <typeparam name="TSource">Type of items in the source sequence.</typeparam>
    /// <typeparam name="TPivot">Type of items in the sequence to pivot.</typeparam>
    /// <param name="source">Source sequence to operate on.</param>
    /// <param name="sequenceToPivot">The sub sequence to pivot.</param>
    /// <param name="nameColumn">The property name in the sub sequence type to use for the pivot column name. The content of that property will become a property in to pivoted output.</param>
    /// <param name="jaggedSequence">A Boolean indicating if all sub sequence have same number of members with the same type of content. When set to false, only the first row is inspected leading to better performance.</param>
    /// <param name="classGenerationEventHandler">The name of the event handler that will handle the ClassGenerationEventArgs passed from the class generator.</param>
    /// <returns>A sequence of objects representing the pivoted values of the original sequence inline with the sub sequence.</returns>
    /// <remarks>If tow or more instances of the type in the sequenceToPivot have the same name (nameColumn may not be distinct) the last instance in the sequence will be used.</remarks>
    public static IEnumerable<object> Pivot<TSource, TPivot>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TPivot>> sequenceToPivot, Func<TPivot, string> nameColumn, bool jaggedSequence, EventHandler<ClassGenerationEventArgs> classGenerationEventHandler)
    {
      return Pivot(source, sequenceToPivot, nameColumn, X => X, jaggedSequence, classGenerationEventHandler);
    }

    /// <summary>
    /// Applies a pivot on a sequence property (sub sequence) in a sequence of objects.
    /// </summary>
    /// <typeparam name="TSource">Type of items in the source sequence.</typeparam>
    /// <typeparam name="TPivot">Type of items in the sequence to pivot.</typeparam>
    /// <typeparam name="TResult">Type of result element.</typeparam>
    /// <param name="source">Source sequence to operate on.</param>
    /// <param name="sequenceToPivot">The sub sequence to pivot.</param>
    /// <param name="nameColumn">The property name in the sub sequence type to use for the pivot column name. The content of that property will become a property in to pivoted output.</param>
    /// <param name="valueColumn">An expression used on members in the sub sequence type to set the pivot column value. The output of this expression will become the value of the pivoted column.</param>
    /// <param name="jaggedSequence">A Boolean indicating if all sub sequence have same number of members with the same type of content. When set to false, only the first row is inspected leading to better performance.</param>
    /// <returns>A sequence of objects representing the pivoted values of the original sequence inline with the sub sequence.</returns>    
    /// <remarks>If tow or more instances of the type in the sequenceToPivot have the same name (nameColumn may not be distinct) the last instance in the sequence will be used.</remarks>
    public static IEnumerable<object> Pivot<TSource, TPivot, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TPivot>> sequenceToPivot, Func<TPivot, string> nameColumn, Func<TPivot, TResult> valueColumn, bool jaggedSequence)
    {
      return Pivot(source, sequenceToPivot, nameColumn, valueColumn, jaggedSequence, null);
    }

    /// <summary>
    /// Applies a pivot on a sequence property (sub sequence) in a sequence of objects.
    /// </summary>
    /// <typeparam name="TSource">Type of items in the source sequence.</typeparam>
    /// <typeparam name="TPivot">Type of items in the sequence to pivot.</typeparam>
    /// <typeparam name="TResult">Type of result element.</typeparam>
    /// <param name="source">Source sequence to operate on.</param>
    /// <param name="sequenceToPivot">The sub sequence to pivot.</param>
    /// <param name="nameColumn">The property name in the sub sequence type to use for the pivot column name. The content of that property will become a property in to pivoted output.</param>
    /// <param name="valueColumn">An expression used on members in the sub sequence type to set the pivot column value. The output of this expression will become the value of the pivoted column.</param>
    /// /// <param name="jaggedSequence">A Boolean indicating if all sub sequence have same number of members with the same type of content. When set to false, only the first row is inspected leading to better performance.</param>
    /// <param name="classGenerationEventHandler">The name of the event handler that will handle the ClassGenerationEventArgs passed from the class generator.</param>
    /// <returns>A sequence of objects representing the pivoted values of the original sequence inline with the sub sequence.</returns>    
    /// <remarks>If tow or more instances of the type in the sequenceToPivot have the same name (nameColumn may not be distinct) the last instance in the sequence will be used.</remarks>
    public static IEnumerable<object> Pivot<TSource, TPivot, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TPivot>> sequenceToPivot, Func<TPivot, string> nameColumn, Func<TPivot, TResult> valueColumn, bool jaggedSequence, EventHandler<ClassGenerationEventArgs> classGenerationEventHandler)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (sequenceToPivot == null)
        throw Error.ArgumentNull("sequenceToPivot");
      if (nameColumn == null)
        throw Error.ArgumentNull("nameColumn");
      if (valueColumn == null)
        throw Error.ArgumentNull("valueColumn");

      TSource[] sourceItems = source.ToArray();
      Helper.InvalidateEmptySequence(sourceItems, "source");
      if (sequenceToPivot(sourceItems.First()) == null)
        throw Error.ArgumentNull("resultOfSequenceToPivot");

      IEnumerable<string> fields;
      if (jaggedSequence)
        fields = GetFields(sourceItems, sequenceToPivot, nameColumn);
      else
        fields = GetFields(sourceItems.First(), sequenceToPivot, nameColumn);

      if (!fields.Any())
      {
        foreach (TSource item in sourceItems)
          yield return item;

        yield break;
      }

      PivotInfo pivotInfo = PrepairPivot<TSource, TPivot, TResult>(fields, classGenerationEventHandler);

      foreach (TSource sourceItem in sourceItems)
        yield return GetPivotedInstance(sourceItem, sequenceToPivot, nameColumn, valueColumn, pivotInfo);
    }

    /// <summary>
    /// Applies a pivot on a sequence property in the source object.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TPivot">The type of the pivot.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="sequenceToPivot">The sequence to pivot.</param>
    /// <param name="nameColumn">The property name in the sub sequence type to use for the pivot column name. The content of that property will become a property in to pivoted output.</param>
    /// <param name="valueColumn">An expression used on members in the sub sequence type to set the pivot column value. The output of this expression will become the value of the pivoted column.</param>    
    /// <returns>A pivoted object where rows of a collection become properties.</returns>
    /// <remarks>If tow or more instances of the type in the sequenceToPivot have the same name (nameColumn may not be distinct) the last instance in the sequence will be used.</remarks>
    public static object Pivot<TSource, TPivot, TResult>(this TSource source, Func<TSource, IEnumerable<TPivot>> sequenceToPivot, Func<TPivot, string> nameColumn, Func<TPivot, TResult> valueColumn)
    {
      return Pivot(source, sequenceToPivot, nameColumn, valueColumn, null);
    }

    /// <summary>
    /// Applies a pivot on a sequence property of the source object.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TPivot">The type of the pivot.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="sequenceToPivot">The sequence to pivot.</param>
    /// <param name="nameColumn">The property name in the sub sequence type to use for the pivot column name. The content of that property will become a property in to pivoted output.</param>
    /// <param name="valueColumn">An expression used on members in the sequence type to set the pivot column value. The output of this expression will become the value of the pivoted column.</param>
    /// <param name="classGenerationEventHandler">The name of the event handler that will handle the ClassGenerationEventArgs passed from the class generator.</param>
    /// <returns>A pivoted object where rows of a collection become properties.</returns>
    /// <remarks>If tow or more instances of the type in the sequenceToPivot have the same name (nameColumn may not be distinct) the last instance in the sequence will be used.</remarks>
    public static object Pivot<TSource, TPivot, TResult>(this TSource source, Func<TSource, IEnumerable<TPivot>> sequenceToPivot, Func<TPivot, string> nameColumn, Func<TPivot, TResult> valueColumn, EventHandler<ClassGenerationEventArgs> classGenerationEventHandler)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (sequenceToPivot == null)
        throw Error.ArgumentNull("sequenceToPivot");
      if (nameColumn == null)
        throw Error.ArgumentNull("nameColumn");
      if (valueColumn == null)
        throw Error.ArgumentNull("valueColumn");

      if (sequenceToPivot(source) == null)
        throw Error.ArgumentNull("resultOfSequenceToPivot");

      IEnumerable<string> fields = GetFields(source, sequenceToPivot, nameColumn);
      if (!fields.Any())
        return source;

      PivotInfo pivotInfo = PrepairPivot<TSource, TPivot, TResult>(fields, classGenerationEventHandler);
      return GetPivotedInstance(source, sequenceToPivot, nameColumn, valueColumn, pivotInfo);

    }

    private static PivotInfo PrepairPivot<TSource, TPivot, TResult>(IEnumerable<string> fields, EventHandler<ClassGenerationEventArgs> classGenerationEventHandler)
    {
      PivotInfo pivotInfo = new PivotInfo();

      Type sourceType = typeof(TSource);
      Type resultType = typeof(TResult);

      pivotInfo.PivotedProperties = new Dictionary<string, PropertyInfo>();

      string className = GetSafeRuntimeName(string.Concat(sourceType.Name, "WithPivotOn", typeof(TPivot).Name));
      string keyName = string.Concat(sourceType.Namespace, ".", className, ".", fields.Aggregate((F1, F2) => string.Concat(F1, ".", F2)), ".", sourceType.Name, ".", resultType.Name);

      if (generatedClasses.ContainsKey(keyName))
        pivotInfo.PivotedClass = generatedClasses[keyName];
      else
      {
        pivotInfo.PivotedClass = new DynamicClassGenerator(className, sourceType, fields, resultType, classGenerationEventHandler);
        generatedClasses[keyName] = pivotInfo.PivotedClass;
      }

      Type pivotedType = pivotInfo.PivotedClass.GetInstance().GetType();
      foreach (string field in fields)
        pivotInfo.PivotedProperties.Add(field, pivotedType.GetProperty(field));

      return pivotInfo;
    }

    private static IEnumerable<string> GetFields<TSource, TPivot>(TSource source, Func<TSource, IEnumerable<TPivot>> sequenceToPivot, Func<TPivot, string> nameColumn)
    {
      return sequenceToPivot(source)
                   .Where(X => X != null)
                   .Select(X => GetSafeRuntimeName(nameColumn(X)))
                   .Where(F => !string.IsNullOrEmpty(F))
                   .Distinct();
    }

    private static IEnumerable<string> GetFields<TSource, TPivot>(IEnumerable<TSource> source, Func<TSource, IEnumerable<TPivot>> sequenceToPivot, Func<TPivot, string> nameColumn)
    {
      return (from item in source
              where sequenceToPivot(item) != null
              from name in sequenceToPivot(item).Where(X => X != null).Select(X => GetSafeRuntimeName(nameColumn(X)))
              select name)
              .Where(F => !string.IsNullOrEmpty(F))
              .Distinct();
    }

    private static object GetPivotedInstance<TSource, TPivot, TResult>(TSource sourceItem, Func<TSource, IEnumerable<TPivot>> sequenceToPivot, Func<TPivot, string> nameColumn, Func<TPivot, TResult> valueColumn, PivotInfo pivotInfo)
    {
      object pivot = pivotInfo.PivotedClass.GetInstance();
      Type sourceType = typeof(TSource);

      foreach (PropertyInfo prop in sourceType.GetProperties().Where(P => P.CanWrite && P.CanRead))
        prop.SetValue(pivot, prop.GetValue(sourceItem, null), null);

      foreach (FieldInfo fld in sourceType.GetFields())
        fld.SetValue(pivot, fld.GetValue(sourceItem));

      IEnumerable<TPivot> pivotItems = sequenceToPivot(sourceItem);
      if (pivotItems != null)
        foreach (TPivot pivotItem in pivotItems.Where(X => X != null))
          try
          {
            pivotInfo.PivotedProperties[GetSafeRuntimeName(nameColumn(pivotItem))].SetValue(pivot, valueColumn(pivotItem), null);
          }
          catch (Exception)
          {
            throw Error.DynamicPropertyAccess(nameColumn(pivotItem));
          }
      return pivot;
    }

    /// <summary>
    /// Creates a CLR safe element name.
    /// </summary>
    /// <param name="name">Raw element name.</param>
    /// <returns>CLR safe element name.</returns>
    private static string GetSafeRuntimeName(string name)
    {
      if (!safeNames.ContainsKey(name))
      {
        char[] safeChars = name.Where(C => char.IsLetterOrDigit(C) || C == '_').ToArray();
        string safeName = new string(safeChars);
        if (char.IsDigit(safeChars[0]))
          safeName = safeNamePrefix + safeName;

        safeNames.Add(name, safeName);
      }
      return safeNames[name];
    }

    /// <summary>
    /// Returns the original name of the property / field name.
    /// </summary>
    /// <param name="safeRuntimeName">Property / field name.</param>
    /// <returns>The original name of the property / field name.</returns>
    /// <exception cref="System.ArgumentException">
    /// The Safe Runtime Name cannot be found.
    /// </exception>
    /// <remarks>
    /// When pivoting a sub collection, the new properties of the pivoted type gets their names from the content of elements in the sub collection. The content might not contain valid CLR names.
    /// THe Pivot function will correct the name to adhere to the .NET requirements.  This method will lookup the original name based on the property / field name. 
    /// </remarks>
    public static string GetName(string safeRuntimeName)
    {
      if (!safeNames.ContainsValue(safeRuntimeName))
        throw Error.MissingFieldOrProperty(safeRuntimeName);

      return safeNames.First(N => N.Value == safeRuntimeName).Key;
    }

    private class PivotInfo
    {
      public Dictionary<string, PropertyInfo> PivotedProperties;
      public DynamicClassGenerator PivotedClass;
    }
  }
}

