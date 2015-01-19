<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DAI.Rule.Validation.Definition.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Domain Object Class&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:DropDownList ID="dominObjList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dominObjList_SelectedIndexChanged" Width="350px">
        </asp:DropDownList>
            <br />
            <br />
            Domain Object Class Field&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:DropDownList ID="fieldList" runat="server" Height="16px" Width="350px">
        </asp:DropDownList>
            <br />
            <br />
            Validation Rule&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:DropDownList ID="validationRule" runat="server" Height="16px" Width="350px" AutoPostBack="True" OnSelectedIndexChanged="validationRule_SelectedIndexChanged">
        </asp:DropDownList>
            <br />
            <br />
            Error Message&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="errTxt" runat="server" Width="340px"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="Save" runat="server" OnClick="Save_Click" Text="Kaydet" />
            <asp:Literal ID="errorMsg" runat="server"></asp:Literal>
        </div>
      

    </form>
</body>
</html>
