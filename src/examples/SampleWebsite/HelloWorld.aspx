<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HelloWorld.aspx.cs" Inherits="SampleWebsite.HelloWorld" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Hello from ASP.Net the time is
            <asp:Label runat="server" ID="TimeLabel" />
        </div>
        <div>
            The value of the configuration setting Key1 is
            <asp:Label runat="server" ID="ConfigLabel" />
        </div>
    </form>
</body>
</html>
