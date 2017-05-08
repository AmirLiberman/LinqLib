<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PivotTestPage.aspx.cs"
    Inherits="Samples.PivotTestPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Pivot test Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btnOriginal" runat="server" Text="Load Original Data" OnClick="btnOriginal_Click" />
        <asp:Button ID="btnPivotPhone" runat="server" Text="Load Pivoted Phone Data" OnClick="btnPivotPhone_Click" />
        <asp:Button ID="btnPivotAddress" runat="server" Text="Load Pivoted Address Data"
            OnClick="btnPivotAddress_Click" />
    </div>
    <p />
    <div>
        <asp:GridView ID="gridView" runat="server" BackColor="#DEBA84" BorderColor="#DEBA84"
            BorderStyle="None" BorderWidth="1px" CellPadding="3" CellSpacing="2">
            <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
            <HeaderStyle BackColor="#A55129" Font-Bold="True" ForeColor="White" />
            <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
            <RowStyle BackColor="#FFF7E7" ForeColor="#8C4510" />
            <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#FFF1D4" />
            <SortedAscendingHeaderStyle BackColor="#B95C30" />
            <SortedDescendingCellStyle BackColor="#F1E5CE" />
            <SortedDescendingHeaderStyle BackColor="#93451F" />
        </asp:GridView>
    </div>
    <asp:Literal ID="CodeLiteral" runat="server"></asp:Literal>
    <br /><br />
    <asp:Literal ID="FilesLiteral" runat ="server"></asp:Literal>
    </form>
</body>
</html>
