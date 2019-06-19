<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PhotosAdmin.aspx.cs" Inherits="Lab5.PhotosAdmin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="style.css"/>
    <script src="jsfuncs.js"> </script>
</head>
<body style="background-color: #F4F9F7;">
    <asp:Label ID="Label1" runat="server" Text="Label" class="adminHead"></asp:Label>
    <hr />
    <form id="form1" runat="server">
        <div>
        </div>
        <asp:Table ID="Table1" runat="server"></asp:Table>
        <hr style="margin-top:25px;"/>
        <p class="adminHead">
            Завантажити фото
        </p>
        <asp:TextBox ID="TextBox1" runat="server" Text="Опис" style="display:block; margin:auto; font-size:16pt;" Height="31px" Width="328px"></asp:TextBox>
        <br />
        <asp:FileUpload ID="FileUpload1" runat="server" style="display:block; margin:auto;" accept="image/*"/>
        <br />
        <asp:Button ID="Button1" runat="server" Text="Завантажити" style="display:block; margin:auto;" OnClick="Button1_Click"/>
        
        <hr style="margin-top:45px;"/>
        <asp:Button ID="Button3" runat="server" Text="Back" Width="78px" style=" float:left;" OnClick="Button3_Click"/>
        <asp:Button ID="Button2" runat="server" Text="Exit" style=" float:right;" Width="78px" OnClick="Button2_Click"/>
        <p>
        
        </p>
    </form>
    <footer style="text-align: center;">
            <hr style="margin-top:35px;" />
	        Gallery, by triod315
	        <br />
	        2019
        </footer>
</body>
</html>
