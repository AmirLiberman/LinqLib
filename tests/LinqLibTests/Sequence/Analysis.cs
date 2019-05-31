using System.Collections.Generic;
using System.Linq;
using LinqLib.Sequence;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinqLibTests.Sequence
{
  [TestClass()]
  public class AnalysisTests
  {
    #region Sequence Relations

    [TestMethod()]
    public void SequenceRelationTest()
    {
      IEnumerable<int> s1;
      IEnumerable<int> s2;

      s1 = new int[] { 1, 2, 3, 4, 5, 6, 7 };
      s2 = new int[] { 1, 2, 3, 4, 5, 6, 7 };
      Assert.IsTrue(s1.SequenceRelation(s2) == SequenceRelationType.Equal);

      //------------------------------//

      s1 = new int[] { 1, 2, 3, 4, 5, 6, 7 };
      s2 = new int[] { 1, 2, 3, 6, 7, 5, 4 };
      Assert.IsTrue(s1.SequenceRelation(s2) == SequenceRelationType.Similar);

      //------------------------------//

      s1 = new int[] { 1, 2, 3, 4, 5, 6, 7 };
      s2 = new int[] { 8, 9, 10, 11, 12, 13, 14 };
      Assert.IsTrue(s1.SequenceRelation(s2) == SequenceRelationType.None);

      //------------------------------//

      s1 = new int[] { 1, 2, 3, 4, 5, 6, 7 };
      s2 = new int[] { 5, 6, 7, 8, 9, 10, 11 };
      Assert.IsTrue(s1.SequenceRelation(s2) == SequenceRelationType.Intersects);

      //------------------------------//

      s1 = new int[] { 1, 2, 3, 4, 5, 6, 7 };
      s2 = new int[] { 2, 4, 6 };
      Assert.IsTrue(s1.SequenceRelation(s2) == SequenceRelationType.Contains);

      //------------------------------//

      s1 = new int[] { 2, 4, 6 };
      s2 = new int[] { 1, 2, 3, 4, 5, 6, 7 };
      Assert.IsTrue(s1.SequenceRelation(s2) == SequenceRelationType.Contained);

      //------------------------------//

      s1 = new int[] { 1, 2, 3, 4, 5, 6, 7 };
      s2 = new int[] { 11, 12 };
      Assert.IsTrue(s1.SequenceRelation(s2) == SequenceRelationType.None);

      //------------------------------//

      s1 = new int[] { 1, 2, 3, 4, 5, 6, 7 };
      s2 = new int[] { 11, 12, 6 };
      Assert.IsTrue(s1.SequenceRelation(s2) == SequenceRelationType.Intersects);

      //------------------------------//

      s1 = new int[] { 11, 12, 6 };
      s2 = new int[] { 1, 2, 3, 4, 5, 6, 7 };
      Assert.IsTrue(s1.SequenceRelation(s2) == SequenceRelationType.Intersects);
    }

    [TestMethod()]
    public void CompareToTest()
    {
      IEnumerable<int> s1;
      IEnumerable<int> s2;
      IEnumerable<int> expected;
      IEnumerable<int> actual;

      s1 = new int[] { 7, 7, 7, 8, 8, 8, 9, 9, 9 };
      s2 = new int[] { 8, 8, 8, 8, 8, 8, 8, 8, 8 };
      expected = new int[] { -1, -1, -1, 0, 0, 0, 1, 1, 1 };
      actual = s1.CompareTo(s2);
      Assert.IsTrue(actual.SequenceEqual(expected));

      //------------------------------//

      s1 = new int[] { 7, 7, 7, 8, 8, 8, 9, 9, 9 };
      s2 = new int[] { 8, 8, 8, 8, 8, 8, 8, 8, 8 };
      expected = new int[] { -1, -1, -1, 0, 0, 0, 1, 1, 1 };
      actual = s1.CompareTo(s2, new TestComparer());
      Assert.IsTrue(actual.SequenceEqual(expected));
    }

    [TestMethod()]
    public void MatchTest()
    {
      IEnumerable<string> s1;
      IEnumerable<string> s2;
      IEnumerable<string> e1;

      s1 = new string[] { "A", "A", "B", "A", "A", "C", "A", "A", "A", "A", null, "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "C", "A", "A", "A", "A", "D", "D" };
      s2 = new string[] { "A", "B", "A", "C", "A", "A", "A", "B", "A", "A", "A", "A", "A", "A", "A", "B", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "D", "D", "D", "D", "D", "D", "D", "D", "D", "D", "D", "X" };

      e1 = new string[] { "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "D" };

      var actual1 = s1.Match(s2).ToArray();

      Assert.IsTrue(actual1.Select(A => A.LeftItem).SequenceEqual(e1));
    }

    [TestMethod()]
    public void AlignTest()
    {
      IEnumerable<string> s1;
      IEnumerable<string> s2;
      IEnumerable<int> n1;
      IEnumerable<string> e1;
      IEnumerable<string> e2;
      IEnumerable<int> e3;
      IEnumerable<string> e4;
      IEnumerable<string> e5;

      s1 = new string[] { "A", "A", "B", "A", "A", "C", "A", "A", "A", "A", null, "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "C", "A", "A", "A", "A", "D", "D", "D", "D", "D", "D", "D", "D", "D", "D", "D", "D", "X" };
      s2 = new string[] { "A", "B", "A", "C", "A", "A", "A", "B", "A", "A", "A", "A", "A", "A", "A", "B", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "D" };
      n1 = new int[] { 1, 2, 1, 3, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4 };

      e1 = new string[] { "A", null, "A", "B", null, "A", "A", "C", "A", null, "A", "A", "A", null, "A", "A", "A", "A", null, "A", "A", "A", "A", "A", "A", "C", "A", "A", "A", "A", null, "D", "D", "D", "D", "D", "D", "D", "D", "D", "D", "D", "D", "X" };
      e2 = new string[] { "A", "B", "A", null, "C", "A", "A", null, "A", "B", "A", "A", "A", null, "A", "A", "A", "A", "B", "A", "A", "A", "A", "A", "A", null, "A", "A", "A", "A", "A", "D", null, null, null, null, null, null, null, null, null, null, null, null };
      e3 = new int[] { 1, 2, 1, -999, 3, 1, 1, -999, 1, 2, 1, 1, 1, -999, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, -999, 1, 1, 1, 1, 1, 4, -999, -999, -999, -999, -999, -999, -999, -999, -999, -999, -999, -999 };
      e4 = new string[] { "A", "A", "B", "A", null, "A", "C", "A", "A", null, "A", "A", null, "A", "A", "A", "A", "A", null, "A", "A", "A", "A", "A", "C", "A", "A", "A", "A", null, null, "D", "D", "D", "D", "D", "D", "D", "D", "D", "D", "D", "D", "X" };
      e5 = new string[] { "A", null, "B", "A", "C", "A", null, "A", "A", "B", "A", "A", null, "A", "A", "A", "A", "A", "B", "A", "A", "A", "A", "A", null, "A", "A", "A", "A", "A", "A", "D", null, null, null, null, null, null, null, null, null, null, null, null };

      var actual1 = s1.Align(s2, (A1, A2) => A1 == A2).ToArray();
      var actual2 = s1.Align(n1, (A1, A2) => (A1 != null ? A1[0] - 64 : 0) == A2, null, -999).ToArray();
      var actual3 = s1.Align(s2).ToArray();
      var actual4 = s2.Align(s1, (A1, A2) => A1 == A2).ToArray();

      Assert.IsTrue(actual1.Select(A => A.LeftItem).SequenceEqual(e1));
      Assert.IsTrue(actual1.Select(A => A.RightItem).SequenceEqual(e2));

      Assert.IsTrue(actual4.Select(A => A.RightItem).SequenceEqual(e4));
      Assert.IsTrue(actual4.Select(A => A.LeftItem).SequenceEqual(e5));

      Assert.IsTrue(actual2.Select(A => A.LeftItem).SequenceEqual(e1));
      Assert.IsTrue(actual2.Select(A => A.RightItem).SequenceEqual(e3));
      Assert.IsTrue(actual3.SequenceEqual(actual1));
    }

    [TestMethod()]
    public void AlignTest2()
    {
      var t1 = LinqLibTests.Properties.Resources.Hallelujah1.Split(new char[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
      var t2 = LinqLibTests.Properties.Resources.Hallelujah2.Split(new char[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

      var actual1 = t1.Align(t2).ToArray();
      int[] L = actual1.Select((RES, INDEX) => new { RES, INDEX }).Where(ITEM => ITEM.RES.AlignType == AlignType.LeftMissing).Select(X => X.INDEX).ToArray();
      int[] R = actual1.Select((RES, INDEX) => new { RES, INDEX }).Where(ITEM => ITEM.RES.AlignType == AlignType.RightMissing).Select(X => X.INDEX).ToArray();

      int[] expectedL = new int[] { 12, 18, 24, 63, 64, 82 };
      int[] expectedR = new int[] { 11, 17, 23, 61, 62, 81 };

      Assert.IsTrue(expectedL.SequenceEqual(L));
      Assert.IsTrue(expectedR.SequenceEqual(R));
    }

    [TestMethod()]
    public void AlignMatchTypeTest()
    {
      MatchResult<int, string> mr1 = new MatchResult<int, string>(5, "A");
      MatchResult<int, string> mr2 = new MatchResult<int, string>(5, "A");

      Assert.IsTrue(mr1.Equals(mr2));

      mr2 = new MatchResult<int, string>(5, "A1");
      Assert.IsTrue(!mr1.Equals(mr2));

      //-----------------//

      MatchResult<string, string> mr3 = new MatchResult<string, string>("5", "A");
      MatchResult<string, string> mr4 = new MatchResult<string, string>("5", "A");

      Assert.IsTrue(mr3.Equals(mr4));

      mr4 = new MatchResult<string, string>("A", "5");
      Assert.IsTrue(!mr3.Equals(mr4));

      Assert.IsTrue(!mr3.Equals(null));

      Assert.IsTrue(mr3.GetHashCode() == mr4.GetHashCode());

      //-----------------//

      AlignResult<int, string> ar1 = new AlignResult<int, string>(5, "A");
      AlignResult<int, string> ar2 = new AlignResult<int, string>(5, "A");

      Assert.IsTrue(ar1.Equals(ar2));

      ar2 = new AlignResult<int, string>(5, "A1");
      Assert.IsTrue(!ar1.Equals(ar2));

      //-----------------//

      AlignResult<string, string> ar3 = new AlignResult<string, string>("5", "A");
      AlignResult<string, string> ar4 = new AlignResult<string, string>("5", "A");

      Assert.IsTrue(ar3.Equals(ar4));

      ar4 = new AlignResult<string, string>("A", "5");
      Assert.IsTrue(!ar3.Equals(ar4));

      Assert.IsTrue(!ar3.Equals(null));

      Assert.IsTrue(ar3.GetHashCode() == ar4.GetHashCode());
    }

    [TestMethod()]
    public void AlignDemoTest()
    {
      string[] sq1 = new string[] { "Y", "Y", "Y", "R", "Y", "Y", "Y", "B", "Y", "Y", "R", "Y", "R", "Y", "Y", "Y", "Y", "Y", "Y", "R", "R", "Y", "R", "R", "Y", "Y", "Y", "Y", "R", "Y", "Y", "R" };
      string[] sq2 = new string[] { "Y", "Y", "R", "Y", "Y", "Y", "R", "Y", "B", "B", "Y", "Y", "Y", "Y", "R", "R", "Y", "Y", "Y", "Y", "Y", "Y", "R", "R", "R", "Y", "Y", "Y", "R", "R", "Y", "Y" };

      var align = sq1.Align(sq2).ToList();
      string[] expected1 = new string[] { "Y", "Y", null, "Y", "R", "Y", "Y", null, "Y", "B", null, "Y", "Y", "R", "Y", "R", "Y", null, null, "Y", "Y", "Y", "Y", "Y", "R", "R", "Y", "R", "R", null, "Y", "Y", "Y", "Y", "R", null, "Y", "Y", "R" };
      string[] expected2 = new string[] { "Y", "Y", "R", "Y", null, "Y", "Y", "R", "Y", "B", "B", "Y", "Y", null, "Y", null, "Y", "R", "R", "Y", "Y", "Y", "Y", "Y", null, null, "Y", "R", "R", "R", "Y", "Y", "Y", null, "R", "R", "Y", "Y", null };

      Assert.IsTrue(align.Select(R => R.LeftItem).SequenceEqual(expected1));
      Assert.IsTrue(align.Select(R => R.RightItem).SequenceEqual(expected2));

    }

    #endregion

    #region Pattern Detection

    [TestMethod()]
    public void PatternTest()
    {
      IEnumerable<int> source;
      IEnumerable<int> expected;
      IEnumerable<int> actual;

      source = new int[] { 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3 };
      expected = new int[] { 1, 2, 3 };

      actual = source.GetPattern();
      Assert.IsTrue(actual.SequenceEqual(expected));

      //------------------------------//

      actual = source.GetPattern(0, 5, true);
      Assert.IsTrue(actual.SequenceEqual(expected));

      //------------------------------//

      actual = source.GetPattern(2, 2, true);
      Assert.IsTrue(actual == null);

      //------------------------------//

      source = new int[] { 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1 };
      actual = source.GetPattern(0, 5, false);
      Assert.IsTrue(actual.SequenceEqual(expected));

      //------------------------------//

      source = new int[] { 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 44 };
      actual = source.GetPattern();
      Assert.IsTrue(actual == null);
    }

    #endregion

    #region Index Of

    [TestMethod()]
    public void IndexOfTest()
    {
      IEnumerable<int> source;
      int expected;
      int actual;

      source = new int[] { 0, 11, 22, 33, 44, 55, 66, 77, 88, 11, 22, 33, 44, 55, 66, 77 };

      expected = 4;
      actual = source.IndexOf(44);
      Assert.IsTrue(actual.Equals(expected));

      //------------------------------//

      expected = -1;
      actual = source.IndexOf(442);
      Assert.IsTrue(actual.Equals(expected));

      //------------------------------//

      expected = -1;
      actual = source.IndexOf(44, 13);
      Assert.IsTrue(actual.Equals(expected));

      //------------------------------//

      expected = -1;
      actual = source.IndexOf(44, 1000);
      Assert.IsTrue(actual.Equals(expected));

      //------------------------------//

      expected = 12;
      actual = source.IndexOf(44, 6);
      Assert.IsTrue(actual.Equals(expected));

      //------------------------------//

      expected = -1;
      actual = source.IndexOf(X => X == 442);
      Assert.IsTrue(actual.Equals(expected));

      //------------------------------//

      expected = -1;
      actual = source.IndexOf(X => X == 44, 13);
      Assert.IsTrue(actual.Equals(expected));

      //------------------------------//

      expected = 4;
      actual = source.IndexOf(X => X == 44);
      Assert.IsTrue(actual.Equals(expected));

      //------------------------------//

      expected = 12;
      actual = source.IndexOf(X => X == 44, 6);
      Assert.IsTrue(actual.Equals(expected));

    }

    [TestMethod()]
    public void IndexesOfTest()
    {
      IEnumerable<int> source;
      IEnumerable<int> expected;
      IEnumerable<int> actual;

      source = new int[] { 0, 11, 22, 33, 44, 55, 66, 77, 88, 11, 22, 33, 44, 55, 66, 77 };

      expected = new int[] { 4, 12 };
      actual = source.IndexesOf(44);
      Assert.IsTrue(actual.SequenceEqual(expected));

      expected = new int[] { };
      actual = source.IndexesOf(11144);
      Assert.IsTrue(actual.SequenceEqual(expected));
    }

    #endregion
  }
}
