<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Photos.aspx.cs" Inherits="Lab5.Photos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="style.css"/>
</head>
<body style="background-color: #CDFAE7;">
    <form id="form1" runat="server">
        <asp:Label ID="Label1" runat="server" Text="Label" class="adminHead"></asp:Label>
        <hr />
        <asp:Panel ID="Panel1" runat="server">
        </asp:Panel>
        <hr style="margin-top:45px;"/>
        <asp:Button ID="Button1" runat="server" Text="Exit" style="display:block;margin:auto;" Width="78px" OnClick="Button1_Click"/>

    </form>
    <footer style="text-align: center;">
            <hr style="margin-top:35px;" />
	        Gallery, by triod315
	        <br />
	        2019
        </footer>
</body>
</html>
