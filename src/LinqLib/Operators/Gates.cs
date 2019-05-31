using System.Collections.Generic;

namespace LinqLib.Operators
{
  /// <summary>
  /// Provides logical AND, OR , XOR, NOT and truth table processing methods.
  /// </summary>
  public static class Gates
  {
    /// <summary>
    /// Applies a Logical 'NOT' over a sequence.
    /// </summary>
    /// <param name="input">A System.Collections.Generic.IEnumerable&lt;bool&gt; to apply the 'NOT' operation over.</param>
    /// <returns>A System.Collections.Generic.IEnumerable&lt;bool&gt; with values that are the 'NOT' representation of the source.</returns>
    /// <exception cref="System.ArgumentNullException">
    /// source is null.
    /// </exception>
    public static IEnumerable<bool> Not(this IEnumerable<bool> input)
    {
      if (input == null)
        throw Error.ArgumentNull("input");

      using (IEnumerator<bool> inp = input.GetEnumerator())
        while (inp.MoveNext())
          yield return !inp.Current;
    }

    /// <summary>
    /// Applies a Logical 'AND' over two sequences.
    /// </summary>
    /// <param name="inputA">First System.Collections.Generic.IEnumerable&lt;bool&gt; to apply the 'AND' operation over.</param>
    /// <param name="inputB">Second System.Collections.Generic.IEnumerable&lt;bool&gt; to apply the 'AND' operation over.</param>
    /// <returns>
    /// A System.Collections.Generic.IEnumerable&lt;bool&gt; with values that are the 'AND' representation of the elements from inputA and inputB.
    /// If inputA and inputB are not of the same length the returned sequence will have as many elements as the shorter of the two inputs.
    /// </returns>
    /// <exception cref="System.ArgumentNullException">
    /// inputA or inputB is null.
    /// </exception>
    public static IEnumerable<bool> And(this IEnumerable<bool> inputA, IEnumerable<bool> inputB)
    {
      if (inputA == null)
        throw Error.ArgumentNull("inputA");
      if (inputB == null)
        throw Error.ArgumentNull("inputB");

      using (IEnumerator<bool> inpA = inputA.GetEnumerator())
      using (IEnumerator<bool> inpB = inputB.GetEnumerator())
        while (inpA.MoveNext() && inpB.MoveNext())
          yield return inpA.Current && inpB.Current;
    }

    /// <summary>
    /// Applies a Logical 'OR' over two sequences.
    /// </summary>
    /// <param name="inputA">First System.Collections.Generic.IEnumerable&lt;bool&gt; to apply the 'OR' operation over.</param>
    /// <param name="inputB">Second System.Collections.Generic.IEnumerable&lt;bool&gt; to apply the 'OR' operation over.</param>
    /// <returns>
    /// A System.Collections.Generic.IEnumerable&lt;bool&gt; with values that are the 'OR' representation of the elements from inputA and inputB.
    /// If inputA and inputB are not of the same length the returned sequence will have as many elements as the shorter of the two inputs.
    /// </returns>
    /// <exception cref="System.ArgumentNullException">inputA or inputB is null.</exception>
    public static IEnumerable<bool> Or(this IEnumerable<bool> inputA, IEnumerable<bool> inputB)
    {
      if (inputA == null)
        throw Error.ArgumentNull("inputA");
      if (inputB == null)
        throw Error.ArgumentNull("inputB");

      using (IEnumerator<bool> inpA = inputA.GetEnumerator())
      using (IEnumerator<bool> inpB = inputB.GetEnumerator())
        while (inpA.MoveNext() && inpB.MoveNext())
          yield return inpA.Current || inpB.Current;
    }

    /// <summary>
    /// Applies a Logical 'XOR' over two sequences.
    /// </summary>
    /// <param name="inputA">First System.Collections.Generic.IEnumerable&lt;bool&gt; to apply the 'XOR' operation over.</param>
    /// <param name="inputB">Second System.Collections.Generic.IEnumerable&lt;bool&gt; to apply the 'XOR' operation over.</param>
    /// <returns>
    /// A System.Collections.Generic.IEnumerable&lt;bool&gt; with values that are the 'XOR' representation of the elements from inputA and inputB.
    /// If inputA and inputB are not of the same length the returned sequence will have as many elements as the shorter of the two inputs.
    /// </returns>
    /// <exception cref="System.ArgumentNullException">
    /// inputA or inputB is null.
    /// </exception>
    public static IEnumerable<bool> Xor(this IEnumerable<bool> inputA, IEnumerable<bool> inputB)
    {
      if (inputA == null)
        throw Error.ArgumentNull("inputA");
      if (inputB == null)
        throw Error.ArgumentNull("inputB");

      using (IEnumerator<bool> inpA = inputA.GetEnumerator())
      using (IEnumerator<bool> inpB = inputB.GetEnumerator())
        while (inpA.MoveNext() && inpB.MoveNext())
          yield return inpA.Current ^ inpB.Current;
    }
  }
}
