using System;
using System.Collections.Generic;
using System.Linq;
using LinqLib.DynamicCodeGenerator;
using LinqLib.Sequence;
using Samples.SampleData;

namespace Samples
{
  public partial class PivotTestPage : System.Web.UI.Page
  {
    private static List<Contact> contacts = Generator.CreateContacts();

    protected void btnOriginal_Click(object sender, EventArgs e)
    {      
      gridView.DataSource = contacts;
      gridView.DataBind();

      CodeLiteral.Text = @"
<div style=""font-family:Courier New; font-size:10pt;"">
<br/>
<br/>
Source Code:
<br/>
<br/>
gridView.DataSource = contacts;
<br/>
gridView.DataBind();
</div>";
    }

    protected void btnPivotPhone_Click(object sender, EventArgs e)
    {
      ResetFileLabel();
      gridView.DataSource = contacts.Pivot(X => X.Phones, X => X.PhoneType, X => string.Concat("(", X.AreaCode, ") ", X.PhoneNumber), true, TransformerClassGenerationEventHandler).ToArray();
      gridView.DataBind();

      CodeLiteral.Text = @"
<div style=""font-family:Courier New; font-size:10pt;"">
<br/>
<br/>
Source Code:
<br/>
<br/>
gridView.DataSource = contacts.Pivot(X => X.Phones, X => X.PhoneType, X => <span style=""color:Blue;"">string</span>.Concat(<span style=""color:Red;"">""(""</span>, X.AreaCode, <span style=""color:Red;"">"") ""</span>, X.PhoneNumber), true).ToArray();
<br/>
gridView.DataBind();
</div>";
    }

    protected void btnPivotAddress_Click(object sender, EventArgs e)
    {
      ResetFileLabel();
      gridView.DataSource = contacts.Pivot(X => X.Addresses, X => X.AddressType, X => string.Concat(X.City, " [", X.State, "]"), true, TransformerClassGenerationEventHandler).ToArray();
      gridView.DataBind();

      CodeLiteral.Text = @"
<div style=""font-family:Courier New; font-size:10pt;"">
<br/>
<br/>
Source Code:
<br/>
<br/>
gridView.DataSource = contacts.Pivot(X => X.Addresses, X => X.AddressType, X => <span style=""color:Blue;"">string</span>.Concat(X.City, <span style=""color:Red;"">"" [""</span>, X.State, <span style=""color:Red;"">""]""</span>), true).ToArray();
<br/>
gridView.DataBind();
</div>";
    }

    protected void TransformerClassGenerationEventHandler(object sender, ClassGenerationEventArgs e)
    {
      FilesLiteral.Text = @"
<div style=""font-family:Courier New; font-size:10pt;"">
<br/>
Base Path: " + e.BasePath + @"
<br/>
CodeFile: " + e.CodeFile;
      if (e.HasError)
        FilesLiteral.Text += "<br>Failed To create Pivot class";
      FilesLiteral.Text += "</div>";
    }

    private void ResetFileLabel()
    {
      FilesLiteral.Text = "Class generation was skipped, Type was already in the cache.";
    }
  }
}
