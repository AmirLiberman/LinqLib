using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqLib.Sort;

namespace SortTester
{
  struct TestInfo
  {
    public int SourceCount;
    public int SourceOrder;

    public int SortLevel;

    public SortType Sort1Type;
    public SortOrder Sort1Order;

    public SortType Sort2Type;
    public SortOrder Sort2Order;

    public SortType Sort3Type;
    public SortOrder Sort3Order;

    public SortType Sort4Type;
    public SortOrder Sort4Order;

    public int TestLoops;
  }

  public enum SortOrder
  {
    None,
    Ascending,
    Descending
  }
}
