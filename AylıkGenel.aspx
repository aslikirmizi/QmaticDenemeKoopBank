<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AylıkGenel.aspx.cs" Inherits="QmaticDeneme.AylıkGenel" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <webopt:bundlereference runat="server" path="~/Content/css" />
            <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="wrapper">
             <asp:ScriptManager ID="ScriptManager1" runat="server">
                        <Scripts>
                            <%--To learn more about bundling scripts in ScriptManager see https://go.microsoft.com/fwlink/?LinkID=301884 --%>
                            <%--Framework Scripts--%>
                            <asp:ScriptReference Name="MsAjaxBundle" />
                            <asp:ScriptReference Name="jquery" />
                            <asp:ScriptReference Name="WebForms.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebForms.js" />
                            <asp:ScriptReference Name="WebUIValidation.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebUIValidation.js" />
                            <asp:ScriptReference Name="MenuStandards.js" Assembly="System.Web" Path="~/Scripts/WebForms/MenuStandards.js" />
                            <asp:ScriptReference Name="GridView.js" Assembly="System.Web" Path="~/Scripts/WebForms/GridView.js" />
                            <asp:ScriptReference Name="DetailsView.js" Assembly="System.Web" Path="~/Scripts/WebForms/DetailsView.js" />
                            <asp:ScriptReference Name="TreeView.js" Assembly="System.Web" Path="~/Scripts/WebForms/TreeView.js" />
                            <asp:ScriptReference Name="WebParts.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebParts.js" />
                            <asp:ScriptReference Name="Focus.js" Assembly="System.Web" Path="~/Scripts/WebForms/Focus.js" />
                            <asp:ScriptReference Name="WebFormsBundle" />
                            <%--Site Scripts--%>
                        </Scripts>
                    </asp:ScriptManager>
                    <br />
                    <!-- menu -->
                    <div class="navbar navbar-inverse navbar-fixed-top">
                        <div class="container">
                            <div class="navbar-header">
                                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                                    <span class="icon-bar"></span>
                                    <span class="icon-bar"></span>
                                    <span class="icon-bar"></span>
                                </button>
                                <a class="navbar-brand" runat="server" href="~/">Qmatic Bilgi Sistemi</a>
                            </div>
                            <div class="navbar-collapse collapse">
                                <ul class="nav navbar-nav">
                                    <li><a runat="server" href="~/">Günlük İstatistikler</a></li>
                                    <li><a runat="server" href="~/Aylık">Aylık Şube İstatistikleri</a></li>
                                    <li><a runat="server" href="~/AylıkGenel">Aylık Genel İstatistikleri</a></li>

                                    <!-- <li><a runat="server" href="~/Contact">Contact</a></li> -->
                                </ul>
                            </div>
                        </div>
                    </div>
        </div>
        <div id="ContentAylık" style="margin-left: 40px">
            &nbsp;<asp:Label ID="Label1" runat="server" Text="Aylık Genel Rapor"></asp:Label>
            <asp:DropDownList ID="PickMonth" runat="server">
            </asp:DropDownList>
            &nbsp;&nbsp;
            <asp:DropDownList ID="PickYear" runat="server">
            </asp:DropDownList>
            <asp:Button ID="Button1" runat="server" Height="23px" OnClick="Button1_Click" Text="Ara" Width="97px" />
            <br />
            <br />
            <br />
            <div class="dataTables">
                <asp:Table ID="Table1" runat="server" Width="300px">
                </asp:Table>
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Table ID="Table2" runat="server" Width="300px" Height="38px">
                </asp:Table>

            </div>
            <br />
            <br />
            <div >
                <asp:Label ID="Label2" runat="server"></asp:Label>
                <ajaxToolkit:LineChart ID="LineChart1" runat="server" Width="800" Height="400" > </ajaxToolkit:LineChart>
            </div>
        &nbsp;&nbsp;&nbsp;&nbsp;<br />
&nbsp;<asp:Label ID="errorLabel" runat="server"></asp:Label>
        </div>
    </form>
</body>
</html>
