<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebApplication1.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <%="前台dll地址"+System.Reflection.Assembly.GetExecutingAssembly().Location %>
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <asp:ListBox ID="ListBox1" runat="server"></asp:ListBox>
        <asp:Button ID="Button1" runat="server" Text="<%=System.Reflection.Assembly.GetExecutingAssembly().Location %>"/>
        <asp:Table ID="Table1" runat="server" ></asp:Table>
    </div>
    </form>
</body>
</html>
