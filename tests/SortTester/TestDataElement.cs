using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SortTester
{
  public class TestDataElement
  {
    public int Property1 { get; set; }
    public DateTime Property2 { get; set; }
    public decimal Property3 { get; set; }
    public string Property4 { get; set; }
    public int Index { get; set; }

    public TestDataElement(int index)
    {
      Guid guid = Guid.NewGuid();

      Property1 = guid.GetHashCode();
      Property2 = DateTime.Now.AddTicks(Property1);
      Property3 = (decimal)Property1 + 1 / (decimal)Property1;
      Property4 = guid.ToString();

      Index = index;
    }
  }

  //public class TestDataElement2
  //{
  //  public int Property1 { get; set; }
  //  public string Property2 { get; set; }
  //  public decimal Property3 { get; set; }
  //  public int Property4 { get; set; }
  //  public int Index { get; set; }

  //  public TestDataElement2(int index)
  //  {      
  //    Property1 = index % 17;
  //    Property2 = string.Format("{0} {1}", index % 7, index % 11);
  //    Property3 = (decimal)Property1 + 1 / (decimal)(Property1 + 1);
  //    Property4 = index;

  //    Index = index;
  //  }
  //}

}
