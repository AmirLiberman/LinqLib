using System;
using System.Collections.Generic;

namespace LinqLib.Operators
{
  /// <summary>
  /// Extension methods applying actions on sequences.
  /// </summary>
  public static class Action
  {
    #region ForEach Action

    /// <summary>
    /// Performs the specified action on each element of the IEnumerable&lt;TSource&gt;.
    /// </summary>
    /// <remarks>
    /// This extension does not defers the execution all elements are processed once the method is called.
    /// If &lt;TSource&gt; is a value type, changes to the source elements will not be reflected in the return value of this function.
    /// It is best NOT to use this method to modify the content of the enumerated items, use the Select extension instead to project the modified value. 
    /// ForEach is a good choice if you need to perform an action and use each element of the sequence as a parameter for your action.  
    /// </remarks>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">The elements to perform the action on.</param>
    /// <param name="action">The System.Action&lt;T&gt; delegate to perform on each element of the IEnumerable&lt;TSource&gt;.</param>
    /// <exception cref="System.ArgumentNullException">source is null</exception>
    /// <exception cref="System.ArgumentNullException">action is null</exception>
    /// <returns>The source sequence with any side effects from the action function.</returns>
    public static IEnumerable<TSource> ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (action == null)
        throw Error.ArgumentNull("action");

      foreach (TSource item in source)
        action(item);

      return source;
    }

    /// <summary>
    /// Performs the specified action on each element of the IEnumerable&lt;TSource&gt;.
    /// </summary>
    /// <remarks>
    /// This extension does not defers the execution all elements are processed once the method is called.
    /// If &lt;TSource&gt; is a value type, changes to the source elements will not be reflected in the return value of this function.
    /// It is best NOT to use this method to modify the content of the enumerated items, use the Select extension instead to project the modified value. 
    /// ForEach is a good choice if you need to perform an action and use each element of the sequence as a parameter for your action.  
    /// </remarks>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <param name="source">The elements to perform the action on.</param>
    /// <param name="action">The System.Action&lt;T&gt; delegate to perform on each element of the IEnumerable&lt;TSource&gt;; the second parameter of the function represents the index of the source element.</param>
    /// <exception cref="System.ArgumentNullException">source is null</exception>
    /// <exception cref="System.ArgumentNullException">action is null</exception>
    /// <returns>The source sequence with any side effects from the action function.</returns>
    public static IEnumerable<TSource> ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource, int> action)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (action == null)
        throw Error.ArgumentNull("action");

      int i = 0;
      foreach (TSource item in source)
      {
        action(item, i);
        i++;
      }

      return source;
    }

    #endregion
  }
}
