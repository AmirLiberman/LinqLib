using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LinqLib.Array;
using LinqLib.Sort;
using LinqLib.Sequence;
using LinqLib.Operators;

namespace SortTester
{
  public partial class MainForm : Form
  {

    public MainForm()
    {
      InitializeComponent();
    }


    private void MainForm_Load(object sender, EventArgs e)
    {
      InitControls();
    }

    private void sort2ComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      SetVisibility(2, sort2ComboBox.SelectedIndex > 0);
    }

    private void sort3ComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      SetVisibility(3, sort3ComboBox.SelectedIndex > 0);
    }

    private void sort4ComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      SetVisibility(4, sort4ComboBox.SelectedIndex > 0);
    }

    private void startButton_Click(object sender, EventArgs e)
    {
      if (!backgroundWorker.IsBusy)
      {
        textBox1.Text = "";
        TestInfo testInfo = GetTestInfo();
        backgroundWorker.RunWorkerAsync(testInfo);

        startButton.Text = "Cancel";
      }
      else
      {
        if (backgroundWorker.WorkerSupportsCancellation == true)
          backgroundWorker.CancelAsync();
      }
    }

    private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      TestInfo testInfo = (TestInfo)e.Argument;

      IEnumerable<TestDataElement> testData = Enumerator.Generate(testInfo.SourceCount, X => new TestDataElement(X));
      if (testInfo.SourceOrder == 0)
        testData = testData.Shuffle();
      else
        testData = PreSortTestData(testData, testInfo);

      TestDataElement[] testArray1;
      TestDataElement[] testArray2;

      testArray1 = testData.ToArray();
      testArray2 = new TestDataElement[testArray1.Length];

      testArray1.CopyTo(testArray2, 0);

      IOrderedEnumerable<TestDataElement> standardSort = GetStandardSort(testArray1, testInfo);
      IComposableSortEnumerable<TestDataElement> linqLibSort = GetLinqLibSort(testArray2, testInfo);

      e.Cancel = DoTest(testInfo, standardSort, linqLibSort);
    }

    private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      SetMessage((string)e.UserState);
    }

    private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      startButton.Text = "Start";

      if (e.Cancelled == true)
        SetMessage("Canceled!");
      else if (e.Error != null)
        SetMessage("Error: " + e.Error.Message);
      else
        SetMessage("Done!");
    }


    private void InitControls()
    {
      string[] items = new string[] { "Quick", "Heap", "Bubble", "Insert", "Merge", "Shell", "Select" };
      this.sort1ComboBox.Items.AddRange(items);

      items = items.Prepend("None").ToArray();
      this.sort2ComboBox.Items.AddRange(items);
      this.sort3ComboBox.Items.AddRange(items);
      this.sort4ComboBox.Items.AddRange(items);

      sourceOrderComboBox.SelectedIndex = 0;
      sort1ComboBox.SelectedIndex = 0;
      sort2ComboBox.SelectedIndex = 0;
    }

    private void SetVisibility(int index, bool visible)
    {
      switch (index)
      {
        case 2:
          sort2CheckBox.Visible = visible;
          sort3ComboBox.Visible = visible;
          SetVisibility(3, visible && sort3ComboBox.SelectedIndex > 0);
          if (!visible)
            sort3ComboBox.SelectedIndex = -1;
          break;
        case 3:
          sort3CheckBox.Visible = visible;
          sort4ComboBox.Visible = visible;
          SetVisibility(4, visible && sort3ComboBox.SelectedIndex > 0);
          if (!visible)
            sort4ComboBox.SelectedIndex = -1;
          break;
        case 4:
          sort4CheckBox.Visible = visible;
          break;
      }
    }

    private TestInfo GetTestInfo()
    {
      TestInfo testInfo = new TestInfo();

      testInfo.SourceCount = (int)ElementsCountInput.Value;
      if (sourceOrderComboBox.SelectedIndex == 1)
        testInfo.SourceOrder = 1;
      else if (sourceOrderComboBox.SelectedIndex == 2)
        testInfo.SourceOrder = -1;
      else
        testInfo.SourceOrder = 0;

      testInfo.Sort1Type = GetSortType(sort1ComboBox.SelectedItem);
      testInfo.Sort1Order = sort1CheckBox.Checked ? SortOrder.Ascending : SortOrder.Descending;
      testInfo.SortLevel = 1;

      testInfo.Sort2Type = GetSortType(sort2ComboBox.SelectedItem);
      testInfo.Sort2Order = sort2CheckBox.Checked ? SortOrder.Ascending : SortOrder.Descending;
      if (sort2CheckBox.Visible)
        testInfo.SortLevel = 2;

      testInfo.Sort3Type = GetSortType(sort3ComboBox.SelectedItem);
      testInfo.Sort3Order = sort3CheckBox.Checked ? SortOrder.Ascending : SortOrder.Descending;
      if (sort3CheckBox.Visible)
        testInfo.SortLevel = 3;

      testInfo.Sort4Type = GetSortType(sort4ComboBox.SelectedItem);
      testInfo.Sort4Order = sort4CheckBox.Checked ? SortOrder.Ascending : SortOrder.Descending;
      if (sort4CheckBox.Visible)
        testInfo.SortLevel = 4;

      testInfo.TestLoops = (int)loopsCountInput.Value;


      return testInfo;
    }

    private SortType GetSortType(object name)
    {
      if (name == null)
        return LinqLib.Sort.SortType.Quick;

      switch (name.ToString())
      {
        case "None":
          return SortType.Quick;
        case "Heap":
          return SortType.Heap;
        case "Quick":
          return SortType.Quick;
        case "Bubble":
          return SortType.Bubble;
        case "Insert":
          return SortType.Insert;
        case "Merge":
          return SortType.Merge;
        case "Shell":
          return SortType.Shell;
        case "Select":
          return SortType.Select;
        default:
          throw new NotImplementedException();
      }
    }

    private bool DoTest(TestInfo testInfo, IOrderedEnumerable<TestDataElement> standardSort, IComposableSortEnumerable<TestDataElement> linqLibSort)
    {

      Stopwatch sw = new Stopwatch();
      double r1, r2;

      for (int i = 0; i < testInfo.TestLoops && !backgroundWorker.CancellationPending; i++)
      {
        sw.Reset();
        sw.Start();
        var arr1 = standardSort.ToArray();
        r1 = sw.Elapsed.TotalMilliseconds;

        sw.Reset();
        sw.Start();
        var arr2 = linqLibSort.ToArray();
        r2 = sw.Elapsed.TotalMilliseconds;

        backgroundWorker.ReportProgress(i, "Standard: " + r1.ToString() + "\r\nCustom: " + r2.ToString() + "\r\n\r\n");
        if (arr1.SequenceRelation(arr2) != SequenceRelationType.Equal)
        {
#if DEBUG
          for (int i1 = 0; i1 < arr1.Length; i1++)
            if (arr1[i1].Index != arr2[i1].Index)
            {
              Debugger.Break();
              break;
            }
#else
          backgroundWorker.ReportProgress(i, "Sort mismatch***.\r\n\r\n");
#endif
        }
      }
      return backgroundWorker.CancellationPending;
    }

    private IComposableSortEnumerable<TestDataElement> GetLinqLibSort(TestDataElement[] testArray, TestInfo testInfo)
    {
      IEnumerable<TestDataElement> sorted = testArray.Select(X => X);

      if (testInfo.Sort1Order == SortOrder.Ascending)
        sorted = sorted.OrderBy(X => X.Property1, testInfo.Sort1Type);
      else
        sorted = sorted.OrderByDescending(X => X.Property1, testInfo.Sort1Type);

      if (testInfo.SortLevel >= 2)
      {
        if (testInfo.Sort2Order == SortOrder.Ascending)
          sorted = ((IComposableSortEnumerable<TestDataElement>)sorted).ThenBy(X => X.Property2, testInfo.Sort2Type);
        else
          sorted = ((IComposableSortEnumerable<TestDataElement>)sorted).ThenByDescending(X => X.Property2, testInfo.Sort2Type);

        if (testInfo.SortLevel >= 3)
        {
          if (testInfo.Sort3Order == SortOrder.Ascending)
            sorted = ((IComposableSortEnumerable<TestDataElement>)sorted).ThenBy(X => X.Property3, testInfo.Sort3Type);
          else
            sorted = ((IComposableSortEnumerable<TestDataElement>)sorted).ThenByDescending(X => X.Property3, testInfo.Sort3Type);

          if (testInfo.SortLevel >= 4)
            if (testInfo.Sort4Order == SortOrder.Ascending)
              sorted = ((IComposableSortEnumerable<TestDataElement>)sorted).ThenBy(X => X.Property4, testInfo.Sort4Type);
            else
              sorted = ((IComposableSortEnumerable<TestDataElement>)sorted).ThenByDescending(X => X.Property4, testInfo.Sort4Type);
        }
      }

      return (IComposableSortEnumerable<TestDataElement>)sorted;

    }

    private IOrderedEnumerable<TestDataElement> GetStandardSort(TestDataElement[] testArray, TestInfo testInfo)
    {
      IEnumerable<TestDataElement> sorted = testArray.Select(X => X);

      if (testInfo.Sort1Order == SortOrder.Ascending)
        sorted = sorted.OrderBy(X => X.Property1);
      else
        sorted = sorted.OrderByDescending(X => X.Property1);

      if (testInfo.SortLevel >= 2)
      {
        if (testInfo.Sort2Order == SortOrder.Ascending)
          sorted = ((IOrderedEnumerable<TestDataElement>)sorted).ThenBy(X => X.Property2);
        else
          sorted = ((IOrderedEnumerable<TestDataElement>)sorted).ThenByDescending(X => X.Property2);

        if (testInfo.SortLevel >= 3)
        {
          if (testInfo.Sort3Order == SortOrder.Ascending)
            sorted = ((IOrderedEnumerable<TestDataElement>)sorted).ThenBy(X => X.Property3);
          else
            sorted = ((IOrderedEnumerable<TestDataElement>)sorted).ThenByDescending(X => X.Property3);

          if (testInfo.SortLevel >= 4)
            if (testInfo.Sort4Order == SortOrder.Ascending)
              sorted = ((IOrderedEnumerable<TestDataElement>)sorted).ThenBy(X => X.Property4);
            else
              sorted = ((IOrderedEnumerable<TestDataElement>)sorted).ThenByDescending(X => X.Property4);
        }
      }

      return (IOrderedEnumerable<TestDataElement>)sorted;
    }

    private IEnumerable<TestDataElement> PreSortTestData(IEnumerable<TestDataElement> testData, TestInfo testInfo)
    {
      TestInfo temp = testInfo;
      if (testInfo.SourceOrder == -1)
      {
        if (temp.Sort1Order == SortOrder.Ascending)
          temp.Sort1Order = SortOrder.Descending;
        else if (temp.Sort1Order == SortOrder.Descending)
          temp.Sort1Order = SortOrder.Ascending;

        if (temp.Sort2Order == SortOrder.Ascending)
          temp.Sort2Order = SortOrder.Descending;
        else if (temp.Sort2Order == SortOrder.Descending)
          temp.Sort2Order = SortOrder.Ascending;

        if (temp.Sort3Order == SortOrder.Ascending)
          temp.Sort3Order = SortOrder.Descending;
        else if (temp.Sort3Order == SortOrder.Descending)
          temp.Sort3Order = SortOrder.Ascending;

        if (temp.Sort4Order == SortOrder.Ascending)
          temp.Sort4Order = SortOrder.Descending;
        else if (temp.Sort4Order == SortOrder.Descending)
          temp.Sort4Order = SortOrder.Ascending;
      }
      return GetStandardSort(testData.ToArray(), temp);
    }

    private void SetMessage(string message)
    {
      if (!textBox1.IsDisposed)
      {
        textBox1.SelectionStart = textBox1.Text.Length;
        textBox1.SelectedText = message;
      }
    }
  }
}
