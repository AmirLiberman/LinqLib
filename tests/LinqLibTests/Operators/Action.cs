using System.Collections.Generic;
using System.Linq;
using LinqLib.Operators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinqLibTests.Operators
{
  [TestClass()]
  public class ActionTests
  {
    #region ForEach Action

    [TestMethod()]
    public void ForEachTestTest1()
    {
      IEnumerable<IntWrap> source = Enumerable.Range(1, 1000).Select(I => new IntWrap(I)).ToArray();
      int[] temp = source.ForEach(IW => IW.V = IW.V * 2).Select(X => X.V).ToArray();
      Assert.AreEqual(temp.Sum(), source.Sum(I => I.V));
    }

    [TestMethod()]
    public void ForEachTestTest2()
    {
      IEnumerable<IntWrap> source = Enumerable.Range(1, 1000).Select(I => new IntWrap(I)).ToArray();
      int[] temp = source.ForEach((IW, I) => IW.V = IW.V * 2 + I).Select(X => X.V).ToArray();
      Assert.AreEqual(temp.Sum(), source.Sum(I => I.V));
    }

    #endregion
  }
}
