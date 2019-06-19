<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Lab5.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Main page</title>
    <link rel="stylesheet" href="style.css" />
    <script type="text/javascript">
    function throwErr()
    {
        document.getElementById('Button1').style.display = 'none';
    }
</script>
</head>
<body style="background-color: #CDFAE7;">
    <form id="form1" runat="server">

            <asp:Table ID="Table1" runat="server">
            </asp:Table>
            
        <hr style="margin-top:45px;"/>
        
        <asp:Panel ID="LoginPanel" runat="server" Height="204px" Width="472px" >
            <asp:Label ID="LoginErrorLabel" runat="server"  Text="Комбінація логіна та пароля є недійсною"></asp:Label>
            <p style="text-align:center; font-size:18px; margin-bottom:5px;margin-top:5px;">Login</p>
            <asp:TextBox ID="TextBox1" runat="server" ></asp:TextBox>
            <br />
            <p style="text-align:center; font-size:18px; margin-bottom:5px;margin-top:5px;">Password</p>
            <asp:TextBox ID="TextBox2" TextMode="Password" runat="server"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" OnClientClick="throwErr();" style="display:block; margin-top:0px; margin-left:auto;margin-right:auto;" Text="Login" ToolTip="Just DO It" Width="128px" />
        </asp:Panel>
        <footer style="text-align: center;">
            <hr style="margin-top:35px;" />
	        Gallery, by triod315
	        <br />
	        2019
        </footer>
    </form>
</body>
</html>
