<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminPage.aspx.cs" Inherits="Lab5.AdminPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="style.css"/>

    <script src="jsfuncs.js"> </script>

</head>
<body style="background-color: #F4F9F7;">
    <form id="form1" runat="server">
        <p class="adminHead">
            Адміністративна сторінка галереї
        </p>
        <asp:Table ID="Table1" runat="server">
            </asp:Table>
        <hr style="margin-top:45px;"/>  
        <p class="adminHead">
            Ім'я галереї
        </p>
        <asp:TextBox ID="TextBox1" runat="server" style="display:block;margin-left:auto;margin-right:auto;margin-top:0px;font-size:18pt;"></asp:TextBox>
                <asp:Button ID="Button2" runat="server" Text="Create gallery" style="display:block;margin-left:auto;margin-right:auto;margin-top:1%;font-size:18pt;font-family: sans-serif;" Width="181px" OnClick="Button2_Click" Height="45px"/>
    
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
