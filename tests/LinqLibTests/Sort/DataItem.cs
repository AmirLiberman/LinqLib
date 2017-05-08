using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace LinqLibTests.Sort
{
  [DebuggerDisplay("{Index} : {P5}")]
  class DataItem
  {
    public int P1 { get; set; }
    public int P2 { get; set; }
    public string P3 { get; set; }
    public decimal P4 { get; set; }
    public string P5 { get; set; }
    public int Index { get; set; }

    public DataItem(int index)
    {
      Guid guid = Guid.NewGuid();

      P1 = guid.GetHashCode() % 1000;
      P2 = guid.GetHashCode();
      P3 = guid.ToString().Substring(8);
      P4 = (decimal)P2 + 1 / (decimal)(Math.Abs(P1) + 1);
      P5 = guid.ToString();
      Index = index;
    }
  }
}
